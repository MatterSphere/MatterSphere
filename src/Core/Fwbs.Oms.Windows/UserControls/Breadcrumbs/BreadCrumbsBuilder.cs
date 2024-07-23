using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FWBS.OMS.Interfaces;
using FWBS.OMS.UI.Windows;
using FWBS.OMS.UI.Windows.Interfaces;

namespace FWBS.OMS.UI.UserControls.Breadcrumbs
{
    internal class BreadCrumbsBuilder : IDisposable
    {
        private const string ARROW_SYMBOL = ">";
        private readonly List<BreadCrumbItem> _breadCrumbs;
        private readonly BreadCrumbsPanel container;
        private readonly string _dashboardTitle = "Dashboard";
        private readonly string _searchResultsTitle = "Search Results";
        private Font _regularFont = new Font("Segoe UI", 12.75F, FontStyle.Regular);
        private Font _semiboldFont = new Font("Segoe UI Semibold", 12.75F);

        public BreadCrumbsBuilder(BreadCrumbsPanel container)
        {
            if (Session.CurrentSession.IsConnected)
            {
                _dashboardTitle = Session.CurrentSession.Resources.GetResource("Dashboard", "Dashboard", string.Empty).Text;
                _searchResultsTitle = Session.CurrentSession.Resources.GetResource("SCRHResults", "Search Results", string.Empty).Text;
            }

            _breadCrumbs = new List<BreadCrumbItem>();
            this.container = container;
        }

        public delegate void LabelClicked(BreadCrumbItem breadCrumb);
        public event LabelClicked onLabelClicked;

        public BreadCrumbItem LastBreadCrumb
        {
            get
            {
                return _breadCrumbs?.LastOrDefault();
            }
        }

        public bool ContainsSearchBreadCrumbItem
        {
            get
            {
                return LastBreadCrumb?.ViewType == ViewEnum.ElasticSearch;
            }
        }

        #region Methods

        public void Build(BreadCrumbItem item)
        {
            container.SuspendLayout();
            container.Controls.Clear();

            AddItem(item);
            InsertLabels();

            container.ResumeLayout();
            container.EnsureLastVisible();
        }

        public void AddSearchResultItem()
        {
            container.SuspendLayout();
            container.Controls.Clear();

            var item = BreadCrumbItem.CreateSearchResultsItem(_searchResultsTitle);
            AddItem(item);
            InsertLabels();

            container.ResumeLayout();
            container.EnsureLastVisible();
        }

        public void RemoveSearchItem()
        {
            if (_breadCrumbs.Last().ViewType == ViewEnum.ElasticSearch)
            {
                RemoveLastItem();
            }
        }

        public ViewEnum RemoveLastItem()
        {
            _breadCrumbs.RemoveAt(_breadCrumbs.Count - 1);
            RemoveLastControls();
            SelectLastItem();

            if (_breadCrumbs.Any())
            {
                _breadCrumbs.Last().NextItem = null;
            }

            return _breadCrumbs.Last().ViewType;
        }

        public void ShowRootItem(string title, ViewEnum viewType)
        {
            container.SuspendLayout();
            container.Controls.Clear();

            AddRootItem(title, viewType);
            InsertLabels();

            container.ResumeLayout();
            container.EnsureLastVisible();
        }

        #endregion

        #region Private methods

        private void SelectLastItem()
        {
            container.Controls.Last<Control>().Font = _semiboldFont;
        }

        private void RemoveLastControls()
        {
            container.SuspendLayout();
            container.Controls.RemoveAt(container.Controls.Count - 1);
            container.Controls.RemoveAt(container.Controls.Count - 1);
            container.Controls.Last<Control>().Cursor = Cursors.Default;
            container.ResumeLayout();
            container.EnsureLastVisible();
        }

