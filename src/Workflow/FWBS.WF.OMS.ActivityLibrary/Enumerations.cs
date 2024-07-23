namespace FWBS.WF.OMS.ActivityLibrary
{
    public enum ActivityMessageBoxIcon
    {
        // Summary:
        //     The message box contain no symbols.
        None = 0,
        //
        // Summary:
        //     The message box contains a symbol consisting of white X in a circle with
        //     a red background.
        Error = 16,
        //
        // Summary:
        //     The message box contains a symbol consisting of a question mark in a circle.
        //     The question-mark message icon is no longer recommended because it does not
        //     clearly represent a specific type of message and because the phrasing of
        //     a message as a question could apply to any message type. In addition, users
        //     can confuse the message symbol question mark with Help information. Therefore,
        //     do not use this question mark message symbol in your message boxes. The system
        //     continues to support its inclusion only for backward compatibility.
        Question = 32,
        //
        // Summary:
        //     The message box contains a symbol consisting of an exclamation point in a
        //     triangle with a yellow background.
        Warning = 48,
        //
        // Summary:
        //     The message box contains a symbol consisting of a lowercase letter i in a
        //     circle.
        Information = 64,
    }
}
