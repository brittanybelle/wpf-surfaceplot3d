using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WPFSurfacePlot3D
{
    public class SurfacePlotVisual3D : ModelVisual3D
    {
        // The modelContainer holds all the component models as children
        private readonly ModelVisual3D modelContainer;

        /// <summary>
        /// The constructor for a new SurfacePlotVisual3D object.
        /// </summary>
        public SurfacePlotVisual3D()
        {
            IntervalX = 1;
            IntervalY = 1;
            IntervalZ = 0.25;
            FontSize = 0.06;
            LineThickness = 0.01;

            modelContainer = new ModelVisual3D();
            Children.Add(modelContainer);
        }

        /// <summary>
        /// Gets or sets the points defining the 3D surface plot, as a 2D-array of Point3D objects.
        /// </summary>
        public Point3D[,] DataPoints
        {
            get { return (Point3D[,])GetValue(DataPointsProperty); }
            set { SetValue(DataPointsProperty, value); }
        }

        public static readonly DependencyProperty DataPointsProperty = DependencyProperty.Register("DataPoints", typeof(Point3D[,]), typeof(SurfacePlotVisual3D), new UIPropertyMetadata(SamplePoints, ModelWasChanged));

        /// <summary>
        /// Gets or sets the color values corresponding to the Points array, as a 2D-array of doubles.
        /// The color values are used as Texture coordinates for the surface.
        /// Remember to set the SurfaceBrush, e.g. by using the BrushHelper.CreateGradientBrush method.
        /// If this property is not set, the z-value of the Points will be used as color value.
        /// </summary>
        public double[,] ColorValues
        {
            get { return (double[,])GetValue(ColorValuesProperty); }
            set { SetValue(ColorValuesProperty, value); }
        }

        public static readonly DependencyProperty ColorValuesProperty = DependencyProperty.Register("ColorValues", typeof(double[,]), typeof(SurfacePlotVisual3D), new UIPropertyMetadata(null, ModelWasChanged));

        /// <summary>
        /// Gets or sets the brush used for the surface.
        /// </summary>
        public Brush SurfaceBrush
        {
            get { return (Brush)GetValue(SurfaceBrushProperty); }
            set { SetValue(SurfaceBrushProperty, value); }
        }

        public static readonly DependencyProperty SurfaceBrushProperty = DependencyProperty.Register("SurfaceBrush", typeof(Brush), typeof(SurfacePlotVisual3D), new UIPropertyMetadata(null, ModelWasChanged));
        
        // todo: make Dependency properties
        public double IntervalX { get; set; }
        public double IntervalY { get; set; }
        public double IntervalZ { get; set; }
        public double FontSize { get; set; }
        public double LineThickness { get; set; }

        /// <summary>
        /// This is called whenever a property of the SurfacePlotVisual3D is changed; it updates the 3D model.
        /// </summary>
        private static void ModelWasChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SurfacePlotVisual3D)d).UpdateModel();
        }

        /// <summary>
        /// This function updates the 3D visual model. It is called whenever a DependencyProperty of the SurfacePlotVisual3D object is called.
        /// </summary>
        private void UpdateModel()
        {
            this.Children.Clear(); // Necessary to remove BillboardTextVisual3D objects (?)
            Children.Add(modelContainer);

            modelContainer.Content = CreateModel();
        }

        /// <summary>
        /// This function contains all the "business logic" for constructing a SurfacePlot 3D. 
        /// </summary>
        /// <returns>A Model3DGroup containing all the component models (mesh, surface definition, grid objects, etc).</returns>
        private Model3DGroup CreateModel()
        {
            var newModelGroup = new Model3DGroup();
            double lineThickness = 0.01;
            double axesOffset = 0.05;

            // Get relevant constaints from the DataPoints object
            int numberOfRows = DataPoints.GetUpperBound(0) + 1;
            int numberOfColumns = DataPoints.GetUpperBound(1) + 1;

            // Determine the x, y, and z ranges of the DataPoints collection
            double minX = double.MaxValue;
            double maxX = double.MinValue;
            double minY = double.MaxValue;
            double maxY = double.MinValue;
            double minZ = double.MaxValue;
            double maxZ = double.MinValue;

            double minColorValue = double.MaxValue;
            double maxColorValue = double.MinValue;

            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    double x = DataPoints[i, j].X;
                    double y = DataPoints[i, j].Y;
                    double z = DataPoints[i, j].Z;
                    maxX = Math.Max(maxX, x);
                    maxY = Math.Max(maxY, y);
                    maxZ = Math.Max(maxZ, z);
                    minX = Math.Min(minX, x);
                    minY = Math.Min(minY, y);
                    minZ = Math.Min(minZ, z);
                    if (ColorValues != null)
                    {
                        maxColorValue = Math.Max(maxColorValue, ColorValues[i, j]);
                        minColorValue = Math.Min(minColorValue, ColorValues[i, j]);
                    }
                }
            }

            /* TEMP */
            int numberOfXAxisTicks = 10;
            int numberOfYAxisTicks = 10;
            int numberOfZAxisTicks = 5;
            double XAxisInterval = (maxX - minX) / numberOfXAxisTicks;
            double YAxisInterval = (maxY - minY) / numberOfYAxisTicks;
            double ZAxisInterval = (maxZ - minZ) / numberOfZAxisTicks;
            /* /TEMP */

            // Set color value to 0 at texture coordinate 0.5, with an even spread in either direction
            if (Math.Abs(minColorValue) < Math.Abs(maxColorValue)) { minColorValue = -maxColorValue; }
            else                                                   { maxColorValue = -minColorValue; }

            // Set the texture coordinates by either z-value or ColorValue
            var textureCoordinates = new Point[numberOfRows, numberOfColumns];
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    double tc;
                    if (ColorValues != null) { tc = (ColorValues[i, j] - minColorValue) / (maxColorValue - minColorValue); }
                    else                     { tc = (DataPoints[i, j].Z - minZ) / (maxZ - minZ); }
                    textureCoordinates[i, j] = new Point(tc, tc);
                }
            }

            // Build the surface model (i.e. the coloured surface model)
            MeshBuilder surfaceModelBuilder = new MeshBuilder();
            surfaceModelBuilder.AddRectangularMesh(DataPoints, textureCoordinates);

            GeometryModel3D surfaceModel = new GeometryModel3D(surfaceModelBuilder.ToMesh(), MaterialHelper.CreateMaterial(SurfaceBrush, null, null, 1, 0));
            surfaceModel.BackMaterial = surfaceModel.Material;

            // Instantiate MeshBuilder objects for the Grid and SurfaceMeshLines meshes
            MeshBuilder surfaceMeshLinesBuilder = new MeshBuilder();
            MeshBuilder surfaceContourLinesBuilder = new MeshBuilder();
            MeshBuilder gridBuilder = new MeshBuilder();

            // Build the axes labels model (i.e. the object that holds the axes labels and ticks)
            ModelVisual3D axesLabelsModel = new ModelVisual3D();

            // Loop through x intervals - for the surface meshlines, the grid, and X axes ticks
            for (double x = minX; x <= maxX + 0.0001; x += XAxisInterval)
            {
                // Add surface mesh lines which denote intervals along the x-axis
                var surfacePath = new List<Point3D>();
                double i = (x - minX) / (maxX - minX) * (numberOfColumns - 1);
                for (int j = 0; j < numberOfColumns; j++)
                {
                    surfacePath.Add(DoBilinearInterpolation(DataPoints, i, j));
                }
                surfaceMeshLinesBuilder.AddTube(surfacePath, lineThickness, 9, false);

                // Axes labels
                BillboardTextVisual3D label = new BillboardTextVisual3D();
                label.Text = string.Format("{0:F2}", x);
                label.Position = new Point3D(x, minY - axesOffset, minZ - axesOffset);
                axesLabelsModel.Children.Add(label);

                // Grid lines
                var gridPath = new List<Point3D>();
                gridPath.Add(new Point3D(x, minY, minZ));
                gridPath.Add(new Point3D(x, maxY, minZ));
                gridPath.Add(new Point3D(x, maxY, maxZ));
                gridBuilder.AddTube(gridPath, lineThickness, 9, false);

            }

            // Loop through y intervals - for the surface meshlines, the grid, and Y axes ticks
            for (double y = minY; y <= maxY + 0.0001; y += YAxisInterval)
            {
                // Add surface mesh lines which denote intervals along the y-axis
                var path = new List<Point3D>();
                double j = (y - minY) / (maxY - minY) * (numberOfRows - 1);
                for (int i = 0; i < numberOfRows; i++)
                {
                    path.Add(DoBilinearInterpolation(DataPoints, i, j));
                }
                surfaceMeshLinesBuilder.AddTube(path, lineThickness, 9, false);

                // Axes labels
                BillboardTextVisual3D label = new BillboardTextVisual3D();
                label.Text = string.Format("{0:F2}", y);
                label.Position = new Point3D(minX - axesOffset, y, minZ - axesOffset);
                axesLabelsModel.Children.Add(label);

                // Grid lines
                var gridPath = new List<Point3D>();
                gridPath.Add(new Point3D(minX, y, minZ));
                gridPath.Add(new Point3D(maxX, y, minZ));
                gridPath.Add(new Point3D(maxX, y, maxZ));
                gridBuilder.AddTube(gridPath, lineThickness, 9, false);
            }

            // Loop through z intervals - for the grid, and Z axes ticks
            for (double z = minZ; z <= maxZ + 0.0001; z += ZAxisInterval)
            {
                // Grid lines
                var path = new List<Point3D>();
                path.Add(new Point3D(minX, maxY, z));
                path.Add(new Point3D(maxX, maxY, z));
                path.Add(new Point3D(maxX, minY, z));
                gridBuilder.AddTube(path, lineThickness, 9, false);

                // Axes labels
                BillboardTextVisual3D label = new BillboardTextVisual3D();
                label.Text = string.Format("{0:F2}", z);
                label.Position = new Point3D(minX - axesOffset, maxY + axesOffset, z);
                axesLabelsModel.Children.Add(label);
            }

            // Add axes labels
            BillboardTextVisual3D xLabel = new BillboardTextVisual3D();
            xLabel.Text = "X Axis";
            xLabel.Position = new Point3D((maxX - minX) / 2, minY - 3 * axesOffset, minZ - 5 * axesOffset);
            axesLabelsModel.Children.Add(xLabel);
            BillboardTextVisual3D yLabel = new BillboardTextVisual3D();
            yLabel.Text = "Y Axis";
            yLabel.Position = new Point3D(minX - 3 * axesOffset, (maxY - minY) / 2, minZ - 5 * axesOffset);
            axesLabelsModel.Children.Add(yLabel);
            BillboardTextVisual3D zLabel = new BillboardTextVisual3D();
            zLabel.Text = "Z Axis";
            zLabel.Position = new Point3D(minX - 5 * axesOffset, maxY + 5 * axesOffset, 0); // Note: trying to find the midpoint of minZ, maxZ doesn't work when minZ = -0.5 and maxZ = 0.5...
            axesLabelsModel.Children.Add(zLabel);

            // Create models from MeshBuilders
            GeometryModel3D surfaceMeshLinesModel = new GeometryModel3D(surfaceMeshLinesBuilder.ToMesh(), Materials.Black);
            GeometryModel3D gridModel = new GeometryModel3D(gridBuilder.ToMesh(), Materials.Black);

            // Update model group
            this.Children.Add(axesLabelsModel);
            newModelGroup.Children.Add(surfaceModel);
            newModelGroup.Children.Add(surfaceMeshLinesModel);
            newModelGroup.Children.Add(gridModel);

            //ScaleTransform3D surfaceTransform = new ScaleTransform3D(20, 20, 20, 0, 0, 0);
            //newModelGroup.Transform = surfaceTransform;

            return newModelGroup;
        }
        
        // <summary>
        /// The bilinear interpolation method calculates a weighted "average" between four points on a discrete grid, allowing us to build a "smooth" path between consecutive points along a grid.
        /// </summary>
        /// <param name="points">Points array - containing the data to be interpolated</param>
        /// <param name="i">First index: i.e., points[i, j]</param>
        /// <param name="j">Second index: i.e., points[i, j]</param>
        /// <returns></returns>
        private static Point3D DoBilinearInterpolation(Point3D[,] points, double i, double j)
        {
            int n = points.GetUpperBound(0);
            int m = points.GetUpperBound(1);
            var i0 = (int)i;
            var j0 = (int)j;
            if (i0 + 1 >= n) i0 = n - 2;
            if (j0 + 1 >= m) j0 = m - 2;

            if (i < 0) i = 0;
            if (j < 0) j = 0;
            double u = i - i0;
            double v = j - j0;
            Vector3D v00 = points[i0, j0].ToVector3D();
            Vector3D v01 = points[i0, j0 + 1].ToVector3D();
            Vector3D v10 = points[i0 + 1, j0].ToVector3D();
            Vector3D v11 = points[i0 + 1, j0 + 1].ToVector3D();
            Vector3D v0 = v00 * (1 - u) + v10 * u;
            Vector3D v1 = v01 * (1 - u) + v11 * u;
            return (v0 * (1 - v) + v1 * v).ToPoint3D();
        }


        /// <summary>
        /// This Point3D data set is used to populate the DataPoints dependency properties when they are initialized.
        /// </summary>
        public static Point3D[,] SamplePoints
        {
            get
            {
                int n = 30;
                Point3D[,] points = new Point3D[n, n];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        double x = i * (Math.PI / n);
                        double y = j * (Math.PI / n);
                        double z = 0.5 * Math.Sin(x * y);
                        points[i, j] = new Point3D(x, y, z);
                    }
                }
                return points;
            }
        }
    }
}
