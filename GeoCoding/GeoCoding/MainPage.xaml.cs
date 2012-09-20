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
using GeoCoding.Resources;
using Microsoft.Phone.Maps.Controls;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Device.Location;
using System.Collections.ObjectModel;

using Microsoft.Phone.Maps.Services;

namespace GeoCoding
{
    public partial class MainPage : PhoneApplicationPage
    {
        MapLayer markerLayer = null;
        GeocodeQuery geoQ = null;
        IList<MapLocation> resList = null;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            geoQ = new GeocodeQuery();
            geoQ.QueryCompleted += geoQ_QueryCompleted;
            Debug.WriteLine("All construction done for GeoCoding");
        }

        private void GeoButton_Click(object sender, RoutedEventArgs e)
        {
            if (markerLayer != null)
            {
                map1.Layers.Remove(markerLayer);
                markerLayer = null;
            }

            markerLayer = new MapLayer();
            map1.Layers.Add(markerLayer);

            if (geoQ.IsBusy == true){
                geoQ.CancelAsync();
            }
            // Set the full address query

            GeoCoordinate setMe = new GeoCoordinate(map1.Center.Latitude, map1.Center.Longitude);
            setMe.HorizontalAccuracy = 1000000;

            geoQ.GeoCoordinate = setMe;
            geoQ.SearchTerm = geoBox.Text;
            geoQ.MaxResultCount = 200;

            geoQ.QueryAsync();
            Debug.WriteLine("GeocodeAsync started for: " + geoBox.Text);
        }

        void geoQ_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            // The result is a GeocodeResponse object
            resList = e.Result;

            Debug.WriteLine("Geo query, error: " + e.Error);
            Debug.WriteLine("Geo query, cancelled: " + e.Cancelled);
            Debug.WriteLine("Geo query, cancelled: " + e.UserState.ToString());
            Debug.WriteLine("Geo query, Result.Count(): " + resList.Count());


            if (resList.Count() > 0)
            {
                for (int i = 0; i < resList.Count(); i++)
                {
                    Debug.WriteLine("Result no.: " + i);

                    Debug.WriteLine("Name: " + resList[i].Information.Name);
                    Debug.WriteLine("Address.ToString: " + resList[i].Information.Address.ToString());
                    Debug.WriteLine("Address.District: " + resList[i].Information.Address.District);
                    Debug.WriteLine("Address.Country: " + resList[i].Information.Address.CountryCode + ": " + resList[i].Information.Address.Country);
                    Debug.WriteLine("Address.County: " + resList[i].Information.Address.County);
                    Debug.WriteLine("Address.Neighborhood: " + resList[i].Information.Address.Neighborhood);
                    Debug.WriteLine("Address.Street: " + resList[i].Information.Address.Street);
                    Debug.WriteLine("Address.PostalCode: " + resList[i].Information.Address.PostalCode);
                    Debug.WriteLine("Address.Continent: " + resList[i].Information.Address.Continent);
                    
                    Debug.WriteLine("GeoCoordinate.Latitude: " + resList[i].GeoCoordinate.Latitude.ToString());
                    Debug.WriteLine("GeoCoordinate.Longitude: " + resList[i].GeoCoordinate.Longitude.ToString());

                    string numNum = "0" + i;
                    if (i > 9){
                        numNum = "" + i;
                    }

                    AddResultToMap(numNum, resList[i].GeoCoordinate);
                }
            }

            if ((markerLayer != null)) // fit all
            {
                if (markerLayer.Count() == 1)
                {
                    map1.Center = markerLayer[0].GeoCoordinate;
                }
                else
                {

                    bool gotRect = false;
                    double north = 0;
                    double west = 0;
                    double south = 0;
                    double east = 0;

                    for (var p = 0; p < markerLayer.Count(); p++ )
                    {
                        if (!gotRect)
                        {
                            gotRect = true;
                            north = south = markerLayer[p].GeoCoordinate.Latitude;
                            west = east = markerLayer[p].GeoCoordinate.Longitude;
                        }
                        else
                        {
                            if (north < markerLayer[p].GeoCoordinate.Latitude) north = markerLayer[p].GeoCoordinate.Latitude;
                            if (west > markerLayer[p].GeoCoordinate.Longitude) west = markerLayer[p].GeoCoordinate.Longitude;
                            if (south > markerLayer[p].GeoCoordinate.Latitude) south = markerLayer[p].GeoCoordinate.Latitude;
                            if (east < markerLayer[p].GeoCoordinate.Longitude) east = markerLayer[p].GeoCoordinate.Longitude;
                        }
                    }

                    if (gotRect)
                    {
                        map1.SetView(new LocationRectangle(north, west, south, east));
                    }
                }
            }
        }

        private void AddResultToMap(String text, GeoCoordinate location)
        {

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
        }

        void textt_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("textt_MouseLeftButtonUp");
            TextBlock textt = sender as TextBlock;
            if (textt != null && (resList != null)){
                int hint = int.Parse(textt.Text);

                if (hint >= 0 && hint < resList.Count())
                {
                    
                    string showString = resList[hint].Information.Name;
                    showString = showString + "\nAddress: ";
                    showString = showString + "\n" + resList[hint].Information.Address.HouseNumber + " " +resList[hint].Information.Address.Street;
                    showString = showString + "\n" + resList[hint].Information.Address.PostalCode + " " + resList[hint].Information.Address.City; 
                    showString = showString + "\n" + resList[hint].Information.Address.Country + " " + resList[hint].Information.Address.CountryCode;
                    showString = showString + "\nDescription: ";
                    showString = showString + "\n" + resList[hint].Information.Description.ToString();

                    MessageBox.Show(showString);
                }
            }
            
        }

    }
}