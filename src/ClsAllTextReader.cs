using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Diagnostics;

namespace WSJTX_DX_Alerter
{
	class ClsAllTextReader
	{
		/********************** parameters ***************************/
		/// <summary>
		/// holds the path to the file
		/// </summary>
		private string m_FilePath = "";
		public string FilePath
		{ get { return m_FilePath; } set { m_FilePath = value; } }

		private string m_LatestError = "";
		public string LatestError { get => m_LatestError; }

		private int iThresholdValue = -99;
		public int ThresholdValue
		{
			set
			{
				iThresholdValue = value;
			}
		}

		/*********************** global vars ************************/
		List<string> lstCallSignsFound = new List<string>(); // knockout list of already checked call signs
		string sLastEntryString = ""; // scan backwards to cut off at last entry in previous scan
		string sCurrentFrequency = "";
		bool bDTfound = false;
		string sTemp = "", sTemp2 = "";
		int iTemp = 0;


		/********************** public methods **********************/

		FileStream OpenStream()
		{
			try
			{
				return new FileStream(m_FilePath + "\\all.txt", FileMode.Open);
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// pick out the last (x) number of call signs and return them
		/// </summary>
		/// <returns>a list of call signs</returns>
		public string[] ReadAllText()
		{
			string[] retVal = new string[] { };
			string[] splitArray2 = new string[] { };
			List<string> lstCallSigns = new List<string>(); // use a fresh list each time we scan
			char[] spaceSplitChar = new char[] { ' ' };

			FileStream oFS = null;
			int iCount = 0;

			while (oFS == null && iCount < 3)
			{
				try
				{
					iCount++;
					oFS = OpenStream();
					if (oFS == null)
					{
						Thread.Sleep(2000);
					}
				}
				catch (Exception e)
				{
					Debug.WriteLine(e.Message);
				}
			}
			if (iCount >= 3)
			{
				throw new Exception("could not read All.txt");
			}
			long iPosJump = oFS.Length - (100 * 48 * 2); // bytes to jump towards the end of file
			oFS.Seek(iPosJump, SeekOrigin.Begin); // 48 char average line times 100 lines UTF-8 about two bytes
			byte[] bytesIn = new byte[oFS.Length - iPosJump];
			oFS.Read(bytesIn, 0, bytesIn.Length);
			oFS.Close();
			oFS.Dispose();
			string[] splitArray = (System.Text.Encoding.UTF8.GetString(bytesIn)).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

			// now walk up the strings until finding the last string from previous scan
			int iStringsMax = splitArray.Length - 1;
			splitArray2 = splitArray[iStringsMax].Split(spaceSplitChar, StringSplitOptions.RemoveEmptyEntries);
			if (!splitArray2[0].Contains("_")) // checking for old version of WSJT-X
				throw new Exception("Wrong Version of WSJT-X. Must use Version 2.0 or above.");
			for (int iIndexer = iStringsMax; iIndexer > 0; iIndexer--) // splitArray[0] is likely a partial string so avoid it
			{
				sTemp = splitArray[iIndexer].Trim();
				if (sTemp == sLastEntryString) break; // all done for now
				sTemp = ParseAllString(sTemp);
				if (sTemp == "") continue;
				if (sTemp.StartsWith("ZZZZ"))
				{
					sTemp = sTemp.Substring(5).Trim();
					bDTfound = true;
					if (!lstCallSignsFound.Contains(sTemp))
					{
						lstCallSigns.Add(sTemp);
						lstCallSignsFound.Add(sTemp);
					}
					//continue;
				} // different day, or frequency/mode
				if (lstCallSignsFound.Contains(sTemp))
				{ continue; } // we've seen him before
				if (sTemp != "") // recheck for empty string
				{
					splitArray2 = splitArray[iIndexer].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
					sTemp2 = splitArray2[4];
					if (sTemp2[0] != '-') sTemp2 = "+" + sTemp2;
					// call sign plus time sent out
					lstCallSigns.Add(sTemp + " " + splitArray[iIndexer].Substring(7, 4) + " " + sTemp2); // these we send to be checked
					lstCallSignsFound.Add(sTemp); // this is a 'seen it already' filter
				}
			}
			sTemp = "";
			sLastEntryString = splitArray[splitArray.Length - 1]; // get the last entry to use as a cutoff on next scan
			if (!bDTfound) // we'll have to look farther back
			{
				StreamReader oSR = new StreamReader(m_FilePath + "\\all.txt", true);
				sTemp = oSR.ReadToEnd();
				oSR.Close();
				oSR.Dispose();
				splitArray = sTemp.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
				splitArray = splitArray.Reverse().ToArray();

				foreach (string s in splitArray)
				{
					if (s[4] == '-')
					{
						sTemp = s;
						break;
					}
				}
				if (sTemp != "")
				{
					lstCallSigns.Add(sTemp);
					bDTfound = true;
				}
			}
			lstCallSigns.Reverse();
#if xxDEBUG
            lstCallSigns.Add("7V7BC 1222 -13");
            lstCallSigns.Add("9Q4NC 1243 +1");
            lstCallSigns.Add("3D2XX 1559 -15");
#endif
			if (lstCallSigns.Count > 0)
			{
				retVal = new string[lstCallSigns.Count];
				lstCallSigns.CopyTo(retVal);
				lstCallSigns.Clear();
			}
			return retVal;
		}

		/************************* private methods ***************************/

		/// <summary>
		/// finds the call sign in the string and returns it
		/// </summary>
		/// <param name="source"></param>
		/// <returns>the callsign</returns>
		string ParseAllString(string source)
		{
			string retVal = "";
			string[] splitArray2 = source.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (splitArray2[1] != sCurrentFrequency) // it's a frequency change or newly started session
			{
				sCurrentFrequency = splitArray2[1];
				string sTemp = splitArray2[0] /*.Substring(7,4)*/ + " Freq = " + " " + splitArray2[1] + " MHz " + splitArray2[3];
				return "ZZZZ " + sTemp;
			}
			// check threshold of rx level
			if (!int.TryParse(splitArray2[4], out iTemp))
			{
				return retVal; // NaN or just not an integer
			}
			else
			{
				if (iTemp < iThresholdValue)
				{
					return retVal; // too low to work them
				}
			}
			// now pick out the call sign
			if (splitArray2.Length < 10)
				return retVal; // not a full qualified entry
			if (splitArray2[7].Trim() == "CQ")
			{
				if (splitArray2[8].Trim() == "DX")
				{
					retVal = splitArray2[9].Trim();
				}
				else
				{
					retVal = splitArray2[8].Trim();
				}
			}
			else
				retVal = splitArray2[8];
			// dump any that do not have a digit, might just be a comment like TNX or PLSE
			if (retVal.Any(char.IsDigit))
				return retVal;
			else
				return "";
		}

	}  // end class

}  // end namespace

/* types of text lines in all.txt
 * Mode change: 2017-11-24 20:57  21.074 MHz  FT8
 * cq call: 205715 -16  0.1 1521 ~  CQ K2PSD FN20
 * response call: 205830 -10  0.1 1679 ~  CE1URG KI4SFT EL88
 * signal report: 205745 -13  0.1  919 ~  K0ZRK N7BBX -15
 * signal reply: 205830 -16  0.1  883 ~  W1CAM AI6MQ R+02
 * signal verify: 205815  -8  0.1  919 ~  K0ZRK N7BBX RRR
 * freq change: 2017-11-25 01:10  10.136 MHz  FT8
 * me tx: 171125_012015  Transmitting 10.136 MHz  FT8:  VP8LP AD0QK EN34
 * mode change: 2017-12-09 13:28  7.0763 MHz  JT65
 * 
 * 050800  -4  0.8 2335 ~  UN7LZ YV5AJI RR73        
 * 050800  -4  1.1 2425 ~  E73Y NE3F R-09           
 * 050800 -11  1.2 2671 ~  E73Y F1HMR -05           
 * 050800 -23  0.8  619 ~  WF7B LX1TI RRR           
 * 050800 -15  0.8  972 ~  KE5SUE K5TA RRR          
 * 050800  -9  0.9 1075 ~  W1KOK N0WVU DM26         
 * 050800 -14  0.8 1038 ~  CQ KK6KC DM03            
 * 2018-01-14 04:31  7.074 MHz  FT8
 * 2018-01-14 04:31  7.0763 MHz  JT65
 * 2018-01-14 04:50  7.074 MHz  FT8
 */
