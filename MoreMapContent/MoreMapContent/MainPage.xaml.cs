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
using MoreMapContent.Resources;
using Microsoft.Phone.Maps.Controls;
using System.Diagnostics;
using System.Device.Location;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;

namespace MoreMapContent
{
    public partial class MainPage : PhoneApplicationPage
    {
        String selected_shape = "";
        MapLayer Rectangle1 = null;
        MapLayer Circle = null;
        GeoCoordinate Circlepoint = null;
        GeoCoordinate Rectangepoint = null;

        MapPolygon PolyCircle = null;
        MapPolygon PolyRect = null;

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

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("We are loaded");
        }

        private void Button_Menubut_Click(object sender, RoutedEventArgs e)
        {
            if (sender == CanCelMenuBut)
            {
                MenuSelectionGrip.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                if (sender == PolygonsBut)
                {
                    selected_shape = "Polygons";
                }
                else if (sender == RectangleBut)
                {
                    selected_shape = "Rectangle";
                }
                else if (sender == CircleBut)
                {
                    selected_shape = "Circle";
                }

                MenuSelectionLiat.SelectedIndex = 0;
                MenuSelectionGrip.Visibility = System.Windows.Visibility.Visible;
            }
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
            if (selected_shape == "Rectangle")
            {
                if (Rectangle1 == null)
                {
                    Rectangle1 = new MapLayer();

                    Rectangle rectanggle = new Rectangle();
                    rectanggle.Opacity = 0.7;

                    //Set the Rectangle properties
                    rectanggle.Name = "jukka";
                    rectanggle.Fill = new SolidColorBrush(Colors.Green);
                    rectanggle.Stroke = new SolidColorBrush(Colors.Blue);
                    rectanggle.StrokeThickness = 4;
                    rectanggle.Width = 200;
                    rectanggle.Height = 200;

                    Rectangepoint = new GeoCoordinate(60.22, 24.81);

                    MapOverlay pin1 = new MapOverlay();
                    pin1.GeoCoordinate = Rectangepoint;
                   // pin1.PositionOrigin = PositionOrigin.Center;
                    pin1.Content = rectanggle;

                    Rectangle1.Add(pin1);
                    map1.Layers.Add(Rectangle1);
                }
            }
            else if (selected_shape == "Circle")
            {
                if (Circle == null)
                {
                    Circle = new MapLayer();

                    Ellipse Circhegraphic = new Ellipse();
                    Circhegraphic.Fill = new SolidColorBrush(Colors.Yellow);
                    Circhegraphic.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                    Circhegraphic.StrokeThickness = 10;
                    Circhegraphic.Opacity = 0.8;
                    Circhegraphic.Height = 100;
                    Circhegraphic.Width = 100;

                    Circlepoint = new GeoCoordinate(60.22, 24.81);

                    MapOverlay circc = new MapOverlay();
                    circc.GeoCoordinate = Circlepoint;
                    // pin1.PositionOrigin = PositionOrigin.Center;
                    circc.Content = Circhegraphic;

                    Circle.Add(circc);
                    map1.Layers.Add(Circle);
                }
            }
            else if (selected_shape == "Polygons")
            {
                if (PolyCircle == null)
                {

                    PolyCircle = new MapPolygon();

                    GeoCoordinateCollection boundingLocations = CreateCircle(new GeoCoordinate(60.30, 24.70), 10);

                    //Set the polygon properties
                    PolyCircle.Path = boundingLocations;
                    PolyCircle.FillColor = Color.FromArgb(0x55, 0xFF, 0xFF, 0x00);
                    PolyCircle.StrokeColor = Color.FromArgb(0xFF, 0xFF, 0x00, 0xFF);
                    PolyCircle.StrokeThickness = 4;

                    map1.MapElements.Add(PolyCircle);

                    PolyRect = new MapPolygon();

                    GeoCoordinateCollection RectLocations = CreateRectangle(new GeoCoordinate(60.35, 24.60), new GeoCoordinate(60.25, 24.80));

                    //Set the polygon properties
                    PolyRect.Path = RectLocations;
                    PolyRect.FillColor = Color.FromArgb(0x55, 0x00, 0x00, 0x00);
                    PolyRect.StrokeColor = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
                    PolyRect.StrokeThickness = 4;

                    map1.MapElements.Add(PolyRect);

                }
            }
        }

        void FitToView()
        {

            bool gotRect = false;
            double north = 0;
            double west = 0;
            double south = 0;
            double east = 0;

            if (selected_shape == "Rectangle" && (Rectangle1 != null))
            {
                map1.Center = Rectangepoint;
            }
            else if (selected_shape == "Circle" && (Circle != null))
            {
                map1.Center = Circlepoint;
            }
            else if (selected_shape == "Polygons" && (PolyCircle != null))
            {
                gotRect = true;

                north = south = PolyCircle.Path[0].Latitude;
                west = east = PolyCircle.Path[0].Longitude;

                foreach (var p in PolyCircle.Path.Skip(1))
                {
                    if (north < p.Latitude) north = p.Latitude;
                    if (west > p.Longitude) west = p.Longitude;
                    if (south > p.Latitude) south = p.Latitude;
                    if (east < p.Longitude) east = p.Longitude;
                }
            }

            if (gotRect)
            {
                map1.SetView(new LocationRectangle(north, west, south, east));
            }
        }

