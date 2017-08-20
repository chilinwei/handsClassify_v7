using System.Windows;
using System.Windows.Navigation;

namespace handsClassify_v7
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.NavigationService.LoadCompleted += NavigationService_LoadCompleted;
        }
        
        private void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (e.ExtraData != null)
            {
                MessageBox.Show(e.ExtraData.ToString());
            }
        }
    }
}
