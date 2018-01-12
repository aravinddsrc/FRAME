using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace Utility
{
   public class ExcelValues
    {
        public static List<ColumnName> Column()
        {
            var p = AppDomain.CurrentDomain.BaseDirectory;
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);


            var path1 = p+@"\Data.xlsx";
            string connString;
            var extension = Path.GetExtension(path1).ToLower();
            var dt = new List<ColumnName>();

            if (extension == ".csv")
            {
                dt = Utility.ConvertCsVtoDataTable(path1);
            }
            else if (extension.Trim() == ".xls")
            {
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                dt = Utility.ConvertXslXtoDataTable(path1, connString);
            }
            else if (extension.Trim() == ".xlsx")
            {
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;HDR=NO;IMEX=2\"";
                dt = Utility.ConvertXslXtoDataTable(path1, connString);
            }
            return dt;
        }
    }

    public static class Utility
    {
        public static List<ColumnName> ConvertCsVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            List<ColumnName> ExcelList = new List<ColumnName>();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    if (rows.Length > 1)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }

            }


            return ExcelList;
        }

        public static List<ColumnName> ConvertXslXtoDataTable(string strFilePath, string connString)
        {
            var oledbConn = new OleDbConnection(connString);
            var excelList = new List<ColumnName>();
            try
            {
                oledbConn.Open();
                const string query = "select * from [Sheet1$A1:ZZ]";
                using (var cmd = new OleDbCommand(query, oledbConn))
                {
                    var oleda = new OleDbDataAdapter {SelectCommand = cmd};
                    var ds = new DataSet();
                    oleda.Fill(ds);
                    var dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        var name = row["Name"].ToString();
                        var number = row["Number"].ToString();
                        try
                        {
                            excelList.Add(new ColumnName { NameModel = name, NumberModel = number });
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2147467259)
                {
                    Console.WriteLine("Please close the Data.xlsx file");
                }

            }
            finally
            {
                oledbConn.Close();
            }
            return excelList;
        }
    }

    public class Item
    {
        public List<ColumnName> ExcelList { get; set; }
    }

    public class ColumnName
    {
        public string NameModel { get; set; }
        public string NumberModel { get; set; }


    }
}
