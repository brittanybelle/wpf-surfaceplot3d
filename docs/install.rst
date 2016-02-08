.. _install:

Installation
============

Download the demo project
-------------------------

If you'd just like to test out the library, this option is for you!

Simply grab `the repo <https://github.com/brittanywelsh/wpf-surfaceplot3d>`_ in your preferred format (zip, git clone, etc) and open up the file ``WPFSurfacePlot3D/WPFSurfacePlot3D.sln``.

At this point, ensure that `HelixToolkit.Wpf <https://www.nuget.org/packages?q=helixtoolkit>`_ and `Extended WPF Toolkit <https://www.nuget.org/packages/Extended.Wpf.Toolkit/>`_ are installed (I recommend installing via NuGet).

From there, you should be able to build the project and play around with a few different surface plots. Enjoy!

Import SurfacePlot3D files into your existing project
-----------------------------------------------------

If you just want to use the SurfacePlot3D files, the easiest way to import them to your existing project is just to copy the following four files into your project:

- `SurfacePlotModel.cs <https://github.com/brittanywelsh/wpf-surfaceplot3d/blob/master/WPFSurfacePlot3D/WPFSurfacePlot3D/SurfacePlotModel.cs>`_
- `SurfacePlotView.xaml <https://github.com/brittanywelsh/wpf-surfaceplot3d/blob/master/WPFSurfacePlot3D/WPFSurfacePlot3D/SurfacePlotView.xaml>`_
- `SurfacePlotView.xaml.cs <https://github.com/brittanywelsh/wpf-surfaceplot3d/blob/master/WPFSurfacePlot3D/WPFSurfacePlot3D/SurfacePlotView.xaml.cs>`_
- `SurfacePlotVisual3D.cs <https://github.com/brittanywelsh/wpf-surfaceplot3d/blob/master/WPFSurfacePlot3D/WPFSurfacePlot3D/SurfacePlotVisual3D.cs>`_

They can be accessed via the namespace ``WPFSurfacePlot3D``. You will also have to ensure that `HelixToolkit.Wpf <https://www.nuget.org/packages?q=helixtoolkit>`_ is installed in your project.

Coming soon
-----------

Installation via NuGet package manager is planned for an upcoming release. Stay tuned!
