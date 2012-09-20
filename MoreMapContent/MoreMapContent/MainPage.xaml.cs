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
        Popup PopupP;
        String selected_shape = "";
        MapLayer Rectangle = null;
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

            AddSelectionPopUp();
            map1.MouseLeftButtonDown += map1_MouseLeftButtonDown;
            PopupP = new Popup();
        }

        void map1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IList<MapElement> listt = map1.GetMapElementsAt(e.GetPosition(map1));
            Debug.WriteLine("map1_MouseLeftButtonDown, MapElement count: " + listt.Count());
        }

        private void AddSelectionPopUp()
        {
            Popup SelectionPopupP = new Popup();


            StackPanel horpanel = new StackPanel();
            horpanel.Orientation = System.Windows.Controls.Orientation.Horizontal;
            horpanel.Background = new SolidColorBrush(Colors.Black);
            horpanel.Opacity = 0.7;

            Button buttonPolygon = new Button();
            buttonPolygon.Content = "Polygons";
            buttonPolygon.Margin = new Thickness(5.0);
            buttonPolygon.Click += new RoutedEventHandler(Button_Polygons_Click);

            Button buttonRectangle = new Button();
            buttonRectangle.Content = "Rectangle";
            buttonRectangle.Margin = new Thickness(5.0);
            buttonRectangle.Click += new RoutedEventHandler(Button_Rectangle_Click);

            Button buttonCircle = new Button();
            buttonCircle.Content = "Circle";
            buttonCircle.Margin = new Thickness(5.0);
            buttonCircle.Click += new RoutedEventHandler(Button_Circle_Click);

            horpanel.Children.Add(buttonPolygon);
            horpanel.Children.Add(buttonRectangle);
            horpanel.Children.Add(buttonCircle);
            SelectionPopupP.Child = horpanel;


            // Set where the popup will show up on the screen.
            SelectionPopupP.VerticalOffset = 30;
            SelectionPopupP.HorizontalOffset = 20;

            // Open the popup.
            SelectionPopupP.IsOpen = true;

        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("We are loaded");
        }

        private void Button_Polygons_Click(object sender, RoutedEventArgs e)
        {
            selected_shape = "Polygons";
            OpenPopUpSelection();
        }

        private void Button_Rectangle_Click(object sender, RoutedEventArgs e)
        {
            selected_shape = "Rectangle";
            OpenPopUpSelection();
        }

        private void Button_Circle_Click(object sender, RoutedEventArgs e)
        {
            selected_shape = "Circle";
            OpenPopUpSelection();
        }

        private void OpenPopUpSelection()
        {
            // Create some content to show in the popup. Typically you would 
            // create a user control.
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Colors.Black);
            border.BorderThickness = new Thickness(5.0);


            StackPanel panel1 = new StackPanel();
            panel1.Background = new SolidColorBrush(Colors.Black);
            panel1.Opacity = 0.7;

            TextBlock textBox1 = new TextBlock();

            textBox1.Text = selected_shape;
            textBox1.FontSize = 60;
            textBox1.Width = 250;
            textBox1.Foreground = new SolidColorBrush(Colors.Blue);
            textBox1.Opacity = 1;

            ListBox listBox1 = new ListBox();

            listBox1.FontSize = 35;
            listBox1.Foreground = new SolidColorBrush(Colors.White);
            listBox1.Opacity = 1;

            listBox1.Items.Add("Add");
            listBox1.Items.Add("Remove");
            listBox1.Items.Add("Bring to top");
            listBox1.Items.Add("Sent to back");
            listBox1.Items.Add("Toggle visibility");
            listBox1.Items.Add("Fit to View");

            listBox1.SelectionChanged += SelectionChangedEventHandler;

            Button button1 = new Button();
            button1.Content = "Close";
            button1.Margin = new Thickness(5.0);
            button1.Foreground = new SolidColorBrush(Colors.White);
            button1.Opacity = 1;
            button1.Click += new RoutedEventHandler(Cancel_Click);

            panel1.Children.Add(textBox1);
            panel1.Children.Add(listBox1);
            panel1.Children.Add(button1);
            border.Child = panel1;

            // Set the Child property of Popup to the border 
            // which contains a stackpanel, textblock and button.
            PopupP.Child = border;

            // Set where the popup will show up on the screen.
            PopupP.VerticalOffset = 100;
            PopupP.HorizontalOffset = 50;

            // Open the popup.
            PopupP.IsOpen = true;

        }

        void SelectionChangedEventHandler(Object sender, SelectionChangedEventArgs e)
        {

            String sellected = (sender as ListBox).SelectedItem.ToString();

            Debug.WriteLine("ListBox1SelectedIndexChanged " + sellected);

            if (sellected == "Add")
            {
                AddItem();
            }
            else if (sellected == "Remove")
            {
                RevomeItem();
            }
            else if (sellected == "Bring to top")
            {
                BringtoTop();
            }
            else if (sellected == "Sent to back")
            {
                SentToBack();
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
                if (Rectangle == null)
                {
                    Rectangle = new MapLayer();

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

                    Rectangle.Add(pin1);
                    map1.Layers.Add( Rectangle);
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

            if (selected_shape == "Rectangle" && (Rectangle != null))
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
                if (Rectangle != null)
                {
                    map1.Layers.Remove(Rectangle);
                    Rectangle = null;
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
         /*   if (selected_shape == "Rectangle")
            {
                if (Rectangle != null)
                {
                    if (Rectangle.Visibility == System.Windows.Visibility.Visible)
                    {
                        Debug.WriteLine("Set Rectangle Visibility off ");
                        Rectangle.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        Debug.WriteLine("Set Rectangle Visibility on ");
                        Rectangle.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
            else if (selected_shape == "Circle")
            {
                if (Circle != null)
                {
                    if (Circle.Visibility == System.Windows.Visibility.Visible)
                    {
                        Debug.WriteLine("Set Circle Visibility off ");
                        Circle.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        Debug.WriteLine("Set Circle Visibility on ");
                        Circle.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
            else if (selected_shape == "Polygons")
            {
                if (PolyCircle != null)
                {
                    if (PolyCircle.Visibility == System.Windows.Visibility.Visible)
                    {
                        Debug.WriteLine("Set Polygon Visibility off ");
                        PolyCircle.Visibility = System.Windows.Visibility.Collapsed;
                        PolyRect.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        Debug.WriteLine("Set Polygon Visibility on ");
                        PolyCircle.Visibility = System.Windows.Visibility.Visible;
                        PolyRect.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }*/
        }

        void SentToBack()
        {
            UIElement PolygonElement = null;//PolyCircle as UIElement;
            UIElement CircleElement = null;//Circle as UIElement;
            UIElement RectangleElement = null;//Rectangle as UIElement;

            var Polygon = 0;
            var RectangleZ = 0;
            var lineZ = 0;

            if (PolygonElement != null)
            {
                Polygon = Canvas.GetZIndex(PolygonElement);
            }

            if (CircleElement != null)
            {
                lineZ = Canvas.GetZIndex(CircleElement);
            }

            if (RectangleElement != null)
            {
                RectangleZ = Canvas.GetZIndex(RectangleElement);
            }

            if (selected_shape == "Rectangle")
            {
                if (RectangleZ >= Polygon)
                    RectangleZ = Polygon - 1;

                if (RectangleZ >= lineZ)
                    RectangleZ = lineZ - 1;
            }
            else if (selected_shape == "Circle")
            {
                if (lineZ >= Polygon)
                    lineZ = Polygon - 1;

                if (lineZ >= RectangleZ)
                    lineZ = RectangleZ - 1;
            }
            else if (selected_shape == "Polygons")
            {
                if (Polygon >= lineZ)
                    Polygon = lineZ - 1;

                if (Polygon >= RectangleZ)
                    Polygon = RectangleZ - 1;
            }

            if (PolygonElement != null)
            {
                Debug.WriteLine("Set PolygonElement: " + Polygon);
                Canvas.SetZIndex(PolygonElement, Polygon);

                UIElement PolyRectEle = null;//PolyRect as UIElement;
                if (PolyRectEle != null)
                {
                    Canvas.SetZIndex(PolyRectEle, Polygon);
                }
            }

            if (CircleElement != null)
            {
                Debug.WriteLine("Set CircleElement: " + lineZ);
                Canvas.SetZIndex(CircleElement, lineZ);
            }

            if (RectangleElement != null)
            {
                Debug.WriteLine("Set RectangleElement: " + RectangleZ);
                Canvas.SetZIndex(RectangleElement, RectangleZ);
            }
        }

        void BringtoTop()
        {
            UIElement PolygonElement = null;// PolyCircle as UIElement;
            UIElement CircleElement = null;//Circle as UIElement;
            UIElement RectangleElement = null;//Rectangle as UIElement;

            var PolygonZ = 0;
            var RectangleZ = 0;
            var lineZ = 0;

            if (PolygonElement != null)
            {
                PolygonZ = Canvas.GetZIndex(PolygonElement);
            }

            if (CircleElement != null)
            {
                lineZ = Canvas.GetZIndex(CircleElement);
            }

            if (RectangleElement != null)
            {
                RectangleZ = Canvas.GetZIndex(RectangleElement);
            }

            if (selected_shape == "Rectangle")
            {
                if (RectangleZ <= PolygonZ)
                    RectangleZ = PolygonZ + 1;

                if (RectangleZ <= lineZ)
                    RectangleZ = lineZ + 1;
            }
            else if (selected_shape == "Circle")
            {
                if (lineZ <= PolygonZ)
                    lineZ = PolygonZ + 1;

                if (lineZ <= RectangleZ)
                    lineZ = RectangleZ + 1;
            }
            else if (selected_shape == "Polygons")
            {
                if (PolygonZ <= lineZ)
                    PolygonZ = lineZ + 1;

                if (PolygonZ <= RectangleZ)
                    PolygonZ = RectangleZ + 1;
            }

            if (PolygonElement != null)
            {
                Debug.WriteLine("Set PolygonElement: " + PolygonZ);
                Canvas.SetZIndex(PolygonElement, PolygonZ);

                UIElement PolyRectEle = null;//PolyRect as UIElement;
                if (PolyRectEle != null)
                {
                    Canvas.SetZIndex(PolyRectEle, PolygonZ);
                }
            }

            if (CircleElement != null)
            {
                Debug.WriteLine("Set CircleElement: " + lineZ);
                Canvas.SetZIndex(CircleElement, lineZ);
            }

            if (RectangleElement != null)
            {
                Debug.WriteLine("Set RectangleElement: " + RectangleZ);
                Canvas.SetZIndex(RectangleElement, RectangleZ);
            }
        }

        void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Close the popup.
            PopupP.IsOpen = false;
            Debug.WriteLine("Cancel_Click ");
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