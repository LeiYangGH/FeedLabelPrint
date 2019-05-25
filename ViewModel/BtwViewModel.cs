using FeedLabelPrint.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Seagull.BarTender.Print;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FeedLabelPrint.ViewModel
{
    public class BtwViewModel : ViewModelBase
    {

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public BtwViewModel(string btwFile)
        {
            this.FullName = btwFile;
            this.FileNameWithoutExt = Path.GetFileNameWithoutExtension(btwFile);
        }


        private string fullName;
        public string FullName
        {
            get
            {
                return this.fullName;
            }
            set
            {
                if (this.fullName != value)
                {
                    this.fullName = value;
                    this.RaisePropertyChanged(nameof(FullName));
                }
            }
        }


        private string fileNameWithoutExt;
        public string FileNameWithoutExt
        {
            get
            {
                return this.fileNameWithoutExt;
            }
            set
            {
                if (this.fileNameWithoutExt != value)
                {
                    this.fileNameWithoutExt = value;
                    this.RaisePropertyChanged(nameof(FileNameWithoutExt));
                }
            }
        }


        private bool isExternalOpening;
        private RelayCommand externalOpenCommand;

        public RelayCommand ExternalOpenCommand
        {
            get
            {
                return externalOpenCommand
                  ?? (externalOpenCommand = new RelayCommand(
                    async () =>
                    {
                        if (isExternalOpening)
                        {
                            return;
                        }

                        isExternalOpening = true;
                        ExternalOpenCommand.RaiseCanExecuteChanged();

                        await ExternalOpen();

                        isExternalOpening = false;
                        ExternalOpenCommand.RaiseCanExecuteChanged();
                    },
                    () => !isExternalOpening));
            }
        }





        private async Task ExternalOpen()
        {
            await Task.Run(() =>
            {
                Log.Instance.Logger.Info($"开始直接打开{this.FullName}!");
                Process.Start(this.FullName);
                Log.Instance.Logger.Info($"结束直接打开{this.FullName}!");
            });
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    if (this != null)
                    {
                        this.Dispose();
                    }
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~MainViewModel() {
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