using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkWPF
{

    public class Record<T,E,W>
    {
        public T X { get; private set; }
        public E Y { get; private set; }
        public W Z { get; private set; }

        public Record(T x,E y,W z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }


    public class GA_NN_Stats
    {

        public List<Record<int,float,float>> IndividualRecords { get; set; }

        public GA_NN_Stats()
        {
            IndividualRecords = new List<Record<int, float,float>>();
        }
    }
}
