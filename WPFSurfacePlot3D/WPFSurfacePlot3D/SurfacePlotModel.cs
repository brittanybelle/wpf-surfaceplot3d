using HelixToolkit.Wpf;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WPFSurfacePlot3D
{
    public enum ColorCoding
    {
        /// <summary>
        /// No color coding, use coloured lights
        /// </summary>
        ByLights,

        /// <summary>
        /// Color code by gradient in y-direction using a gradient brush with white ambient light
        /// </summary>
        ByGradientY
    }

    class SurfacePlotModel : INotifyPropertyChanged
    {
        private int defaultFunctionSampleSize = 100;

        // So the overall goal of this section is to output the appropriate values to SurfacePlotVisual3D - namely,
        // - DataPoints as Point3D, plus xAxisTicks (and y, z) as double[]
        // - plus all the appropriate properties, which can be directly edited/bindable by the user

        public SurfacePlotModel()
        {
            Title = "New Surface Plot";
            XAxisLabel = "x-Axis";
            YAxisLabel = "y-Axis";
            ZAxisLabel = "z-Axis";

            ColorCoding = ColorCoding.ByLights;

            // Initialize the DataPoints collection
            Func<double, double, double> sampleFunction = (x, y) => 10 * Math.Sin(Math.Sqrt(x * x + y * y)) / Math.Sqrt(x * x + y * y);
            PlotFunction(sampleFunction, -10, 10);
        }

        #region === Public Methods ===


        public void PlotData(double[,] zData2DArray)
        {
            int n = zData2DArray.GetLength(0);
            int m = zData2DArray.GetLength(1);
            Point3D[,] newDataArray = new Point3D[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Point3D point = new Point3D(i, j, zData2DArray[i, j]);
                    newDataArray[i, j] = point;
                }
            }
            dataPoints = newDataArray;
            RaisePropertyChanged("DataPoints");
        }

        public void PlotData(double[,] zData2DArray, double xMinimum, double xMaximum, double yMinimum, double yMaximum)
        {

        }

        public void PlotData(double[,] zData2DArray, double[] xArray, double[] yArray)
        {
            // Note - check that dimensions match!!


        }

        public void PlotData(Point3D[,] point3DArray)
        {
            // Directly plot from a Point3D array
        }

        public void PlotFunction(Func<double, double, double> function)
        {
            PlotFunction(function, -1, 1, -1, 1, defaultFunctionSampleSize, defaultFunctionSampleSize);
        }

        public void PlotFunction(Func<double, double, double> function, double minimumXY, double maximumXY)
        {
            PlotFunction(function, minimumXY, maximumXY, minimumXY, maximumXY, defaultFunctionSampleSize, defaultFunctionSampleSize);
        }

        public void PlotFunction(Func<double, double, double> function, double minimumXY, double maximumXY, int sampleSize)
        {
            PlotFunction(function, minimumXY, maximumXY, minimumXY, maximumXY, sampleSize, sampleSize);
        }

        public void PlotFunction(Func<double, double, double> function, double xMinimum, double xMaximum, double yMinimum, double yMaximum)
        {
            PlotFunction(function, xMinimum, xMaximum, yMinimum, yMaximum, defaultFunctionSampleSize, defaultFunctionSampleSize);
        }

        public void PlotFunction(Func<double, double, double> function, double xMinimum, double xMaximum, double yMinimum, double yMaximum, int sampleSize)
        {
            PlotFunction(function, xMinimum, xMaximum, yMinimum, yMaximum, sampleSize, sampleSize);
        }

        public void PlotFunction(Func<double, double, double> function, double xMinimum, double xMaximum, double yMinimum, double yMaximum, int xSampleSize, int ySampleSize)
        {
            // todo - implement checks to ensure the input parameters make sense. Maybe a SetXYRange internal method?
            xMin = xMinimum;
            xMax = xMaximum;
            yMin = yMinimum;
            yMax = yMaximum;

            double[] xArray = CreateLinearlySpacedArray(xMinimum, xMaximum, xSampleSize);
            double[] yArray = CreateLinearlySpacedArray(yMinimum, yMaximum, ySampleSize);

            DataPoints = CreateDataArrayFromFunction(function, xArray, yArray);
            switch (ColorCoding)
            {
                case ColorCoding.ByGradientY:
                    ColorValues = FindGradientY(DataPoints);
                    break;
                case ColorCoding.ByLights:
                    ColorValues = null;
                    break;
            }
            RaisePropertyChanged("DataPoints");
            RaisePropertyChanged("ColorValues");
            RaisePropertyChanged("SurfaceBrush");
        }

        #endregion

        #region === Private Methods ===

        private Point3D[,] CreateDataArrayFromFunction(Func<double, double, double> f, double[] xArray, double[] yArray)
        {
            Point3D[,] newDataArray = new Point3D[xArray.Length, yArray.Length];
            for (int i = 0; i < xArray.Length; i++)
            {
                double x = xArray[i];
                for (int j = 0; j < yArray.Length; j++)
                {
                    double y = yArray[j];
                    newDataArray[i, j] = new Point3D(x, y, f(x, y));
                }
            }
            return newDataArray;
        }

        private double[] CreateLinearlySpacedArray(double minValue, double maxValue, int numberOfPoints)
        {
            double[] array = new double[numberOfPoints];
            double intervalSize = (xMax - xMin) / (numberOfPoints - 1);
            for (int i = 0; i < numberOfPoints; i++)
            {
                array[i] = minValue + i * intervalSize;
            }
            return array;
        }

        /*
        private void SetTicksAutomatically()
        {
            xTickMin = xMin;
            xTickMax = xMax;
            xNumberOfTicks = 10;
            xTickInterval = (xTickMax - xTickMin) / (xNumberOfTicks - 1);
            for (int i = 0; i < xNumberOfTicks; i++)
            {
                //xTickMin
            }
        } */

        #endregion

        #region === Exposed Properties ===

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

        private Point3D[,] dataPoints;
        public Point3D[,] DataPoints
        {
            get { return dataPoints; }
            set
            {
                dataPoints = value;
                //RaisePropertyChanged("DataPoints");
            }
        }

        private double[] xAxisTicks;
        public double[] XAxisTicks
        {
            get { return xAxisTicks; }
            set
            {
                xAxisTicks = value;
                //RaisePropertyChanged("DataPoints");
            }
        }

        private double[] yAxisTicks;
        public double[] YAxisTicks
        {
            get { return yAxisTicks; }
            set
            {
                yAxisTicks = value;
                //RaisePropertyChanged("DataPoints");
            }
        }

        private double[] zAxisTicks;
        public double[] ZAxisTicks
        {
            get { return zAxisTicks; }
            set
            {
                zAxisTicks = value;
                //RaisePropertyChanged("DataPoints");
            }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }

        private string xAxisLabel;
        public string XAxisLabel
        {
            get { return xAxisLabel; }
            set
            {
                xAxisLabel = value;
                RaisePropertyChanged("XAxisLabel");
            }
        }

        private string yAxisLabel;
        public string YAxisLabel
        {
            get { return yAxisLabel; }
            set
            {
                yAxisLabel = value;
                RaisePropertyChanged("YAxisLabel");
            }
        }

        private string zAxisLabel;
        public string ZAxisLabel
        {
            get { return zAxisLabel; }
            set
            {
                zAxisLabel = value;
                RaisePropertyChanged("ZAxisLabel");
            }
        }

        private bool showSurfaceMesh;
        public bool ShowSurfaceMesh
        {
            get { return showSurfaceMesh; }
            set
            {
                showSurfaceMesh = value;
                RaisePropertyChanged("ShowSurfaceMesh");
            }
        }

        private bool showContourLines;
        public bool ShowContourLines
        {
            get { return showContourLines; }
            set
            {
                showContourLines = value;
                RaisePropertyChanged("ShowContourLines");
            }
        }

        private bool showMiniCoordinates;
        public bool ShowMiniCoordinates
        {
            get { return showMiniCoordinates; }
            set
            {
                showMiniCoordinates = value;
                RaisePropertyChanged("ShowMiniCoordinates");
            }
        }

        #endregion

        /* // Do we actually need to keep any of these persistent variables for any reason...? (binding?)

        private int xNumberOfPoints;
        private int yNumberOfPoints;

        private int xNumberOfTicks;
        private int yNumberOfTicks;
        private int zNumberOfTicks;

        private double xTickInterval, yTickInterval, zTickInterval;
        private double xTickMin, xTickMax, yTickMin, yTickMax, zTickMin, zTickMax; */
        private double xMin, xMax, yMin, yMax, zMin, zMax;

        /* OLD STUFF */

        public double[,] ColorValues { get; set; }

        public ColorCoding ColorCoding { get; set; }

        public Model3DGroup Lights
        {
            get
            {
                var group = new Model3DGroup();
                switch (ColorCoding)
                {
                    case ColorCoding.ByGradientY:
                        group.Children.Add(new AmbientLight(Colors.White));
                        break;
                    case ColorCoding.ByLights:
                        group.Children.Add(new AmbientLight(Colors.Gray));
                        group.Children.Add(new PointLight(Colors.Red, new Point3D(0, -1000, 0)));
                        group.Children.Add(new PointLight(Colors.Blue, new Point3D(0, 0, 1000)));
                        group.Children.Add(new PointLight(Colors.Green, new Point3D(1000, 1000, 0)));
                        break;
                }
                return group;
            }
        }

        public Brush SurfaceBrush
        {
            get
            {
                // Brush = BrushHelper.CreateGradientBrush(Colors.White, Colors.Blue);
                // Brush = GradientBrushes.RainbowStripes;
                // Brush = GradientBrushes.BlueWhiteRed;
                switch (ColorCoding)
                {
                    case ColorCoding.ByGradientY:
                        return BrushHelper.CreateGradientBrush(Colors.Red, Colors.White, Colors.Blue);
                    case ColorCoding.ByLights:
                        return Brushes.White;
                }
                return null;
            }
        }

        // http://en.wikipedia.org/wiki/Numerical_differentiation
        public double[,] FindGradientY(Point3D[,] data)
        {
            int n = data.GetUpperBound(0) + 1;
            int m = data.GetUpperBound(0) + 1;
            var K = new double[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    // Finite difference approximation
                    var p10 = data[i + 1 < n ? i + 1 : i, j - 1 > 0 ? j - 1 : j];
                    var p00 = data[i - 1 > 0 ? i - 1 : i, j - 1 > 0 ? j - 1 : j];
                    var p11 = data[i + 1 < n ? i + 1 : i, j + 1 < m ? j + 1 : j];
                    var p01 = data[i - 1 > 0 ? i - 1 : i, j + 1 < m ? j + 1 : j];

                    //double dx = p01.X - p00.X;
                    //double dz = p01.Z - p00.Z;
                    //double Fx = dz / dx;

                    double dy = p10.Y - p00.Y;
                    double dz = p10.Z - p00.Z;

                    K[i, j] = dz / dy;
                }
            return K;
        }
    }
}
