using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SignalR.WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            HubConnection connection = new HubConnection("http://localhost:12345");
            IHubProxy proxy = connection.CreateHubProxy("myHub");
            connection.Start().ContinueWith(task =>
            {
                proxy.Invoke("Send", ".NET Client Connected");

                proxy.On<string>("sendMessage", (message) =>
                {
                    this.Dispatcher.Invoke((Action)(() => { _uiMessage.Items.Add(message); }));
                });
            });

        }
    }
}
