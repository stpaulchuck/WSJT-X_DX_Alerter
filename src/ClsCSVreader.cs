using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace WSJTX_DX_Alerter
{
	public class ClsCSVreader
	{
		private string m_LogfilePath = "";
		public string LogfilePath { get => m_LogfilePath; set => m_LogfilePath = value; }

		private string m_LatestError = "";
		public string LatestError { get => m_LatestError; }

		/// <summary>
		/// reads the log data and gets unique DX entities worked
		/// </summary>
		/// <remark>
		/// the csv data columns must be call, country, qslReceived, lotwReceived; header is not needed
		/// </remark>
		/// <returns>list of unique entities</returns>
		/// <remarks>format of return value: entity string and an * if it is confirmed</remarks>
		public SortedList<string, char> GetCSVLoggedEntities()
		{
			m_LatestError = "";
			SortedList<string, char> retVal = new SortedList<string, char>() { };

			try
			{
				StreamReader reader = new StreamReader(m_LogfilePath, Encoding.UTF8);

				char[] splitchar = new char[] { ',' };
				string sTemp = "";
				char cSplat = ' ';
				string[] splitArray = null;
				while (reader.Peek() >= 0)
				{
					splitArray = reader.ReadLine().Split(splitchar);
					if (splitArray.Length < 4) continue;

					if (int.Parse(splitArray[2]) > 0 || int.Parse(splitArray[3]) > 0)
					{
						cSplat = '*';
					}
					else
					{
						cSplat = ' ';
					}
					sTemp = splitArray[1];
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
				m_LatestError = "CSV file read FAILED: " + ex.Message;
			}

			/* - for testing odd results only
            var results = from myRow in myDataTable.AsEnumerable()
                          where myRow.Field<int>(3) == 20171108
                          select myRow;
            */
			return retVal;
		}



	}  //  end class

}  // end namespace
