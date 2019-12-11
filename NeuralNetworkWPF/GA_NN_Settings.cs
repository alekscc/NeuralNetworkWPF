using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkWPF
{
    public class GA_NN_Settings
    {

        public enum SelectionMethod
        {
            Roulette,
            Tournament
        };
        public enum CrossoverMethod
        {
            Standard,
            BLX
        }

        public string LearningDataPath { get; set; }
        public string TestDataPath { get; set; }
        public string ValidateDataPath { get; set; }

        public float MinErr { get; set; }
        public int PopulationSize { get; set; }

        public SelectionMethod Selection { get; set; }
        public CrossoverMethod Crossover { get; set; }

        public float MutationRate { get; set; }
        public float ElitismPerc { get; set; }
        
        public int OutputNumber { get; set; }
        public int InputNumber { get; set; }
        public int[] HiddneLayers { get; set; }

        public int[] Layers { get; set; }

        public GA_NN_Settings()
        {
            LearningDataPath = "mushrooms2/n_ldata.csv";
            TestDataPath = "mushrooms2/n_tdata.csv";
            ValidateDataPath = "mushrooms2/n_vdata.csv";

            MutationRate = 0.01f;
            ElitismPerc = 0.05f;
            PopulationSize = 200;
            MinErr = 0.03f;

            Layers = new int[] { 21, 5, 2 };

            //OutputNumber = 2;
            //InputNumber = 21;
            //HiddneLayers = new int[] { 5 };

            Selection = SelectionMethod.Tournament;
            Crossover = CrossoverMethod.BLX;
        }

    }
}
