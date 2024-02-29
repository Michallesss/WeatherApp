using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json;
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
        string apiid = "bbb384ea90180777bcc406a7ab401bf9";
        private static HttpClient client = new HttpClient();
        Pushpin pin = new Pushpin();

        public MainWindow()
        {
            InitializeComponent();
            map.Children.Add(pin);
        }

        private async void MapWithPushpins_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point mousePosition = e.GetPosition(this);
            Location pinLocation = map.ViewportPointToLocation(mousePosition);
            pin.Location = pinLocation;

            HttpResponseMessage response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={pinLocation.Latitude}&lon={pinLocation.Longitude}&apiid={apiid}");
            string jsonString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                WeatherInfo.root details = JsonConvert.DeserializeObject<WeatherInfo.root>(jsonString);
                MessageBox.Show(details.weather[0].description.ToString());
            }
            else
            {
                MessageBox.Show(jsonString);
            }
        }
    }
}
