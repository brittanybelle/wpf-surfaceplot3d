.. _plottingfunctions:

Plotting Functions
=================

[plotting functions]
(5) Now let's add our own plot based on a mathematical function. First we have to define a function that has two double "input" variables and one double "output variable", hence a function of the type Func<double, double, double>. (note: you have to include using System; to get access to this data type).

One simple way of doing this is defining our function via the lambda operator:
Func<double, double, double> sampleFunction = (x, y) => x * y;

Easy enough, right? let's define something even more interesting!
sampleFunction = (x, y) => 10 * Math.Sin(Math.Sqrt(x * x + y * y)) / Math.Sqrt(x * x + y * y);

Now we're ready to plot our function. It's as easy as passing in the function to the appropriate method:

PlotFunction(mySampleFunction);

This will plot your function in the range x:[-1, 1], y:[-1, 1]. If you want to change the default range, you can do so through the same function:

PlotFunction(mySampleFunction, -10, 5);

This plots the function in the range x:[-10, 5], y:[-10, 5].

If you want to have even more control over the range, you can do that too! Let's plot the function in the domain x:[-3, 9], y:[-20, 0]:

PlotFunction(mySampleFunction, -3, 9, -20, 20);

That's everything you need to get started with your plot! From here, why not customize your plot's appearance? Or you can learn how to plot data that you define yourself.

Finally, it's worth noting that in order to plot the function, we are first "sampling" that function over the defined domain. The default sample size in both x and y directions is 100 points, resulting in a collection of point objects of size 100x100 = 10000. You can add arguments to the PlotFunction method in order to change the sample size.
