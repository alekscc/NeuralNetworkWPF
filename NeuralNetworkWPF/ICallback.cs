using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkWPF
{
    class ICallback
    {
        public Action UpdateCallback { get; set; }
        public Action FinalizeCallback { get; set; }
    }
}
