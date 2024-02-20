using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
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
        string apiid = "";
        private static HttpClient client = new HttpClient();
        Pushpin pin = new Pushpin();
        Location pinLocation;

        public MainWindow()
        {
            InitializeComponent();
            map.Children.Add(pin);
        }

        private void MapWithPushpins_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point mousePosition = e.GetPosition(this);
            pinLocation = map.ViewportPointToLocation(mousePosition);
            pin.Location = pinLocation;

            address.Text = pinLocation.ToString();
        }

        private async void address_TextChanged(object sender, TextChangedEventArgs e)
        {
            HttpResponseMessage response;
            TextBox addressTextBox = (TextBox)sender;

            if (addressTextBox.Text == pinLocation.ToString())
                response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/onecall?lat={pinLocation.Latitude}&lon={pinLocation.Longitude}&apiid={apiid}");
            else
                response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/onecall?q={addressTextBox.Text}&apiid={apiid}");

            MessageBox.Show(await response.Content.ReadAsStringAsync());
        }
    }
}
