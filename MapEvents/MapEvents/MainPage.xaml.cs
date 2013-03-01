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
using MapEvents.Resources;
using Microsoft.Phone.Maps.Controls;
using System.Diagnostics;
using System.Device.Location;

namespace MapEvents
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            Debug.WriteLine("add handlers");

            map1.Loaded += map1_Loaded_1;
            map1.GotFocus += map1_GotFocus;
            map1.LostFocus += map1_LostFocus;

            map1.LayoutUpdated += map1_LayoutUpdated_1;
            map1.ResolveCompleted += map1_ResolveCompleted;
            map1.SizeChanged += map1_SizeChanged;

            map1.CenterChanged += map1_CenterChanged_1;
            map1.TransformCenterChanged += map1_TransformCenterChanged;

            map1.DoubleTap += map1_DoubleTap_1;
            map1.Hold += map1_Hold_1;
            map1.Tap += map1_Tap;

            map1.KeyUp += map1_KeyUp_1;
            map1.KeyDown += map1_KeyDown_1;
            

            map1.ManipulationCompleted += map1_ManipulationCompleted_1;
            map1.ManipulationDelta += map1_ManipulationDelta_1;
            map1.ManipulationStarted += map1_ManipulationStarted_1;
  
            map1.ViewChanging += map1_ViewChanging_1;
            map1.ViewChanged += map1_ViewChanged_1;

            map1.ZoomLevelChanged += map1_ZoomLevelChanged_1;
            map1.HeadingChanged += map1_HeadingChanged_1;
            map1.PitchChanged += map1_PitchChanged;

            map1.LandmarksEnabledChanged += map1_LandmarksEnabledChanged_1;
            map1.PedestrianFeaturesEnabledChanged += map1_PedestrianFeaturesEnabledChanged;
            map1.ColorModeChanged += map1_ColorModeChanged;
            map1.CartographicModeChanged += map1_CartographicModeChanged;

            map1.MouseEnter += map1_MouseEnter;
            map1.MouseLeave += map1_MouseLeave;
            map1.MouseLeftButtonDown += map1_MouseLeftButtonDown;
            map1.MouseLeftButtonUp += map1_MouseLeftButtonUp;
            map1.MouseMove += map1_MouseMove;
            map1.MouseWheel += map1_MouseWheel;


            Debug.WriteLine("Here we go");
        }


        private void map1_Loaded_1(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Map is loaded");
        }

        void map1_LostFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Map map1_LostFocus");
        }

        void map1_GotFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Map map1_GotFocus");
        }


        private void map1_LayoutUpdated_1(object sender, EventArgs e)
        {
            Debug.WriteLine("Map Layout is Updated");

        }

        void map1_ResolveCompleted(object sender, MapResolveCompletedEventArgs e)
        {
            Debug.WriteLine("Map map1_Resolve is Completed");
        }

        void map1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine("Map map1_Size is Changed");
        }

        private void map1_CenterChanged_1(object sender, MapCenterChangedEventArgs e)
        {
            Debug.WriteLine("Map Center changed");
        }

        void map1_TransformCenterChanged(object sender, MapTransformCenterChangedEventArgs e)
        {
            Debug.WriteLine("Map TransformCenter changed");
        }

  

        private void map1_DoubleTap_1(object sender, GestureEventArgs e)
        {
            Debug.WriteLine("Double tap handler called");
        }

        private void map1_Hold_1(object sender, GestureEventArgs e)
        {
            Debug.WriteLine("Hold handler called");
        }

        void map1_Tap(object sender, GestureEventArgs e)
        {
            Debug.WriteLine("single tap handler called");
        }

        
        private void map1_KeyUp_1(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("map1_KeyUp_1");
        }

        private void map1_KeyDown_1(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("map1_KeyDown_1");
        }

        private void map1_ManipulationCompleted_1(object sender, ManipulationCompletedEventArgs e)
        {
            Debug.WriteLine("map1_ManipulationCompleted_1");
        }

        private void map1_ManipulationDelta_1(object sender, ManipulationDeltaEventArgs e)
        {
            Debug.WriteLine("map1_ManipulationDelta_1");
        }

        private void map1_ManipulationStarted_1(object sender, ManipulationStartedEventArgs e)
        {
            Debug.WriteLine("map1_ManipulationStarted_1");
        }

        private void map1_ViewChanging_1(object sender, MapViewChangingEventArgs e)
        {
            Debug.WriteLine("map1_ViewChanging_1");
        }

        private void map1_ViewChanged_1(object sender, MapViewChangedEventArgs e)
        {
            Debug.WriteLine("map1_ViewChanged_1");
        }

        private void map1_ZoomLevelChanged_1(object sender, MapZoomLevelChangedEventArgs e)
        {
            Debug.WriteLine("map1_ZoomLevelChanged_1");
        }

        private void map1_HeadingChanged_1(object sender, MapHeadingChangedEventArgs e)
        {
            Debug.WriteLine("map1_HeadingChanged_1");
        }

        void map1_PitchChanged(object sender, MapPitchChangedEventArgs e)
        {
            Debug.WriteLine("map1_PitchChanged");
        }

        private void map1_LandmarksEnabledChanged_1(object sender, MapLandmarksEnabledChangedEventArgs e)
        {
            Debug.WriteLine("map1_LandmarksEnabledChanged_1");
        }

        void map1_PedestrianFeaturesEnabledChanged(object sender, MapPedestrianFeaturesEnabledChangedEventArgs e)
        {
            Debug.WriteLine("map1_PedestrianFeaturesEnabledChanged");
        }

        void map1_ColorModeChanged(object sender, MapColorModeChangedEventArgs e)
        {
            Debug.WriteLine("map1_ColorModeChanged");
        }

        void map1_CartographicModeChanged(object sender, MapCartographicModeChangedEventArgs e)
        {
            Debug.WriteLine("map1_CartographicModeChanged");
        }

        void map1_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Debug.WriteLine("map1_MouseWheel");
        }

        void map1_MouseMove(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("map1_MouseMove");
        }

        void map1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("map1_MouseLeftButtonUp");
        }

        void map1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("map1_MouseLeftButtonDown");
        }

        void map1_MouseLeave(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("map1_MouseLeave");
        }

        private void map1_MouseEnter(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("map1_MouseEnter");
        }


        private void MenuItem_Heading_Click(object sender, EventArgs e)
        {
            map1.Heading = map1.Heading + 12;
            Debug.WriteLine("map1.Heading = " + map1.Heading);
        }

        private void IconButton_ZoomIn_Click(object sender, EventArgs e)
        {
            double zoom;
            zoom = map1.ZoomLevel;
            map1.ZoomLevel = ++zoom;
        }

        private void IconButton_ZoomOut_Click(object sender, EventArgs e)
        {
            double zoom;
            zoom = map1.ZoomLevel;
            map1.ZoomLevel = --zoom;
        }

        private void MenuItem_GotoSello_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("GotoSello");
            //map1.Center = new GeoCoordinate(60.22, 24.81);
            // using setview, gives us possibility on using the animations
            map1.SetView(new GeoCoordinate(60.22, 24.81), map1.ZoomLevel, map1.Heading, map1.Pitch, MapAnimationKind.Parabolic);
        }

        private void MenuItem_Mode_Click(object sender, EventArgs e)
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
        }
    }
}