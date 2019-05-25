using FeedLabelPrint.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Seagull.BarTender.Print;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
        private LabelOperator labelOperator;


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

                Task.Run(() => this.InitComponents());

            }
        }

        private void InitComponents()
        {
            if (!Directory.Exists(Constants.AppDataFeedLabelPrintDir))
                Directory.CreateDirectory(Constants.AppDataFeedLabelPrintDir);
            this.ListBtwDirs();
            this.BatchNumber = "";
            this.NeedDate = true;
            this.SelectedDate = DateTime.Today;
            this.PrintPages = 1;
            this.StartingNumber = 1;
            SqliteHistory.CreateDb();
            this.labelOperator = new LabelOperator(this.BtEngine);

        }



        private void ListBtwDirs()
        {
            string[] dirs = Directory.GetDirectories(Constants.btwTopDir);
            this.ObsBtwDirs = new ObservableCollection<string>(dirs);
        }

        private void ListBtwFilesInDir(string dir)
        {
            string[] files = Directory.GetFiles(dir, "*.btw");
            this.ObsBtwVMs = new ObservableCollection<BtwViewModel>(files.Select(f => new BtwViewModel(f)));
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

        private ObservableCollection<BtwViewModel> obsBtwVMs;
        public ObservableCollection<BtwViewModel> ObsBtwVMs
        {
            get
            {
                return this.obsBtwVMs;
            }
            set
            {
                if (this.obsBtwVMs != value)
                {
                    this.obsBtwVMs = value;
                    this.RaisePropertyChanged(nameof(ObsBtwVMs));
                }
            }
        }


        private BtwViewModel selectedBtwVM;
        public BtwViewModel SelectedBtwVM
        {
            get
            {
                return this.selectedBtwVM;
            }
            set
            {
                if (this.selectedBtwVM != value)
                {
                    this.selectedBtwVM = value;
                    this.RaisePropertyChanged(nameof(SelectedBtwVM));
                    this.UpdatePrintedCount();
                    if (this.SelectedBtwVM != null)
                        this.Message = $"��ǩ���ֶΣ�{string.Join(",", this.labelOperator.GetLabelFields(this.SelectedBtwVM.FullName))}";

                }
            }
        }


        private bool needDate;
        public bool NeedDate
        {
            get
            {
                return this.needDate;
            }
            set
            {
                if (this.needDate != value)
                {
                    this.needDate = value;
                    this.RaisePropertyChanged(nameof(NeedDate));
                }
            }
        }

        private DateTime selectedDate;
        public DateTime SelectedDate
        {
            get
            {
                return this.selectedDate;
            }
            set
            {
                if (this.selectedDate != value)
                {
                    this.selectedDate = value;
                    this.RaisePropertyChanged(nameof(SelectedDate));
                }
            }
        }


        //private string productType;
        //public string ProductType
        //{
        //    get
        //    {
        //        return this.productType;
        //    }
        //    set
        //    {
        //        if (this.productType != value)
        //        {
        //            this.productType = value;
        //            this.RaisePropertyChanged(nameof(ProductType));

        //        }
        //    }
        //}


        private string batchNumber;
        public string BatchNumber
        {
            get
            {
                return this.batchNumber;
            }
            set
            {
                if (this.batchNumber != value)
                {
                    this.batchNumber = value;
                    this.RaisePropertyChanged(nameof(BatchNumber));
                }
            }
        }


        private int startingNumber;
        public int StartingNumber
        {
            get
            {
                return this.startingNumber;
            }
            set
            {
                if (this.startingNumber != value)
                {
                    this.startingNumber = value;
                    this.RaisePropertyChanged(nameof(StartingNumber));
                }
            }
        }


        private int printPages;
        public int PrintPages
        {
            get
            {
                return this.printPages;
            }
            set
            {
                if (this.printPages != value)
                {
                    this.printPages = value;
                    this.RaisePropertyChanged(nameof(PrintPages));
                }
            }
        }


        private int printedCountOfCurrent;
        public int PrintedCountOfCurrent
        {
            get
            {
                return this.printedCountOfCurrent;
            }
            set
            {
                if (this.printedCountOfCurrent != value)
                {
                    this.printedCountOfCurrent = value;
                    this.RaisePropertyChanged(nameof(PrintedCountOfCurrent));
                }
            }
        }


        private int printedCountOfAll;
        public int PrintedCountOfAll
        {
            get
            {
                return this.printedCountOfAll;
            }
            set
            {
                if (this.printedCountOfAll != value)
                {
                    this.printedCountOfAll = value;
                    this.RaisePropertyChanged(nameof(PrintedCountOfAll));
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


        private LabelFormatDocument SetLabelValues(string file)
        {
            Log.Instance.Logger.Info($"��ʼ���ñ�ǩ�ֶ�ֵ,�ļ�{file}");
            LabelFormatDocument label = this.labelOperator.OpenLabel(file);
            string[] fieldsIn = this.labelOperator.GetLabelFields(file);
            Log.Instance.Logger.Info($"��ǩ�������ֶΣ�{string.Join(",", fieldsIn)}");
            this.Message = $"��ǩ�������ֶΣ�{string.Join(",", fieldsIn)}";
            if (fieldsIn.Contains(Constants.FieldPrintDate))
            {
                string dateValueString = "";
                if (this.NeedDate)
                    dateValueString = this.SelectedDate.ToShortDateString();
                label.SubStrings[Constants.FieldPrintDate].Value = dateValueString;
                Log.Instance.Logger.Info($"����{Constants.FieldPrintDate}={dateValueString}");
            }

            if (fieldsIn.Contains(Constants.FieldProductType))
            {
                label.SubStrings[Constants.FieldProductType].Value = this.BatchNumber.Trim();
                Log.Instance.Logger.Info($"����{Constants.FieldProductType}={this.BatchNumber.Trim()}");
            }


            if (fieldsIn.Contains(Constants.FieldStartNumber))
            {
                label.SubStrings[Constants.FieldStartNumber].Value = this.StartingNumber.ToString();
                Log.Instance.Logger.Info($"����{Constants.FieldStartNumber}={this.StartingNumber}");
            }




            Log.Instance.Logger.Info($"�������ñ�ǩ�ֶ�ֵ,�ļ�{file}");
            return label;
        }


        private void UpdatePrintedCount()
        {
            if (this.SelectedBtwVM != null && LabelOperator.isObjectExistingFile(this.SelectedBtwVM.FullName))
                this.PrintedCountOfCurrent = SqliteHistory.QueryTotalByLabel(this.SelectedBtwVM.FullName.Replace(
                        Constants.btwTopDir, ""));
            this.PrintedCountOfAll = SqliteHistory.QueryTotalAll();
        }

        private async Task Print()
        {

            await Task.Run(() =>
            {
                Log.Instance.Logger.Info($"������ӡ!");
                if (this.SelectedBtwVM == null)
                {
                    Log.Instance.Logger.Error($"δѡ���κ��ļ����˳���ӡ!");
                    return;
                }
                if (LabelOperator.isObjectExistingFile(this.SelectedBtwVM.FullName))
                {
                    if (this.BtEngine == null)
                    {
                        Log.Instance.Logger.Error($"bartenderδ��ȷ��ʼ�����޷���ӡ!");
                        return;
                    }
                    Log.Instance.Logger.Info($"׼����ӡ{this.SelectedBtwVM.FullName}!");

                    LabelFormatDocument label =
                    this.SetLabelValues(this.SelectedBtwVM.FullName);
                    if (this.SelectedBtwVM == null)
                    {
                        Log.Instance.Logger.Error($"δѡ���κ��ļ����˳���ӡ!");
                        return;
                    }
                    string BtwTemplate = this.SelectedBtwVM.FullName.Replace(
                        Constants.btwTopDir, "");
                    SqliteHistory.InsertPrintHistroy(BtwTemplate, this.PrintPages);
                    this.UpdatePrintedCount();
#if DEBUG
                    this.Message = "DEBUG������ʵ��ӡ";

#else
                    string msg = BtwPrintWrapper.PrintBtwFile(label, this.PrintPages, this.BtEngine);
                    this.Message = msg.Trim();
#endif
                }
                else
                {
                    Log.Instance.Logger.Error($"�޷���ӡ���ļ������ڣ�{this.SelectedBtwVM.FullName}!");
                }
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
                    {
                        this.m_engine.Stop(Seagull.BarTender.Print.SaveOptions.DoNotSaveChanges);
                        this.m_engine.Dispose();
                    }
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