using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SimpleContent.Resources;
using Microsoft.Phone.Maps.Controls;
using System.Diagnostics;
using System.Device.Location;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;

namespace SimpleContent
{
    public partial class MainPage : PhoneApplicationPage
    {
        String selected_shape = "";
        MapPolygon poly = null;
        MapPolyline polyline = null;
        MapLayer markerLayer = null;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Debug.WriteLine("We are started"); //Check to see if the "Redirect all Output Window text to the Immediate Window" is checked under Tools -> Options -> Debugging -> General.  
            map1.MouseLeftButtonDown += map1_MouseLeftButtonDown;
        }

        void map1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IList<MapElement> listt = map1.GetMapElementsAt(e.GetPosition(map1));
            Debug.WriteLine("map1_MouseLeftButtonDown, MapElement count: " + listt.Count());
        }

        private void Button_Menubut_Click(object sender, RoutedEventArgs e)
        {
            if (sender == CanCelMenuBut)
            {
                MenuSelectionGrip.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                if (sender == MarkerBut)
                {
                    selected_shape = "Markers";
                }
                else if (sender == PolygonBut)
                {
                    selected_shape = "Polygon";
                }
                else if (sender == PolylineBut)
                {
                    selected_shape = "Polyline";
                }

                MenuSelectionLiat.SelectedIndex = 0;
                MenuSelectionGrip.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("We are loaded");
        }


        void SelectionChangedEventHandler(Object sender, SelectionChangedEventArgs e)
        {
            ListBox BOXX = (sender as ListBox);
            String sellected = (BOXX.SelectedItem as TextBlock).Text;

            Debug.WriteLine("ListBox1SelectedIndexChanged " + sellected);

            if (sellected == "Add")
            {
                AddItem();
            }
            else if (sellected == "Remove")
            {
                RevomeItem();
            }
            else if (sellected == "Toggle visibility")
            {
                ToggleVisibility();
            }
            else if (sellected == "Fit to View")
            {
                FitToView();
            }
        }


        void AddItem()
        {
            if (selected_shape == "Polygon")
            {
                if (poly == null)
                {
                    poly = new MapPolygon();
                    
                    //Define the polygon vertices
                    GeoCoordinateCollection boundingLocations = new GeoCoordinateCollection();
                    boundingLocations.Add(new GeoCoordinate(-1.22, -2.81));
                    boundingLocations.Add(new GeoCoordinate(60.22, 24.81));
                    boundingLocations.Add(new GeoCoordinate(60.30, 24.70));
                    boundingLocations.Add(new GeoCoordinate(60.14, 24.57));

                    //Set the polygon properties
                    poly.Path = boundingLocations;
                    poly.FillColor = Color.FromArgb(0x55, 0x00, 0xFF, 0x00);
                    poly.StrokeColor = Color.FromArgb(0xFF, 0x00, 0x00, 0xFF);
                    poly.StrokeThickness = 20;


                    //Add the polygon to the map
                    map1.MapElements.Add(poly);
                }
            }
            else if (selected_shape == "Polyline")
            {
                if (polyline == null)
                {
                    polyline = new MapPolyline();
                    polyline.StrokeColor = Color.FromArgb(0xFF, 0xFF, 0x00, 0x00);
                    polyline.StrokeThickness = 10;

                    polyline.Path = new GeoCoordinateCollection() { 
                            new GeoCoordinate(60.27, 24.81), 
                            new GeoCoordinate(60.35, 24.70), 
                            new GeoCoordinate(60.19, 24.57), 
                            new GeoCoordinate(60.27, 24.81) 
                        };

                    map1.MapElements.Add(polyline);
                }
            }
            else if (selected_shape == "Markers")
            {
                if (markerLayer == null)
                {
                    markerLayer = new MapLayer();

                    MapOverlay pin1 = new MapOverlay();
                    pin1.GeoCoordinate = new GeoCoordinate(60.27, 24.80);

                    Ellipse Circhegraphic = new Ellipse();
                    Circhegraphic.Fill = new SolidColorBrush(Colors.Green);
                    Circhegraphic.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Purple);
                    Circhegraphic.StrokeThickness = 5;
                    Circhegraphic.Opacity = 0.8;
                    Circhegraphic.Height = 40;
                    Circhegraphic.Width = 40;

                    pin1.Content = Circhegraphic;
                    pin1.PositionOrigin = new Point(0.5, 0.5);

                  
                    MapOverlay pin2 = new MapOverlay();
                    pin2.GeoCoordinate = new GeoCoordinate(60.22, 24.70);

                    Ellipse circ2 = new Ellipse();
                    circ2.Fill = new SolidColorBrush(Colors.Green);
                    circ2.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Purple);
                    circ2.StrokeThickness = 5;
                    circ2.Opacity = 0.8;
                    circ2.Height = 40;
                    circ2.Width = 40;
                    pin2.Content = circ2;
                    pin2.PositionOrigin = new Point(0.5, 0.5);

                    MapOverlay pin3 = new MapOverlay();
                    pin3.GeoCoordinate = new GeoCoordinate(60.27, 24.70);
                    Ellipse circ3 = new Ellipse();
                    circ3.Fill = new SolidColorBrush(Colors.Green);
                    circ3.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Purple);
                    circ3.StrokeThickness = 5;
                    circ3.Opacity = 0.8;
                    circ3.Height = 40;
                    circ3.Width = 40;
                    pin3.Content = circ3;

                    pin3.PositionOrigin = new Point(0.5, 0.5);

                    markerLayer.Add(pin1);
                    markerLayer.Add(pin2);
                    markerLayer.Add(pin3);
                    map1.Layers.Add(markerLayer);
                }
            }
        }

        bool PointInRectangle(Point pt, double North, double East, double South, double West)
        {
            // you may want to check that the point is a valid coordinate
            if (West < East)
            {
                return pt.X < East && pt.X > West && pt.Y < North && pt.Y > South;
            }
            else // it crosses the date line
            {
                return (pt.X < East || pt.X > West) && pt.Y < North && pt.Y > South;
            }
        }

        void FitToView()
        {
            LocationRectangle setRect = null;
            if (selected_shape == "Polygon" && (poly != null))
            {
                Debug.WriteLine("Fitting polygon into the view");

                setRect = LocationRectangle.CreateBoundingRectangle(poly.Path);
            }
            else if (selected_shape == "Polyline" && (polyline != null))
            {
                double north = 0;
                double west = 0;
                double south = 0;
                double east = 0;

                north = south = polyline.Path[0].Latitude;
                west = east = polyline.Path[0].Longitude;

                foreach (var p in polyline.Path.Skip(1))
                {
                    if (north < p.Latitude) north = p.Latitude;
                    if (west > p.Longitude) west = p.Longitude;
                    if (south > p.Latitude) south = p.Latitude;
                    if (east < p.Longitude) east = p.Longitude;
                }

                setRect = new LocationRectangle(north, west, south, east);
            }
            else if (selected_shape == "Markers" && (markerLayer != null))
            {
                Debug.WriteLine("Fitting: " + markerLayer.Count() + " markers into the view");
                GeoCoordinate[] geoArr = new GeoCoordinate[markerLayer.Count()];
                for (var p = 0; p < markerLayer.Count(); p++)
                {
                    geoArr[p] = markerLayer[p].GeoCoordinate;
                }

                setRect = LocationRectangle.CreateBoundingRectangle(geoArr);
            }

            if (setRect != null)
            {
                map1.SetView(setRect);
            }
        }

        void RevomeItem()
        {
            if (selected_shape == "Polygon")
            {
                if (poly != null)
                {
                    map1.MapElements.Remove(poly);
                    poly = null;
                }
            }
            else if (selected_shape == "Polyline")
            {
                if (polyline != null)
                {
                    map1.MapElements.Remove(polyline);
                    polyline = null;
                }
            }
            else if (selected_shape == "Markers")
            {
                if (markerLayer != null)
                {
                    map1.Layers.Remove(markerLayer);
                    markerLayer = null;
                }
            }
        }

        void ToggleVisibility()
        {
            if (selected_shape == "Polygon")
            {
                if (poly != null)
                {
                    if (poly.StrokeColor == Color.FromArgb(0xFF, 0x00, 0x00, 0xFF))
                    {
                        poly.FillColor = Color.FromArgb(0x00, 0x00, 0xFF, 0x00);
                        poly.StrokeColor = Color.FromArgb(0x00, 0x00, 0x00, 0xFF);
                    }
                    else
                    {
                        poly.FillColor = Color.FromArgb(0x55, 0x00, 0xFF, 0x00);
                        poly.StrokeColor = Color.FromArgb(0xFF, 0x00, 0x00, 0xFF);
                    }
                }
            }
            else if (selected_shape == "Polyline")
            {
                if (polyline != null)
                {
                    if(polyline.StrokeColor == Color.FromArgb(0xFF, 0xFF, 0x00, 0x00)){
                        polyline.StrokeColor = Color.FromArgb(0x00, 0xFF, 0x00, 0x00);
                    }else{
                        polyline.StrokeColor = Color.FromArgb(0xFF, 0xFF, 0x00, 0x00);
                    }
                }
            }
            else if (selected_shape == "Markers")
            {
                if (markerLayer != null)
                {
                    for (var i = 0; i < markerLayer.Count(); i++)
                    {
                        Ellipse markker = (markerLayer[i].Content as Ellipse);
                        if (markker != null)
                        {
                            if (markker.Visibility == System.Windows.Visibility.Visible)
                            {
                                Debug.WriteLine("Set marker Visibility off ");
                                markker.Visibility = System.Windows.Visibility.Collapsed;
                            }
                            else
                            {
                                Debug.WriteLine("Set marker Visibility on ");
                                markker.Visibility = System.Windows.Visibility.Visible;
                            }
                        }
                    }
                }
            }
        }
    }
}