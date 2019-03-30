using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;


namespace Tribe_StreamReady
{
    public partial class Form_Main_Windows : Form
    {

        

        public Form_Main_Windows()
        {
            InitializeComponent();     


            progressBar1.Style = ProgressBarStyle.Continuous;
            
        }

        



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Install_all();
        }

        private void Install_all()
        {

            if (!Directory.Exists(@"C:\Users\" + Environment.UserName + @"\Documents\TribeXR"))
            {
                Directory.CreateDirectory(@"C:\Users\" + Environment.UserName + @"\Documents\TribeXR");
            }


            Set_Env();

            
            Install_FFMPEG();

            Install_YTDL();



        }


        [System.Runtime.InteropServices.ComVisible(true)]
        private void Set_Env()
        {

         StringBuilder sb = new StringBuilder();
         bool env_tribe_exist = false;

            foreach (DictionaryEntry de in Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine))
            {
                if (Convert.ToString(de.Key) == "Path")
                {
                    sb.AppendFormat(de.Value + "; ");

                    if (de.Value.ToString().Contains( @"C:\Users\" + Environment.UserName + @"\Documents\TribeXR"))
                    {
                        env_tribe_exist = true;
                    }
                }
            }

            if (!env_tribe_exist)
            {
                sb.AppendFormat(@" C:\Users\" + Environment.UserName + @"\Documents\TribeXR");
            }


            string env = sb.ToString();

            Environment.SetEnvironmentVariable("Path", env, EnvironmentVariableTarget.Machine);
            
        }

        private void Install_FFMPEG()
        {


            Uri uri = new Uri(Properties.Settings.Default.ffmpeg);

            if (File.Exists(@"C:\temp\" + "ffmpeg.zip"))
            {
                File.Delete(@"C:\temp\" + "ffmpeg.zip");
            }

            WebClient wc = new WebClient();
            wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)");
            wc.Headers.Add("Content-Type", "application / exe, application / octet - stream");
            wc.Headers.Add("Accept-Encoding", "exe");
            wc.Headers.Add("Referer", "http://google.at");
            wc.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            wc.DownloadFileAsync(uri, @"C:\temp\ffmpeg.zip");
            wc.DownloadProgressChanged += wc_DownloadProgressChanged;
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);   

        }


        static string directory = string.Format(@"C:\Users\"+ Environment.UserName +@"\Documents\TribeXR\");
        private string downloadname;

        private void Install_YTDL()
        {
            Uri uri = new Uri(Properties.Settings.Default.youtubedl);



            try
            {
                if (File.Exists(directory + "youtube-dl"))
                {
                    File.Delete(directory + "youtube-dl");
                }


                downloadname = "youtube-dl";
                
                WebClient wc_yt = new WebClient();
                wc_yt.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)");
                wc_yt.Headers.Add("Content-Type", "application / exe, application / octet - stream");
                wc_yt.Headers.Add("Accept-Encoding", "exe");
                wc_yt.Headers.Add("Referer", "http://google.at");
                wc_yt.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                wc_yt.DownloadFileAsync(uri, directory + "youtube-dl.exe");
                wc_yt.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_yt_DownloadProgressChanged);
                wc_yt.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_yt_DownloadFileCompleted);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void wc_yt_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            progressBar1.Visible = false;
            progressBar2.Visible = false;

            MessageBox.Show("Success you can now drop your beat into Streaming! Cheers!", "Installation Complete",
                MessageBoxButtons.OK, MessageBoxIcon.Information);


            DialogResult result = (MessageBox.Show("Do you want to restart now?", "Restart?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information));

            if (result == DialogResult.Yes)
            {
                var psi = new ProcessStartInfo("shutdown", "/r /t 0");
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                Process.Start(psi);
                this.Close();
            }

            if (result == DialogResult.No)
            {
                progressBar1.Visible = true;
                progressBar2.Visible = true;
            }
        }

        private void wc_yt_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar2.Value = e.ProgressPercentage;
            if (progressBar2.Value == progressBar2.Maximum)
            {
                progressBar2.Value = 0;
            }
        }

        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            

            progressBar1.Value = e.ProgressPercentage;
            if (progressBar1.Value == progressBar1.Maximum)
            {
                progressBar1.Value = 0;
            }
        }

        private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {

                

                ZipFile.ExtractToDirectory(@"C:\temp\ffmpeg.zip", @"C:\temp\");

                

                if(File.Exists(@"C:\Users\" + Environment.UserName + @"\Documents\TribeXR\ffmpeg.exe")) File.Delete(@"C:\Users\" + Environment.UserName + @"\Documents\TribeXR\ffmpeg.exe");
                if (File.Exists(@"C:\Users\" + Environment.UserName + @"\Documents\TribeXR\ffplay.exe")) File.Delete(@"C:\Users\" + Environment.UserName + @"\Documents\TribeXR\ffplay.exe");
                if (File.Exists(@"C:\Users\" + Environment.UserName + @"\Documents\TribeXR\ffprobe.exe")) File.Delete(@"C:\Users\" + Environment.UserName + @"\Documents\TribeXR\ffprobe.exe");


                File.Move(@"C:\temp\ffmpeg-4.1.1-win32-static\bin\ffmpeg.exe", @"C:\Users\" + Environment.UserName + @"\Documents\TribeXR\ffmpeg.exe");

                File.Move(@"C:\temp\ffmpeg-4.1.1-win32-static\bin\ffplay.exe", @"C:\Users\" + Environment.UserName + @"\Documents\TribeXR\ffplay.exe");

                File.Move(@"C:\temp\ffmpeg-4.1.1-win32-static\bin\ffprobe.exe", @"C:\Users\" + Environment.UserName + @"\Documents\TribeXR\ffprobe.exe");

                Directory.Delete(@"C:\temp\ffmpeg-4.1.1-win32-static\", true);
                File.Delete(@"C:\temp\ffmpeg.zip");



            }
            else
            {
                if (downloadname == "ffmpeg") MessageBox.Show("Unable to download zip file, please check your connection", "Download failed!");
            }

           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Install_FFMPEG();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Install_YTDL();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Set_Env();
        }

        private bool mouseDown;
        private Point lastLocation;

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void Update_Url()
        {
            throw new NotImplementedException();
        }
    }
}
