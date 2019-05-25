using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FeedLabelPrint
{
    public static class Constants
    {

        public const string FeedLabelPrint = "FeedLabelPrint";
        public static readonly string btwTopDir = @"C:\打印模板";
        public static readonly string AppDataFeedLabelPrintDir = Path.Combine(
         Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FeedLabelPrint);

        public static readonly string SqliteFileName = Path.Combine(AppDataFeedLabelPrintDir, "PrintHistory.db");
        public const string FieldProductType = "产品编码";
        public const string FieldPrintDate = "生产日期";
        public const string FieldStartNumber = "开始数";

    }
}
