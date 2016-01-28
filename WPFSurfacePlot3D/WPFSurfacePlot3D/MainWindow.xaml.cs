using System.Windows;

namespace WPFSurfacePlot3D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new SurfacePlotViewModel();
        }

        private void FileExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
