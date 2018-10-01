using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace WebAPI.Backend.Data
{
    public class SQL
    {
        private OleDbConnection Conn;
        private string DBase = "";
        public SQL(string DataBase)
        {
            DBase = DataBase;
            RestartConn();
        }

        private void RestartConn()
        {
            if (Conn != null) { if (Conn.State == System.Data.ConnectionState.Open) { Conn.Close(); } }
            Conn = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + DBase + ".accdb");
            Conn.Open();
        }

        public List<String[]> ExecuteReader(String sCommand)
        {
            OleDbCommand Command = new OleDbCommand(sCommand, Conn);
            OleDbDataReader Results = Command.ExecuteReader();
            List<String[]> LResults = new List<string[]> { };
            while (Results.Read())
            {
                string[] Data = new string[Results.FieldCount];
                for (int i = 0; i < Results.FieldCount; i++) { Data[i] = Results.GetValue(i).ToString(); }
                LResults.Add(Data);
            }
            Results.Close();
            return LResults;
        }

        public void Execute(String sCommand)
        {
            OleDbCommand Command = new OleDbCommand(sCommand, Conn);
            try { Command.ExecuteNonQuery(); RestartConn(); } catch (Exception E) { Console.WriteLine(E); }
        }
    }
}
