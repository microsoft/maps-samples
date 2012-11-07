

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

namespace AreaSelector
{
    public partial class AreaSelectorPage : PhoneApplicationPage
    {
        int areaRadius = 10000;
        bool draggingNow = false;
        MapLayer markerLayer = null;
     
        MapOverlay oneMarker = null;

        MapPolygon PolyCircle = null;

        // Constructor
        public AreaSelectorPage()
        {
            InitializeComponent();

            markerLayer = new MapLayer();
            map1.Layers.Add(markerLayer);


            System.Windows.Input.Touch.FrameReported += Touch_FrameReported;

            map1.Tap += map1_Tap;
            map1.ZoomLevelChanged += map1_ZoomLevelChanged;
        }

        void map1_ZoomLevelChanged(object sender, MapZoomLevelChangedEventArgs e)
        {
            zoomSlider.Value = map1.ZoomLevel;
        }

        void map1_Tap(object sender, GestureEventArgs e)
        {
            if (oneMarker != null)
            { 
                Point markPoint = map1.ConvertGeoCoordinateToViewportPoint(oneMarker.GeoCoordinate);

                if ((markPoint.X < 0 || markPoint.Y < 0)
                || (map1.ActualWidth < markPoint.X) || (map1.ActualHeight < markPoint.Y))
                {
                    // tap event when we do not have the marker visible, so lets move it here
                    oneMarker.GeoCoordinate = map1.ConvertViewportPointToGeoCoordinate(e.GetPosition(map1));

                    if (PolyCircle != null)
                    {
                        GeoCoordinateCollection boundingLocations = CreateCircle(oneMarker.GeoCoordinate, areaRadius);
                        PolyCircle.Path = boundingLocations;
                    }
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
                        oneMarker.GeoCoordinate = map1.ConvertViewportPointToGeoCoordinate(tp.Position);
                        if (PolyCircle != null)
                        {
                            GeoCoordinateCollection boundingLocations = CreateCircle(oneMarker.GeoCoordinate, areaRadius);
                            PolyCircle.Path = boundingLocations;
                        }
                    }
                }
                else if (tp.Action == TouchAction.Up)
                {
                    draggingNow = false;
                    map1.IsEnabled = true;
                }
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string target = "";

            if (this.NavigationContext.QueryString.ContainsKey("target"))
            {
                target = this.NavigationContext.QueryString["target"];
            }

            Debug.WriteLine("OnNavigatedTo, target: " + target);

            base.OnNavigatedTo(e);

            TitleBox.Text = "Select " + target;

            if ((Application.Current as App).CircleAreaRadius != null && (Application.Current as App).CircleAreaRadius > 0)
            {
                areaRadius = (Application.Current as App).CircleAreaRadius;
            }
            else
            {
                areaRadius = 10000;
            }

            if ((Application.Current as App).SelectedLocation == null)
            {
                AddResultToMap(map1.Center);
            }
            else
            {
                    AddResultToMap((Application.Current as App).SelectedLocation);
            }

            zoomSlider.Value = map1.ZoomLevel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (OkBut == sender)
            {
                (Application.Current as App).SelectedLocation = oneMarker.GeoCoordinate;
                (Application.Current as App).CircleAreaRadius = areaRadius;
            }

           this.NavigationService.GoBack();
        }
        


        void textt_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("textt_MouseLeftButtonDown");
            if (oneMarker != null)
            {
                draggingNow = true;
                map1.IsEnabled = false;
            }
        }


        private void zoomSlider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (zoomSlider != null)
            {
                map1.ZoomLevel = zoomSlider.Value;
            }
        }

        private void AddResultToMap(GeoCoordinate location)
        {
            if (PolyCircle != null)
            {
                map1.MapElements.Remove(PolyCircle);
                PolyCircle = null;
            }
            if (markerLayer != null)
            {
                map1.Layers.Remove(markerLayer);
                markerLayer = null;
            }

            markerLayer = new MapLayer();
            map1.Layers.Add(markerLayer);

            oneMarker = new MapOverlay();
            oneMarker.GeoCoordinate = location;

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

            PolyCircle = new MapPolygon();

            GeoCoordinateCollection boundingLocations = CreateCircle(oneMarker.GeoCoordinate, areaRadius);

            //Set the polygon properties
            PolyCircle.Path = boundingLocations;
            PolyCircle.FillColor = Color.FromArgb(0x55, 0xFF, 0xFF, 0x00);
            PolyCircle.StrokeColor = Color.FromArgb(0xFF, 0xFF, 0x00, 0xFF);
            PolyCircle.StrokeThickness = 1;

            map1.MapElements.Add(PolyCircle);
            
            markerLayer.Add(oneMarker);

            map1.Center = oneMarker.GeoCoordinate;
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
            var earthRadius = 6367000.0; // radius in meters
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