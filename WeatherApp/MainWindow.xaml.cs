using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using System.Windows.Media.Animation;
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

        void maPin(object sender, MouseEventArgs e)
        {
            e.Handled = true;

            Point mouse = e.GetPosition(this);
            Location pinLocation = map.ViewportPointToLocation(mouse);
            pin.Location = pinLocation;
            double lat = pinLocation.Latitude;
            double lng = pinLocation.Longitude;

            // Current
            var result = client.GetAsync(new Uri($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lng}&appid={apiid}&units=metric&lang=pl")).Result.Content.ReadAsStringAsync().Result;
            var current = JObject.Parse(result);

            // Future 
            result = client.GetAsync(new Uri($"https://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lng}&appid={apiid}&units=metric&lang=pl")).Result.Content.ReadAsStringAsync().Result;
            var future = JObject.Parse(result);

            display(current, future);
        }

        private void SearchCity(object sender, RoutedEventArgs e)
        {
            var result = client.GetAsync(new Uri($"https://api.openweathermap.org/geo/1.0/direct?q={searchBar.Text}&appid={apiid}&units=metric")).Result.Content.ReadAsStringAsync().Result;
            var geo = JArray.Parse(result);

            // Current
            result = client.GetAsync(new Uri($"https://api.openweathermap.org/data/2.5/weather?lat={geo[0]["lat"]}&lon={geo[0]["lon"]}&appid={apiid}&units=metric&lang=pl")).Result.Content.ReadAsStringAsync().Result;
            var current = JObject.Parse(result);
            
            // Future 
            result = client.GetAsync(new Uri($"https://api.openweathermap.org/data/2.5/forecast?lat={geo[0]["lat"]}&lon={geo[0]["lon"]}&appid={apiid}&units=metric&lang=pl")).Result.Content.ReadAsStringAsync().Result;
            var future = JObject.Parse(result);
            
            display(current, future);
        }

        private void display(JObject current, JObject future)
        {
            // Current
            DataCity.Text = $"Miasto: {current["name"]}";
            currentDataAura.Text = $"Aura: {current["weather"][0]["description"]}";
            currentDataTemp.Text = $"Temperatura: {current["main"]["temp"]} °C";
            currentDataWater.Text = $"Wilgotność: {current["main"]["humidity"]}%";
            currentDataPressure.Text = $"Ciśnienie: {current["main"]["pressure"]} hPa";

            // Future
            futureDataAura.Text = $"Aura: {future["list"][1]["weather"][0]["description"]}";
            futureDataTemp.Text = $"Temperatura: {future["list"][1]["main"]["temp"]} °C";
            futureDataWater.Text = $"Wilgotność: {future["list"][1]["main"]["humidity"]}%";
            futureDataPressure.Text = $"Ciśnienie: {future["list"][1]["main"]["pressure"]} hPa";
        }
    }
}
