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
using DynamicPolyLine.Resources;
using Microsoft.Phone.Maps.Controls;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Device.Location;
using System.Collections.ObjectModel;


namespace DynamicPolyLine
{
    public partial class MainPage : PhoneApplicationPage
    {
        bool addPoint = false;
        bool markerSelected = false;
        int MarkerCounter = 0;

        MapPolyline dynamicPolyline = null;
        MapLayer markerLayer = null;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            map1.MouseLeftButtonDown += map1_MouseLeftButtonDown;
            map1.MouseLeftButtonUp += map1_MouseLeftButtonUp;
            map1.MouseMove += map1_MouseMove;
        }

        void map1_MouseMove(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("map1_MouseMove");
            addPoint = false;
        }

        void map1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("map_MouseLeftButtonUp " + markerSelected + ", addPoint: " + addPoint);

            if (addPoint)
            {

                if (markerLayer == null) // create layer if it does not exists yet
                {
                    markerLayer = new MapLayer();
                    map1.Layers.Add(markerLayer);
                }


                MarkerCounter++;

                MapOverlay oneMarker = new MapOverlay();
                oneMarker.GeoCoordinate = map1.ConvertViewportPointToGeoCoordinate(e.GetPosition(map1));

                Ellipse Circhegraphic = new Ellipse();
                Circhegraphic.Fill = new SolidColorBrush(Colors.Yellow);
                Circhegraphic.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                Circhegraphic.StrokeThickness = 10;
                Circhegraphic.Opacity = 0.8;
                Circhegraphic.Height = 30;
                Circhegraphic.Width = 30;

                oneMarker.Content = Circhegraphic;

                oneMarker.PositionOrigin = new Point(0.5, 0.5);
                Circhegraphic.MouseLeftButtonDown += Circhegraphic_MouseLeftButtonDown;
                Circhegraphic.MouseLeftButtonUp += Circhegraphic_MouseLeftButtonUp;

                markerLayer.Add(oneMarker);

                if (dynamicPolyline == null) // create polyline if it does not exists yet
                {
                    dynamicPolyline = new MapPolyline();
                    dynamicPolyline.StrokeColor = Color.FromArgb(0x80, 0xFF, 0x00, 0x00);
                    dynamicPolyline.StrokeThickness = 5;

                    dynamicPolyline.Path = new GeoCoordinateCollection() { 
                            map1.ConvertViewportPointToGeoCoordinate(e.GetPosition(map1))
                        };

                    map1.MapElements.Add(dynamicPolyline);
                }
                else // just add points to polyline here
                {
                    dynamicPolyline.Path.Add(map1.ConvertViewportPointToGeoCoordinate(e.GetPosition(map1)));
                }
            }
            addPoint = false;
            markerSelected = false;
        }



        void map1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("map_MouseLeftButtonDown " + markerSelected);
            
            if (!markerSelected)
            {
                addPoint = true;
            }
            else
            {
                addPoint = false;
            }

            IList<MapElement> listt = map1.GetMapElementsAt(e.GetPosition(map1));

            Debug.WriteLine("MapElement list cunt: " + listt.Count());
            
        }


        void Circhegraphic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse clickedOne = sender as Ellipse;
            if (clickedOne != null)
            {
                Debug.WriteLine("oneMarker_MouseLeftButtonDown");
                markerSelected = true;
                addPoint = false;
            }
        }

        void Circhegraphic_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Ellipse clickedOne = sender as Ellipse;
            if (clickedOne != null && markerLayer != null)
            {
                Debug.WriteLine("Circhegraphic_MouseLeftButtonUp");

                for (var i = 0; i < markerLayer.Count(); i++)
                {
                    if (markerLayer[i].Content == clickedOne)
                    {
                        Debug.WriteLine("removing index: " + i);
                        dynamicPolyline.Path.Remove(markerLayer[i].GeoCoordinate); // remove point from the polyline
                        markerLayer.Remove(markerLayer[i]);// remove marker from the map
                    }
                }
            }
            markerSelected = false;
        }
    }
}