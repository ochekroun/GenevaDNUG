namespace U2UC.WinLob.App
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            // Create HttpCient and make a request to api/northwind 
            var client = new HttpClient();
            var json = client.GetStringAsync(new Uri("http://localhost:9000/api/northwind")).Result;

            var employees = JsonConvert.DeserializeObject<List<Employee>>(json);

            this.Employees.ItemsSource = employees;
        }
    }
}
