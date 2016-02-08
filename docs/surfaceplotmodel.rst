SurfacePlotModel
=======================

.. note: This section is under construction!

[SurfacePlotViewModel API]
- complete list of functions you can call on SurfacePlotModel objects.

.. todo:  FOR THE README/WIKI... where to put the SurfaceBrush? and the Colors object?

File Organization/Class Relationships
-------------------------------------

The general design behind these classes is straightforward.

The view inherits from UserControl. This allows the designer to easily grab and manipulate the control in the XAML editor of their application.

The View exposes various DependencyProperties. In general, the View is concerned just with plotting. The Model, on the other hand, takes care of anything needed to get from user input to the plotting data. This usercontrol basically takes its datacontext from the Model and creates the View needed (in a Helix Viewport), interacting with the methods from SurfaePlotVisual3D when necessary.

SurfaePlotVisual3D inherits from Visual3D and it is the collection of 3D visual objects that are drawn to the screen.

The Model is intended to be the main point of entry for the user; everything else is intended to be dealt with privately.