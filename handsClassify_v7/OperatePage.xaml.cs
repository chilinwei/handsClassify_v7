using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Speech.Synthesis;
using System.Windows.Threading;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.Structure;

namespace handsClassify_v7
{
    /// <summary>
    /// OperatePage.xaml 的互動邏輯
    /// </summary>
    public partial class OperatePage : Page
    {
        private List<exam> _examlst;
        private List<settings> _settingslst;

        SpeechSynthesizer speech = new SpeechSynthesizer();

        private Capture _capture = null; //Camera
        private string savefolder;
        private string linkfile;
        private string filename;
        private string mainpicsfolder;

        //define CV ROI & draw rectangle on frame
        private Rectangle roiRect = new Rectangle(170, 40, 300, 400);
        private Bgr color = new Bgr(System.Drawing.Color.Red);

        //single thread
        DispatcherTimer timer;

        public OperatePage(List<exam> lst)
        {
            InitializeComponent();
            _examlst = lst;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //get setting from settings.xml
            settings _settings = new settings();
            _settingslst = _settings.GetSettings();
            var camtype = _settingslst[0].camtype;
            savefolder = _settingslst[0].savefolder;
            linkfile = _settingslst[0].linkfile;
            mainpicsfolder = _settingslst[0].mainpicsfolder;

            //Initialize result.txt
            File.WriteAllText(Path.GetFullPath(linkfile), "");

            //define captrue resource
            if (camtype == "ipcam")
            {
                _capture = new Capture(_settingslst[0].camdevice.ToString());
            }
            else if (camtype == "cam")
            {
                _capture = new Capture(Int16.Parse(_settingslst[0].camdevice));
            }

            //single thread use timer to display frame
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer.Start();

            //show exam data on human body picture
            ExamDataShow(_examlst);
            //speeech information
            ExamDataSpeech(_examlst);
            //import currect picture
            ExamMainImageChange(_examlst);
        }

        //get camera image frame
        private void timer_Tick(object sender, EventArgs e)
        {
            Mat frame = _capture.QueryFrame();

            if (frame != null)
            {
                Image<Bgr, Byte> bgrFrame = frame.ToImage<Bgr, Byte>();
                bgrFrame.Draw(roiRect, color, 3);
                imgFrame.Source = ToBitmapSource(bgrFrame);
            }
        }

        //Run detect process
        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            //capture image & save
            imageCatch(savefolder);
            //dispose camera
            _capture.Dispose();
            //show progressbar
            labMsg.Content = filename + " 影像識別中...";
            gridMain.Visibility = Visibility.Collapsed;
            gridSub.Visibility = Visibility.Visible;
            delayGoForward(3000);
        }

        //return GetExamDataPage
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            //dispose camera
            _capture.Dispose();
            SetCNoPage scpage = new SetCNoPage();
            this.NavigationService.Navigate(scpage);
        }

        //display exam data
        private void ExamDataShow(List<exam> lst)
        {
            labName.Content = lst[0].name;
            labAge.Content = lst[0].age + " 歲";
            labSex.Content = lst[0].sex_text;
            labCNo.Content = lst[0].cno;
            labCpart.Content = lst[0].cpart_text;
            labCKind.Content = lst[0].ckind_text;
        }

        //speeech information
        private void ExamDataSpeech(List<exam> lst)
        {
            speech.Rate = -2;
            speech.Volume = 100;
            speech.SpeakAsync("姓名" + lst[0].name);
            speech.SpeakAsync("年齡" + lst[0].age + "歲");
            speech.SpeakAsync("性別" + lst[0].sex_text);
            speech.SpeakAsync("檢查項目" + lst[0].cpart_text);
            speech.SpeakAsync("檢查方向" + lst[0].ckind_text);
        }

        //import currect picture
        private void ExamMainImageChange(List<exam> lst)
        {
            string _filename = mainpicsfolder + lst[0].mainimage;
            mediaElement.Source = new Uri(_filename, UriKind.RelativeOrAbsolute);
        }

        //capture image & save
        private void imageCatch(string path)
        {
            filename = _examlst[0].cno + "_" + DateTime.Now.ToString("yyyyMMddhhmmssms") + ".jpg";
            String filepath = Path.GetFullPath(path + filename);
            //MessageBox.Show(filename + " saved.");

            Mat frame = _capture.QueryFrame();
            if (frame != null)
            {
                Image<Bgr, Byte> bgrFrame = frame.ToImage<Bgr, Byte>();
                bgrFrame.ROI = Rectangle.Empty;
                Rectangle roi = roiRect; // set the roi
                bgrFrame.ROI = roi;
                bgrFrame.Save(filepath);
                File.WriteAllText(linkfile, filepath);
            }
        }

        private async void delayGoForward(int _sec)
        {
            await Task.Delay(_sec);
            ShowResultPage srpage = new ShowResultPage(_examlst, _settingslst);
            this.NavigationService.Navigate(srpage);
        }


        //convert EmguCV image format into BitmapSource
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        public static BitmapSource ToBitmapSource(IImage image)
        {
            using (System.Drawing.Bitmap source = image.Bitmap)
            {
                IntPtr ptr = source.GetHbitmap(); //obtain the Hbitmap

                BitmapSource bs = System.Windows.Interop
                  .Imaging.CreateBitmapSourceFromHBitmap(
                  ptr,
                  IntPtr.Zero,
                  Int32Rect.Empty,
                  System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(ptr); //release the HBitmap
                return bs;
            }
        }
    }
}
