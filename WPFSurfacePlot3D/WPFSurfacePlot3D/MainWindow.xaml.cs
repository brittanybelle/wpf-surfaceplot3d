using System;
using System.Windows;
using System.Windows.Documents;

namespace WPFSurfacePlot3D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml - all of the application logic lives here
    /// </summary>
    public partial class MainWindow : Window
    {
        private SurfacePlotViewModel viewModel;

        /// <summary>
        /// Initialize the main window (hence, this function runs on application start).
        /// You should initialize your SurfacePlotViewModel here, and set it as the
        /// DataContext for your SurfacePlotView (which is defined in MainWindow.xaml).
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Initialize surface plot objects
            viewModel = new SurfacePlotViewModel();
            propertyGrid.DataContext = viewModel;
            surfacePlotView.DataContext = viewModel;

            // Populate the functionSelectorComboBox
            functionSelectorComboBox.ItemsSource = Enum.GetValues(typeof(FunctionOptions));
        }

        /// <summary>
        /// Used to control which demo function the user has chosen to display.
        /// </summary>
        enum FunctionOptions { Sinc, Gaussian, Funnel, Origami, Simple };

        /// <summary>
        /// This function is called whenever the user selects a different demo function to plot.
        /// </summary>
        private void FunctionSelectionWasChanged(object sender, RoutedEventArgs e)
        {
            FunctionOptions currentOption = FunctionOptions.Simple;
            Func<double, double, double> function;

            if (functionSelectorComboBox.SelectedItem == null)
            {
                Console.WriteLine("No function selected");
            }
            else
            {
                currentOption = (FunctionOptions)functionSelectorComboBox.SelectedItem;
            }

            switch (currentOption)
            {
                case FunctionOptions.Gaussian:
                    function = (x, y) => 5 * Math.Exp(-1 * Math.Pow(x, 2) / 4 - Math.Pow(y, 2) / 4) / (Math.Sqrt(2 * Math.PI));
                    viewModel.PlotFunction(function, -5, 5);
                    break;

                case FunctionOptions.Sinc:
                    function = (x, y) => 10 * Math.Sin(Math.Sqrt(x * x + y * y)) / Math.Sqrt(x * x + y * y);
                    viewModel.PlotFunction(function, -10, 10);
                    break;
                    
                case FunctionOptions.Funnel:
                    function = (x, y) => -1 / (x * x + y * y);
                    viewModel.PlotFunction(function, -1, 1);
                    break;
                    
                case FunctionOptions.Origami:
                    function = (x, y) => Math.Cos(Math.Abs(x) + Math.Abs(y)) * (Math.Abs(x) + Math.Abs(y));
                    viewModel.PlotFunction(function, -1, 1);
                    break;

                case FunctionOptions.Simple:
                    function = (x, y) => x * y;
                    viewModel.PlotFunction(function, -1, 1);
                    break;

                default:
                    function = (x, y) => 0;
                    viewModel.PlotFunction(function, -1, 1);
                    break;
            }
        }

        private void FileExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
