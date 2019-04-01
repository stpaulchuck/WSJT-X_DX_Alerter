using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace WSJTX_DX_Alerter
{
	public partial class FrmDXAlerterMain : Form
	{
		/***************      global vars        ***************/
		ClsEntityReader oEReader;
		ClsAllTextReader oATextReader;
		ClsXMLlogReader oXLogReader;
		ClsCSVreader oCsvReader;
		ClsSMSsender oSMSsender = new ClsSMSsender();
		ClsEmailSender oSendMail = new ClsEmailSender();
		ClsLogWriter oLog = new ClsLogWriter();
		System.Timers.Timer oTimer = new System.Timers.Timer();
		volatile bool bIsSyncing = false, bMBalltextConfirmed = true, bMBconfirmed = true;
		bool bXMLlogChanged = false, bAllTxtChanged = false, bQuietMode = false;
		bool bInitializing = true, bThresholdChanged = false;
		List<string> lstDXcallsigns = new List<string>();
		ToolTip oTT = new ToolTip
		{
			IsBalloon = false,
			ReshowDelay = 4000,
			InitialDelay = 2000,
		};
		WaveOut oWaveOut = null;
		WaveFileReader oWaveReader = null;
		int iThresholdCutoff = Properties.Settings.Default.ThresholdValue;

		/*****************  constructor   **********************/
		public FrmDXAlerterMain()
		{
			InitializeComponent();
			Application.DoEvents();
		}

		/*****************  private methods   *******************/

		private void PlaySound()
		{
			if (oWaveOut != null)
			{
				oWaveOut.Dispose();
			}
			oWaveOut = new WaveOut(this.Handle);
			oWaveReader = new WaveFileReader(Properties.Resources.AHOOGAx2);
			int iHowMany = WaveOut.DeviceCount;
			string sTemp = "";
			for (int i = 0; i < iHowMany; i++)
			{
				WaveOutCapabilities oCap = WaveOut.GetCapabilities(i);
				Debug.WriteLine(oCap.ProductName);
				sTemp = oCap.ProductName.ToLower();
				if (sTemp.Contains("speakers") && !sTemp.Contains("(usb "))
				{
					oWaveOut.DeviceNumber = i;
					break;
				}
			}
			oWaveOut.Init(oWaveReader);
			oWaveOut.Play();
		}

		private void DoAlerts(string[] sEntitiesFound)
		{
			// this is running in a background worker of the caller so no need to do a nested one

			if (sEntitiesFound.Length == 0) return;

			if (InvokeRequired)
			{
				MethodInvoker del = delegate { DoAlerts(sEntitiesFound); };
				this.Invoke(del);
				return;
			}

			bool bIsFreqChange = false; // used to prevent ahooga horn on just a freq change
			if (sEntitiesFound.Length == 1 && (sEntitiesFound[0].ToString().Trim()[6] == '_')) // fifth char in date time is a dash
			{
				bIsFreqChange = true;
			}

			string sTemp = "";
			foreach (string s in sEntitiesFound)
			{
				listBox1.Items.Add(s);
				sTemp += s + "\r\n";
			}
			if (sEntitiesFound.Length > 0)
				listBox1.SelectedIndex = listBox1.Items.Count - 1;

			if (!bIsFreqChange)
			{
				if (!bQuietMode) // then sound alert and show message box
				{
					// let the sound player play every time we have new DX stations
					PlaySound();
				}
				if (bMBconfirmed)
				{
					this.Focus();
					// use background worker thread for messagebox so method 
					// completes without waiting for MB to close
					BackgroundWorker oGB = new BackgroundWorker();
					// set the boolean and pop up the message box
					oGB.DoWork += new DoWorkEventHandler
					(
						 delegate (object o, DoWorkEventArgs args)
						 {
							 bMBconfirmed = false;
							 MessageBox.Show("Found new DX station!", "DX Alert");
						 }
					);
					oGB.RunWorkerCompleted += new RunWorkerCompletedEventHandler
					(
						 // can't happen until the messagebox is closed
						 delegate (object o, RunWorkerCompletedEventArgs args)
						 {
							 bMBconfirmed = true;
						 }
					);
					oGB.RunWorkerAsync();
				}
			}

			// send email if selected
			if (chkSendEmail.Checked)
			{
				oSendMail.SendTheEmail("Found new DX contact on WSJT-X!\r\n" + sTemp);
			}
			if (chkSendText.Checked)
			{
				oSMSsender.SendSMS("Found new DX contact on WSJT-X!\r\n" + sTemp);
			}
			if (chkLogHits.Checked)
			{
				oLog.WriteTheLog(sEntitiesFound);
			}
		}

		// needed this separate due to possible cross thread calls from Timer
		private void UpdateStatusLabel(string LabelText)
		{
			if (this.InvokeRequired)
			{
				MethodInvoker del = delegate { UpdateStatusLabel(LabelText); };
				this.Invoke(del);
				return;
			}
			// if on the same thread then just do the deed
			tsStatusLabel.Text = LabelText;
			Application.DoEvents();
		}

		private void ChangeSyncColor(string ColorName)
		{
			if (this.InvokeRequired)
			{
				MethodInvoker del = delegate { ChangeSyncColor(ColorName); };
				this.Invoke(del);
				return;
			}
			// if on the same thread then just do the deed
			if (ColorName.ToUpper() == "YELLOW")
			{
				lblRunStatus.BackColor = Color.Yellow;
				lblRunStatus.Text = "Syncing";
			}
			else
			{
				lblRunStatus.BackColor = Color.LimeGreen;
				lblRunStatus.Text = "Running";
			}
		}

		/*****************  event handlers   *******************/
		private void FrmDXAlerterMain_Load(object sender, EventArgs e)
		{
			Application.DoEvents();
			oTimer.Elapsed += OTimer_Elapsed;
			oTimer.Enabled = true;
			oTimer.Stop();
			string sTemp = Properties.Settings.Default.xmlLogLocation;
			if (sTemp != "")
			{
				txtXMLlogLocation.Text = sTemp;
			}
			sTemp = Properties.Settings.Default.alltxtLocation;
			if (sTemp != "")
			{
				txtDirPath.Text = sTemp;
			}
#if xxDEBUG
            //txtDirPath.Text = "..\\..";
#endif
			chkQuietMode.Checked = Properties.Settings.Default.QuietMode;
			chkAutoStart.Checked = Properties.Settings.Default.AutoStart;
			chkUnconfirmedAlert.Checked = Properties.Settings.Default.AlertUnconfirmed;
			chkSendText.Checked = Properties.Settings.Default.SendText;
			chkSendEmail.Checked = Properties.Settings.Default.SendEmail;
			txtThreshold.Text = iThresholdCutoff.ToString("0");
			bInitializing = false;
			Application.DoEvents();
			try
			{
				// read the various files and load them
				oATextReader = new ClsAllTextReader();
				oATextReader.ThresholdValue = iThresholdCutoff;
				if (File.Exists(txtDirPath.Text + "\\all.txt"))
				{
					oATextReader.FilePath = txtDirPath.Text;
					bAllTxtChanged = false;
				}
				else
				{
					bAllTxtChanged = true;
					throw new Exception(message: "ALL.TXT does not exist at path shown.");
				}
				// returns a dictionary of prefix and also a splat if it's verified by QSL or LOTW
				oXLogReader = new ClsXMLlogReader();
				oCsvReader = new ClsCSVreader();
				SortedList<string, char> slTemp = null;
				if (File.Exists(txtXMLlogLocation.Text))
				{
					bXMLlogChanged = false;
					if (txtXMLlogLocation.Text.ToLower().EndsWith(".csv"))
					{
						oCsvReader.LogfilePath = txtXMLlogLocation.Text;
						slTemp = oCsvReader.GetCSVLoggedEntities();
					}
					else
					{
						oXLogReader.LogfilePath = txtXMLlogLocation.Text;
						slTemp = oXLogReader.GetLoggedEntities();
					}
				}
				else
				{
					bXMLlogChanged = true;
					throw new Exception(message: "XML log file does not exist at path shown.");
				}
				// need the qso list or an empty dictionary to start the entity reader
				if (slTemp.Count < 1)
				{
					this.Focus();
					if (oXLogReader.LatestError.Contains("file already in use"))
					{
						MessageBox.Show(this, "XMLog file already in use. Close XMLog and then restart this program.");
					}
					else if (oXLogReader.LatestError != "")
					{
						MessageBox.Show(this, "Error reading XMLog log entries: " + oXLogReader.LatestError
							 + ". Fix the issue and then restart this program.");
					}
					if (oCsvReader.LatestError.Contains("file already in use"))
					{
						MessageBox.Show(this, "CS Log file already in use. Close CSV Log file and then restart this program.");
					}
					else if (oCsvReader.LatestError != "")
					{
						MessageBox.Show(this, "Error reading CSV Log entries: " + oCsvReader.LatestError + ". Fix the issue and then restart this program.");
					}
					this.Close();
				}
				oEReader = new ClsEntityReader(slTemp, chkUnconfirmedAlert.Checked);
				Application.DoEvents();
				if (bXMLlogChanged || bAllTxtChanged)
				{
					MessageBox.Show(this, "Locate " + (bXMLlogChanged ? "XMLlog" : "All.TXT") + " before starting!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					//return;
				}
				if (chkAutoStart.Checked)
				{
					btnRun.Text = "Stop";
					lblRunStatus.BackColor = Color.Yellow;
					lblRunStatus.Text = "Syncing";
					bIsSyncing = true;
					Application.DoEvents();
					oTimer.Interval = 1000;
					oTimer.Start();
				}
				else
				{
					UpdateStatusLabel("Idle... waiting Start command");
				}
				Application.DoEvents();
			}
			catch (Exception em)
			{
				MessageBox.Show(this, "Error on startup! " + em.Message + "\nFix the error then restart program.", "Fatal Error");
				//this.Close();
				return;
			}
			btnRun.Enabled = true;
		}

		private void OTimer_Elapsed(object sender, EventArgs e)
		{
#if DEBUG
            //Debug.WriteLine("Tick: " + DateTime.Now);
#endif

			// always check the sync, resync if needed
			int iSeconds = DateTime.Now.Second;
			bool bIsSynced = false;
			int iMod15Seconds = iSeconds % 15;
			int iMod30Seconds = iSeconds % 30;
			if (iSeconds >= 3)
			{
				if (rbFT8.Checked && (iMod15Seconds <= 12))
				{
					bIsSynced = true;
				}
				else if (rbMSK144.Checked && (iMod30Seconds <= 26))
				{
					bIsSynced = true;
				}
				else // one of the JT modes
				{
					if (iSeconds <= 54)
					{
						bIsSynced = true;
					}
				}
			}
			if (!bIsSynced)
			{
				if (!bIsSyncing)
				{
					ChangeSyncColor("Green");
					bIsSyncing = true;
					oTimer.Stop();
					oTimer.Interval = 1000;
					oTimer.Start();
					Application.DoEvents();
				}
#if xxDEBUG
                //UpdateStatusLabel("TimerTick() - OUT of sync: Seconds = " + iSeconds.ToString()
                //    + "   iMOD15Seconds = " + iMod15Seconds.ToString());
                Debug.WriteLine("Time = " + DateTime.Now +  " : Syncing, iSeconds = " + iSeconds.ToString() 
                    + "   iMOD15Seconds = " + iMod15Seconds.ToString());
#endif

				return;
			}
			if (bIsSyncing)
			{
				bIsSyncing = false;
				lblRunStatus.BackColor = Color.LimeGreen;
				ChangeSyncColor("Green");
				Application.DoEvents();
				oTimer.Stop();
				if (rbFT8.Checked)
				{
					oTimer.Interval = 15000; // 15 second heartbeat
				}
				else if (rbMSK144.Checked)
				{
					oTimer.Interval = 30000; // 30 seconds
				}
				else // one of the JT modes
				{
					oTimer.Interval = 60000; // 60 seconds
				}
				Application.DoEvents();
				oTimer.Start();
#if xxDEBUG
                Debug.WriteLine("Time = " + DateTime.Now + " : Synced, iSeconds = " + iSeconds.ToString()
                    + "   iMOD15Seconds = " + iMod15Seconds.ToString());
#endif
			}

#if xxDEBUG
            UpdateStatusLabel("TimerTick() - IN sync: Seconds = " + iSeconds.ToString()
                + "   iMOD15Seconds = " + iMod15Seconds.ToString());
#endif

			/*********************** get files and look for new DX hits **************************/

			//-------- rund this in it's own thread so the timer stays synced
			BackgroundWorker oGB = new BackgroundWorker();
			// set the boolean and pop up the message box
			oGB.DoWork += new DoWorkEventHandler
			(

				 delegate (object o, DoWorkEventArgs args)
				 {
					 string[] saTemp = new string[] { };
						 // occasionally the file is busy, if so then resync
						 try
					 {
						 saTemp = oATextReader.ReadAllText();
#if DEBUG
                        Debug.WriteLine("--- saTemp ---");
                        foreach (string s in saTemp)
                        {
                            Debug.WriteLine(s);
                        }
                        Debug.WriteLine("count = " + saTemp.Length.ToString() + "\n");
#endif

							 string[] sResults = oEReader.CheckForNewEntities(saTemp);
#if xxDEBUG
                        Debug.WriteLine("saTemp.Length = " + saTemp.Length.ToString() + "\n");
#endif
							 int iBadCount = 0;
						 Debug.WriteLine("new entities - ");
						 foreach (string s in sResults)
						 {
							 Debug.WriteLine("entity - " + s);
							 if (s.Length >= 4)
							 {
								 if (s[4] == '-') iBadCount++;
							 }
						 }
#if xxDEBUG
                        Debug.WriteLine("status label text = " + tsStatusLabel.Text + "\n");
#endif
						if (!tsStatusLabel.Text.Contains("TimerTick"))
						 {
							 UpdateStatusLabel("Found " + saTemp.Length.ToString("0") + " new callsigns "
									  + "and " + (sResults.Length - iBadCount).ToString("0") + " new entities.");
						 }
						 Application.DoEvents(); // make sure it posts before moving on.
														 // show alerts if we got some DX hits
							 if (sResults.Length > 0)
						 {
							 DoAlerts(sResults);
						 }
					 }
					 catch (Exception e1)
					 {
						 Debug.WriteLine("file read error - " + e1.Message);
						 if (e1.Message.ToLower().Contains("all.txt"))
						 {
							 bIsSyncing = true;
							 oTimer.Stop();
							 oTimer.Interval = 1000; // one second sync check rate
								 oTimer.Start();
							 ChangeSyncColor("Yellow");
							 Application.DoEvents();
						 }

						 if (bMBalltextConfirmed)
						 {
							 bMBalltextConfirmed = false;

								 // launch this on it's own thread
								 BackgroundWorker oBG2 = new BackgroundWorker();

							 oBG2.DoWork += new DoWorkEventHandler
								 (
									  delegate (object o2, DoWorkEventArgs args2)
									  {
										  MessageBox.Show("Error: " + e1.Message);
									  }
								 );

							 oBG2.RunWorkerCompleted += new RunWorkerCompletedEventHandler
								 (
									  delegate (object o3, RunWorkerCompletedEventArgs args3)
									  {
										  bMBalltextConfirmed = true;
									  }
								 );

							 oBG2.RunWorkerAsync();
						 }

					 }
					 return;
				 }  //  end dowork delegate
			);  //  end dowork handler

			oGB.RunWorkerCompleted += new RunWorkerCompletedEventHandler
			(
				 delegate (object o, RunWorkerCompletedEventArgs args)
				 {
					 bMBalltextConfirmed = true;
				 }
			);

			oGB.RunWorkerAsync();

		}

		private void BtnFindFile_Click(object sender, EventArgs e)
		{
			OpenFileDialog oDlg = new OpenFileDialog();
			if (Directory.Exists(txtDirPath.Text))
			{ oDlg.InitialDirectory = txtDirPath.Text; }
			else
			{ oDlg.InitialDirectory = Directory.GetCurrentDirectory(); }

			if (oDlg.ShowDialog() == DialogResult.OK)
			{
				// split the path from the file name
				string sPath = "", sFile = "";
				int iPos = oDlg.FileName.LastIndexOf('\\');
				if (iPos < 3)
				{
					MessageBox.Show("Bad Directory Path", "ALL.TXT");
					return;
				}
				else
				{
					sPath = oDlg.FileName.Substring(0, iPos);
					sFile = oDlg.FileName.Substring(iPos + 1);
				}
				if (sFile == "ALL.TXT")
				{
					txtDirPath.Text = sPath;
					oATextReader.FilePath = sPath;
					Properties.Settings.Default.alltxtLocation = sPath;
					Properties.Settings.Default.Save();
					bAllTxtChanged = false;
				}
				else
				{
					MessageBox.Show(this, "Wrong file name!", "ALL.TXT");
					return;
				}
			}
			else
			{
				MessageBox.Show(this, "No new location selected.", "Results");
			}
			oDlg.Dispose();
		}

		private void BtnFindXMLlog_Click(object sender, EventArgs e)
		{
			OpenFileDialog oDlg = new OpenFileDialog
			{
				InitialDirectory = Directory.GetCurrentDirectory()
			};

			if (oDlg.ShowDialog() == DialogResult.OK)
			{
				string sTemp = oDlg.FileName.ToLower();
				if (sTemp.EndsWith(".log") || sTemp.EndsWith(".mdb") || sTemp.EndsWith(".accdb"))
				{
					txtXMLlogLocation.Text = oDlg.FileName;
					oXLogReader.LogfilePath = oDlg.FileName;
					Properties.Settings.Default.xmlLogLocation = oDlg.FileName;
					Properties.Settings.Default.Save();
					bXMLlogChanged = false;
					try
					{
						if (oXLogReader.GetLoggedEntities().Count < 1)
						{ throw new Exception("no QSO's found in file"); }
					}
					catch (Exception e2)
					{
						string sErr = oXLogReader.LatestError;
						if (sErr.Length < 1) sErr = e2.Message;
						MessageBox.Show(this, "Error reading XML log. Try again.");
						return;
					}
				}
				else if (sTemp.EndsWith(".csv"))
				{
					txtXMLlogLocation.Text = oDlg.FileName;
					oCsvReader.LogfilePath = oDlg.FileName;
					Properties.Settings.Default.xmlLogLocation = oDlg.FileName;
					Properties.Settings.Default.Save();
					bXMLlogChanged = false;
					try
					{
						if (oCsvReader.GetCSVLoggedEntities().Count < 1)
						{ throw new Exception("no QSO's found in file"); }
					}
					catch (Exception e2)
					{
						string sErr = oCsvReader.LatestError;
						if (sErr.Length < 1) sErr = e2.Message;
						MessageBox.Show(this, "Error reading XML log. Try again.");
						return;
					}
				}
				else
				{
					MessageBox.Show(this, "Wrong file name! " + sTemp, "XML Log");
					return;
				}
			}
			else
			{
				MessageBox.Show(this, "No new location selected.", "Results");
			}
			oDlg.Dispose();
		}

		private void BtnRun_Click(object sender, EventArgs e)
		{
			if (bXMLlogChanged || bAllTxtChanged)
			{
				MessageBox.Show(this, "Locate " + (bXMLlogChanged ? "XMLlog" : "All.TXT") + " before starting!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			Button btn = (Button)sender;
			if (btn.Text == "Stop")
			{
				oTimer.Stop();
				btnRun.Text = "Run";
				lblRunStatus.Text = "Stopped";
				lblRunStatus.BackColor = Color.Red;
			}
			else // it's "Run"
			{
				btnRun.Text = "Stop";
				lblRunStatus.Text = "Syncing";
				lblRunStatus.BackColor = Color.Yellow;
				bIsSyncing = true;
				oTimer.Interval = 1000;
				Application.DoEvents();
				oTimer.Start();
				btnRun.Text = "Stop";
				lblRunStatus.Text = "Syncing";
				lblRunStatus.BackColor = Color.Yellow;
			}
		}

		private void RbMode_CheckedChanged(object sender, EventArgs e)
		{
			if (bIsSyncing || bInitializing) return;

			if (btnRun.Text == "Stop") // then it is running
			{
				oTimer.Stop();
				oTimer.Interval = 1000;
				oTimer.Start();
				lblRunStatus.Text = "Syncing";
				lblRunStatus.BackColor = Color.Yellow;
				bIsSyncing = true;
			}
		}

		private void BtnExit_Click(object sender, EventArgs e)
		{
			oTimer.Stop();
			Close();
		}

		private void chkAutoStart_CheckedChanged(object sender, EventArgs e)
		{
			if (bInitializing) return;
			Properties.Settings.Default.AutoStart = chkAutoStart.Checked;
			Properties.Settings.Default.Save();
		}

		private void chkUnconfirmedAlert_CheckedChanged(object sender, EventArgs e)
		{
			if (bInitializing) return;
			Properties.Settings.Default.AlertUnconfirmed = chkUnconfirmedAlert.Checked;
			Properties.Settings.Default.Save();
			// reread the entities list and retrim with myqso list
			oEReader.RetrimLists(chkUnconfirmedAlert.Checked);
		}

		private void chkSendEmail_CheckedChanged(object sender, EventArgs e)
		{
			if (bInitializing) return;
			Properties.Settings.Default.SendEmail = chkSendEmail.Checked;
			Properties.Settings.Default.Save();
		}

		private void groupBox1_MouseHover(object sender, EventArgs e)
		{
			oTT.Show("Select mode for proper timing.", groupBox1);
		}

		private void txtDirPath_TextChanged(object sender, EventArgs e)
		{
			if (bInitializing) return;
			bAllTxtChanged = true;
		}

		private void chkLogHits_CheckedChanged(object sender, EventArgs e)
		{
			if (bInitializing) return;
			Properties.Settings.Default.LogHits = chkLogHits.Checked;
			Properties.Settings.Default.Save();
		}

		private void tabControl1_Selected(object sender, TabControlEventArgs e)
		{
			if (bThresholdChanged)
			{
				int itemp = iThresholdCutoff;
				if (!int.TryParse(txtThreshold.Text, out iThresholdCutoff)) // something wrong with value
				{
					bThresholdChanged = false;
					MessageBox.Show(this, "Threshold cutoff value not a valid integer. Restoring previous value.");
					tabControl1.SelectedTab = tabPage2;
					txtThreshold.Text = itemp.ToString("0");
				}
				else
				{
					Properties.Settings.Default.ThresholdValue = iThresholdCutoff;
					oATextReader.ThresholdValue = iThresholdCutoff;
				}  //  end tryparse
			}  // end bthresholdchanged
		}  // end tabcontrol1 selected

		private void txtThreshold_TextChanged(object sender, EventArgs e)
		{
			bThresholdChanged = true;
		}

		private void btnClearList_Click(object sender, EventArgs e)
		{
			listBox1.Items.Clear();
		}

		private void chkQuietMode_CheckedChanged(object sender, EventArgs e)
		{
			if (bInitializing) return;
			bQuietMode = chkQuietMode.Checked;
			Properties.Settings.Default.QuietMode = bQuietMode;
			Properties.Settings.Default.Save();
		}

		private void txtXMLlogLocation_TextChanged(object sender, EventArgs e)
		{
			if (bInitializing) return;
			bXMLlogChanged = true;
		}

		private void chkSendText_CheckedChanged(object sender, EventArgs e)
		{
			if (bInitializing) return;
			Properties.Settings.Default.SendText = chkSendText.Checked;
			Properties.Settings.Default.Save();
		}

	}  // end form class
}  // end namespace
