using DevExpress.Xpf.Core;
using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;

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
                    Output.Items.Add(psObject.BaseObject);
                }
            }
        }

        private async void Kill_Click(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri($"http://{Ip.Text}:{Port.Text}/");
            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            for (int i = 0; i < KillCount.Value; i++)
            {
                var response = await client.PostAsync("/shutdown", null);
                if (response.IsSuccessStatusCode)
                {
                    Output.Items.Add(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    Output.Items.Add("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
            }
        }

        private async void Check_Click(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri($"http://{Ip.Text}:{Port.Text}/");
            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            for (int i = 0; i < CheckCount.Value; i++)
            {
                var response = await client.GetAsync("/");
                if (response.IsSuccessStatusCode)
                {
                    Output.Items.Add(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    Output.Items.Add("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _rs.CloseAsync();
            base.OnClosed(e);
        }

        void Clean_OnClick(object sender, RoutedEventArgs e)
        {
            Output.Items.Clear();
        }
    }
}
