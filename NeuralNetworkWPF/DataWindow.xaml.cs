using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace NeuralNetworkWPF
{
    /// <summary>
    /// Interaction logic for DataWindow.xaml
    /// </summary>
    public partial class DataWindow : Window
    {
        private string dataPathName_l, dataPathName_v, dataPathName_t;

        public DataWindow()
        {
           
            InitializeComponent();

            textBoxDataL.Text = Global.Instance.Settings.LearningDataPath;
            textBoxDataV.Text = Global.Instance.Settings.ValidateDataPath;
            textBoxDataT.Text = Global.Instance.Settings.TestDataPath;
        }

        private void btnBrowseDataL_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                textBoxDataL.Text = openFileDialog.FileName;
                dataPathName_l = textBoxDataL.Text;

            }
        }

        private void btnBrowseDataV_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                textBoxDataV.Text = openFileDialog.FileName;
                dataPathName_v = textBoxDataV.Text;

            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnBrowseDataT_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                textBoxDataT.Text = openFileDialog.FileName;
                dataPathName_t = textBoxDataT.Text;

            }
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            Global.Instance.Settings.LearningDataPath = textBoxDataL.Text;
            Global.Instance.Settings.ValidateDataPath = textBoxDataV.Text;
            Global.Instance.Settings.TestDataPath = textBoxDataT.Text;

            this.Close();
        }
    }
}
