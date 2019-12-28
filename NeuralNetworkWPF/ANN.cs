using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using static NeuralNetworkWPF.GA_NN_Settings;
using System.IO;

namespace NeuralNetworkWPF
{
    class ANN : ICallback
    {
        public TimeSpan ElapsedTime { get; private set; }

        public float Err { get; private set; }
        public float MaxErr { get; private set; }
        public float ValidateErr { get; private set; }

        public float TestErr { get; private set; }
        public float AvgFitness { get; private set; }
        public float Bias { get; private set; }

        public string[][] Outputs { get; private set; }

        public int GenerationNumber
        {
            get
            {
                return population?.GenerationNumber ?? 0;
            }
        }

        private float maxErr;
        private Population population;
        private int[] layers;
        private float[][] weights;
        private float[][] wsRecord;


        public Func<float, float> ActivationFunc { get; set; }

        public ANN(int[] layers)
        {

            weights = new float[layers.Length - 1][];

            for (int i = 0; i < layers.Length; i++)
            {
                if (i > 0)
                {
                    int layersWeights = layers[i - 1] * layers[i];

                    weights[i - 1] = new float[layersWeights];
                }
            }

            this.layers = layers;

        }
        public void Initialize(int populationSize, float maxErr, float elitism, float mutationRate,SelectionMethod selectionMethod, float bias = 1f)
        {
            ActivationFunc = (x) => (float)Math.Tanh((float)x);
            MaxErr = maxErr;
            Bias = bias;
            population = new Population(populationSize, Utils.ArrayToIntTree<float>(weights), elitism, mutationRate,selectionMethod);

        }
        //public void Test(float[][] tdata)
        //{


        //    //comErr = new int[4] { 1, 1,1,1};
        //    Err = FeedForward(tdata, population.GetRecordSolution().Genes);

        //    //double comErrSum = comErr.Sum();

        //    Console.ForegroundColor = ConsoleColor.DarkGreen;
        //    Console.WriteLine("Test data: " + Err);
        //    Console.ForegroundColor = ConsoleColor.DarkRed;
        //    // Console.WriteLine("ComErr 1:" + comErr[0] / (float)comErrSum + " 2: " + comErr[1] / (float)comErrSum + " 3: " + comErr[2] / (float)comErrSum + " 4: " + comErr[3] / (float)comErrSum);
        //    Console.ForegroundColor = ConsoleColor.White;

        //    Utils.ConsoleDisplay2DArray(population.GetRecordSolution().Genes);

        //}
        public void Test(float[][] tdata)
        {
            TestErr = FeedForward(tdata, wsRecord,true);
        }
        public void Train(float[][] ldata, float[][] vdata, float[][] tdata)
        {
            Stopwatch stopwatch = new Stopwatch();
            Err = 1f;
            float err = 1f;

            wsRecord = population.GetCurrentSolution().Genes;


            bool isNewRecord = false;

            int i = 1, curPart=0, nParts=10;
            int lenPart = ldata.Length / nParts;
            float fitnessSum = 0f;




            stopwatch.Start();

            float[][] _ldata = Utils.SubArray2(ldata, curPart, nParts, true);
            float[][] _vdata = Utils.SubArray2(ldata, curPart++, nParts);

            do
            {
                float[][] ws = population.GetCurrentSolution().Genes;

                err = FeedForward(_ldata, ws);

                float evaluation = 1f - err;

                fitnessSum += evaluation;

                population.EvaluateSolution(evaluation);

                if (err < Err)
                {


                    Err = err;

                    wsRecord = ws;

                    ValidateErr = FeedForward(_vdata, wsRecord);

                    isNewRecord = true;

                }

                if (population.NextSolution())
                {

                    AvgFitness = fitnessSum / (float)i;
                    fitnessSum = 0f;
                    i = 1;

                    if (isNewRecord)
                    {
                        Global.Instance.Stats.IndividualRecords.Add(new Record<int, float, float>(population.GenerationNumber, Err, ValidateErr));

                        if (Err <= MaxErr)
                            break;

                        isNewRecord = false;
                    }

                    _ldata = Utils.SubArray2(ldata, curPart, nParts, true);
                    _vdata = Utils.SubArray2(ldata, curPart++, nParts);

                    if (curPart >= nParts)
                        curPart = 0;

                }

                ElapsedTime = stopwatch.Elapsed;

                UpdateCallback();

                ++i;

            } while (true);


            TestErr = FeedForward(tdata, wsRecord, true);

            stopwatch.Stop();

            ElapsedTime = stopwatch.Elapsed;

            File.WriteAllLines("outputs.csv",ToCsv(Outputs));

            FinalizeCallback();

            //Console.ForegroundColor = ConsoleColor.DarkMagenta;

            //Console.WriteLine("TestErr: " + TestErr.ToString());

            //Console.ForegroundColor = ConsoleColor.DarkMagenta;


        }

    private static IEnumerable<String> ToCsv<T>(T[][] data, string separator = ";")
    {
        for (int i = 0; i < data.Length; ++i)
            yield return string.Join(separator, Enumerable
              .Range(0, data[i].Length)
              .Select(j => data[i][j])); 
    }
        public float FeedForward(float[][] data, float[][] weights, bool isTest = false)
        {
            int index = 0;

            int nErrs = 0;

            if (isTest)
            {
                Outputs = new string[data.Length][];
            }

            foreach (float[] row in data)
            {
                float[] outputs = new float[row.Length - 1];

                for (int i = 0; i < outputs.Length; i++)
                    outputs[i] = row[i];

                for (int i = 1; i < layers.Length; i++)
                {
                    float[] sums = new float[layers[i]];

                    for (int j = 0; j < outputs.Length; j++)
                    {

                        for (int z = 0; z < sums.Length; z++)
                        {
                            sums[z] += outputs[j] * weights[i - 1][j * sums.Length + z];

                        }

                    }



                    outputs = new float[sums.Length];

                    outputs = sums.Select(x => ActivationFunc(x + Bias)).ToArray();


                }
                bool isFault = false;

                float max = outputs.Max();
                int selOutput = 0;
                for (int i = 0, j = 0; i < outputs.Length; i++)
                {
                    if (max == outputs[i])
                    {
                        j++;
                        selOutput = i;

                    }
                    if (j > 1)
                    {
                        isFault = true;
                        break;
                    }

                }


                if (selOutput != (int)row[row.Length - 1] - 1 || isFault)
                {
                    nErrs++;
                    isFault = true;
                }

                if (isTest)
                {
                    int i;
                    Outputs[index] = new string[outputs.Length + 2];
                    for (i = 0; i < Outputs[index].Length - 2; i++)
                        Outputs[index][i] = outputs[i].ToString();

                    Outputs[index][i++] = row[row.Length - 1].ToString();
                    Outputs[index][i] = (!isFault) ? "OK" : "BŁĄD";
                }

                ++index;
            }

            return (nErrs > 0) ? nErrs / (float)data.Length : 0f;
        }
    }
}
