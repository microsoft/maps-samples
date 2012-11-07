

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

namespace LocationSelector
{
    public partial class LocationSelectorPage : PhoneApplicationPage
    {
        
        bool isOrigin = false;
        bool draggingNow = false;
        MapLayer markerLayer = null;
        GeocodeQuery geoQ = null;
        MapOverlay oneMarker = null;

        // Constructor
        public LocationSelectorPage()
        {
            InitializeComponent();

            markerLayer = new MapLayer();
            map1.Layers.Add(markerLayer);

            geoQ = new GeocodeQuery();
            geoQ.QueryCompleted += geoQ_QueryCompleted;
            Debug.WriteLine("All construction done for GeoCoding");

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

            if (target.Length > 0 && (target == "Origin")){
                isOrigin = true;
                if ((Application.Current as App).RouteOriginLocation == null)
                {
                    AddResultToMap(map1.Center);
                }
                else
                {
                    AddResultToMap((Application.Current as App).RouteOriginLocation);
                }
            }else{
                isOrigin = false;
                if ((Application.Current as App).SelectedLocation == null)
                {
                    AddResultToMap(map1.Center);
                }
                else
                {
                    AddResultToMap((Application.Current as App).SelectedLocation);
                }
            }

            zoomSlider.Value = map1.ZoomLevel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (OkBut == sender)
            {
                if (isOrigin)
                {
                    (Application.Current as App).RouteOriginLocation = oneMarker.GeoCoordinate;
                }
                else
                {
                    (Application.Current as App).SelectedLocation = oneMarker.GeoCoordinate;
                }
            }

           this.NavigationService.GoBack();
        }
        

        private void GeoButton_Click(object sender, RoutedEventArgs e)
        {
            if (geoBox.Text.Length > 0)
            {
                if (geoQ.IsBusy == true)
                {
                    geoQ.CancelAsync();
                }

                geoQ.GeoCoordinate = map1.Center;
                geoQ.SearchTerm = geoBox.Text;
                geoQ.MaxResultCount = 1;

                geoQ.QueryAsync();
                Debug.WriteLine("GeocodeAsync started for: " + geoBox.Text);
            }
        }

        void geoQ_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            Debug.WriteLine("Geo query, error: " + e.Error);
            Debug.WriteLine("Geo query, cancelled: " + e.Cancelled);
            Debug.WriteLine("Geo query, cancelled: " + e.UserState.ToString());
            Debug.WriteLine("Geo query, Result.Count(): " + e.Result.Count());

            if (e.Result.Count() > 0)
            {
                oneMarker.GeoCoordinate = e.Result[0].GeoCoordinate;
                map1.Center = oneMarker.GeoCoordinate;
            }
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

            markerLayer.Add(oneMarker);

            map1.Center = oneMarker.GeoCoordinate;
        }

        private void geoBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
          
        }
    }
}