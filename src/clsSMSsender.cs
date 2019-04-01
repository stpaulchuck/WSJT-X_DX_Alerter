using System.ComponentModel;
using System.Windows.Forms;


namespace WSJTX_DX_Alerter
{
    class ClsSMSsender
    {
        // todo: implement the SMS sender class

        public bool SendSMS(string SMStext)
        {
            MessageBox.Show("SMS not implemented yet.", "Notice");

            /*
            // use a backgroundworker so it does not slow down the monitoring or change the sync
            BackgroundWorker oBW = new BackgroundWorker();
            oBW.DoWork += new DoWorkEventHandler
                (
                    delegate (object o, DoWorkEventArgs args)
                    {
                        try
                        {
                        }
                        catch (Exception j)
                        {
                            MessageBox.Show("Email send failed! msg = " + j.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    } // end delgate
                ); // end dowork
            oBW.RunWorkerAsync(); // ya gotta fire it up, duh
            */

            return true;  // dummy for testing
        }
    } // end class

} // end namespace
