using DevExpress.Xpf.Core;
using k8s_wpf_api_client.ViewModels;
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

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ApiViewModel();
        }
    }
}
