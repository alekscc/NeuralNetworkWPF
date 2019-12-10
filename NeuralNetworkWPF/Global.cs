using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkWPF
{
    public sealed class Global
    {
        private static Global instance = null;
        private static readonly object padlock = new object();

        public GA_NN_Settings Settings { get; set; } 
        public GA_NN_Stats Stats { get; set; }

        Global()
        {
            Settings = new GA_NN_Settings();
            Stats = new GA_NN_Stats();
        }

        public static Global Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Global();
                    }
                    return instance;
                }
            }
        }
    }
}