        void RevomeItem()
        {
            if (selected_shape == "Rectangle")
            {
                if (Rectangle1 != null)
                {
                    map1.Layers.Remove(Rectangle1);
                    Rectangle1 = null;
                }
            }
            else if (selected_shape == "Circle")
            {
                if (Circle != null)
                {
                    map1.Layers.Remove(Circle);
                    Circle = null;
                }
            }
            else if (selected_shape == "Polygons")
            {
                if (PolyCircle != null)
                {
                    map1.MapElements.Remove(PolyCircle);
                    PolyCircle = null;

                    map1.MapElements.Remove(PolyRect);
                    PolyRect = null;
                }
            }
        }

        void ToggleVisibility()
        {
            if (selected_shape == "Rectangle")
            {
                if (Rectangle1 != null && (Rectangle1.Count() > 0))
                {
                    Rectangle rect = (Rectangle1[0].Content as Rectangle);

                    if (rect.Visibility == System.Windows.Visibility.Visible)
                    {
                        Debug.WriteLine("Set Rectangle Visibility off ");
                        rect.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        Debug.WriteLine("Set Rectangle Visibility on ");
                        rect.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
            else if (selected_shape == "Circle")
            {
                if (Circle != null && (Circle.Count() >  0))
                {
                    Ellipse circcle = (Circle[0].Content as Ellipse);

                    if (circcle.Visibility == System.Windows.Visibility.Visible)
                    {
                        Debug.WriteLine("Set Circle Visibility off ");
                        circcle.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        Debug.WriteLine("Set Circle Visibility on ");
                        circcle.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
            else if (selected_shape == "Polygons")
            {
                if (PolyCircle != null)
                {
                    if (PolyCircle.StrokeColor == Color.FromArgb(0xFF, 0xFF, 0x00, 0xFF))
                    {
                        Debug.WriteLine("Set Polygon Visibility off ");
                        PolyCircle.FillColor = Color.FromArgb(0x00, 0xFF, 0xFF, 0x00);
                        PolyCircle.StrokeColor = Color.FromArgb(0x00, 0xFF, 0x00, 0xFF);
                        PolyRect.FillColor = Color.FromArgb(0x00, 0x00, 0x00, 0x00);
                        PolyRect.StrokeColor = Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF);
                    }
                    else
                    {
                        Debug.WriteLine("Set Polygon Visibility on ");
                        PolyCircle.FillColor = Color.FromArgb(0x55, 0xFF, 0xFF, 0x00);
                        PolyCircle.StrokeColor = Color.FromArgb(0xFF, 0xFF, 0x00, 0xFF);
                        PolyRect.FillColor = Color.FromArgb(0x55, 0x00, 0x00, 0x00);
                        PolyRect.StrokeColor = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
                    }
                }
            }
        }

        public static double ToRadian(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public static double ToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }

        public static GeoCoordinateCollection CreateCircle(GeoCoordinate center, double radius)
        {
            var earthRadius = 6367.0; // radius in kilometers
            var lat = ToRadian(center.Latitude); //radians
            var lng = ToRadian(center.Longitude); //radians
            var d = radius / earthRadius; // d = angular distance covered on earth's surface
            var locations = new GeoCoordinateCollection();

            for (var x = 0; x <= 360; x++)
            {
                var brng = ToRadian(x);
                var latRadians = Math.Asin(Math.Sin(lat) * Math.Cos(d) + Math.Cos(lat) * Math.Sin(d) * Math.Cos(brng));
                var lngRadians = lng + Math.Atan2(Math.Sin(brng) * Math.Sin(d) * Math.Cos(lat), Math.Cos(d) - Math.Sin(lat) * Math.Sin(latRadians));

                locations.Add(new GeoCoordinate(ToDegrees(latRadians), ToDegrees(lngRadians)));
            }

            return locations;
        }

        public static GeoCoordinateCollection CreateRectangle(GeoCoordinate topLeft, GeoCoordinate bottomRight)
        {
            var locations = new GeoCoordinateCollection();

            locations.Add(new GeoCoordinate(topLeft.Latitude, topLeft.Longitude));
            locations.Add(new GeoCoordinate(topLeft.Latitude, bottomRight.Longitude));
            locations.Add(new GeoCoordinate(bottomRight.Latitude, bottomRight.Longitude));
            locations.Add(new GeoCoordinate(bottomRight.Latitude, topLeft.Longitude));

            return locations;
        }
    }
}