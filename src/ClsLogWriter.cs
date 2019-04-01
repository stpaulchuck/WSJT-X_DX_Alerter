using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;


namespace WSJTX_DX_Alerter
{
    class ClsLogWriter
    {
        /************************ parameters ************************/
        string[] m_CallSignList = { };
        public string[] CallSignList
        { set => m_CallSignList = value; get { return m_CallSignList; } }

        /********************* constructors *************************/
        internal ClsLogWriter(string[] Callsigns)
        {
            m_CallSignList = Callsigns;
        }

        internal ClsLogWriter()
        {
        }

        /*********************** private functions ******************/


        /********************** public functions ********************/
        internal void WriteTheLog(string[] CallSigns)
        {
            m_CallSignList = CallSigns;
            WriteTheLog();
        }

        internal void WriteTheLog()
        {
            if (m_CallSignList.Length <= 0)
            { return; }

            // use backgroundworker so as not to slow down the main timer loop
            BackgroundWorker oBG = new BackgroundWorker();
            oBG.DoWork += new DoWorkEventHandler
            (
                delegate (object o, DoWorkEventArgs args)
                {
                    string sPath = Application.StartupPath + "\\EntitiesFound.log";
                    StreamWriter oStrWtr = new StreamWriter(sPath, true);
                    foreach (string s in m_CallSignList)
                    {
                        oStrWtr.WriteLine(s);
                    }
                    oStrWtr.Close();
                    oStrWtr.Dispose();
                }
            );
            oBG.RunWorkerAsync();

        }  // end writethelog()

    }  // end class

}  //  end namespace
