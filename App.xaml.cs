using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FeedLabelPrint
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private Mutex mutex;
        protected override void OnStartup(StartupEventArgs e)
        {

            //LicensingHelper.CreateLicense();
            bool isNewInstance = false;
            bool valid = true;
            mutex = new Mutex(true, Constants.FeedLabelPrint, out isNewInstance);
            if (!isNewInstance)
            {
                valid = false;

                MessageBox.Show("不能同时运行多个程序实例！");
                App.Current.Shutdown();
            }
            else
            {
                string licenseFile = "License.xml";
                if (!File.Exists(licenseFile))
                {
                    valid = false;

                    MessageBox.Show("没找到注册信息，请联系供应商！");
                    App.Current.Shutdown();
                }
                else
                {

                    var results_names = LicensingHelper.ValidateLicense(licenseFile)
                        .Select(r => r.GetType().Name).ToArray();

                    if (results_names.Any(r => r == "InvalidSignatureValidationFailure"))
                    {
                        valid = false;

                        MessageBox.Show("不能自行更改注册信息，请联系供应商！");
                        App.Current.Shutdown();
                    }
                    else if (results_names.Any(r => r == "InvalidMac"))
                    {
                        valid = false;

                        MessageBox.Show("只能在指定电脑使用，请联系供应商！");
                        App.Current.Shutdown();
                    }
                    else if (results_names.Any(r => r == "LicenseExpiredValidationFailure"))
                    {
                        valid = false;

                        MessageBox.Show("试用期结束，请联系供应商！");
                        App.Current.Shutdown();
                    }

                    else if (DateTime.Now < new DateTime(2019, 5, 26))
                    {
                        valid = false;

                        MessageBox.Show("您不能让时间倒流！");
                        App.Current.Shutdown();
                    }
                    else if (valid)
                        base.OnStartup(e);
                }


            }

        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (this.mutex != null)
                        this.mutex.Dispose();
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~App() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion

    }

}
