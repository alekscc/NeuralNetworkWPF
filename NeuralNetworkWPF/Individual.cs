using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkWPF
{
    class Individual : GenericIndividual<float, double>
    {
        public Individual(int[] n, Random rand = null)
        {


            Genes = new float[n.Length][];

            for (int i = 0; i < Genes.Length; i++)
                Genes[i] = new float[n[i]];

            if (rand != null)
                RandomizeGenes(rand);
        }
        public Individual(float[][] weights)
        {
            Genes = weights;
        }
        public Individual(GenericIndividual<float, double> parent)
        {
            Genes = parent.Genes;
            Fitness = parent.Fitness;
        }
        public override void Mutation(double rate, Random rand)
        {
            for (int i = 0; i < Genes.Length; i++)
            {
                for (int j = 0; j < Genes[i].Length; j++)
                {
                    if (rand.NextDouble() < rate)
                        Genes[i][j] = GetRandomGene(1, rand).First();
                }
            }
        }
        private IEnumerable<float> GetRandomGene(int n, Random rand)
        {
            while (n-- > 0)
                yield return rand.Next(-1000, 1000) * 0.01f;
        }
        public override void RandomizeGenes(Random rand)
        {
            for (int i = 0; i < Genes.Length; i++)
            {
                //Genes[i] = GetRandomGene(Genes[i].Length, rand).ToArray();

                for (int j = 0; j < Genes[i].Length; j++)
                {
                    Genes[i][j] = GetRandomGene(1, rand).First();
                }
            }

        }
        public override GenericIndividual<float, double> Crossover(GenericIndividual<float, double> o, Random rand, float a)
        {
            Individual child = new Individual(Utils.ArrayToIntTree<float>(Genes));

            //child.Genes = Genes.Zip(o.Genes, (a, b) => a.Zip(b,(x,y)=> (x+y)*0.5f).ToArray() ).ToArray();



            for (int i = 0; i < Genes.Length; i++)
            {

                for (int j = 0; j < Genes[i].Length; j++)
                {

                    //float diff = Math.Abs(Genes[i][j] - o.Genes[i][j]);
                    //float m;
                    //if (rand.NextDouble()<0.8)
                    //{
                    //    m = (rand.NextDouble() < 0.55) ? Genes[i][j] : o.Genes[i][j];

                    //}
                    //else
                    //{
                    //    m = Lerp(Genes[i][j], o.Genes[i][j], (float)rand.NextDouble());
                    //}
                    //child.Genes[i][j] = m;

                    //float perc = (rand.NextDouble() < 0.55) ? .75f : 0.25f;
                    // float perc = (i % 3 == 0) ? 0.15f : .85f;
                    //child.Genes[i][j] = (rand.NextDouble() < 0.5) ? Genes[i][j]  : (rand.Next()<0.5) ? Lerp(o.Genes[i][j], Genes[i][j], 0.5f) : o.Genes[i][j];
                    //child.Genes[i][j] = Lerp(o.Genes[i][j], Genes[i][j], (float)rand.NextDouble());
                    //Lerp(o.Genes[i][j], Genes[i][j], perc);
                    //child.Genes[i][j] = Lerp(Genes[i][j] - a*(o.Genes[i][j] - Genes[i][j]),o.Genes[i][j] + a*(o.Genes[i][j] - Genes[i][j]),(float)rand.NextDouble());

                    float x1 = Genes[i][j],
                            x2 = o.Genes[i][j];

                    a = rand.Next(0, 100) * 0.01f;
                    //if (o.Genes[i][j] > Genes[i][j])
                    //{
                    //    x2 = o.Genes[i][j];
                    //    x1 = Genes[i][j];
                    //}
                    //else
                    //{
                    //    x1 = o.Genes[i][j];
                    //    x2 = Genes[i][j];
                    //}


                    child.Genes[i][j] = Lerp(x1 - a * (x2 - x1), x2 + a * (x2 - x1), (float)rand.NextDouble());
                }
            }

            return child;
        }
        float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }

    }
}








//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace NeuralNetworkWPF
//{
//    class Individual : GenericIndividual<float,double>
//    {
//        public Individual(int[] n, Random rand = null)
//        {


