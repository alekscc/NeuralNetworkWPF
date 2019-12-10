using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkWPF
{
    abstract class GenericIndividual<T, X>
    {
        public T[][] Genes { get; protected set; }
        public X Fitness { get; set; }
        public X Probality { get; set; }

        public Func<GenericIndividual<T, X>, GenericIndividual<T, X>> CrossoverFunc { get; set; }
        public Action<double, Random> MutationAct { get; set; }

        public abstract void RandomizeGenes(Random rand);
        public abstract GenericIndividual<T, X> Crossover(GenericIndividual<T, X> o,Random rand,float a);
        public abstract void Mutation(double rate, Random rand);

    }
}
