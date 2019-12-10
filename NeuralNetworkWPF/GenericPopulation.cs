using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkWPF
{
    abstract class GenericPopulation<T>
    {
        public T[] Individuals { get; protected set; }



        protected int currentSolution=0,
                      generationNumber=0;

        public abstract void GeneratePopulation(int n,int[] m);
        public abstract void Evolve();
        public abstract void Selection(Random rand);
        public abstract void Mutation(double rate);

        public Action SelectionAct { get; set; }

      
        public T GetCurrentSolution()
        {
            return Individuals[currentSolution];
        }
        public int GenerationNumber
        {
            get { return generationNumber; }
        }


    }
}
