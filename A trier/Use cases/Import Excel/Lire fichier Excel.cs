//Installer le driver https://www.microsoft.com/en-us/download/details.aspx?id=23734

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;


namespace App
{
    static class DataReader
    {
        public static DataTable GetDtFromExcel(string filePath, string table)
        {
            using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Excel 12.0;HDR=YES;"))
            {
                using (OleDbDataAdapter adp = new OleDbDataAdapter("SELECT * FROM [" + table + "$]", conn))
                {
                    var ds = new DataSet();
                    adp.Fill(ds);
                    return ds.Tables[0];
                }
            }
        }
	}
}