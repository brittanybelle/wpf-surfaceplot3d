Tutorial
========

So you've got the :ref:`files you need <install>`_ located somewhere in your demo project, right?

To start using 3D plots, we need to follow a simple set of steps.

#. In the code-behind for the view (Window, App, etc.) where you'd like to display a surface plot, first ensure that the appropriate namespace is referenced with a ``using`` statement:

``using WPFSurfacePlot3D;``

You'll also have to ensure the namespace is called from the XAML file as well:

<Window x:Class="MyWindow"
        xmlns:SurfacePlot="clr-namespace:WPFSurfacePlot3D">

#. Next, go to the XAML file. You can grab a SurfacePlotView control from your toolbox and drag it into your XAML file, or simply add the following XAML:

``<SurfacePlot:SurfacePlotView  x:Name="mySurfacePlotView" />``

#. Once you've added mySurfacePlot, now you need to set up the DataContext in the code-behind. Back in your .xaml.cs file, add the following:

``mySurfacePlotModel = new SurfacePlotModel();
mySurfacePlotView.DataContext = mySurfacePlotModel;``

You can, of course, rename ``mySurfacePlotModel`` and ``ySurfacePlotView`` to whatever you please.

.. note:: After you set up your data binding by setting the DataContext, the object called mySurfacePlotModel is now your main point of control to interact with the plot.

Great - now you're ready to start :ref:`plotting functions <plottingfunctions>`_!