        private void AddItem(BreadCrumbItem item)
        {
            if (_breadCrumbs.Any())
            {
                if (_breadCrumbs.Last().ViewType == ViewEnum.ElasticSearch)
                {
                    _breadCrumbs.RemoveAt(_breadCrumbs.Count - 1);
                }

                var lastDisplay = _breadCrumbs.LastOrDefault()?.Display;
                if (lastDisplay != null && lastDisplay == item.Display)
                {
                    _breadCrumbs.RemoveAt(_breadCrumbs.Count - 1);
                }
            }

            var lastItem = _breadCrumbs.LastOrDefault();
            if (lastItem != null)
            {
                lastItem.NextItem = item;
            }

            _breadCrumbs.Add(item);
        }

        private void InsertLabels()
        {
            foreach (var breadCrumb in _breadCrumbs)
            {
                if (breadCrumb.IsRootItem)
                {
                    var firstLabel = CreateLabel(breadCrumb.Key, breadCrumb.Title, breadCrumb.ViewType == ViewEnum.SearchManager, breadCrumb == _breadCrumbs.Last());
                    container.Controls.Add(firstLabel);
                }
                else
                {
                    var arrow = CreateArrowLabel();
                    container.Controls.Add(arrow);
                    string title = CreateBreadCrumbTitle(breadCrumb.Title, breadCrumb.Display?.Object, breadCrumb.Display?.IsUser ?? false);
                    var label = CreateLabel(breadCrumb.Key, title, breadCrumb != _breadCrumbs.Last(), breadCrumb == _breadCrumbs.Last());
                    container.Controls.Add(label);
                }
            }

            container.Controls.Last<Control>().Cursor = Cursors.Default;
        }

        private Label CreateLabel(Guid key, string title, bool clickable, bool isLastItem)
        {
            var label = new Label
            {
                AutoSize = true,
                Font = isLastItem
                    ? _semiboldFont
                    : _regularFont,
                ForeColor = Color.FromArgb(1, 51, 51, 51),
                TextAlign = ContentAlignment.MiddleLeft,
                Text = title,
                Tag = key
            };

            if (clickable)
            {
                label.Cursor = Cursors.Hand;
                label.MouseClick += (sender, e) =>
                {
                    if (e.Button != MouseButtons.Left)
                    {
                        return;
                    }

                    var labelSender = (Label)sender;
                    Guid clickedItemKey = (Guid)labelSender.Tag;
                    var breadCrumb = _breadCrumbs.First(item => item.Key == clickedItemKey);
                    onLabelClicked?.Invoke(breadCrumb);
                };
            }

            return label;
        }

        private Label CreateArrowLabel()
        {
            var arrow = new Label
            {
                AutoSize = true,
                Font = _regularFont,
                ForeColor = Color.FromArgb(1, 102, 102, 102),
                Text = ARROW_SYMBOL,
                TextAlign = ContentAlignment.MiddleCenter
            };

            return arrow;
        }

        private string CreateBreadCrumbTitle(string title, IOMSType omsType, bool isUser)
        {
            if (omsType == null)
            {
                return title;
            }

            if (omsType is OMSFile)
            {
                var file = omsType as OMSFile;
                title = $"{file.Client.ClientNo}/{file.FileNo} {title}";
            }
            else if (omsType is Client)
            {
                var client = omsType as Client;
                title = $"{client.ClientNo} {title}";
            }
            else if (omsType is Contact)
            {
                var contact = omsType as Contact;
                title = $"{contact.Name} {title}";
            }
            else if (omsType is Associate)
            {
                var associate = omsType as Associate;
                title = $"{associate.Contact.Name} {title}";
            }
            else if (omsType is Appointment)
            {
                var appointment = omsType as Appointment;
                title = $"{appointment.Description} {title}";
            }
            else if (omsType is Task)
            {
                var task = omsType as Task;
                title = $"{task.Description} {title}";
            }
            else if (isUser)
            {
                var user = omsType as User;
                title = $"{user.FullName} {title}";
            }

            return title;
        }

        private void AddRootItem(string title, ViewEnum viewType)
        {
            _breadCrumbs.Clear();
            var item = BreadCrumbItem.CreateRootItem(title, viewType);
            AddItem(item);
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (_regularFont != null)
            {
                _regularFont.Dispose();
                _regularFont = null;
            }

            if (_semiboldFont != null)
            {
                _semiboldFont.Dispose();
                _semiboldFont = null;
            }
        }

        #endregion
    }
}
