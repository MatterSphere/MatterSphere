using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Horizon.Common;
using Horizon.Providers;
using Horizon.TestiFilter.Common;
using Horizon.ViewModels.Common;
using Microsoft.Win32;

namespace Horizon.ViewModels.IFilters
{
    public class UserDocumentTestViewModel : PageViewModel
    {
        public UserDocumentTestViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            Title = "Custom Tests";
            DocumentItems = new ObservableCollection<DocumentItemViewModel>();
        }
        
        #region Properties
        private ObservableCollection<UserDocumentTestResultItemViewModel> _userDocumentTestResultItems;
        public ObservableCollection<UserDocumentTestResultItemViewModel> UserDocumentTestResultItems
        {
            get { return _userDocumentTestResultItems; }
            set
            {
                if (_userDocumentTestResultItems != value)
                {
                    _userDocumentTestResultItems = value;
                    OnPropertyChanged();
                }
            }
        }

        private UserDocumentTestResultItemViewModel _selectedUserDocumentTestResultItem;
        public UserDocumentTestResultItemViewModel SelectedUserDocumentTestResultItem
        {
            get { return _selectedUserDocumentTestResultItem; }
            set
            {
                if (_selectedUserDocumentTestResultItem != value)
                {
                    _selectedUserDocumentTestResultItem = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SelectedDocumentResultItem));
                }
            }
        }

        private ObservableCollection<DocumentItemViewModel> _documentItems;
        public ObservableCollection<DocumentItemViewModel> DocumentItems
        {
            get { return _documentItems; }
            set
            {
                if (_documentItems != value)
                {
                    _documentItems = value;
                    OnPropertyChanged();
                }
            }
        }

        private DocumentItemViewModel _selectedDocumentItem;
        public DocumentItemViewModel SelectedDocumentItem
        {
            get { return _selectedDocumentItem; }
            set
            {
                if (_selectedDocumentItem != value)
                {
                    _selectedDocumentItem = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedDocumentResultItem
        {
            get
            {
                return SelectedUserDocumentTestResultItem != null
                    ? SelectedUserDocumentTestResultItem.FileName
                    : "Select a document";
            }
        }
        #endregion

        #region ChooseDocuments
        private ICommand _chooseDocumentsCommand;
        public ICommand ChooseDocumentsCommand
        {
            get
            {
                if (_chooseDocumentsCommand == null)
                {
                    _chooseDocumentsCommand = new RelayCommand(
                        rc => this.CanChooseDocuments(),
                        rc => this.ChooseDocuments());
                }
                return _chooseDocumentsCommand;
            }
        }

        private bool CanChooseDocuments()
        {
            return true;
        }

        private void ChooseDocuments()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var fileName in openFileDialog.FileNames)
                {
                    if (DocumentItems.All(doc => doc.Path != fileName))
                    {
                        DocumentItems.Add(new DocumentItemViewModel(fileName));
                    }
                }
            }
        }
        #endregion

        #region RemoveDocuments
        private ICommand _removeDocumentsCommand;
        public ICommand RemoveDocumentsCommand
        {
            get
            {
                if (_removeDocumentsCommand == null)
                {
                    _removeDocumentsCommand = new RelayCommand(
                        rc => this.CanRemoveDocuments(),
                        rc => this.RemoveDocuments());
                }
                return _removeDocumentsCommand;
            }
        }

        private bool CanRemoveDocuments()
        {
            return DocumentItems != null && DocumentItems.Any(it => it.IsSelected);
        }

        private void RemoveDocuments()
        {
            var documents = DocumentItems.Where(it => it.IsSelected).ToList();
            foreach (var document in documents)
            {
                DocumentItems.Remove(document);
            }
        }
        #endregion

        #region OpenFolder
        private ICommand _openFolderCommand;
        public ICommand OpenFolderCommand
        {
            get
            {
                if (_openFolderCommand == null)
                {
                    _openFolderCommand = new RelayCommand(
                        rc => this.CanOpenFolder(),
                        rc => this.OpenFolder());
                }
                return _openFolderCommand;
            }
        }

        private bool CanOpenFolder()
        {
            return SelectedUserDocumentTestResultItem != null && !string.IsNullOrEmpty(SelectedUserDocumentTestResultItem.Path);
        }

        private void OpenFolder()
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo(SelectedUserDocumentTestResultItem.Path);
            Process.Start("explorer.exe", "/select, \"" + startInfo.FileName + "\"");
        }
        #endregion

        #region RunTest
        private ICommand _runTestCommand;
        public ICommand RunTestCommand
        {
            get
            {
                if (_runTestCommand == null)
                {
                    _runTestCommand = new RelayCommand(
                        rc => this.CanRunTest(),
                        rc => this.RunTest());
                }
                return _runTestCommand;
            }
        }

        private bool CanRunTest()
        {
            return DocumentItems != null && DocumentItems.Any();
        }

        private void RunTest()
        {
            UserDocumentTestResultItems = null;
            var documents = DocumentItems.Select(s => new FileInfo(s.Path)).ToArray();
            var provider = new CustomTestProvider(documents);
            var manager = new TestManager(provider, Callback);
            manager.RunTestsAsync();
        }

        public void Callback(TestCallback callback)
        {
            var documentItem = new UserDocumentTestResultItemViewModel(callback);

            if (UserDocumentTestResultItems == null)
            {
                UserDocumentTestResultItems = new ObservableCollection<UserDocumentTestResultItemViewModel>(new[] { documentItem });
            }
            else
            {
                UserDocumentTestResultItems.Add(documentItem);
            }
        }
        #endregion

        #region DeleteDocuments
        private ICommand _deleteDocumentsCommand;
        public ICommand DeleteDocumentsCommand
        {
            get
            {
                if (_deleteDocumentsCommand == null)
                {
                    _deleteDocumentsCommand = new RelayCommand(
                        rc => this.CanDeleteDocuments(),
                        rc => this.DeleteDocuments());
                }
                return _deleteDocumentsCommand;
            }
        }

        private bool CanDeleteDocuments()
        {
            return DocumentItems != null && DocumentItems.Any(it => it.IsSelected);
        }

        private void DeleteDocuments()
        {
            var items = DocumentItems.Where(it => it.IsSelected).ToList();
            foreach (var documentItem in items)
            {
                DocumentItems.Remove(documentItem);
            }
        }
        #endregion
    }
}
