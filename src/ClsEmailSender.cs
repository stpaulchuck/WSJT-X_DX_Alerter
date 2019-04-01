using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;


namespace WSJTX_DX_Alerter
{
    class ClsEmailSender
    {
        /*****************  global vars   *******************/
        byte[] KeyString = Encoding.UTF8.GetBytes("NoWis!@#Time4AllGOOd&*(M");
        byte[] IVstring = Encoding.UTF8.GetBytes("aNY#oldk@ey!W@ilLDOifyOU");


        /*****************  private methods   *******************/
        string decodeEncoded(string EncodedString)
        {
            string retVal = "ERROR";
            try
            {
                SymmetricAlgorithm oAlgorithm = TripleDES.Create();
                ICryptoTransform oTransform = oAlgorithm.CreateDecryptor(KeyString, IVstring);
                byte[] textToByte = Convert.FromBase64String(EncodedString);
                byte[] decrypted = oTransform.TransformFinalBlock(textToByte, 0, textToByte.Length);
                retVal = Encoding.Unicode.GetString(decrypted);
            }
            catch
            {
                MessageBox.Show("Error trying to decode string!", "ERROR");
                //oTimer.Stop();
                //oTimer.Enabled = false;
            }
            return retVal;
        }


        /*****************  public methods   *******************/
        public void SendTheEmail(string MessageToSend)
        {
            MailMessage email = new MailMessage();
            BackgroundWorker oBW = new BackgroundWorker();
            oBW.DoWork += new DoWorkEventHandler
                (
                    delegate (object o, DoWorkEventArgs args)
                    {
                        try
                        {
                            email.To.Add(decodeEncoded(Properties.Settings.Default.EmailTo));
                            email.From = new MailAddress(decodeEncoded(Properties.Settings.Default.EmailFrom));
                            email.Subject = "WSJT-X Message: " + DateTime.Now.ToLocalTime();
                            email.Body = MessageToSend;
                            //            email.Attachments.Add(new Attachment("D:\\snapshot1-1.jpg"));
                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = decodeEncoded(Properties.Settings.Default.SMTPserver);
                            smtp.Port = Properties.Settings.Default.SMTPport;
                            NetworkCredential cred = new NetworkCredential(decodeEncoded(Properties.Settings.Default.EmailUID), decodeEncoded(Properties.Settings.Default.EmailPWD));
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = cred;
                            smtp.Send(email);
                            email.Dispose();
                            smtp.Dispose();
                        }
                        catch (Exception j)
                        {
                            MessageBox.Show("Email send failed! msg = " + j.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    } // end delgate
                ); // end dowork
            oBW.RunWorkerAsync(); // ya gotta fire it up, duh
        } // end sendthemail

    } // end class
} // end namespace
