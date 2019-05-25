using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedLabelPrint.Model
{
    public static class SqliteHistory
    {
        private static string connStr = $"Data Source={Constants.SqliteFileName};Version=3;";
        public static void CreateDb()
        {
            if (!System.IO.File.Exists(Constants.SqliteFileName))
            {
                Log.Instance.Logger.Info($"开始创建条码打印纪录数据库文件:{Constants.SqliteFileName}.");
                try
                {
                    SQLiteConnection.CreateFile(Constants.SqliteFileName);

                    using (var conn = new SQLiteConnection(connStr))
                    {
                        conn.Open();
                        string sql = "create table barcodehistory (labelname TEXT, printcount INTEGER, printdate TEXT)";
                        SQLiteCommand command = new SQLiteCommand(sql, conn);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Log.Instance.Logger.Error($"创建条码打印纪录数据库文件出错:{ex.Message}");
                }

            }
        }

        public static void InsertPrintHistroy(string labelname, int printcount)
        {
            try
            {
                string sql = "insert into barcodehistory (labelname, printcount, printdate) values (@labelname, @printcount, @printdate)";
                using (var conn = new SQLiteConnection(connStr))
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@labelname", labelname);
                    cmd.Parameters.AddWithValue("@printcount", printcount);
                    cmd.Parameters.AddWithValue("@printdate", DateTime.Now.ToString());
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Log.Instance.Logger.Error($"添加条码打印纪录出错:{ex.Message}");
            }
        }

        public static int QueryTotalByLabel(string labelname)
        {
            string sql = "select sum(printcount) from barcodehistory where labelname = @labelname";
            try
            {
                using (var conn = new SQLiteConnection(connStr))
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@labelname", labelname);
                    conn.Open();
                    object re = cmd.ExecuteScalar();
                    return Convert.ToInt32(re);
                }
            }
            catch (Exception ex)
            {
                Log.Instance.Logger.Error($"sql={sql}出错:{ex.Message}");
                return 0;
            }
        }


        public static int QueryTotalAll()
        {
            string sql = $"select sum(printcount) from barcodehistory";
            try
            {
                using (var conn = new SQLiteConnection(connStr))
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = sql;
                    conn.Open();
                    object re = cmd.ExecuteScalar();
                    return Convert.ToInt32(re);
                }
            }
            catch (Exception ex)
            {
                Log.Instance.Logger.Error($"sql={sql}出错:{ex.Message}");
                return 0;
            }
        }

    }
}
