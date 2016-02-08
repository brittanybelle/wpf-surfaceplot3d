Plotting 3D Data
=================

This is where SurfacePlot3D really shines: providing an easy entry point for your custom 3D data set. Depending on the type of data you're starting with, there are a number of different arguments you can pass into the PlotData function.

Plotting a 2D Array of Values
-----------------------------

This is the easiest possible case. Let's say we've got a 2D array of doubles set up::

  double[,] arrayOfPoints = new double[10, 15];
  for (int i = 0; i < 10; i++)
  {
    for (int j = 0; j < 15; j++)
    {
      arrayOfPoints[i, j] = Math.Cos(Math.Abs(i) + Math.Abs(j)) * (Math.Abs(i) + Math.Abs(j));
    }
  }

Now all we have to do is plug it into our PlotData function::

  PlotData(arrayOfPoints);

Great! But you should notice how only passing in an array of points will give you automatic indexing: that is, your x and y axes will be indexed from 0 to n-1, where n is the number of points in either dimension.

If you have a function, like say you wanted to plot the same function as above, but you wanted to plot 20 and 25 points between 0 and 1 in each the x and y axes, respectively. For this, we also need to define a double array of the corresponding size for each axis. We might set up such a plot like so::

  int N = 20;
  int M = 25;

  double[] xArray = double[N];
  double[] yArray = double[M];
  double[,] zArray = double[N, M]; // Notice how the size of the array in each dimension has to match up with the size of the corresponding "axes" array!
  for (int i = 0; i < N; i++)
  {
    double x = i / N;
    for (int j = 0; j < M; j++)
    {
      double y = j / M;
      arrayOfPoints[i, j] = Math.Cos(Math.Abs(x) + Math.Abs(y)) * (Math.Abs(x) + Math.Abs(y));
    }
  }

Finally, we'll plot each of the arrays::

  PlotData(zArray, xArray, yArray);

Perfect!

Plotting an Array of 3DPoint Objects
------------------------------------

Since the SurfacePlot3D library works primary through Point3D arrays, this is the most straightforward plot. You can plot a 2-dimensional array of Point3D objects by simply passing the array into the PlotData function::

  PlotData(point3DArray);
