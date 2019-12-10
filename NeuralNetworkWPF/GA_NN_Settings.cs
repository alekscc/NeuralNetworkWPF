using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkWPF
{
    public class GA_NN_Settings
    {
        public string LearningDataPath { get; set; }
        public string TestDataPath { get; set; }
        public string ValidateDataPath { get; set; }

        public float MutationRate { get; set; }
        public float ElitePerc { get; set; }
        public int PopulationNumber { get; set; }
        
        public int OutputNumber { get; set; }
        public int InputNumber { get; set; }
        public int[] HiddneLayers { get; set; }

        public GA_NN_Settings()
        {
            LearningDataPath = "mushrooms2/n_ldata.csv";
            TestDataPath = "mushrooms2/n_tdata.csv";
            ValidateDataPath = "mushrooms2/n_vdata.csv";

            MutationRate = 0.01f;
            ElitePerc = 0.1f;
            PopulationNumber = 200;

            OutputNumber = 2;
            InputNumber = 21;
            HiddneLayers = new int[] { 5 };
        }

    }
}
