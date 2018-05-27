using System.Windows;
using DevExpress.Xpf.Core;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace k8s_wpf_api_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        private Runspace _rs;

        public MainWindow()
        {
            InitializeComponent();
            _rs = RunspaceFactory.CreateRunspace();
            _rs.OpenAsync();
        }


        private void Get_Pod_List_Click(object sender, RoutedEventArgs e)
        {
            using (var ps = PowerShell.Create())
            {
                ps.AddScript("kubectl get pods");
                ps.Runspace = _rs;
                var collection = ps.Invoke();
                foreach (var psObject in collection)
                {
                    output.Items.Add(psObject.BaseObject);
                }
            }
        }

        private async void Kill_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:12789/");
            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.PostAsync("/shutdown", null);
            if (response.IsSuccessStatusCode)
            {
                output.Items.Add(await response.Content.ReadAsStringAsync());
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        private async void Check_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:12789/");
            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.GetAsync("/");
            if (response.IsSuccessStatusCode)
            {
                output.Items.Add(await response.Content.ReadAsStringAsync());
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _rs.CloseAsync();
            base.OnClosed(e);
        }
    }
}
