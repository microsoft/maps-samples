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
using MyLocation.Resources;

using Microsoft.Phone.Maps.Controls;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;

using System.Device.Location;
using System.Windows.Threading;

namespace MyLocation
{
    public partial class MainPage : PhoneApplicationPage
    {
        GeoCoordinateWatcher watcher = null;
        TextBlock statusText = null;
        TextBlock lastValues = null;
        TextBlock latitudeText = null;
        TextBlock longitudeText = null;
        TextBlock accurazyText = null;
        TextBlock headingText = null;

        MapPolygon PolyCircle = null;
        DispatcherTimer timmer;

        int SecondsCounter = 0;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Debug.WriteLine("We are started"); //Check to see if the "Redirect all Output Window text to the Immediate Window" is checked under Tools -> Options -> Debugging -> General.  

            AddStatusPopUp();

            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            watcher.MovementThreshold = 20; // 20 meters

            watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(OnStatusChanged);
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(OnPositionChanged);

            watcher.Start();

            timmer = new DispatcherTimer();
            timmer.Tick += delegate(object s, EventArgs args)
            {
                SecondsCounter++;
                lastValues.Text = "Last: " + SecondsCounter + " sec.";
            };
            timmer.Interval = new TimeSpan(0, 0, 1); // one second
            timmer.Start();
        }

        private void AddStatusPopUp()
        {
            Popup SelectionPopupP = new Popup();


            StackPanel horpanel = new StackPanel();
            horpanel.Background = new SolidColorBrush(Colors.Black);
            horpanel.Opacity = 0.7;

            statusText = new TextBlock();
            statusText.FontSize = 20;
            statusText.Foreground = new SolidColorBrush(Colors.White);
            statusText.Text = "Status: unknown";

            lastValues = new TextBlock();
            lastValues.FontSize = 20;
            lastValues.Foreground = new SolidColorBrush(Colors.White);
            lastValues.Text = "Last: 0 sec.";

            latitudeText = new TextBlock();
            latitudeText.FontSize = 20;
            latitudeText.Foreground = new SolidColorBrush(Colors.White);
            latitudeText.Text = "Lat: ";

            longitudeText = new TextBlock();
            longitudeText.FontSize = 20;
            longitudeText.Foreground = new SolidColorBrush(Colors.White);
            longitudeText.Text = "Lon: ";

            accurazyText = new TextBlock();
            accurazyText.FontSize = 20;
            accurazyText.Foreground = new SolidColorBrush(Colors.White);
            accurazyText.Text = "Acc: ";

            headingText = new TextBlock();
            headingText.FontSize = 20;
            headingText.Foreground = new SolidColorBrush(Colors.White);
            headingText.Text = "Head: ";

            horpanel.Children.Add(statusText);
            horpanel.Children.Add(lastValues);
            horpanel.Children.Add(latitudeText);
            horpanel.Children.Add(longitudeText);
            horpanel.Children.Add(accurazyText);
            horpanel.Children.Add(headingText);
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


        void OnStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            Debug.WriteLine("OnStatusChanged: " + e.ToString());

            if (statusText != null)
            {
                if (e.Status == GeoPositionStatus.Disabled)
                {
                    statusText.Text = "Status: Disabled";
                }
                else if (e.Status == GeoPositionStatus.Initializing)
                {
                    statusText.Text = "Status: Initializing";
                }
                else if (e.Status == GeoPositionStatus.NoData)
                {
                    statusText.Text = "Status: NoData";
                }
                else if (e.Status == GeoPositionStatus.Ready)
                {
                    statusText.Text = "Status: Ready";
                }
                else
                {
                    statusText.Text = "Status: " + e.ToString();
                }
            }

        }


        void OnPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            SecondsCounter = 0; //reset counter
            double accuracy = e.Position.Location.HorizontalAccuracy;

            if (accuracy < e.Position.Location.VerticalAccuracy)
            {
                accuracy = e.Position.Location.VerticalAccuracy;
            }

            if (PolyCircle == null)
            {
                PolyCircle = new MapPolygon();

                PolyCircle.FillColor = Color.FromArgb(0x55, 0x00, 0xFF, 0x00);
                PolyCircle.StrokeColor = Color.FromArgb(0xFF, 0x00, 0x00, 0xFF);
                PolyCircle.StrokeThickness = 4;

                map1.MapElements.Add(PolyCircle);
            }
            Debug.WriteLine("locationa ccuracy :" +accuracy);
            
            if(accuracy < 50){
                accuracy = 50; // to be able to show the polygon
            }

            PolyCircle.Path = CreateCircle(e.Position.Location, accuracy);

            map1.Center = e.Position.Location;

            if (accuracy < 100)
            {
                map1.ZoomLevel = 16;
            }
            else
            {
                map1.ZoomLevel = 10;
            }
            if (latitudeText != null)
            {
                latitudeText.Text = "Lat: " + e.Position.Location.Latitude.ToString();
            }
            if (longitudeText != null)
            {
                longitudeText.Text = "Lon: " + e.Position.Location.Longitude.ToString();
            }
            if (accurazyText != null)
            {
                accurazyText.Text = "Acc: " + accuracy.ToString();
            }
            if (headingText != null)
            {
                headingText.Text = "Head: " + e.Position.Location.Course.ToString();
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
            var earthRadius = 6367000; // radius in meters
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
    }
}