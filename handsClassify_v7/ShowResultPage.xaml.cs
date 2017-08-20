using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace handsClassify_v7
{
    /// <summary>
    /// ShowResultPage.xaml 的互動邏輯
    /// </summary>
    public partial class ShowResultPage : Page
    {
        private List<exam> _examlst;
        private List<settings> _settingslst;

        SpeechSynthesizer speech = new SpeechSynthesizer();

        String[] result;
        string _ckind, camImg, xrayImg, nullImg, mainImg, subImg, _setTag;

        public ShowResultPage(List<exam> _em, List<settings> _st)
        {
            InitializeComponent();

            _examlst = _em;
            _settingslst = _st;

            //get result.txt value
            result = resultReader(_settingslst[0].linkfile);
            
            _ckind = _examlst[0].ckind;
            
            camImg = result[0];
            xrayImg = _settingslst[0].mainpicsfolder + _examlst[0].xrayimage;
            nullImg = _settingslst[0].mainpicsfolder + "null.jpg";

            imageDetection();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Jpg Files(*.jpg)|*.jpg";

            Nullable<bool> result = sfd.ShowDialog();
            string fileName = "result";

            if (result == true)
            {
                fileName = sfd.FileName;
                Size size = mainImage.RenderSize;
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
                mainImage.Measure(size);
                mainImage.Arrange(new Rect(size)); // This is important
                rtb.Render(mainImage);
                JpegBitmapEncoder jpg = new JpegBitmapEncoder();
                jpg.Frames.Add(BitmapFrame.Create(rtb));
                using (Stream stm = File.Create(fileName))
                {
                    jpg.Save(stm);
                }
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            SetCNoPage scpage = new SetCNoPage();
            this.NavigationService.Navigate(scpage);
        }

        private void btnSetLeft_Click(object sender, RoutedEventArgs e)
        {
            showImage(camImg, xrayImg, "L");
        }

        private void btnSetRight_Click(object sender, RoutedEventArgs e)
        {
            showImage(camImg, xrayImg, "R");
        }

        //read result.txt
        private String[] resultReader(String path)
        {
            string[] result;
            using (StreamReader sr = new StreamReader(path))
            {
                String line = sr.ReadToEnd();
                result = line.Replace("{", "").Replace("}", "").Split(',');
            }
            if (result.Length > 1)
            {
                result[1] = result[1].Replace(" ", "");
            }
            return result;
        }

        //get mathemetica result
        private void imageDetection()
        {
            if (result.Length == 2)
            {
                string _chkckind = result[1];

                if (_ckind == _chkckind)
                {
                    speech.Rate = -2;
                    speech.Volume = 100;
                    speech.SpeakAsync("正確，請執行");
                    mainImg = camImg;
                    subImg = xrayImg;
                    _setTag = _ckind;
                }
                else
                {
                    speech.Rate = -2;
                    speech.Volume = 100;
                    speech.SpeakAsync("與醫生開單不符，請重新確認");

                    mainImg = camImg;
                    subImg = nullImg;
                    _setTag = "";
                    //Button Visible
                    btnSetLeft.Visibility = Visibility.Visible;
                    btnSetRight.Visibility = Visibility.Visible;
                }
                showImage(mainImg, subImg, _setTag);
            }
            else
            {
                speech.Rate = -2;
                speech.Volume = 100;
                speech.SpeakAsync("尚未完成辨識，請重新啟動程序");
                MessageBox.Show("尚未完成辨識，請重新啟動程序");
                return;
            }
        }

        private void showImage(string mainImagePath, string subImagePath, string tag)
        {
            if (mainImagePath.Length == 0 || subImagePath.Length == 0)
            {
                return;
            }
            else
            {
                //drawing mainImage
                ImageSource mImg = drawingImage(new BitmapImage(new Uri(mainImagePath, UriKind.RelativeOrAbsolute)),
                    tag, Brushes.Black
                    );
                mainImage.Source = mImg;

                //drawing mainImage
                ImageSource sImg = drawingImage(new BitmapImage(new Uri(subImagePath, UriKind.RelativeOrAbsolute)),
                    tag, Brushes.White
                    );
                subImage.Source = sImg;
            }
        }

        //drawing image
        private ImageSource drawingImage(BitmapImage _img, string _tag, Brush _color)
        {
            BitmapImage bmp = _img;
            double h = bmp.Height;
            double w = bmp.Width;
            var visual = new DrawingVisual();
            DrawingContext drawingContext = visual.RenderOpen();
            drawingContext.DrawImage(bmp, new Rect(0, 0, w, h));
            drawingContext.DrawText(
                        new FormattedText(_tag, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                            new Typeface("Segoe UI"), 64, _color), new System.Windows.Point(0, 0));
            drawingContext.Close();
            var img = new DrawingImage(visual.Drawing);
            return img;
        }

        
    }
}
