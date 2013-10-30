Windows Phone 8 Maps Examples
=============================

This project is just a collection of Maps examples for Windows Phone 8. The
basic use cases were previously implemented for other platforms and the examples
are available on the wiki:

* Windows Phone 7: [Maps Examples for Windows phone](http://www.developer.nokia.com/Community/Wiki/Maps_Examples_for_Windows_phone)
* Java: [Examples for Maps API for Java ME](http://www.developer.nokia.com/Community/Wiki/Examples_for_Maps_API_for_Java_ME)
* Qt: [Qt Maps Examples](http://www.developer.nokia.com/Community/Wiki/Qt_Maps_Examples)
* Qt Quick: [Qt Quick Maps Examples](http://www.developer.nokia.com/Community/Wiki/QtQuick_Maps_Examples)
* Ovi Js Maps: [Maps Examples for Ovi Maps API](http://www.developer.nokia.com/Community/Wiki/Maps_Examples_for_Ovi_Maps_API)

This project is hosted in GitHub:
https://github.com/nokia-developer/maps-samples


## Hello Map ##
 
Basic map with kinetic panning and pinch zooming. Also includes menu option for
disabling the map (makes it static).

## Map events ##
 
Shows on how different events can be captured, and which events are generated
with different map changes. 

## Map interaction ##
 
Shows map moving to predefined locations:

* Switching different animation modes (parabolic, linear, none)
* Changing  heading, pitch & zoom levels
* Switching between map color (light/dark) modes & map types (road, Arial,
  hybrid, terrain)
* Toggling pedestrian features & landmarks on/off
* Setting multiple values with `setview()`

## Simple Map content ##
 
Shows how to add and remove Markers (MapOverlay), Polyline and Polygon to the
Map. Example also shows how to zoom the map in order to fit the selected map
content into the view. 

There is also related wiki articles for this example code:
[Fitting content into the view with Windows Phone maps API](http://www.developer.nokia.com/Community/Wiki/Fitting_content_into_the_view_with_Windows_Phone_maps_API)
and
[Hiding map content with Windows Phone maps API](http://www.developer.nokia.com/Community/Wiki/Showing/Hiding_map_content_with_Windows_Phone_maps_API).

## More Map content ##

Shows how to make rectangle & circle with polygon and how to add them into the
map. Also shows differences adding ellipse & rectangle objects to the map
(non-map objects).

There is also related wiki articles for this example code:
[Drawing shapes with Windows Phone maps_API](http://www.developer.nokia.com/Community/Wiki/Drawing_shapes_with_Windows_Phone_maps_API).

## Draggable Marker ##
 
Just an simple example illustrating minimal implementation for draggable
markers. There is also Wiki page made to explain the code a bit:
[Draggable markers with Windows Phone maps API](http://www.developer.nokia.com/Community/Wiki/Draggable_markers_with_Windows_Phone_maps_API)

## Map markers ##

Work in progress, is here just so it could be compared with other platform
examples.

## Dynamic polyline ##
 
Shows how to add and remove markers to the map with click events, as well as how
to dynamically add and remove points from polyline. 

## My Location ##

Shows how to use `GeoCoordinateWatcher` to get and monitor GPS position and how
to show that on the map using circle polygon to show the accuracy of the
position. 

## Geo Coding ##
 
Simple example showing on how you can Geo code an address to a location(s)
utilizing the Geo coding service. 

## Reverse Geo coding ##
 
Simple example showing on how you can Reverse Geo code location from an address
by utilizing the reverse Geo coding service .

## Routing ##
 
Very Simple example showing how you can route between two points and show the
route in a map.

## Advanced Routing ##

Nicer UI example for the routing.

## Location Selector ##

Example showing how to implement location selector page and add it to your
project.

## Area Selector ##

Example showing how to implement Circle Area selector page and add it to your
project. The special numeric input box used in this project is explained in
[Implementing numerical inputbox](http://www.developer.nokia.com/Community/Wiki/Implementing_numerical_inputbox)
wiki article.

## LaunchDirectionsTask ##

Example based on advanced routing UI, which shows how to instead draw the route
to Maps application by utilizing the MapsDirectionsTask API.


## LaunchMapSearchTask ##

Example showing how to use mapsTask API for location based searching with Maps.

## LaunchMapTask ##

Example showing how to use mapsTask API for showing Map location.

## GetMyGeoposition ## 

Very simple example illustrating how to use Geolocator to get single shot
location.

## TrackMyPosition and TrackMyPositionTwo ## 

Simple Examples showing the differences for the the location tracking between
the Geolocator and GeoCoordinateWatcher API.

## TrackMeInBackground ##

Very simple example for showing how to run position tracking in the background.

---

*Copyright (c) 2012-2013 Nokia Corporation. All rights reserved.*
