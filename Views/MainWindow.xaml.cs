using FeedLabelPrint.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FeedLabelPrint.Views
{
    public partial class MainWindow : Window
    {
        public static MainWindow mainWin;
        private MainViewModel mainVM;

        public MainWindow()
        {
            InitializeComponent();
            this.mainVM = this.DataContext as MainViewModel;
            mainWin = this;
            this.ShowVersion();
            Log.Instance.Logger.Debug("\r\nend of MainWindow()!");
        }

        private void ShowVersion()
        {
            string version = System.Reflection.Assembly.GetExecutingAssembly()
                                           .GetName()
                                           .Version
                                           .ToString();
            this.Title += " -" + version;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mainVM != null)
                mainVM.Dispose();
            Log.Instance.Logger.Debug("\r\nend of Window_Closing()!");
        }
    }
}
