using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {

        SettingsWindowViewModel viewModel;
        public SettingsWindow()
        {
            InitializeComponent();

            viewModel = (SettingsWindowViewModel)DataContext;


            Setup();
        }
        private void Setup()
        {
            comboBoxCrossover.ItemsSource = Enum.GetValues(typeof(GA_NN_Settings.CrossoverMethod)).Cast<GA_NN_Settings.CrossoverMethod>();
            comboBoxSelection.ItemsSource = Enum.GetValues(typeof(GA_NN_Settings.SelectionMethod)).Cast<GA_NN_Settings.SelectionMethod>();

            comboBoxCrossover.SelectedItem = Global.Instance.Settings.Crossover;
            comboBoxSelection.SelectedItem = Global.Instance.Settings.Selection;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {

            Global.Instance.Settings.Selection = (GA_NN_Settings.SelectionMethod)comboBoxSelection.SelectedItem;
            Global.Instance.Settings.Crossover = (GA_NN_Settings.CrossoverMethod)comboBoxCrossover.SelectedItem;

            Global.Instance.Settings.MutationRate = float.Parse(textBoxMutationRate.Text, CultureInfo.InvariantCulture.NumberFormat);
            Global.Instance.Settings.ElitismPerc = float.Parse(textBoxElitismPerc.Text, CultureInfo.InvariantCulture.NumberFormat);
            Global.Instance.Settings.MaxErr = float.Parse(textBoxMaxErr.Text, CultureInfo.InvariantCulture.NumberFormat);
            Global.Instance.Settings.PopulationSize = int.Parse(textBoxPopulationSize.Text);
            Global.Instance.Settings.Layers = Utils.StringToIntArray(textBoxANNStructure.Text);
            Global.Instance.Settings.Bias = float.Parse(textBoxBias.Text, CultureInfo.InvariantCulture.NumberFormat);

            Close();
        }
    }
}
