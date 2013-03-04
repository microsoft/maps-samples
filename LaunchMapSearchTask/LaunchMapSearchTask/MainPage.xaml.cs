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
using Microsoft.Phone.Maps.Controls;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Device.Location;
using System.Collections.ObjectModel;
using Microsoft.Phone.Maps.Services;

using Microsoft.Phone.Tasks;
using System.Device.Location;

namespace LaunchMapSearchTask
{
    public partial class MainPage : PhoneApplicationPage
    {
        GeoCoordinateWatcher watcher = null;
        GeoCoordinate myPosition = null;
        MapPolygon PolyCircle = null;

        bool draggingNow = false;
        MapLayer markerLayer = null; 
        MapOverlay oneMarker = null;

        public MainPage()
        {
            InitializeComponent();

            markerLayer = new MapLayer();
            map1.Layers.Add(markerLayer);

            System.Windows.Input.Touch.FrameReported += Touch_FrameReported;

            map1.Tap += map1_Tap;
            map1.ZoomLevelChanged += map1_ZoomLevelChanged;

            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            watcher.MovementThreshold = 20; // 20 meters

            watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(OnStatusChanged);
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(OnPositionChanged);

            watcher.Start();

            AddResultToMap(map1.Center);
            zoomSlider.Value = map1.ZoomLevel;  
        }

        void SetMarkerLocation(GeoCoordinate newCoordinate)
        {
            oneMarker.GeoCoordinate = newCoordinate;
        }

        void map1_ZoomLevelChanged(object sender, MapZoomLevelChangedEventArgs e)
        {
            zoomSlider.Value = map1.ZoomLevel;
        }

        void map1_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (oneMarker != null)
            {
                Point markPoint = map1.ConvertGeoCoordinateToViewportPoint(oneMarker.GeoCoordinate);

                if ((markPoint.X < 0 || markPoint.Y < 0)
                || (map1.ActualWidth < markPoint.X) || (map1.ActualHeight < markPoint.Y))
                {
                    // tap event when we do not have the marker visible, so lets move it here
                    SetMarkerLocation(map1.ConvertViewportPointToGeoCoordinate(e.GetPosition(map1)));
                }
            }
        }

        void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            if (draggingNow == true)
            {
                TouchPoint tp = e.GetPrimaryTouchPoint(map1);

                if (tp.Action == TouchAction.Move)
                {
                    if (oneMarker != null)
                    {
                        SetMarkerLocation(map1.ConvertViewportPointToGeoCoordinate(tp.Position));
                    }
                }
                else if (tp.Action == TouchAction.Up)
                {
                    draggingNow = false;
                    map1.IsEnabled = true;
                }
            }
        }

        void textt_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (oneMarker != null)
            {
                draggingNow = true;
                map1.IsEnabled = false;
            }
        }

        private void AddResultToMap(GeoCoordinate location)
        {
            if (markerLayer != null)
            {
                map1.Layers.Remove(markerLayer);
                markerLayer = null;
            }

            markerLayer = new MapLayer();
            map1.Layers.Add(markerLayer);

            oneMarker = new MapOverlay();
            SetMarkerLocation(location);

            Ellipse Circhegraphic = new Ellipse();
            Circhegraphic.Fill = new SolidColorBrush(Colors.Yellow);
            Circhegraphic.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            Circhegraphic.StrokeThickness = 30;
            Circhegraphic.Opacity = 0.8;
            Circhegraphic.Height = 80;
            Circhegraphic.Width = 80;

            oneMarker.Content = Circhegraphic;

            oneMarker.PositionOrigin = new Point(0.5, 0.5);
            Circhegraphic.MouseLeftButtonDown += textt_MouseLeftButtonDown;

            markerLayer.Add(oneMarker);

            map1.Center = oneMarker.GeoCoordinate;
        }

        


        private void SearchTerm_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtbox = (sender as TextBox);
            if (txtbox != null && (txtbox.Text.Length > 0))
            {
                LaunchButton.IsEnabled = true;
            }
            else
            {
                LaunchButton.IsEnabled = false;
            }
        }

        private void zoomSlider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (zoomSlider != null)
            {
                map1.ZoomLevel = zoomSlider.Value;
            }
        }

        void OnPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            myPosition = e.Position.Location;
            MylocDot.Fill = new SolidColorBrush(Colors.Green);
            UpDateMyPositionCircle();
        }

        void OnStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Ready:
                    break;
                case GeoPositionStatus.Disabled:
                case GeoPositionStatus.Initializing:
                case GeoPositionStatus.NoData:
                default:
                    {
                        myPosition = null;
                        MylocDot.Fill = new SolidColorBrush(Colors.Gray);
                        UpDateMyPositionCircle();
                    }
                    break;
            }
        }

        void UpDateMyPositionCircle()
        {
            if (myPosition != null)
            {
                double accuracy = myPosition.HorizontalAccuracy;

                if (accuracy < myPosition.VerticalAccuracy)
                {
                    accuracy = myPosition.VerticalAccuracy;
                }

                if (PolyCircle == null)
                {
                    PolyCircle = new MapPolygon();

                    PolyCircle.FillColor = Color.FromArgb(0x55, 0x00, 0xFF, 0x00);
                    PolyCircle.StrokeColor = Color.FromArgb(0xFF, 0x00, 0xFF, 0x00);
                    PolyCircle.StrokeThickness = 1;

                    map1.MapElements.Add(PolyCircle);
                }

                if (accuracy < 50)
                {
                    accuracy = 50; // to be able to show the polygon
                }

                PolyCircle.Path = CreateCircle(myPosition, accuracy);
            }
            else if (PolyCircle != null)
            {
                map1.MapElements.Remove(PolyCircle);
                PolyCircle = null;
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

        void myLocation_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (myPosition != null)
            {
                map1.Center = myPosition;
            }
        }

        private void Button_gridbut_Click(object sender, RoutedEventArgs e)
        {
            if (sender == LaunchButton)
            {
                MapsTask mapsTask = new MapsTask();

                //You could Omit the Center property to use the user's current location.
                mapsTask.Center = oneMarker.GeoCoordinate;
                mapsTask.SearchTerm = SearchTermBox.Text;
                mapsTask.ZoomLevel = zoomSlider.Value;

                mapsTask.Show();
            }
        }
    }
}