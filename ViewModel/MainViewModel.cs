using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Seagull.BarTender.Print;
using System;
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
        private Engine m_engine = null;


        private Engine BtEngine
        {
            get
            {
                if (m_engine == null)
                {
                    m_engine = new Engine(true);
                }
                return m_engine;
            }
        }


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
            this.ObsBtwDirs = new ObservableCollection<string>() { "������", "������", "������" };
            this.ObsBtwFiles = new ObservableCollection<string>() { "111", "222", "333", "444", "555", "666", "777", "888", "999" };
        }

        private void ListBtwDirs()
        {
            string[] dirs = Directory.GetDirectories(Constants.btwTopDir);
            this.ObsBtwDirs = new ObservableCollection<string>(dirs);

        }

        private void ListBtwFilesInDir(string dir)
        {
            string[] files = Directory.GetFiles(dir, "*.btw");
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
                string msg = BtwPrintWrapper.PrintBtwFile(this.SelectedBtwFile, this.BtEngine);
                this.Message = msg.Trim();
            });

        }

        #region IDisposable Support
        private bool disposedValue = false; // Ҫ����������

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: �ͷ��й�״̬(�йܶ���)��
                    if (this.m_engine != null)
                        this.m_engine.Stop();
                    this.m_engine.Dispose();
                }

                // TODO: �ͷ�δ�йܵ���Դ(δ�йܵĶ���)������������������ս�����
                // TODO: �������ֶ�����Ϊ null��

                disposedValue = true;
            }
        }

        // TODO: �������� Dispose(bool disposing) ӵ�������ͷ�δ�й���Դ�Ĵ���ʱ������ս�����
        // ~MainViewModel() {
        //   // ������Ĵ˴��롣���������������� Dispose(bool disposing) �С�
        //   Dispose(false);
        // }

        // ��Ӵ˴�������ȷʵ�ֿɴ���ģʽ��
        public void Dispose()
        {
            // ������Ĵ˴��롣���������������� Dispose(bool disposing) �С�
            Dispose(true);
            // TODO: ���������������������ս�������ȡ��ע�������С�
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}