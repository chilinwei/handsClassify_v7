using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace handsClassify_v7
{
    /// <summary>
    /// SetCNoPage.xaml 的互動邏輯
    /// </summary>
    public partial class SetCNoPage : Page
    {
        public SetCNoPage()
        {
            InitializeComponent();
            txtCNo.Focus();
            txtCNo.Text = "";
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string _input = txtCNo.Text.Trim();

            if (_input.Length == 0)
            {
                MessageBox.Show("請輸入診斷單單號");
            }
            else
            {
                exam _exam = new exam();
                List<exam> lst = _exam.GetCUData(_input);

                if (lst.Count == 0)
                {
                    MessageBox.Show("您輸入的診斷單單號不存在");
                }
                else
                {
                    OperatePage opage = new OperatePage(lst);
                    this.NavigationService.Navigate(opage);
                }
            }            
        }

            private void btnClear_Click(object sender, RoutedEventArgs e)
            {
                txtCNo.Focus();
                txtCNo.Text = "";
            }
        }
    }
