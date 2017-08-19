namespace U2UC.WinLob.ClassicClient
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Windows.Forms;
    using U2UC.WinLob.Legacy;
    using U2UC.WinLob.Legacy.Dto;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Dal.GetAllEmployees();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            // Create HttpCient and make a request to api/northwind 
            var client = new HttpClient();
            var json = await client.GetStringAsync(new Uri("http://localhost:9000/api/northwind"));

            var employees = JsonConvert.DeserializeObject<List<Employee>>(json);

            dataGridView1.DataSource = employees;
        }
    }
}
