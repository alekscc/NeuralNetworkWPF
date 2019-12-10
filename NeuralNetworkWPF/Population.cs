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
        private float eliteRate = 0.1f;
        private int nElite = 0;

        public Population(int n, int[] m)
        {
            GeneratePopulation(n, m);
            recordSolution = Individuals[0];
            nElite =(int)(Individuals.Length * eliteRate);
        }
        public Population()
        {
            // TODO
        }
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

            Mutation(0.01f);

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
            while(k > 0 && maxIt++ > 0)
            {
                int r = random.Next(0, Individuals.Length);

                if (!selected.Contains(r))
                {
                    selected[--k] = r;
                }
            }

            Individual winner = Individuals[0];

            for(int i = 1; i < selected.Length; i++)
            {
                if (winner.Fitness < Individuals[selected[i]].Fitness)
                {
                    winner = Individuals[selected[i]];
                }
            }

            return winner;

        }
        private Individual SelectIndividualByTournament(int k =4)
        {

            var set = Individuals.OrderBy(x => random.NextDouble());

            Individual[] competitors = set.Take(k).ToArray();
            Individual winner = competitors[0];
            for (int i = 1; i < competitors.Length; i++)
            {
                if (winner.Fitness < competitors[i].Fitness)
                    winner = competitors[i];
            }

            //individual winner = individuals[random.next(0, individuals.length)];

            //while (--k > 0)
            //{
            //    individual competitor = individuals[random.next(0, individuals.length)];

            //    if (competitor.equals(winner))
            //    {
            //        ++k;
            //        continue;
            //    }

            //    if (winner.fitness < competitor.fitness)
            //        winner = competitor;

            //}

            return winner;
        }
        private void CalculateFitnessBasedOnRanking()
        {
            var set = Individuals.OrderByDescending(x => x.Fitness).ToArray();

            for(int i = 0; i < set.Length; i++)
            {
                Individuals[i].Fitness = (set.Length - i-1) / (float)set.Length;

                Individuals[i].Fitness = Math.Pow(Individuals[i].Fitness, 6);
            }
        }
        private Individual ChooseRandomLINQ()
        {
            double r = random.NextDouble();
            var set = Individuals.OrderBy(x => r);

            Individual selected = set.SkipWhile(x => x.Probality > r).FirstOrDefault();

            return (selected != null) ? selected : set.First() ;
        }
        public override void Selection(Random rand)
        {
            Individual[] pop = new Individual[Individuals.Length];

            float a = rand.Next(0,100) * 0.01f;

            Individuals = Individuals.OrderByDescending(x => x.Fitness).ToArray();

            //CalculateFitnessBasedOnRanking();

            for (int i = 0; i < nElite; i++)
            {
                pop[i] = new Individual(Individuals[i]);
            }

            for (int i = nElite; i < Individuals.Length; i++)
            {
                Individual parentA, parentB;

                do
                {
                    parentA = SelectIndividualByTournament();
                    parentB = SelectIndividualByTournament();

                } while (parentA.Equals(parentB));

                pop[i] = new Individual(parentA.Crossover(parentB, rand, a));

                if (parentA.Fitness > parentB.Fitness)
                {
                    pop[i] = new Individual(parentA.Crossover(parentB, rand,a));
                }
                else
                {
                    pop[i] = new Individual(parentB.Crossover(parentA, rand,a));
                }


            }
            Individuals = pop;
            //    Individuals = pop;
            //}
            //public override void Selection(Random rand)
            //{
            //    Individual[] pop = new Individual[Individuals.Length];
            //}

        }
    }
}
