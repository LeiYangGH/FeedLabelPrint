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
        public const string PublicKey = @"MIIBKjCB4wYHKoZIzj0CATCB1wIBATAsBgcqhkjOPQEBAiEA/////wAAAAEAAAAAAAAAAAAAAAD///////////////8wWwQg/////wAAAAEAAAAAAAAAAAAAAAD///////////////wEIFrGNdiqOpPns+u9VXaYhrxlHQawzFOw9jvOPD4n0mBLAxUAxJ02CIbnBJNqZnjhE50mt4GffpAEIQNrF9Hy4SxCR/i85uVjpEDydwN9gS3rM6D0oTlF2JjClgIhAP////8AAAAA//////////+85vqtpxeehPO5ysL8YyVRAgEBA0IABDGJsdMdo84r0Hhiu3//74/cWlMRyvxv6A69dLx9hTfTmGL1iMGhIbS+Irgg3O1vwIdPO1yd9onjRSuhs/auRhc=";
        public const string MacAddress = "MacAddress";
        public const string TargetMacAddress = "C0F8DA7C373F";
        //02004C4F4F50 ly
    }
}
