using Seagull.BarTender.Print;
using System;

namespace FeedLabelPrint
{
    public class BtwPrintWrapper
    {

        public static string PrintBtwFile(LabelFormatDocument updatedFormat, int printPages, Engine btEngine)
        {

            Messages messages;
            int waitForCompletionTimeout = 10000; // 10 seconds
            updatedFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
            updatedFormat.PrintSetup.NumberOfSerializedLabels = printPages;
            Result result = updatedFormat.Print("标签速打系统", waitForCompletionTimeout, out messages);
            string messageString = "\n\nMessages:";
            updatedFormat.PrintSetup.Cache.FlushInterval = 0;
            foreach (Seagull.BarTender.Print.Message message in messages)
            {
                messageString += "\n\n" + message.Text;
            }

            return messageString;

        }
    }
}
