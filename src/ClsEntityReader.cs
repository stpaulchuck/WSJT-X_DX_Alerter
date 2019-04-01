using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;

namespace WSJTX_DX_Alerter
{
    class ClsEntityReader
    {
        /************************* global vars **************************/
        // base lists
        SortedList<string, string> slEntities = new SortedList<string, string>(); // <prefix, country>
        SortedList<string, string> slEntitySpans = new SortedList<string, string>(); // like: FM1-FT7; prefixes, country
        // working lists, after trim
        SortedList<string, string> slEntitiesTrimmed; // <prefix, country>
        SortedList<string, string> slEntitySpansTrimmed; // like: FM1-FT7; prefixes, country
        char[] splitChar = { '/' }; // split char


        /************************* parameters **************************/
        SortedList<string, char> m_MyQSOs; // saves having to pass it around
        public SortedList<string, char> MyQSOs
        { get => m_MyQSOs; set => m_MyQSOs = value; }

        bool bAlertNotVerified = false;
        public bool AlertNotVerified
        {
			get { return bAlertNotVerified; }
			set { bAlertNotVerified = value; }
		}


        /************************* constructors **************************/
        public ClsEntityReader()
        {
            if (m_MyQSOs.Count < 1)
            {
                MessageBox.Show("Must have at least one QSO in the XML log!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ReadTheFile();
            TrimTheFiles();
        }

        public ClsEntityReader(SortedList<string, char> MyQSOs, bool AlertUnconfirmed)
        {
            m_MyQSOs = MyQSOs;
            bAlertNotVerified = AlertUnconfirmed;
            ReadTheFile();
            TrimTheFiles();
        } // end method


        /************************* private methods *************************/
        string CheckIsNewDXentity(string sCallSign)
        {
            string retVal = ""; // empty string is 'no hit' 
            string sSimpleKey = "", sSpanKey = "";
            int iSpan = 0, iSimple = 0;

            if (!sCallSign.Contains(' '))
				return retVal;
            string[] splitArray = sCallSign.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (splitArray.Length < 3) return retVal;

            string sHitTime = splitArray[1] + "Z";
            string sTemp = splitArray[0]; // for holding just the callsign

            try
            {
                iSpan = SpanSearch(sTemp) - 900;
                if (iSpan >= 0)
                {
                    sSpanKey = slEntitySpansTrimmed.Keys[iSpan];
                    if (sSpanKey != "")
                    {
                        {
                            retVal = sTemp + " : " + sSpanKey + " - " + slEntitySpansTrimmed[sSpanKey] + "; "
                                + sHitTime + "; RX: " + splitArray[2];
                        }
                    }
                }
                else
                {
                    iSimple = SearchSimple(sTemp) - 900;
                    if (iSimple >= 0)
                    {
                        sSimpleKey = slEntitiesTrimmed.Keys[iSimple];
                        if (sSimpleKey != "")
                        {
                            retVal = sTemp + " : " + sSimpleKey + " - " + slEntitiesTrimmed[sSimpleKey] + "; "
                                + sHitTime + "; RX: " + splitArray[2];
                        }
                    }
                }
            }
            catch (Exception q)
            {
                Debug.WriteLine("checkisnewdxidentity error: " + q.Message);
            }
            return (retVal); // return the callsign if it's a new DX call
        }


        /************************* Entity searches: *************************
         * these are recursive, they return -899 for not found or 900 + index
         *  of hit. on a hit we dig up the entity entry for the index
         ********************************************************************/

        // uses slEntitiesTrimmed list
        int SearchSimple(string CallSign)
        {
            string[] splitArray = null;  //  two empty strings
            string sCallsignString = "", sKey = "";
            int retVal = 899, iKeyLen = 0, iCallLen = 0, iResult = 0;
            int iStop = slEntitiesTrimmed.Count - 1, iStart = 0, iSplit = 0;
            bool bCallHasSlash = false, bKeyHasSlash = false;

            while (iStart <= iStop)
            {
                iSplit = (iStop + iStart) / 2;
                
                sKey = slEntitiesTrimmed.Keys[iSplit].Trim().ToUpper();
                sCallsignString = CallSign.ToUpper();

                // keys with suffix requirement
                bCallHasSlash = false;
                bKeyHasSlash = false;
                if (sCallsignString.Contains("/"))
                {
                    bCallHasSlash = true;
                }
                if (sKey.Contains("/"))
                {
                    bKeyHasSlash = true;
                }
                if (bKeyHasSlash ^ bCallHasSlash) // only one has the slash
                {
                    if (bCallHasSlash)
                    {
                        sCallsignString = sCallsignString.Substring(0, sCallsignString.IndexOf('/'));
                    }
                    else
                    {
                        sKey = sKey.Substring(0, sKey.IndexOf('/'));
                    }
                    iCallLen = sCallsignString.Length;
                    iKeyLen = sKey.Length;
                    iCallLen = (iCallLen < iKeyLen ? iCallLen : iKeyLen);

                    iResult = string.Compare(sCallsignString.Substring(0,iCallLen), sKey.Substring(0,iCallLen));
                    switch (iResult)
                    {
                        case 0:
                            if (bKeyHasSlash)
                            {
                                iStop = iSplit - 1;
                            }
                            else
                            {
                                iStart = iSplit + 1;
                            }
                            continue;
                        case 1:
                            iStart = iSplit + 1;
                            continue;
                        case -1:
                            iStop = iSplit - 1;
                            continue;
                    }
                }

                // check for suffix match on callsign
                if (bCallHasSlash)
                {
                    splitArray = sKey.Split(splitChar);
                    if (sCallsignString.EndsWith("/" + splitArray[1]))
                    {
                        sKey = splitArray[0];
                    }
                    else
                    {
                        iResult = string.Compare(sCallsignString, sKey);
                        switch (iResult) // can never be equal
                        {
                            case 1:
                                iStart = iSplit + 1;
                                continue;
                            case -1:
                                iStop = iSplit - 1;
                                continue;
                        }
                    }
                    // split off just the callsign without the suffix
                    splitArray = sCallsignString.Split(splitChar);
                    sCallsignString = splitArray[0];
                }

                // trim the call sign to same length as entity
                iKeyLen = sKey.Length;
                iCallLen = sCallsignString.Length;
                if (iKeyLen > iCallLen) return 899;
                if (iCallLen > iKeyLen)
                {
                    // trim the call sign to same length as filter start
                    sCallsignString = sCallsignString.Substring(0, sKey.Length);
                }

                iResult = String.Compare(sCallsignString, sKey);
                switch (iResult)
                {
                    case 0:
                        retVal = 900 + iSplit;
                        iStart = iStop + 5;
                        break;
                    case -1:
                        if (iSplit == 0)
                        {
                            retVal = 899;
                            iStart = iStop + 5;
                            break;
                        }
                        iStop = iSplit -1;
                        break;
                    case 1:
                        if (iSplit == iStop)
                        {
                            retVal = 899;
                            iStart = iStop + 5;
                            break;
                        }
                        iStart = iSplit + 1;
                        break;
                } // ends switch
            }  //  end while()

            return retVal;
        }  // end SearchSimple()

        // uses slEntitySpansTrimmed list
        int SpanSearch(string CallSign)
        {
            bool bKeyHasNumber = false, bNumbersMatch = false;
            int iKeyNumber = 0, iCallNumber = 0;
            int retVal = -44, iResult = 0;
            int iStop = slEntitySpansTrimmed.Count -1, iStart = 0, iSplit = 0;
            string sKey = "", sCompareString = "", sCallsignString = "";
            string[] splitArray = null;
            char[] cDashSplit = new char[] { '-' };

            while (iStart <= iStop)
            {
                bKeyHasNumber = false; bNumbersMatch = false;

                iSplit = (iStop + iStart) / 2;
                if (iSplit < 0)
                {
                    break;
                }
                sKey = slEntitySpansTrimmed.Keys[iSplit];
                if (!sKey.Contains("-"))
                {
                    Debug.WriteLine("span key not right, no dash");
                    retVal = 899;
                    iStart = iStop + 5;
                    continue; ;
                }
                sCallsignString = CallSign;

                // get the left half of the span
                splitArray = sKey.Split(cDashSplit, StringSplitOptions.None);
                sKey = splitArray[0].Trim().ToUpper(); // start with the left half of the spread
                if (sCallsignString.Length > sKey.Length)
                {
                    // trim the call sign to same length as span start
                    sCompareString = sCallsignString.Substring(0, sKey.Length);
                }

                if (int.TryParse(sKey.Substring(sKey.Length - 1), out iKeyNumber))
                {
                    bKeyHasNumber = true;
                }
                else
                {
                    iKeyNumber = 7;
                }
                if (!int.TryParse(sCompareString.Substring(sCompareString.Length - 1), out iCallNumber))
                {
                    iCallNumber = 3;
                }
                if (iCallNumber == iKeyNumber)
                {
                    bNumbersMatch = true;
                }

                iResult = String.Compare(sCompareString, sKey);
                switch (iResult)
                {
                    case 0: // the prefix matches
                        if (bNumbersMatch || !bKeyHasNumber)
                        {
                            retVal = (900 + iSplit);
                            iStart = iStop + 5;
                            continue;
                        }
                        if (iCallNumber < iKeyNumber)
                        {
                            iStop = iSplit - 1;
                            continue;
                        }
                        else
                        {
                            iStart = iSplit + 1;
                            continue;
                        }
                    case -1:
                        iStop = iSplit - 1;
                        continue;
                    case 1:
                        if (!bNumbersMatch && bKeyHasNumber)
                        {
                            iStart = iSplit + 1;
                            continue;
                        }
                        break;
                        // if callsign is > key and numbers match or no numbers then fall through and see 
                        // if it's less than the right half of key
                } // ends switch


                // now to test the high part of the span
                sKey = splitArray[1];

                iResult = string.Compare(sCompareString, sKey);
                if (iResult <= 0) // its a hit on the prefix
                {
                    retVal = 900 + iSplit;
                    iStart = iStop + 5;
                    continue;
                }
                else
                {
                    iStart = iSplit + 1;
                    continue;
                }
            }
            return retVal;
        } // end spansearch()

        /*
EA-EH//Spain
EA6-EH6//Balearic Is.
EA8-EH8//Canary Is.
EA9-EH9//Ceuta & Melilla
EI-EJ//Ireland
UA1-UI1,UA3-UI3,UA4-UI4//European Russia
RA1-RI1,RA3-RI3,RA4-RI4//European Russia
UA2,RA2//Kaliningrad
UA8-UI8,UI9,UI0,RA8-RZ8,RZ9,RZ0//Asiatic Russia
         */

        void ReadTheFile()
        {
            char[] splitChar = new char[] { ',' };
            string[] splitString = { "][" };
            string sTemp = "";
            string[] splitArray = { }, splitArray3 = { };

            string sPath = Application.StartupPath;
            try
            {
                StreamReader oStream = new StreamReader(sPath + "\\DXCC entity list.txt");
                while (oStream.Peek() > -1)
                {
                    sTemp = oStream.ReadLine().Trim();
                    // strip off any empty lines or lines with headers, etc.
                    if (sTemp.StartsWith("Prefix", StringComparison.CurrentCulture))
                    { continue; }
                    if (!sTemp.Contains(splitString[0]))
                    { continue; }
                    if (sTemp.Length < 5)
                    { continue; }

                    splitArray = sTemp.Split(splitString, StringSplitOptions.None);

                    // now split any multiple prefixes pre country
                    splitArray3 = splitArray[0].Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string s in splitArray3)
                    {
                        if (s.Contains("-"))
                        {
                            if (slEntitySpans.ContainsKey(s))
                            { continue; }
                            slEntitySpans.Add(s, splitArray[1]);
                            continue;
                        }
                        if (slEntities.ContainsKey(s))
                        { continue; }

                        slEntities.Add(s, splitArray[1]);
                    } // end foreach
                } // end while

            }  // end try
            catch (Exception e)
            {
                throw new Exception(message: "Reading entity list failed: " + splitArray[0] + " - " + e.Message);
            } // end catch
        }

        void TrimTheFiles()
        {
            string sFoundCountry = ""; // holds the country string
            int iPtr = 0;

            // first load the trimmed sorted lists with copies of the entities lists
            slEntitiesTrimmed = new SortedList<string, string>(slEntities);
            slEntitySpansTrimmed = new SortedList<string, string>(slEntitySpans);
            SortedList<string, char> slMyQSOsTrimmed = new SortedList<string, char>(m_MyQSOs);
            List<int> lstQSOknockouts = new List<int>();

            // now play the knockout game using my qso's
            foreach (KeyValuePair<string, char> oKVP in slMyQSOsTrimmed)
            {
                if (oKVP.Value != '*' && bAlertNotVerified == true) // if alert not confirmed
                { continue; }
                // check the single value list for hits from my qso's
                if (slEntitiesTrimmed.ContainsValue(oKVP.Key)) // got one
                {
                    sFoundCountry = oKVP.Key;
                    while (slEntitiesTrimmed.ContainsValue(sFoundCountry)) // knock out any references to the country
                    {
                        iPtr = slEntitiesTrimmed.IndexOfValue(sFoundCountry);
                        slEntitiesTrimmed.RemoveAt(iPtr);
                    }
                    /*
                    while (slEntitySpansTrimmed.ContainsValue(sFoundCountry))
                    {
                        iPtr = slEntitySpansTrimmed.IndexOfValue(sFoundCountry);
                        slEntitySpansTrimmed.RemoveAt(iPtr);
                    }
                    */
                } // end if single list contains qso entity
                else
                {
                    //Debug.WriteLine("### this country not found in single list: " + oKVP.Key);
                }
            } // end foreach

            // knockout entries in span list from qso list
            foreach (KeyValuePair<string, char> oKVP in slMyQSOsTrimmed)
            {
                sFoundCountry = oKVP.Key;
                while (slEntitySpansTrimmed.ContainsValue(sFoundCountry))
                {
                    iPtr = slEntitySpansTrimmed.IndexOfValue(sFoundCountry);
                    slEntitySpansTrimmed.RemoveAt(iPtr);
                } // clear out any sisters in the list
            } // foreach kvp in qso

        } // end trim the files

        /************************* public methods **************************/
        /// <summary>
        /// checks if any of the new DX call signs are present
        /// </summary>
        /// <param name="CallSigns"></param>
        /// <returns>returns list of new entities if any</returns>
        public string[] CheckForNewEntities(string[] CallSigns)
        {
            string sTemp = "";
            List<string> sGoodHits = new List<string>();

            foreach (string s in CallSigns)
            {
                if (s.Length >= 4)
                {
                    if (s[6] == '_')
                    {
                        sGoodHits.Add(s);
                        continue;
                    }
                }
                try
                {
                    sTemp = CheckIsNewDXentity(s);
                }
                catch (Exception p)
                {
                    Debug.WriteLine("problem with callsign " + s + ": " + p.Message);
                }
                if (sTemp.Length > 0)
                {
                    sGoodHits.Add(sTemp);
                }
            }
            return (string[])(sGoodHits.ToArray());
        }

        public void RetrimLists(bool AlertUnconfirmed = true)
        {
            // called when checkbox for alerting on unconfirmed qsos is changed
            TrimTheFiles();
        }
    }  // end class

}  // end namespace
