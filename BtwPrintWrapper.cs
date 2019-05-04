using Seagull.BarTender.Print;

namespace FeedLabelPrint
{
    public class BtwPrintWrapper
    {
        public static string PrintBtwFile(string btwFile)
        {
            Engine engine = new Engine(true);
            Messages messages;
            LabelFormatDocument format = engine.Documents.Open(btwFile);
            int waitForCompletionTimeout = 10000; // 10 seconds
            Result result = format.Print("测试打印" + System.IO.Path.GetFileName(btwFile), waitForCompletionTimeout, out messages);
            string messageString = "\n\nMessages:";

            foreach (Seagull.BarTender.Print.Message message in messages)
            {
                messageString += "\n\n" + message.Text;
            }
            return messageString;
        }
    }
}
