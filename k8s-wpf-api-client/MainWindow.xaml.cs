using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;

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
        }


        private void Get_Pod_List_Click(object sender, RoutedEventArgs e)
        {
            output.Items.Add("Get Pod List");
        }

        private void Kill_Click(object sender, RoutedEventArgs e)
        {
            output.Items.Add("Kill");
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            output.Items.Add("Check");
        }
    }
}
