WPF-SurfacePlot3D
=================

Easy, attractive 3D plots in WPF
--------------------------------

WPF-SurfacePlot3D is a tiny C# library containing easy-to-use 3D surface plotting components for WPF (.NET) applications.

You can get started in as few as four lines::

    var myPlot = new SurfacePlotViewModel();
    myPlotView.DataContext = myPlot;
    function = (x, y) => x * y;
    viewModel.PlotFunction(function, -1, 1);

Features
--------

- Flexible data input formats:

  - Plot functions directly
  - Use your own data arrays

- Easy data binding for real-time updates
- Leverage the beauty and power of `Helix 3D Toolkit <https://github.com/helix-toolkit/helix-toolkit>`_

Installation
------------

While the project is in beta, simply grab the class files from `the Github repo <https://github.com/brittanywelsh/wpf-surfaceplot3d>`_ and add them to your own project. For more information, read the :ref:`install` section.

COMING SOON: Installation via NuGet package manager (planned for version 1.0.0).

Contribute
----------

This project is organized using `Github <https://github.com/brittanywelsh/wpf-surfaceplot3d>`_.

- Please report all bugs, feature requests, and other issues through the `issue tracker <https://github.com/brittanywelsh/wpf-surfaceplot3d/issues>`_.
- Feel free to add your thoughts to the `project wiki <https://github.com/brittanywelsh/wpf-surfaceplot3d/wiki>`_!

Contact
-------

If you're having issues or just want to send along some feedback, feel free to email me: hello@brittanywelsh.com.

License
-------

This project is licensed under the `GNU General Public License (version 3) <http://www.gnu.org/licenses/gpl-3.0.en.html>`_.

.. toctree::
   :caption: Getting Started
   :maxdepth: 2

   Introduction <self>
   install
   tutorial

.. toctree::
   :caption: User Manual
   :maxdepth: 2

   plottingfunctions
   plottingdata
   appearance

.. toctree::
   :caption: Complete API Reference
   :maxdepth: 2

   surfaceplotmodel
