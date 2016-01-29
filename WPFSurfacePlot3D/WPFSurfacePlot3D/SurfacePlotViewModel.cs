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

    class SurfacePlotViewModel : INotifyPropertyChanged
    {
        private int defaultSampleSize = 100;

        //private double[] xDataPointsArray, yDataPointsArray;

        public SurfacePlotViewModel()
        {
            Title = "New Surface Plot";
            XAxisLabel = "x-Axis";
            YAxisLabel = "y-Axis";
            ZAxisLabel = "z-Axis";

            MinX = 0;
            MaxX = 3;
            MinY = 0;
            MaxY = 3;
            Rows = 91;
            Columns = 91;

            Function = (x, y) => Math.Sin(x * y) * 0.5;
            ColorCoding = ColorCoding.ByLights;
            UpdateModel();
            //UpdatePlotData();
        }

        #region === Public Methods ===


        public void PlotFunction(Func<double, double, double> function, double minimumXY, double maximumXY)
        {
            PlotFunction(function, minimumXY, maximumXY, minimumXY, maximumXY, defaultSampleSize, defaultSampleSize);
        }

        public void PlotFunction(Func<double, double, double> function, double minimumXY, double maximumXY, int sampleSize)
        {
            PlotFunction(function, minimumXY, maximumXY, minimumXY, maximumXY, sampleSize, sampleSize);
        }

        public void PlotFunction(Func<double, double, double> function, double xMinimum, double xMaximum, double yMinimum, double yMaximum)
        {
            PlotFunction(function, xMinimum, xMaximum, yMinimum, yMaximum, defaultSampleSize, defaultSampleSize);
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

            Data = CreateDataArrayFromFunction(function, xArray, yArray);
            switch (ColorCoding)
            {
                case ColorCoding.ByGradientY:
                    ColorValues = FindGradientY(Data);
                    break;
                case ColorCoding.ByLights:
                    ColorValues = null;
                    break;
            }
            RaisePropertyChanged("Data");
            RaisePropertyChanged("ColorValues");
            RaisePropertyChanged("SurfaceBrush");
        }

        #endregion

        #region === Private Methods ===

        private Point3D[,] CreateDataArrayFromFunction(Func<double, double, double> f, double[] xArray, double[] yArray)
        {
            Point3D[,] dataArray = new Point3D[xArray.Length, yArray.Length];
            for (int i = 0; i < xArray.Length; i++)
            {
                double x = xArray[i];
                for (int j = 0; j < yArray.Length; j++)
                {
                    double y = yArray[j];
                    dataArray[i, j] = new Point3D(x, y, f(x, y));
                }
            }
            return dataArray;
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

        private void SetTicksAutomatically()
        {
            xTickMin = xMin;
            xTickMax = xMax;
            xNumberOfTicks = 10;
            xTickInterval = (xTickMax - xTickMin) / xNumberOfTicks;
        }

        #endregion

        #region === Properties & Event Handlers ===

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
                RaisePropertyChanged("DataPoints");
            }
        }

        #endregion

        #region === String/Text Properties ===

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

        #endregion

        #region === Boolean Properties ===

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

        #region === Double Properties ===

        private double xMin;
        public double XMin
        {
            get { return xMin; }
            set
            {
                xMin = value;
                RaisePropertyChanged("XMin");
            }
        }

        private double xMax;
        public double XMax
        {
            get { return xMax; }
            set
            {
                xMax = value;
                RaisePropertyChanged("XMax");
            }
        }

        private double yMin;
        public double YMin
        {
            get { return yMin; }
            set
            {
                yMin = value;
                RaisePropertyChanged("YMin");
            }
        }

        private double yMax;
        public double YMax
        {
            get { return yMax; }
            set
            {
                yMax = value;
                RaisePropertyChanged("YMax");
            }
        }
        
        private double zMin;
        public double ZMin
        {
            get { return zMin; }
            set
            {
                zMin = value;
                RaisePropertyChanged("ZMin");
            }
        }

        private double zMax;
        public double ZMax
        {
            get { return zMax; }
            set
            {
                zMax = value;
                RaisePropertyChanged("ZMax");
            }
        }
        
        private double xTickInterval;
        public double XTickInterval
        {
            get { return xTickInterval; }
            set
            {
                xTickInterval = value;
                RaisePropertyChanged("XTickInterval");
            }
        }

        private double yTickInterval;
        public double YTickInterval
        {
            get { return yTickInterval; }
            set
            {
                yTickInterval = value;
                RaisePropertyChanged("YTickInterval");
            }
        }

        private double zTickInterval;
        public double ZTickInterval
        {
            get { return zTickInterval; }
            set
            {
                zTickInterval = value;
                RaisePropertyChanged("ZTickInterval");
            }
        }
        
        #endregion

        #region === Int Properties ===

        private int xNumberOfPoints;
        private int yNumberOfPoints;

        private int xNumberOfTicks;
        private int yNumberOfTicks;
        private int zNumberOfTicks;

        private double xTickMin, xTickMax, yTickMin, yTickMax, zTickMin, zTickMax;
        //private double xTickInterval, yTickInterval, zTickInterval;

        #endregion



        /* OLD STUFF */

        public double MinX { get; set; }
        public double MinY { get; set; }
        public double MaxX { get; set; }
        public double MaxY { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public Func<double, double, double> Function { get; set; }
        public Point3D[,] Data { get; set; }
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

        private void UpdateModel()
        {
            Data = CreateDataArray(Function);
            switch (ColorCoding)
            {
                case ColorCoding.ByGradientY:
                    ColorValues = FindGradientY(Data);
                    break;
                case ColorCoding.ByLights:
                    ColorValues = null;
                    break;
            }
            RaisePropertyChanged("Data");
            RaisePropertyChanged("ColorValues");
            RaisePropertyChanged("SurfaceBrush");
        }

        public Point GetPointFromIndex(int i, int j)
        {
            double x = MinX + (double)j / (Columns - 1) * (MaxX - MinX);
            double y = MinY + (double)i / (Rows - 1) * (MaxY - MinY);
            return new Point(x, y);
        }

        public Point3D[,] CreateDataArray(Func<double, double, double> f)
        {
            var data = new Point3D[Rows, Columns];
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                {
                    var pt = GetPointFromIndex(i, j);
                    data[i, j] = new Point3D(pt.X, pt.Y, f(pt.X, pt.Y));
                }
            return data;
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
