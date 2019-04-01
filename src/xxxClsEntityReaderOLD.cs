using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;

namespace WSJTX_DX_Alerter
{
    class ClsEntityReaderOLD
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
        { get { return bAlertNotVerified; } set { bAlertNotVerified = value; } }


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

            if (!sCallSign.Contains(' ')) return retVal;
            string[] splitArray = sCallSign.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (splitArray.Length < 3) return retVal;

            string sHitTime = splitArray[1] + "Z";
            string sTemp = splitArray[0]; // for holding just the callsign

            int iSpan = SpanSearch(0, slEntitySpansTrimmed.Count - 1, sTemp) - 900;
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
                int iSimple = SearchSimple(0, slEntitiesTrimmed.Count - 1, sTemp) - 900;
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
            return (retVal); // return the callsign if it's a new DX call
        }


        /************************* Entity searches: *************************
         * these are recursive, they return -899 for not found or 900 + index
         *  of hit. on a hit we dig up the entity entry for the index
         ********************************************************************/

            // todo: rewrite searches as loops instead of recursives!!

        // uses slEntitiesTrimmed list
        int SearchSimple(int LowIndex, int HighIndex, string CallSign)
        {
            string[] splitArray = new string[] { "", "" };  //  two empty strings
            int retVal = 899;
            int iSplit = HighIndex - LowIndex;
            bool bHasSlash = false, bSlashMatch = false;


            if (iSplit < 1)
            {
                return retVal;
            }
            if (iSplit == 1) // this covers the case where we have not yet compared to the HighIndex item
            {
                iSplit = 2;
            }
            iSplit = (iSplit / 2) + LowIndex;
            string sKey = slEntitiesTrimmed.Keys[iSplit].Trim();

            // keys with suffix requirement
            if (sKey.Contains("/"))
            {
                splitArray = sKey.Split(splitChar);
                bHasSlash = true;
                if (CallSign.EndsWith("/" + splitArray[1]))
                {
                    bSlashMatch = true;
                    sKey = splitArray[0];
                }
            }
            else
            {
                splitArray = new string[] { sKey };
            }

            // trim the call sign to same length as entity
            string sCompareString = CallSign;
            // check for prefix or suffix on callsign
            if (sCompareString.Contains("/"))
            {
                string[] splitArrayCS = sCompareString.Split(splitChar);
                sCompareString = splitArrayCS[0];
            }
            if (sCompareString.Length > splitArray[0].Length)
            {
                // trim the call sign to same length as filter start
                sCompareString = sCompareString.Substring(0, splitArray[0].Length);
            }

            int iResult = String.Compare(sCompareString, splitArray[0]);
            switch (iResult)
            {
                case 0:
                    if (bHasSlash == false || (bHasSlash && bSlashMatch))
                    {
                        return 900 + iSplit;
                    }
                    else if (bHasSlash && !bSlashMatch)
                    {
                        int iTemp = splitArray[1].Length;
                        // compare the end pieces and call this method with higher or lower
                        if (CallSign.Length >= iTemp)
                        {
                            iTemp = CallSign.Length - iTemp;
                            iTemp = string.Compare(CallSign.Substring(iTemp), splitArray[1]);
                        }
                        else
                        {
                            iTemp = string.Compare(CallSign, splitArray[1]);
                        }
                        if (iTemp < 0)
                        {
                            return SearchSimple(LowIndex, iSplit, CallSign);
                        }
                        else
                        {
                            return SearchSimple(iSplit, HighIndex, CallSign);
                        }
                    }
                    return 899;
                case -1:
                    if (iSplit == HighIndex)
                    { return 899; }
                    return SearchSimple(LowIndex, iSplit, CallSign);
                case 1:
                    if (iSplit == HighIndex)
                    { return 899; }
                    return SearchSimple(iSplit, HighIndex, CallSign);
                default:
                    return 899;
            } // ends switch

        }

        // uses slEntitySpansTrimmed list
        int SpanSearch(int LowIndex, int HighIndex, string CallSign)
        {
            bool bNumberMatch = false;
            int retVal = 899, iTemp = -99;
            int iSplit = HighIndex - LowIndex;
            if (iSplit < 1)
            {
                return retVal;
            }
            if (iSplit == 1)
            {
                iSplit = 2;
            }
            iSplit = (iSplit / 2) + LowIndex;
            string sKey = slEntitySpansTrimmed.Keys[iSplit];
            // get the left half of the span
            string[] splitArray = sKey.Split(new char[] { '-' }, StringSplitOptions.None);
            sKey = splitArray[0].Trim(); // start with the left half of the spread
            string sCompareString = "";
            if (CallSign.Length > sKey.Length)
            {
                // trim the call sign to same length as span start
                sCompareString = CallSign.Substring(0, sKey.Length);
            }
            else
            { sCompareString = CallSign; }
            // check to see if span ends in number
            retVal = 0; iTemp = 0; // preload
            if (int.TryParse(sKey.Substring(sKey.Length - 1), out retVal)) // number series
            {
                // see if callsign ends in # also
                if (int.TryParse(sCompareString.Substring(sCompareString.Length - 1), out iTemp))
                {
                    // are the numbers the same?
                    if (iTemp == retVal) // yes
                    {
                        // trim callsign and span ends
                        char[] cChop = new char[] { (char)retVal };
                        sCompareString = sCompareString.TrimEnd(cChop);
                        splitArray[0] = splitArray[0].Trim().TrimEnd(cChop);
                        splitArray[1] = splitArray[1].Trim().TrimEnd(cChop);
                    } // if numbers match
                } // if callsign prefix ends in number
            } // if span left ends on number
            if (iTemp == retVal)
            {
                bNumberMatch = true;
            }

            int iResult = String.Compare(sCompareString, sKey);
            switch (iResult)
            {
                case 0: // the prefix matches
                    if (bNumberMatch)
                    { return 900 + iSplit; }
                    if (iTemp < retVal) // callsign number < key number
                    {
                        return SpanSearch(LowIndex, iSplit, CallSign);
                    }
                    else
                    {
                        return SpanSearch(iSplit, HighIndex, CallSign);
                    }
                case -1:
                    if (iSplit == HighIndex)
                    { return 899; }
                    return SpanSearch(LowIndex, iSplit, CallSign);
            } // ends switch
            // now to test the high part of the span
            iResult = string.Compare(sCompareString, splitArray[1]);
            if (iResult <= 0) // its a hit on the prefix
            {
                if (bNumberMatch)
                { return 900 + iSplit; }
                if (iSplit == HighIndex)
                {
                    return 899;
                }
                if (iTemp < retVal) // callsign number < key number
                {
                    return SpanSearch(LowIndex, iSplit, CallSign);
                }
                else
                {
                    return SpanSearch(iSplit, HighIndex, CallSign);
                }
            }
            if (iSplit == HighIndex) // we were checking the end of array value
            {
                return 899;
            }
            else
            {
                return SpanSearch(iSplit, HighIndex, CallSign);
            }
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
                if (oKVP.Value != '*' && bAlertNotVerified == false) // if alert not confirmed
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
                    Debug.WriteLine("### this country not found: " + oKVP.Key);
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
                if (s[4] == '-')
                {
                    sGoodHits.Add(s);
                    continue;
                }
                sTemp = CheckIsNewDXentity(s);
                if (sTemp.Length > 0)
                {
                    sGoodHits.Add(sTemp);
                }
            }

            return sGoodHits.ToArray();
        }

        public void RetrimLists(bool AlertUnconfirmed = true)
        {
            // called when checkbox for alerting on unconfirmed qsos is changed
            TrimTheFiles();
        }

    }  // end class

}  // end namespace

