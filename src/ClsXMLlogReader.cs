using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;



namespace WSJTX_DX_Alerter
{
    class ClsXMLlogReader
    {
        private string m_LogfilePath = "";
        public string LogfilePath {get => m_LogfilePath; set => m_LogfilePath = value; }

        private string m_LatestError = "";
        public string LatestError { get => m_LatestError; }

        /// <summary>
        /// reads the log table and gets unique DX entities worked
        /// </summary>
        /// <returns>list of unique entities</returns>
        /// <remarks>format of return value: entity string and an * if it is confirmed</remarks>
        public SortedList<string, char> GetLoggedEntities()
        {
			m_LatestError = "";
            DataTable myDataTable = new DataTable();
            SortedList<string, char> retVal = new SortedList<string, char>() { };
            //
            string sConn = @"Driver={Microsoft Access Driver (*.mdb)};DBQ=" + m_LogfilePath + ";Mode=Share Deny None";

            try
            {
                // Open ODBC Connection
                 OdbcConnection myConnection = new OdbcConnection{ ConnectionString = sConn };
                 myConnection.Open();

                // Execute Queries
                OdbcCommand cmd = myConnection.CreateCommand();
                cmd.CommandText = "SELECT CALL, COUNTRY, QSLRcvd, LOTWRCVD FROM MLog where PREFIX <> '?'";
                OdbcDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // close conn after complete

                // Load the result into a DataTable
                myDataTable.Load(reader);
                if (myDataTable.Rows.Count < 1)
                {
                    throw new Exception("No QSO rows in XMLog data table MLog.");
                }

                int iRowStart = myDataTable.Rows.Count - 1;
                string sTemp = "";
                char cSplat = ' ';
                for (int i = iRowStart; i >= 0; i--)
                {
                    sTemp = myDataTable.Rows[i][1].ToString().Trim();
                    if (myDataTable.Rows[i].Field<int>(2) > 0 || myDataTable.Rows[i].Field<int>(3) > 0)
                    {
                        cSplat = '*';
                    }
                    else
                    {
                        cSplat = ' ';
                    }
                    if (retVal.ContainsKey(sTemp))
                    {
                        //---- see if we can add a splat
                        if (cSplat == '*')
                        {
                            retVal[sTemp] = '*';
                        }
                    }
                    else
                    {
                        retVal.Add(sTemp, cSplat);
                    }
                }

            }
            catch (Exception ex)
            {
                m_LatestError = "ODBC Connection FAILED: " + ex.Message;
            }

            /* - for testing odd results only
            var results = from myRow in myDataTable.AsEnumerable()
                          where myRow.Field<int>(3) == 20171108
                          select myRow;
            */
            return retVal;
        }

    }  // end class

}  // end namespace
