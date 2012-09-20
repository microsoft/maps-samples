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

namespace MapInteraction
{
    public partial class Page1 : PhoneApplicationPage
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void list_ItemSelected(object sender, SelectionChangedEventArgs e)
        {

            string lattitude = "";
            string longitude = "";

            switch (listboxx.SelectedIndex)
            {
                case 0: // Helsinki
                    {
                        lattitude = "60.2397";
                        longitude = "24.8708";
                    }
                    break;
                case 1: //Arhus
                    {
                        lattitude = "56.19028120301664";
                        longitude = "10.292647937312722";
                    }
                    break;
                case 2: //Aachen
                    {
                        lattitude = "50.708475122228265";
                        longitude = "6.092609027400613";
                    }
                    break;
                case 3: //Budabest
                    {
                        lattitude = "47.496322626248";
                        longitude = "19.126385310664773";
                    }
                    break;
                case 4: //Venetzia
                    {
                        lattitude = "45.23451148532331";
                        longitude = "12.481056908145547";
                    }
                    break;
                case 5: //Hanburg
                    {
                        lattitude = "53.503450406715274";
                        longitude = "9.971555331721902";
                    }
                    break;
                case 6: //Hangover
                    {
                        lattitude = "52.37499645911157";
                        longitude = "9.67447922565043";
                    }
                    break;
            }

            string urlToGo = "/MainPage.xaml?latitude=" + lattitude + "&longitude=" + longitude;

            this.NavigationService.Navigate (new Uri(urlToGo, UriKind.RelativeOrAbsolute));
        }
    }
}