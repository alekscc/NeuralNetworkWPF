using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace NeuralNetworkWPF
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private ANN nn;
        private Thread thread;
        
        private bool isStopped;
        public bool IsStopped { get { return isStopped; } set { isStopped = value; NotifyAllButtons(); } }

        public float OutputErr { get { return nn.Err; }  }
        public float OutputVErr { get { return nn.ValidateErr; } }
        public float OutputTErr { get { return nn.TestErr; } }
        public float OutputMinErr { get { return nn.MaxErr; } }
        public float OutputAvgFit { get { return nn.AvgFitness; } }
        public int OutputGen { get { return nn.GenerationNumber; } }
        public string OutputElapsedTime { get { return String.Format("{0:00}:{1:00}:{2:00}", nn.ElapsedTime.Minutes, nn.ElapsedTime.Seconds, nn.ElapsedTime.Milliseconds/10f); } }

        public MainWindowViewModel()
        {
            IsStopped = true;

            SetupNeuralNetwork();
        }
        
        public void SetupNeuralNetwork()
        {
             //nn = new NeuralNetwork(Global.Instance.Settings.Layers) { UpdateCallback = NotifyAllOutputs, FinalizeCallback = Stop };
            nn = new ANN(Global.Instance.Settings.Layers) { UpdateCallback = NotifyAllOutputs, FinalizeCallback = Stop };


            nn.Initialize(Global.Instance.Settings.PopulationSize,
                            Global.Instance.Settings.MaxErr,
                            Global.Instance.Settings.ElitismPerc,
                            Global.Instance.Settings.MutationRate,
                            Global.Instance.Settings.Selection,
                            Global.Instance.Settings.Bias);

            //nn.SetCallBack(NotifyAllOutputs);
        }
        private void NotifyAllButtons()
        {
            NotifyPropertyChanged("IsStopped");

        }
        private void NotifyAllOutputs()
        {
            NotifyPropertyChanged("OutputErr");
            NotifyPropertyChanged("OutputMinErr");
            NotifyPropertyChanged("OutputAvgFit");
            NotifyPropertyChanged("OutputGen");
            NotifyPropertyChanged("OutputVErr");
            NotifyPropertyChanged("OutputTErr");
            NotifyPropertyChanged("OutputElapsedTime");
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
        public void StartTraining()
        {
            Global.Instance.Stats.IndividualRecords.Clear();
            SetupNeuralNetwork();

            float[][] ldata  = File.ReadLines(Global.Instance.Settings.LearningDataPath).Select(line => line.Split(';')).Select(x => x.Select(y => float.Parse(y)).ToArray()).ToArray();
            float[][] vdata = File.ReadLines(Global.Instance.Settings.ValidateDataPath).Select(line => line.Split(';')).Select(x => x.Select(y => float.Parse(y)).ToArray()).ToArray();
            float[][] tdata = File.ReadLines(Global.Instance.Settings.TestDataPath).Select(line => line.Split(';')).Select(x => x.Select(y => float.Parse(y)).ToArray()).ToArray();

            thread = new Thread(() => nn.Train(ldata, vdata, tdata));
            thread.IsBackground = true;
            thread.Start();

            IsStopped = false;

  
        }
        public void ShowStatisticWindow()
        {
            StatisticsWindow wnd = new StatisticsWindow();

            wnd.FillDataGrid(nn.Outputs);

            wnd.Show();
        }
        public void Stop()
        {

            NotifyAllOutputs();

            IsStopped = true;

            thread.Abort();


            float[][] tdata = File.ReadLines(Global.Instance.Settings.TestDataPath).Select(line => line.Split(';')).Select(x => x.Select(y => float.Parse(y)).ToArray()).ToArray();

            nn.Test(tdata);

            //File.WriteAllLines("data.csv",
            //ToCsv(nn.Outputs));

        }
        private static IEnumerable<String> ToCsv<T>(T[][] data, string separator = ",")
        {
            for (int i = 0; i < data.Length; ++i)
                yield return string.Join(separator, Enumerable
                  .Range(0, data[i].Length)
                  .Select(j => data[i][j])); // simplest, we don't expect ',' and '"' in the items
        }
        public void Reset()
        {
            
        }
        ~MainWindowViewModel()
        {
            //thread.Abort();
            //thread.Interrupt();
        }



    }
}
