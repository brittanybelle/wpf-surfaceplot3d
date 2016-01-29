using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFSurfacePlot3D
{
    /// <summary>
    /// Interaction logic for SurfacePlotView.xaml
    /// </summary>
    public partial class SurfacePlotView : UserControl
    {
        public SurfacePlotView()
        {
            InitializeComponent();
            DataContext = LayoutRoot.DataContext;
            hViewport.ZoomExtentsGesture = new KeyGesture(Key.Space);
        }

        #region String/Text Properties

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(SurfacePlotView), new FrameworkPropertyMetadata("Surface Plot Title"));

        public string XAxisLabel
        {
            get { return (string)GetValue(XAxisLabelProperty); }
            set { SetValue(XAxisLabelProperty, value); }
        }

        public static readonly DependencyProperty XAxisLabelProperty = DependencyProperty.Register("XAxisLabel", typeof(string), typeof(SurfacePlotView), new FrameworkPropertyMetadata("X Axis Label"));

        public string YAxisLabel
        {
            get { return (string)GetValue(YAxisLabelProperty); }
            set { SetValue(YAxisLabelProperty, value); }
        }

        public static readonly DependencyProperty YAxisLabelProperty = DependencyProperty.Register("YAxisLabel", typeof(string), typeof(SurfacePlotView), new FrameworkPropertyMetadata("Y Axis Label"));

        public string ZAxisLabel
        {
            get { return (string)GetValue(ZAxisLabelProperty); }
            set { SetValue(ZAxisLabelProperty, value); }
        }

        public static readonly DependencyProperty ZAxisLabelProperty = DependencyProperty.Register("ZAxisLabel", typeof(string), typeof(SurfacePlotView), new FrameworkPropertyMetadata("Z Axis Label"));

        #endregion

        #region Boolean Properties

        public bool ShowSurfaceMesh
        {
            get { return (bool)GetValue(ShowSurfaceMeshProperty); }
            set { SetValue(ShowSurfaceMeshProperty, value); }
        }

        public static readonly DependencyProperty ShowSurfaceMeshProperty = DependencyProperty.Register("ShowSurfaceMesh", typeof(bool), typeof(SurfacePlotView), new FrameworkPropertyMetadata(true));

        public bool ShowContourLines
        {
            get { return (bool)GetValue(ShowContourLinesProperty); }
            set { SetValue(ShowContourLinesProperty, value); }
        }

        public static readonly DependencyProperty ShowContourLinesProperty = DependencyProperty.Register("ShowContourLines", typeof(bool), typeof(SurfacePlotView), new FrameworkPropertyMetadata(true));

        public bool ShowMiniCoordinates
        {
            get { return (bool)GetValue(ShowMiniCoordinatesProperty); }
            set { SetValue(ShowMiniCoordinatesProperty, value); }
        }

        public static readonly DependencyProperty ShowMiniCoordinatesProperty = DependencyProperty.Register("ShowMiniCoordinates", typeof(bool), typeof(SurfacePlotView), new FrameworkPropertyMetadata(true));
        
        #endregion

        #region Double Properties

        public double XMin
        {
            get { return (double)GetValue(XMinProperty); }
            set { SetValue(XMinProperty, value); }
        }

        public static readonly DependencyProperty XMinProperty = DependencyProperty.Register("XMin", typeof(double), typeof(SurfacePlotView), new FrameworkPropertyMetadata(0.0));

        public double XMax
        {
            get { return (double)GetValue(XMaxProperty); }
            set { SetValue(XMaxProperty, value); }
        }

        public static readonly DependencyProperty XMaxProperty = DependencyProperty.Register("XMax", typeof(double), typeof(SurfacePlotView), new FrameworkPropertyMetadata(10.0));

        public double YMin
        {
            get { return (double)GetValue(YMinProperty); }
            set { SetValue(YMinProperty, value); }
        }

        public static readonly DependencyProperty YMinProperty = DependencyProperty.Register("YMin", typeof(double), typeof(SurfacePlotView), new FrameworkPropertyMetadata(0.0));

        public double YMax
        {
            get { return (double)GetValue(YMaxProperty); }
            set { SetValue(YMaxProperty, value); }
        }

        public static readonly DependencyProperty YMaxProperty = DependencyProperty.Register("YMax", typeof(double), typeof(SurfacePlotView), new FrameworkPropertyMetadata(10.0));

        public double ZMin
        {
            get { return (double)GetValue(ZMinProperty); }
            set { SetValue(ZMinProperty, value); }
        }

        public static readonly DependencyProperty ZMinProperty = DependencyProperty.Register("ZMin", typeof(double), typeof(SurfacePlotView), new FrameworkPropertyMetadata(0.0));

        public double ZMax
        {
            get { return (double)GetValue(ZMaxProperty); }
            set { SetValue(ZMaxProperty, value); }
        }

        public static readonly DependencyProperty ZMaxProperty = DependencyProperty.Register("ZMax", typeof(double), typeof(SurfacePlotView), new FrameworkPropertyMetadata(10.0));

        public double XTickInterval
        {
            get { return (double)GetValue(XTickIntervalProperty); }
            set { SetValue(XTickIntervalProperty, value); }
        }

        public static readonly DependencyProperty XTickIntervalProperty = DependencyProperty.Register("XTickInterval", typeof(double), typeof(SurfacePlotView), new FrameworkPropertyMetadata(1.0));

        public double YTickInterval
        {
            get { return (double)GetValue(YTickIntervalProperty); }
            set { SetValue(YTickIntervalProperty, value); }
        }

        public static readonly DependencyProperty YTickIntervalProperty = DependencyProperty.Register("YTickInterval", typeof(double), typeof(SurfacePlotView), new FrameworkPropertyMetadata(1.0));

        public double ZTickInterval
        {
            get { return (double)GetValue(ZTickIntervalProperty); }
            set { SetValue(ZTickIntervalProperty, value); }
        }

        public static readonly DependencyProperty ZTickIntervalProperty = DependencyProperty.Register("ZTickInterval", typeof(double), typeof(SurfacePlotView), new FrameworkPropertyMetadata(1.0));

        #endregion

    }
}
