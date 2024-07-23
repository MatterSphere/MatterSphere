using System;

namespace Fwbs
{
    namespace WinFinder
    {
        using System.ComponentModel;
        using System.Windows.Forms;

        public class LocalWindow : Window
        {
            #region Fields

            private InternalLocalWindow internalwin;

            #endregion

            #region Events

            #region Event Keys

            private static readonly object EVENT_CLOSING = new object();
            private static readonly object EVENT_CLOSED = new object();

            private static readonly object EVENT_MESSAGE = new object();

            #endregion

            #region Event Fields

            private EventHandlerList events = new EventHandlerList();

            #endregion

            #region Event Methods

            protected void AddHandler(object key, Delegate handler)
            {
                events.AddHandler(key, handler);
            }

            protected void RemoveHandler(object key, Delegate handler)
            {
                events.RemoveHandler(key, handler);
            }

            #endregion

            #region Event Accessors

            public event EventHandler<EventArgs> Closed
            {
                add
                {
                    AddHandler(EVENT_CLOSED, value);
                }
                remove
                {
                    RemoveHandler(EVENT_CLOSED, value);
                }
            }

            public event EventHandler<MessageEventArgs> Closing
            {
                add
                {
                    AddHandler(EVENT_CLOSING, value);
                }
                remove
                {
                    RemoveHandler(EVENT_CLOSING, value);
                }
            }


            public event EventHandler<MessageEventArgs> Message
            {
                add
                {
                    AddHandler(EVENT_MESSAGE, value);
                }
                remove
                {
                    RemoveHandler(EVENT_MESSAGE, value);
                }
            }

            #endregion

            #region Event Raisers

            protected void OnClosed(EventArgs e)
            {
                EventHandler<EventArgs> ev = (EventHandler<EventArgs>)events[EVENT_CLOSED];
                if (ev != null)
                    ev(this, e);
            }

            protected void OnClosing(MessageEventArgs e)
            {
                EventHandler<MessageEventArgs> ev = (EventHandler<MessageEventArgs>)events[EVENT_CLOSING];
                if (ev != null)
                {
                    ev(this, e);
                }
            }

            protected void OnMessage(MessageEventArgs e)
            {
                EventHandler<MessageEventArgs> ev = (EventHandler<MessageEventArgs>)events[EVENT_MESSAGE];
                if (ev != null)
                    ev(this, e);
            }
            

            #endregion

            #endregion

            #region Constructors

            public LocalWindow(IntPtr handle)
                : base(handle)
            {
                if (!IsLocal)
                    throw new InvalidOperationException(Properties.Resources.ExceptionNotLocalWindow);

                internalwin = new InternalLocalWindow(this);
            }

            #endregion

            #region Methods

            protected override void Dispose(bool disposing)
            {
                Release();
                events.Dispose();
                base.Dispose(disposing);
            }

            public void Release()
            {
                if (internalwin != null)
                    internalwin.ReleaseHandle();
            }

            #endregion

            private class InternalLocalWindow : NativeWindow
            {
                private LocalWindow win;

                internal InternalLocalWindow(LocalWindow win)
                {
                    this.win = win;
                    AssignHandle(win.Handle);
                }

                protected override void WndProc(ref Message message)
                {
                    MessageEventArgs args = new MessageEventArgs(message);
                    win.OnMessage(args);
                    if (args.Handled)
                        return;

                    switch ((WindowMessage)message.Msg)
                    {
                        case WindowMessage.Close:
                            {
                              
                                win.OnClosing(args);
                                if (args.Handled)
                                    return;

                                win.OnClosed(EventArgs.Empty);
                            }
                            break;
                    }



                    base.WndProc(ref message);
                }
            }


        }
    }
}