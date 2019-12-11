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
        private NeuralNetwork nn;
        private Thread thread;
        
        private bool isStopped;
        public bool IsStopped { get { return isStopped; } set { isStopped = value; NotifyAllButtons(); } }

        public float OutputErr { get { return nn.Err; }  }
        public float OutputVErr { get { return nn.ValidateErr; } }
        public float OutputTErr { get { return nn.TestErr; } }
        public float OutputMinErr { get { return nn.MinErr; } }
        public float OutputAvgFit { get { return nn.AvgFit; } }
        public int OutputGen { get { return nn.Generation; } }
        public string OutputElapsedTime { get { return String.Format("{0:00}:{1:00}:{2:00}", nn.ElapsedTime.Minutes, nn.ElapsedTime.Seconds, nn.ElapsedTime.Milliseconds/10f); } }

        public MainWindowViewModel()
        {
            IsStopped = true;

            SetupNeuralNetwork();
        }
        
        public void SetupNeuralNetwork()
        {
            nn = new NeuralNetwork(new int[] { 21, 5, 2 }) { UpdateCallback = NotifyAllOutputs, FinalizeCallback = Stop };


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

            thread = new Thread(()=>nn.Train(ldata, vdata,tdata,0.03));
            thread.IsBackground = true;
            thread.Start();

            IsStopped = false;

  
        }
        private void ShowStatisticWindow()
        {
            StatisticsWindow wnd = new StatisticsWindow();
            wnd.Show();
        }
        public void Stop()
        {

            NotifyAllOutputs();

            IsStopped = true;

            thread.Abort();
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
