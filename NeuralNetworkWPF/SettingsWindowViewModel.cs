using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkWPF
{
    class SettingsWindowViewModel : INotifyPropertyChanged
    {
        public float MinErr { get { return Global.Instance.Settings.MinErr; }  }
        public float Elitism { get { return Global.Instance.Settings.ElitismPerc; } }
        public float PopulationSize { get { return Global.Instance.Settings.PopulationSize; } }
        public float MutationRate { get { return Global.Instance.Settings.MutationRate; } }
        public string ANNStructure { get { return Utils.IntArrayToString(Global.Instance.Settings.Layers); } }

        public SettingsWindowViewModel()
        {
     
        }
        public event PropertyChangedEventHandler PropertyChanged;

       
        public void NotifyPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
