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
using MapInteraction.Resources;
using Microsoft.Phone.Maps.Controls;
using System.Diagnostics;
using System.Device.Location;

namespace MapInteraction
{
    public partial class MainPage : PhoneApplicationPage
    {
        MapAnimationKind useAnim = MapAnimationKind.Parabolic;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            
            map1.HeadingChanged += map1_HeadingChanged_1;
            map1.PitchChanged += map1_PitchChanged;
            map1.ZoomLevelChanged += map1_ZoomLevelChanged_1;

            map1.LandmarksEnabledChanged += map1_LandmarksEnabledChanged_1;
            map1.ResolveCompleted += map1_ResolveCompleted;

            map1.PedestrianFeaturesEnabledChanged += map1_PedestrianFeaturesEnabledChanged;
            map1.ColorModeChanged += map1_ColorModeChanged;
            map1.CartographicModeChanged += map1_CartographicModeChanged;

            Debug.WriteLine("We are started");
        }

        

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string lattitude = "";
            string longitude = "";

            if (this.NavigationContext.QueryString.ContainsKey("latitude"))
            {
                lattitude = this.NavigationContext.QueryString["latitude"];
            }

            if (this.NavigationContext.QueryString.ContainsKey("longitude"))
            {
                longitude = this.NavigationContext.QueryString["longitude"];
            }

            Debug.WriteLine("OnNavigatedTo, longitude: " + longitude + ", latitude: " + lattitude);

            base.OnNavigatedTo(e);

            if (longitude.Length > 0 && lattitude.Length > 0)
            {
                Debug.WriteLine("moving center to , longitude: " + longitude + ", latitude: " + lattitude);
                map1.Center = new GeoCoordinate(double.Parse(lattitude), double.Parse(longitude));
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (sender == Hress)
            {
                map1.SetView(map1.Center, 10, 0, 0, useAnim);
            }else if (sender == HQqq)
            {
                map1.SetView(new GeoCoordinate(60.1717919, 24.82839155), 18, 50, 75, useAnim);
            }
            else if (sender == GoSel)
            {
                this.NavigationService.Navigate(new Uri("/GotoSelection.xaml", UriKind.RelativeOrAbsolute));
            }

            
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

            if (sender == Lands)
            {
                if (map1.LandmarksEnabled == true)
                {
                    map1.LandmarksEnabled = false;
                }
                else
                {
                    map1.LandmarksEnabled = true;
                }
            }
            else if (sender == pedsFF)
            {
                if (map1.PedestrianFeaturesEnabled == true)
                {
                    map1.PedestrianFeaturesEnabled = false;
                }
                else
                {
                    map1.PedestrianFeaturesEnabled = true;
                }
            }else if (sender == AniS)
            {
                if (useAnim == MapAnimationKind.Parabolic){
                    useAnim = MapAnimationKind.Linear;
                }else if (useAnim == MapAnimationKind.Linear){
                    useAnim = MapAnimationKind.None;
                }else{
                    useAnim = MapAnimationKind.Parabolic;
                }
                AniS.Content = useAnim;
                Debug.WriteLine("Animation style set to: " + useAnim);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (sender == pRoad)
            {
                Debug.WriteLine("set MapCartographicMode to Road");
                map1.CartographicMode = MapCartographicMode.Road;
            }
            else if (sender == pArial)
            {
                Debug.WriteLine("set MapCartographicMode to Aerial");
                map1.CartographicMode = MapCartographicMode.Aerial;
            }
            else if (sender == pHybrid)
            {
                Debug.WriteLine("set MapCartographicMode to Hybrid");
                map1.CartographicMode = MapCartographicMode.Hybrid;
            }
            else if (sender == pTerrain)
            {
                Debug.WriteLine("set MapCartographicMode to Terrain");
                map1.CartographicMode = MapCartographicMode.Terrain;
            }
            else if (sender == TogColor)
            {
                if (map1.ColorMode == MapColorMode.Dark)
                {
                    Debug.WriteLine("set MapColorMode to Light1");
                    map1.ColorMode = MapColorMode.Light;
                }
                else
                {
                    Debug.WriteLine("set MapColorMode to Dark");
                    map1.ColorMode = MapColorMode.Dark;
                }

                TogColor.Content = map1.ColorMode;
            }

            
        }
    
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(sender == Hplus){
                map1.Heading = (map1.Heading + 12); 
            }else if(sender == Hmins){
                map1.Heading = (map1.Heading - 12); 
            }else if(sender == Pplus){
                double holp = (map1.Pitch + 3);   // where to get max min pitch ?
                if (holp > 75) { holp = 75; }
                map1.Pitch = holp;
            }else if(sender == Pmins){
                double hjelp = (map1.Pitch - 3);
                if (hjelp < 0){hjelp = 0;}
                map1.Pitch = hjelp;
            }else if(sender == Zplus){
                double zoom;
                zoom = map1.ZoomLevel;
                map1.ZoomLevel = ++zoom;
            }
            else if (sender == Zmins)
            {
                double zoom;
                zoom = map1.ZoomLevel;
                map1.ZoomLevel = --zoom;
            }
        }

        private void Button_printValues(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("ZoomLevel: " + map1.ZoomLevel);
            Debug.WriteLine("Heading: " + map1.Heading);
            Debug.WriteLine("Pitch: " + map1.Pitch);

            Debug.WriteLine("LandmarksEnabled: " + map1.LandmarksEnabled);
            Debug.WriteLine("CartographicMode: " + map1.CartographicMode);
            Debug.WriteLine("ColorMode: " + map1.ColorMode);
            Debug.WriteLine("PedestrianFeaturesEnabled : " + map1.PedestrianFeaturesEnabled);
    
        }

        private void map1_ZoomLevelChanged_1(object sender, MapZoomLevelChangedEventArgs e)
        {
            Debug.WriteLine("Map ZoomLevel Changed to: " + map1.ZoomLevel);
        }

        private void map1_HeadingChanged_1(object sender, MapHeadingChangedEventArgs e)
        {
            Debug.WriteLine("Map Heading Changed to: " + map1.Heading);
        }

        void map1_PitchChanged(object sender, MapPitchChangedEventArgs e)
        {
            Debug.WriteLine("Map Pitch Changed to: " + map1.Pitch);
        }

        private void map1_LandmarksEnabledChanged_1(object sender, MapLandmarksEnabledChangedEventArgs e)
        {
            Debug.WriteLine("Map LandmarksEnabled Changed to : " + map1.LandmarksEnabled);
        }

        void map1_ResolveCompleted(object sender, MapResolveCompletedEventArgs e)
        {
               Debug.WriteLine("Map map1_Resolve is Completed");
        }

        void map1_CartographicModeChanged(object sender, MapCartographicModeChangedEventArgs e)
        {
            Debug.WriteLine("Map CartographicMod Changed to : " + map1.CartographicMode);
        }

        void map1_ColorModeChanged(object sender, MapColorModeChangedEventArgs e)
        {
            Debug.WriteLine("Map ColorMode Changed to : " + map1.ColorMode);
        }

        void map1_PedestrianFeaturesEnabledChanged(object sender, MapPedestrianFeaturesEnabledChangedEventArgs e)
        {
            Debug.WriteLine("Map PedestrianFeaturesEnabled Changed to : " + map1.PedestrianFeaturesEnabled);
        }
    }
}