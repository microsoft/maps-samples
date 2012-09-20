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
using RevGeoCoding.Resources;
using Microsoft.Phone.Maps.Controls;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Device.Location;
using System.Collections.ObjectModel;

using Microsoft.Phone.Maps.Services;

namespace RevGeoCoding
{
    public partial class MainPage : PhoneApplicationPage
    {
        MapLayer markerLayer = null;
        ReverseGeocodeQuery geoQ = null;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            AddResultToMap("GC", map1.Center);

            map1.CenterChanged += map1_CenterChanged;

            geoQ = new ReverseGeocodeQuery();
            geoQ.QueryCompleted += geoQ_QueryCompleted;
            Debug.WriteLine("All construction done for GeoCoding");
        }

        void map1_CenterChanged(object sender, MapCenterChangedEventArgs e)
        {
            if(markerLayer != null && (markerLayer.Count > 0)){
                markerLayer[0].GeoCoordinate = map1.Center;
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
            MapOverlay oneMarker = new MapOverlay();
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
            Canvas.SetLeft(textt,10);
            Canvas.SetTop(textt, 5);
            Canvas.SetZIndex(textt, 5);

            canCan.Children.Add(textt);
            oneMarker.Content = canCan;

            oneMarker.PositionOrigin = new Point(0.5, 0.5);
            textt.MouseLeftButtonUp += textt_MouseLeftButtonUp;

            markerLayer.Add(oneMarker);
            map1.Layers.Add(markerLayer);
        }

        void textt_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (geoQ.IsBusy == true)
            {
                geoQ.CancelAsync();
            }
            // Set the geo coordinate for the query
            geoQ.GeoCoordinate = map1.Center;

            geoQ.QueryAsync();
            Debug.WriteLine("RevGeocodeAsync started for location: ");
        }


        void geoQ_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            Debug.WriteLine("Geo query, error: " + e.Error);
            Debug.WriteLine("Geo query, cancelled: " + e.Cancelled);
            Debug.WriteLine("Geo query, cancelled: " + e.UserState.ToString());
            Debug.WriteLine("Geo query, Result.Count(): " + e.Result.Count());


            if (e.Result.Count() > 0)
            {
                string showString = e.Result[0].Information.Name;
                showString = showString + "\nAddress: ";
                showString = showString + "\n" + e.Result[0].Information.Address.HouseNumber + " " +e.Result[0].Information.Address.Street;
                showString = showString + "\n" + e.Result[0].Information.Address.PostalCode + " " + e.Result[0].Information.Address.City; 
                showString = showString + "\n" + e.Result[0].Information.Address.Country + " " + e.Result[0].Information.Address.CountryCode;
                showString = showString + "\nDescription: ";
                showString = showString + "\n" + e.Result[0].Information.Description.ToString();

                MessageBox.Show(showString);
            }   
        }
    }
}