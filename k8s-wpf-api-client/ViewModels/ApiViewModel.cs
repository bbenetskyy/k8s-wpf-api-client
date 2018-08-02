using DevExpress.Xpf.Editors;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace k8s_wpf_api_client.ViewModels
{
    public class ApiViewModel : ReactiveObject
    {
        private Runspace _rs;
        
        public ApiViewModel()
        {
            _rs = RunspaceFactory.CreateRunspace();
            _rs.OpenAsync();

            GetPodList = ReactiveCommand.Create(() =>
            {
                using (var ps = PowerShell.Create())
                {
                    ps.AddScript("kubectl get pods");
                    ps.Runspace = _rs;
                    var collection = ps.Invoke();
                    foreach (var psObject in collection)
                    {
                        Output.Add(psObject.BaseObject);
                    }
                }
            });
            Kill = ReactiveCommand.Create(async () =>
            {
                //todo refactor creation of httpClient
                var client = new HttpClient();
                client.BaseAddress = new Uri($"http://{Ip}:{Port}/");
                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));
                for (int i = 0; i < KillCount; i++)
                {
                    var response = await client.PostAsync("/shutdown", null);
                    if (response.IsSuccessStatusCode)
                    {
                        Output.Add(await response.Content.ReadAsStringAsync());
                    }
                    else
                    {
                        Output.Add("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    }
                }
            });
            Check = ReactiveCommand.Create(async () =>
            {
                //todo refactor creation of httpClient
                var client = new HttpClient();
                client.BaseAddress = new Uri($"http://{Ip}:{Port}/");
                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));
                for (int i = 0; i < CheckCount; i++)
                {
                    var response = await client.GetAsync("/");
                    if (response.IsSuccessStatusCode)
                    {
                        Output.Add(await response.Content.ReadAsStringAsync());
                    }
                    else
                    {
                        Output.Add("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    }
                }
            });
            Clean = ReactiveCommand.Create(() => Output.Clear());
        }

        ~ApiViewModel()
        {
            _rs.CloseAsync();
        }

        public ReactiveCommand GetPodList { get; }
        public ReactiveCommand Kill { get; }
        public ReactiveCommand Check { get; }
        public ReactiveCommand Clean { get; }

        private List<object> output = new List<object>();
        public List<object> Output
        {
            get => output;
            set => this.RaiseAndSetIfChanged(ref output, value);
        }

        //todo replace it to const values in separate file
        private int killCount = 1;//by business logic default 0 is incorrect
        public int KillCount
        {
            get => killCount;
            set => this.RaiseAndSetIfChanged(ref killCount, value);
        }

        //todo replace it to const values in separate file
        private int checkCount = 1;//by business logic default 0 is incorrect
        public int CheckCount
        {
            get => checkCount;
            set => this.RaiseAndSetIfChanged(ref checkCount, value);
        }
        
        private string ip = default(string);
        public string Ip
        {
            get => ip;
            set => this.RaiseAndSetIfChanged(ref ip, value);
        }

        private string port = default(string);
        public string Port
        {
            get => port;
            set => this.RaiseAndSetIfChanged(ref port, value);
        }
    }
}
