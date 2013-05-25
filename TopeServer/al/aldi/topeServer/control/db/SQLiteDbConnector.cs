using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace TopeServer.al.aldi.topeServer.control.db
{

    class SQLiteDbConnector
    {
        public static String DB_TOPE_CONNECTION = "TopeServer.db";

        private String dbConnection = null;

        private SQLiteConnection connection = null;

        public SQLiteDbConnector()
        {
            dbConnection = DB_TOPE_CONNECTION;
        }

        public SQLiteDbConnector(String source)
        {
            dbConnection = source;
        }

        /// <summary>
        /// Can use it for create, update and delete
        /// </summary>
        /// <param name="nonQuery"></param>
        /// <returns></returns>
        public int executeNonQuery(String nonQuery)
        {
            openConnection();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = nonQuery;
            int retValue =  command.ExecuteNonQuery();

            closeConnection();
            return retValue;
        }

        public DataTable getDataTable(String query)
        {
            DataTable dt = new DataTable();
            openConnection();

            try
            {
                SQLiteCommand mycommand = new SQLiteCommand(connection);
                mycommand.CommandText = query;
                SQLiteDataReader reader = mycommand.ExecuteReader();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);// TODO: do smth else with the exception
            }

            closeConnection();
            return dt;
        }

        public String dbParamStringBuilder(Dictionary<String, String> paramOpts)
        {
            String str = "";
            foreach (KeyValuePair<String, String> row in paramOpts)
            {
                str += String.Format("{0}={1} ", row.Key, row.Value);
            }
            str = str.Trim().Substring(0, str.Length - 1);
            return str;
        }


        private void openConnection()
        {
            connection = new SQLiteConnection();
            connection.ConnectionString = new DbConnectionStringBuilder()
            {
                {"Data Source", dbConnection},
                {"Version", "3"},
                {"FailIfMissing", "False"},
            }.ConnectionString;
            connection.Open();
        }

        private void closeConnection()
        {
            connection.Close();
            connection.Dispose();
        }

    }
}
