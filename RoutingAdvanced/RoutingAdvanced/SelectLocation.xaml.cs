
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
using RoutingAdvanced.Resources;
using Microsoft.Phone.Maps.Controls;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Device.Location;
using System.Collections.ObjectModel;
using Microsoft.Phone.Maps.Services;

namespace RoutingAdvanced
{
    public partial class SelectLocation : PhoneApplicationPage
    {
        bool isEnd = false;
        bool draggingNow = false;
        MapLayer markerLayer = null;
        GeocodeQuery geoQ = null;
        MapOverlay oneMarker = null;

        // Constructor
        public SelectLocation()
        {
            InitializeComponent();

            markerLayer = new MapLayer();
            map1.Layers.Add(markerLayer);

            geoQ = new GeocodeQuery();
            geoQ.QueryCompleted += geoQ_QueryCompleted;
            Debug.WriteLine("All construction done for GeoCoding");

            System.Windows.Input.Touch.FrameReported += Touch_FrameReported;
            
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

            if (target.Length > 0 && (target == "End")){
                isEnd = true;
                AddResultToMap("X", (Application.Current as App).end_point);
            }else{
                isEnd = false;
                AddResultToMap("X", (Application.Current as App).start_point);
            }

            zoomSlider.Value = map1.ZoomLevel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (OkBut == sender)
            {
                if (isEnd)
                {
                    (Application.Current as App).end_point = oneMarker.GeoCoordinate;
                }
                else
                {
                    (Application.Current as App).start_point = oneMarker.GeoCoordinate;
                }
            }

           this.NavigationService.GoBack();
        }
        

        private void GeoButton_Click(object sender, RoutedEventArgs e)
        {
            if (geoQ.IsBusy == true){
                geoQ.CancelAsync();
            }
            // Set the full address query
            GeoCoordinate setMe = new GeoCoordinate(map1.Center.Latitude, map1.Center.Longitude);
            setMe.HorizontalAccuracy = 1000000;

            geoQ.GeoCoordinate = setMe;
            geoQ.SearchTerm = geoBox.Text;
            geoQ.MaxResultCount = 1;

            geoQ.QueryAsync();
            Debug.WriteLine("GeocodeAsync started for: " + geoBox.Text);
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

        private void AddResultToMap(String text, GeoCoordinate location)
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

            Canvas canCan = new Canvas();

            Ellipse Circhegraphic = new Ellipse();
            Circhegraphic.Fill = new SolidColorBrush(Colors.Brown);
            Circhegraphic.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
            Circhegraphic.StrokeThickness = 5;
            Circhegraphic.Opacity = 0.8;
            Circhegraphic.Height = 40;
            Circhegraphic.Width = 40;

            canCan.Children.Add(Circhegraphic);
            TextBlock textt = new TextBlock { Text = text };
            textt.HorizontalAlignment = HorizontalAlignment.Center;
            Canvas.SetLeft(textt, 10);
            Canvas.SetTop(textt, 5);
            Canvas.SetZIndex(textt, 5);

            canCan.Children.Add(textt);
            oneMarker.Content = canCan;

            oneMarker.PositionOrigin = new Point(0.5, 0.5);
            textt.MouseLeftButtonDown += textt_MouseLeftButtonDown;
            markerLayer.Add(oneMarker);

            map1.Center = oneMarker.GeoCoordinate;
        }
    }
}