//            Genes = new float[n.Length][];

//            for (int i = 0; i < Genes.Length; i++)
//                Genes[i] = new float[n[i]];

//            if (rand != null)
//                RandomizeGenes(rand);
//        }
//        public Individual(float[][] weights)
//        {
//            Genes = weights;
//        }
//        public Individual(GenericIndividual<float, double> parent)
//        {
//            Genes = parent.Genes;
//            Fitness = parent.Fitness;
//        }
//        public override void Mutation(double rate,Random rand)
//        {
//            for(int i=0;i<Genes.Length;i++)
//            {
//                for(int j=0;j<Genes[i].Length;j++)
//                {
//                    if (rand.NextDouble() < rate)
//                        Genes[i][j] = GetRandomGene(1, rand).First();
//                }
//            }
//        }
//        private IEnumerable<float> GetRandomGene(int n,Random rand)
//        {
//            while(n-->0)
//                yield return rand.Next(-1000, 1000) * 0.01f;
//        }
//        public override void RandomizeGenes(Random rand)
//        {
//            for (int i = 0; i < Genes.Length; i++)
//            {
//                //Genes[i] = GetRandomGene(Genes[i].Length, rand).ToArray();

//                for (int j = 0; j < Genes[i].Length; j++)
//                {
//                    Genes[i][j] = GetRandomGene(1, rand).First();
//                }
//            }

//        }
//        public override GenericIndividual<float, double> Crossover(GenericIndividual<float, double> o,Random rand,float a)
//        {
//            Individual child = new Individual(Utils.ArrayToIntTree<float>(Genes));

//            //child.Genes = Genes.Zip(o.Genes, (a, b) => a.Zip(b,(x,y)=> (x+y)*0.5f).ToArray() ).ToArray();



//            for (int i = 0; i < Genes.Length; i++)
//            {

//                for (int j = 0; j < Genes[i].Length; j++)
//                {

//                    //float diff = Math.Abs(Genes[i][j] - o.Genes[i][j]);
//                    //float m;
//                    //if (rand.NextDouble() < 0.5)
//                    //{
//                    //    m = (rand.NextDouble() < 0.65) ? Genes[i][j] : o.Genes[i][j];

//                    //}
//                    //else
//                    //{
//                    //    m = Lerp(Genes[i][j], o.Genes[i][j], (float)rand.NextDouble());
//                    //}
//                    //child.Genes[i][j] = m;

//                    //float perc = (rand.NextDouble() < 0.55) ? .75f : 0.25f;
//                    // float perc = (i % 3 == 0) ? 0.15f : .85f;
//                    //child.Genes[i][j] = (rand.NextDouble() < 0.5) ? Genes[i][j]  : (rand.Next()<0.5) ? Lerp(o.Genes[i][j], Genes[i][j], 0.5f) : o.Genes[i][j];
//                    //child.Genes[i][j] = Lerp(o.Genes[i][j], Genes[i][j], (float)rand.NextDouble());
//                    //Lerp(o.Genes[i][j], Genes[i][j], perc);
//                    //child.Genes[i][j] = Lerp(Genes[i][j] - a*(o.Genes[i][j] - Genes[i][j]),o.Genes[i][j] + a*(o.Genes[i][j] - Genes[i][j]),(float)rand.NextDouble());

//                    //child.Genes[i][j] = (rand.NextDouble() < 0.6) ? Genes[i][j] : o.Genes[i][j];

//                    float x1, x2;

//                    if (o.Genes[i][j] > Genes[i][j])
//                    {
//                        x2 = o.Genes[i][j];
//                        x1 = Genes[i][j];
//                    }
//                    else
//                    {
//                        x1 = o.Genes[i][j];
//                        x2 = Genes[i][j];
//                    }


//                    child.Genes[i][j] = Lerp(x1 - a * (x2 - x1), x2 + a * (x2 - x1), (float)rand.NextDouble());
//                }
//            }

//            return child;
//        }
//        float Lerp(float firstFloat, float secondFloat, float by)
//        {
//            return firstFloat * (1 - by) + secondFloat * by;
//        }

//    }
//}
