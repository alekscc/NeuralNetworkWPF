using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkWPF
{
    class Population : GenericPopulation<Individual>
    {
        private Random random = new Random();

        private Individual recordSolution;
        // 0.7
        //private float eliteRate = 0.08f;
        public float Elitism { get; set; }
        public float MutationRate { get; set; }
        private int nElite = 0;
        private int tournamentGroupSize;


        private Func<Individual> SelectIndividual;

        private delegate void OnSelectionDelegate();
        private event OnSelectionDelegate OnSelectionEvent;

        public Population(int n, int[] m,float elitism,float mutationRate, GA_NN_Settings.SelectionMethod selectionMethod)
        {
            MutationRate = mutationRate;
            Elitism = elitism;
         
            GeneratePopulation(n, m);
            recordSolution = Individuals[0];
            nElite = (int)(Individuals.Length * Elitism);

            tournamentGroupSize = (int)(Individuals.Length * 0.02f);

            OnSelectionEvent+= ()=> Individuals = Individuals.OrderByDescending(x => x.Fitness).ToArray();

            switch (selectionMethod)
            {
                case GA_NN_Settings.SelectionMethod.Tournament:
                    {
                        SelectIndividual = SelectIndividualByTournament;

                        //System.Windows.MessageBox.Show("Metoda turniejowa");

                    }break;
                case GA_NN_Settings.SelectionMethod.Ranking:
                    {
                        SelectIndividual = ChooseRandomIndividual;
                        OnSelectionEvent += CalculateFitnessBasedOnRanking;

                        //System.Windows.MessageBox.Show("Metoda Rankingowa");
                    }
                    break;
            }


        }
        //public Population()
        //{
        //    // TODO
        //}
        public bool NextSolution()
        {
            if (recordSolution.Fitness < GetCurrentSolution().Fitness)
                recordSolution = GetCurrentSolution();

            if (++currentSolution >= Individuals.Length)
            {

                //Console.ForegroundColor = ConsoleColor.DarkBlue;
                //Console.WriteLine("curSol/AllSol " + currentSolution + " / " + Individuals.Length);

                currentSolution = 0;
                generationNumber++;

                Evolve();



                return true;
            }

            return false;
        }
        public void EvaluateSolution(double fitness)
        {
            Individuals[currentSolution].Fitness = Math.Pow(fitness, 6);
        }

        public Individual GetRecordSolution()
        {
            return recordSolution;
        }

        public override void Evolve()
        {
            //CalculateFitnessBasedOnRanking();

            EvaluatePopualtion();

            Selection(random);

            Mutation(MutationRate);

        }
        private void EvaluatePopualtion()
        {
            double fitnessSum = 0;


            foreach (Individual i in Individuals)
            {
                fitnessSum += i.Fitness;
                //if (i.Fitness > recordSolution.Fitness)
                //    recordSolution = i;
            }


            for (int i = 0; i < Individuals.Length; i++)
            {
                Individuals[i].Probality = Individuals[i].Fitness / fitnessSum;
            }
        }
        public override void GeneratePopulation(int n, int[] m)
        {
            Individuals = new Individual[n];

            Individuals = GenerateIndividuals(n, m).ToArray();
        }
        private IEnumerable<Individual> GenerateIndividuals(int n, int[] m)
        {
            while (n-- > 0)
                yield return new Individual(m, random);
        }
        public override void Mutation(double rate)
        {
            //foreach (Individual i in Individuals)
            //    i.Mutation(rate, random);

            for (int i = nElite; i < Individuals.Length; i++)
                Individuals[i].Mutation(rate, random);
        }
        private Individual ChooseRandomIndividual()
        {
            double r = random.NextDouble();
            int i = 0;

            while (r > 0)
            {
                r -= Individuals[i++].Probality;
            }

            return Individuals[--i]; // BLAD
        }
        private Individual TournamentSelection(int k = 4)
        {
            int[] selected = new int[k];
            int maxIt = 1000;
            while (k > 0 && maxIt++ > 0)
            {
                int r = random.Next(0, Individuals.Length);

                if (!selected.Contains(r))
                {
                    selected[--k] = r;
                }
            }

            Individual winner = Individuals[0];

            for (int i = 1; i < selected.Length; i++)
            {
                if (winner.Fitness < Individuals[selected[i]].Fitness)
                {
                    winner = Individuals[selected[i]];
                }
            }

            return winner;

        }
        private Individual SelectIndividualByTournament()
        {

            var set = Individuals.OrderBy(x => random.NextDouble());

            Individual[] competitors = set.Take(tournamentGroupSize).ToArray();
            Individual winner = competitors[0];
            for (int i = 1; i < competitors.Length; i++)
            {
                if (winner.Fitness < competitors[i].Fitness)
                    winner = competitors[i];
            }


            return winner;
        }
        private void CalculateFitnessBasedOnRanking()
        {
            //var set = Individuals.OrderByDescending(x => x.Fitness).ToArray();


            double fitSum = 0f;
            for (int i = 0; i < Individuals.Length; i++)
            {
                Individuals[i].Fitness = (Individuals.Length - i) / (double)Individuals.Length;

                fitSum += Individuals[i].Fitness;
            }

            for(int i =0;i< Individuals.Length;i++)
            {
                Individuals[i].Fitness /= fitSum;
                Individuals[i].Fitness = Math.Pow(Individuals[i].Fitness, 4);
            }

        }
        private Individual ChooseRandomLINQ()
        {
            double r = random.NextDouble();
            var set = Individuals.OrderBy(x => r);

            Individual selected = set.SkipWhile(x => x.Probality > r).FirstOrDefault();

            return (selected != null) ? selected : set.First();
        }
     
        public override void Selection(Random rand)
        {
            Individual[] pop = new Individual[Individuals.Length];


            OnSelectionEvent();

            float a = rand.Next(0, 100) * 0.01f;

            for (int i = 0; i < nElite; i++)
            {
                pop[i] = new Individual(Individuals[i]);
            }

            for (int i = nElite; i < Individuals.Length; i++)
            {
                Individual parentA, parentB;

                do
                {


                    parentA = SelectIndividual();
                    parentB = SelectIndividual();

                } while (parentA.Equals(parentB));


                if (parentA.Fitness > parentB.Fitness)
                {
                    pop[i] = new Individual(parentA.Crossover(parentB, rand, a));
                }
                else
                {
                    pop[i] = new Individual(parentB.Crossover(parentA, rand, a));
                }


            }
            Individuals = pop;
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


//    class Population : GenericPopulation<Individual>
//    {
//        public float Elitism { get; set; }
//        public float MutationRate { get; set; }


//        private Random random = new Random();

//        private Individual recordSolution;
//        // 0.7
//        private int nElite = 0;

//        public Population(int n, int[] m)
//        {
//            GeneratePopulation(n, m);
//            recordSolution = Individuals[0];
//            nElite =(int)(Individuals.Length * Elitism);
//        }
//        public Population()
//        {
//            // TODO
//        }
//        public bool NextSolution()
//        {
//            if (recordSolution.Fitness < GetCurrentSolution().Fitness)
//                recordSolution = GetCurrentSolution();

//            if (++currentSolution >= Individuals.Length)
//            {

//                //Console.ForegroundColor = ConsoleColor.DarkBlue;
//                //Console.WriteLine("curSol/AllSol " + currentSolution + " / " + Individuals.Length);

//                currentSolution = 0;
//                generationNumber++;

//                Evolve();



//                return true;
//            }

//            return false;
//        }
//        public void EvaluateSolution(double fitness)
//        {
//            Individuals[currentSolution].Fitness = Math.Pow(fitness, 6);
//        }

//        public Individual GetRecordSolution()
//        {
//            return recordSolution;
//        }

//        public override void Evolve()
//        {
//            //CalculateFitnessBasedOnRanking();

//            EvaluatePopualtion();

//            Selection(random);

//            Mutation(MutationRate);

//        }
//        private void EvaluatePopualtion()
//        {
//            double fitnessSum = 0;


//            foreach (Individual i in Individuals)
//            {
//                fitnessSum += i.Fitness;
//                //if (i.Fitness > recordSolution.Fitness)
//                //    recordSolution = i;
//            }


//            for (int i = 0; i < Individuals.Length; i++)
//            {
//                Individuals[i].Probality = Individuals[i].Fitness / fitnessSum;
//            }
//        }
//        public override void GeneratePopulation(int n, int[] m)
//        {
//            Individuals = new Individual[n];

//            Individuals = GenerateIndividuals(n, m).ToArray();
//        }
//        private IEnumerable<Individual> GenerateIndividuals(int n, int[] m)
//        {
//            while (n-- > 0)
//                yield return new Individual(m, random);
//        }
//        public override void Mutation(double rate)
//        {
//            //foreach (Individual i in Individuals)
//            //    i.Mutation(rate, random);

//            for (int i = nElite; i < Individuals.Length; i++)
//                Individuals[i].Mutation(rate, random);
//        }
//        private Individual ChooseRandomIndividual()
//        {
//            double r = random.NextDouble();
//            int i = 0;

//            while (r > 0)
//            {
//                r -= Individuals[i++].Probality;
//            }

//            return Individuals[--i]; // BLAD
//        }
//        private Individual TournamentSelection(int k = 4)
//        {
//            int[] selected = new int[k];
//            int maxIt = 1000;
//            while(k > 0 && maxIt++ > 0)
//            {
//                int r = random.Next(0, Individuals.Length);

//                if (!selected.Contains(r))
//                {
//                    selected[--k] = r;
//                }
//            }

//            Individual winner = Individuals[0];

//            for(int i = 1; i < selected.Length; i++)
//            {
//                if (winner.Fitness < Individuals[selected[i]].Fitness)
//                {
//                    winner = Individuals[selected[i]];
//                }
//            }

//            return winner;

//        }
//        private Individual SelectIndividualByTournament(int k =4)
//        {

//            var set = Individuals.OrderBy(x => random.NextDouble());

//            Individual[] competitors = set.Take(k).ToArray();
//            Individual winner = competitors[0];
//            for (int i = 1; i < competitors.Length; i++)
//            {
//                if (winner.Fitness < competitors[i].Fitness)
//                    winner = competitors[i];
//            }

//            //Individual winner = Individuals[random.Next(0, Individuals.Length)];

//            //while (--k > 0)
//            //{
//            //    Individual competitor = Individuals[random.Next(0, Individuals.Length)];

//            //    if (competitor.Equals(winner))
//            //    {
//            //        ++k;
//            //        continue;
//            //    }

//            //    if (winner.Fitness < competitor.Fitness)
//            //        winner = competitor;

//            //}

//            return winner;
//        }
//        private void CalculateFitnessBasedOnRanking()
//        {
//            var set = Individuals.OrderByDescending(x => x.Fitness).ToArray();

//            for(int i = 0; i < set.Length; i++)
//            {
//                Individuals[i].Fitness = (set.Length - i-1) / (float)set.Length;

//                Individuals[i].Fitness = Math.Pow(Individuals[i].Fitness, 6);
//            }
//        }
//        private Individual ChooseRandomLINQ()
//        {
//            double r = random.NextDouble();
//            var set = Individuals.OrderBy(x => r);

//            Individual selected = set.SkipWhile(x => x.Probality > r).FirstOrDefault();

//            return (selected != null) ? selected : set.First() ;
//        }
//        public override void Selection(Random rand)
//        {
//            Individual[] pop = new Individual[Individuals.Length];



//            Individuals = Individuals.OrderByDescending(x => x.Fitness).ToArray();

//            //CalculateFitnessBasedOnRanking();
//            float a =  rand.Next(0, 100) * 0.01f;
//            for (int i = 0; i < nElite; i++)
//            {
//                pop[i] = new Individual(Individuals[i]);
//            }

//            for (int i = nElite; i < Individuals.Length; i++)
//            {
//                Individual parentA, parentB;

//                do
//                {
//                    parentA = SelectIndividualByTournament();
//                    parentB = SelectIndividualByTournament();

//                } while (parentA.Equals(parentB));

//                pop[i] = new Individual(parentA.Crossover(parentB, rand, a));

//                if (parentA.Fitness > parentB.Fitness)
//                {
//                    pop[i] = new Individual(parentA.Crossover(parentB, rand,a));
//                }
//                else
//                {
//                    pop[i] = new Individual(parentB.Crossover(parentA, rand,a));
//                }


//            }
//            Individuals = pop;
//            //    Individuals = pop;
//            //}
//            //public override void Selection(Random rand)
//            //{
//            //    Individual[] pop = new Individual[Individuals.Length];
//            //}

//        }
//    }
//}
