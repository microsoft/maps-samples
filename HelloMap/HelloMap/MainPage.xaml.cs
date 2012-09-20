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
using HelloMap.Resources;
using Microsoft.Phone.Maps.Controls;
using System.Diagnostics;

namespace HelloMap
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            ///Sample code to call helper function to localize the ApplicationBar
            //BuildApplicationBar();

            map1.IsEnabledChanged += map1_IsEnabledChanged_1;
        }

        private void Toggle_disable_Click(object sender, EventArgs e)
        {
            if (map1.IsEnabled == true)
            {
                map1.IsEnabled = false;
                Debug.WriteLine("Map Disabled");
            }
            else
            {
                map1.IsEnabled = true;
                Debug.WriteLine("Map Enabled");
            }
        }

        private void map1_IsEnabledChanged_1(object sender, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("map1_IsEnabledChanged_1 called : ");
            if (sender == map1)
            {
                if (map1.IsEnabled == true)
                {
                    MenuDisEna.Text = "Map Enabled";
                }
                else
                {
                    MenuDisEna.Text = "Map Disabled";
                }
            }
        }
    }
}