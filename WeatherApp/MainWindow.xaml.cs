using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeatherApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Pushpin pin = new Pushpin();

        public MainWindow()
        {
            InitializeComponent();
            map.Children.Add(pin);
        }

        private void MapWithPushpins_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point mousePosition = e.GetPosition(this);
            Location pinLocation = map.ViewportPointToLocation(mousePosition);
            pin.Location = pinLocation;

            address.Text = pinLocation.ToString();

            // request https://api.openweathermap.org/data/2.5/onecall?lat={pinLocation.Latitude}&lon={pinLocation.Longitude}
            // by https://openweathermap.org/api/one-call-api
        }

        private void address_TextChanged(object sender, TextChangedEventArgs e)
        {
            // https://openweathermap.org/current
        }
    }
}
