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
            SurfacePlotViewModel viewModel = new SurfacePlotViewModel();
            propertyGrid.DataContext = viewModel;
            surfacePlotView.DataContext = viewModel;
        }

        private void FileExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
