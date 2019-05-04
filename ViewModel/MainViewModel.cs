using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace FeedLabelPrint.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
            }
            else
            {
                //this.CreateSampleData();

                this.ListBtwDirs();
            }
        }

        private void CreateSampleData()
        {
            this.ObsBtwDirs = new ObservableCollection<string>() { "º¶À«¡œ", "”„À«¡œ", "÷ÌÀ«¡œ" };
            this.ObsBtwFiles = new ObservableCollection<string>() { "111", "222", "333", "444", "555", "666", "777", "888", "999" };
        }

        private void ListBtwDirs()
        {
            string[] dirs = Directory.GetDirectories(Constants.btwTopDir);
            this.ObsBtwDirs = new ObservableCollection<string>(dirs);

        }

        private void ListBtwFilesInDir(string dir)
        {
            string[] files = Directory.GetFiles(dir);
            this.ObsBtwFiles = new ObservableCollection<string>(files);

        }

        private ObservableCollection<string> obsBtwDirs;
        public ObservableCollection<string> ObsBtwDirs
        {
            get
            {
                return this.obsBtwDirs;
            }
            set
            {
                if (this.obsBtwDirs != value)
                {
                    this.obsBtwDirs = value;
                    this.RaisePropertyChanged(nameof(ObsBtwDirs));
                }
            }
        }


        private string selectedBtwDir;
        public string SelectedBtwDir
        {
            get
            {
                return this.selectedBtwDir;
            }
            set
            {
                if (this.selectedBtwDir != value)
                {
                    this.selectedBtwDir = value;
                    this.ListBtwFilesInDir(value);
                    this.RaisePropertyChanged(nameof(SelectedBtwDir));
                }
            }
        }

        private ObservableCollection<string> obsBtwFiles;
        public ObservableCollection<string> ObsBtwFiles
        {
            get
            {
                return this.obsBtwFiles;
            }
            set
            {
                if (this.obsBtwFiles != value)
                {
                    this.obsBtwFiles = value;
                    this.RaisePropertyChanged(nameof(ObsBtwFiles));
                }
            }
        }


        private string selectedBtwFile;
        public string SelectedBtwFile
        {
            get
            {
                return this.selectedBtwFile;
            }
            set
            {
                if (this.selectedBtwFile != value)
                {
                    this.selectedBtwFile = value;
                    this.RaisePropertyChanged(nameof(SelectedBtwFile));
                }
            }
        }



        private string message;
        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                if (this.message != value)
                {
                    this.message = value;
                    this.RaisePropertyChanged(nameof(Message));
                }
            }
        }


        private bool isPrinting;
        private RelayCommand printCommand;

        public RelayCommand PrintCommand
        {
            get
            {
                return printCommand
                  ?? (printCommand = new RelayCommand(
                    async () =>
                    {
                        if (isPrinting)
                        {
                            return;
                        }

                        isPrinting = true;
                        PrintCommand.RaiseCanExecuteChanged();

                        await Print();

                        isPrinting = false;
                        PrintCommand.RaiseCanExecuteChanged();
                    },
                    () => !isPrinting));
            }
        }
        private async Task Print()
        {
            await Task.Run(() =>
            {
                string msg = BtwPrintWrapper.PrintBtwFile(this.SelectedBtwFile);
                this.Message = msg.Trim();
            });

        }

    }
}