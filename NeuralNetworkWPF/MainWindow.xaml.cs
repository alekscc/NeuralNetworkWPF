using System;
using System.Collections.Generic;
using System.IO;
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

namespace NeuralNetworkWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //MainWindowViewModel viewModel;

        MainWindowViewModel viewModel;

        public MainWindow()
        {
           
            InitializeComponent();

            viewModel = (MainWindowViewModel)DataContext;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("czekaj");
            //((MainWindowViewModel)DataContext).StartTraining();
            //MessageBox.Show("nauczone");

            viewModel.StartTraining();
        }


        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Stop();
        }

        private void btnData_Click(object sender, RoutedEventArgs e)
        {
            DataWindow wnd = new DataWindow();

            wnd.Show();

        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            //StatisticsWindow wnd = new StatisticsWindow();
            //wnd.Show();

            SettingsWindow wnd = new SettingsWindow();
            wnd.Show();
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            StatisticsWindow wnd = new StatisticsWindow();
            wnd.Show();
        }
    }
}
