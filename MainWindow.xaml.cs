using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EpicNum.Model;

namespace EpicNum
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region EpicFrame stuff
        public class AppinfoItem
        {
            public string AppName { get; set; }
            public string Credit { get; set; }
            public string AppVersion { get; set; }
        }

        public AppinfoItem EpicAppInfo;
        public static string EpicAppName = "Epic Num";
        public static string EpicAppCredit = "Edgars Miškins © Daina EL 2020";

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ContentControl MainContent = EpicFrame;
            EpicAppInfo = new AppinfoItem { AppName = EpicAppName, Credit = EpicAppCredit };
            MainContent.DataContext = EpicAppInfo;
            //Button BTN_Close = (Button)MainContent.Template.FindName("BTN_MainClose", MainContent);
            //BTN_Close.Click += new RoutedEventHandler(MainWindowClose);
            Grid MoveGrip = (Grid)MainContent.Template.FindName("MoveGrip", MainContent);
            MoveGrip.MouseDown += new MouseButtonEventHandler(Grid_MouseDown);

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void MainWindowClose(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Shutdown();
   
        }

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            VM = new ViewModel.EpicNumViewModel();
            this.DataContext = VM;
        }

        ViewModel.EpicNumViewModel VM;





    }
}
