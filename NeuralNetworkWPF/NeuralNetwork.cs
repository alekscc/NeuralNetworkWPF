using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkWPF
{
    public class NeuralNetwork
    {
        public string[][] Content { get; set; }

        public float Err { get; private set; }
        public float MinErr { get; private set; }
        public int Generation { get; private set; }
        public float AvgFit { get; private set; }


        public Action UpdateCallback { get; set; }
        public Action FinalizeCallback { get; set; }

        public float ValidateErr { get; private set; }
        public float TestErr { get; private set; }

        public TimeSpan ElapsedTime { get; private set; }

        private Population population;

        private int nInputs, indexOutput;
        private int[] layers;

        private float[][] weights;


        private int[] comErr;

        private Action callBack;

        public void SetCallBack(Action action)
        {
            callBack = action;
        }

        public NeuralNetwork(int[] layers)
        {
            Err = 0f;
            MinErr = 0f;
            AvgFit = 0f;
            Generation = 0;

            nInputs = layers[0];
            indexOutput = layers[layers.Length - 1];

            weights = new float[layers.Length-1][];

            for(int i=0;i<layers.Length;i++)
            {

                if (i>0)
                {
                    int layersWeights = layers[i - 1] * layers[i];
                    //Console.WriteLine(i.ToString()+" Layer weights = " + layersWeights);

                    weights[i-1] = new float[layersWeights];
                }

                //Console.Write("L" + i.ToString() + ": " + layers[i]);


                //if (i == 0)
                //    Console.Write(" <-INPUT");
                //else if (i == layers.Length - 1)
                //    Console.Write(" <-OUTPUT");
                //else Console.Write(" <-HIDDEN");


                    Console.WriteLine();
            }

            this.layers = layers;

            int[] ar = Utils.ArrayToIntTree<float>(weights);

            //foreach (int i in ar)
            //    Console.Write(i.ToString() + " ; ");
            // 200
            population = new Population(200, Utils.ArrayToIntTree<float>(weights));

        }
        public void Test(float[][] tdata)
        {
            double err = 0;


            //comErr = new int[4] { 1, 1,1,1};
            err = FeedForward(tdata, population.GetRecordSolution().Genes,true);

            //double comErrSum = comErr.Sum();

           // Console.ForegroundColor = ConsoleColor.DarkGreen;
           // Console.WriteLine("Test data: " + err);
           // Console.ForegroundColor = ConsoleColor.DarkRed;
           //// Console.WriteLine("ComErr 1:" + comErr[0] / (float)comErrSum + " 2: " + comErr[1] / (float)comErrSum + " 3: " + comErr[2] / (float)comErrSum + " 4: " + comErr[3] / (float)comErrSum);
           // Console.ForegroundColor = ConsoleColor.White;

           // Utils.ConsoleDisplay2DArray(population.GetRecordSolution().Genes);

        }
        public bool Train(float[][] ldata,float[][] vdata,float[][] tdata, double minErr=0.005,int k = 10)
        {
            Stopwatch stopwatch = new Stopwatch();



            double err = 1,
                   errRecord = 1,
                   validationRecord = 1;
            float[][] ws;

            int i = 1;
            double fitnessSum = 0;
            double sumAllErrs = 0;

            stopwatch.Start();
            do
            {

                //comErr = new int[4] { 1, 1,1,1 };

                //float[][] valData = Utils.SubArray<float[]>(ldata, i * foldSize, i * foldSize + foldSize);
                //float[][] testData = Utils<float[]>.SubArray(ldata, i * foldSize + foldSize,
                ws = population.GetCurrentSolution().Genes;

                err = FeedForward(ldata,ws);

                fitnessSum += (1 - err) ;
                //sumAllErrs += comErr.Sum();

                population.EvaluateSolution(1 - err);

                if (err < errRecord)
                {
                    //Console.ForegroundColor = ConsoleColor.DarkYellow;

                    //Console.WriteLine("Err: " + err.ToString() + " Gen: " + population.GenerationNumber + " <minErr" + minErr + " Fitness: " + population.GetRecordSolution().Fitness);
                    ////int comErrSum = comErr.Sum();
                    //Console.ForegroundColor = ConsoleColor.DarkRed;
                    //Console.WriteLine("DistErr 1:" + comErr[0] / (float)comErrSum + " // " + comErr[0] + " 2: " + comErr[1] / (float)comErrSum + " // " + comErr[1] + " 3: " + comErr[2] / (float)comErrSum + " // " + comErr[2] + " 4: " + comErr[3] / (float)comErrSum + " // " + comErr[3] + " totalErr:"+ comErrSum);
             

                    errRecord = err;
                    Err = (float)err;

                    ValidateErr = (float)FeedForward(vdata, ws);

                    

                }

                if(population.NextSolution())
                {
                    //Console.ForegroundColor = ConsoleColor.DarkCyan;
                    //Console.WriteLine("Generation: " + population.GenerationNumber + " Avg. Fitness: " + fitnessSum / i +  " Avg. Errs: " + sumAllErrs / i);
                    Generation = population.GenerationNumber;
                    AvgFit = (float)(fitnessSum / i);

                    Global.Instance.Stats.IndividualRecords.Add(new Record<int, float, float>(Generation-1, Err, ValidateErr));

                    sumAllErrs = 0;
                    fitnessSum = 0;
                    i = 1;

               
                }

                
                MinErr = (float)minErr;

                ElapsedTime = stopwatch.Elapsed;

                UpdateCallback();

                ++i;   

                
    
            } while (err>minErr);

           

            TestErr = (float)FeedForward(tdata, ws);

            stopwatch.Stop();

            ElapsedTime = stopwatch.Elapsed;

            FinalizeCallback();
            //Console.WriteLine("Err: " + err.ToString() + " Gen: " + population.GenerationNumber + " <minErr" + minErr);

            return true;
        }
          private double FeedForward(float[][] ldata,float[][] weights,bool fillResult = false)
        {

            if(fillResult != false)
            {
                Content = new string[ldata.Length][];
            }

            int nErrs = 0;
            //if(fillResult)
            //{
            //    Console.WriteLine();
            //    Utils.ConsoleDisplay2DArray(weights);

            //    Console.ReadKey();
            //}


            int index = 0;
            //Console.WriteLine();
            foreach (float[] row in ldata)
            {
                float[] data = new float[row.Length-1];

                for (int i = 0; i < data.Length; i++)
                    data[i] = row[i];

                for(int i=1;i<layers.Length;i++)
                {
                    float[] sums = new float[layers[i]];
                    //Console.WriteLine("w[" + i + "]");
                    for (int j=0;j< data.Length;j++)
                    {
                        
                        for (int z=0;z<sums.Length;z++)
                        {
                            sums[z] += data[j] * weights[i-1][j*sums.Length+z];

                          //  Console.Write(weights[i - 1][j * sums.Length + z]+"; ");
                          // Console.WriteLine("sum[" + z + "]=" + sums[z].ToString());
                         // Console.WriteLine("")
                        }
                       // Console.WriteLine();
                    }
                   // Console.WriteLine("++++++++");


                    data = new float[sums.Length];

                    data = sums.Select(x => Activation(x+1f)).ToArray();

                   
                }

                //Console.ReadKey();


                //Console.ReadKey();
                //Console.ReadKey();
                //if(maxVal>1f)
                //    Console.WriteLine("hi: " + maxVal);

                //float[] output = data.Where(x => x > 0f).ToArray();

                //if (output.Length > 1)
                //    nErrs++;
                //else if (output[0] != row[row.Length-1])
                //    nErrs++;

                //Console.WriteLine("Data " + data[0] + " " + data[1] + " " +data[2] + " " + data[3]);

                //int hiValue = 0;


                //foreach (float f in data)
                //    if (f > 0f) hiValue++;

        

                //float max = data[0];
                //int selected = 0;
                //int iMax = 0;
                bool isFault = false;

                float max = data.Max();
                int selected = 0;
                for (int i = 0,j=0; i < data.Length; i++)
                {
                    if (max == data[i])
                    {
                        j++;
                        selected = i;
                    }
                        

                    if (j > 1)
                    {
                        isFault = true;
                        break;
                    }
                }


                    //for(int ii = 0; ii < data.Length; ii++)
                    //{
                    //    if (data[ii] >= max)
                    //    {
                    //        selected = ii;
                    //        max = data[ii];
                    //    }
                    //}

                    //for (int ii = 0; ii < data.Length; ii++)
                    //{
                    //    if(ii!=selected && max==data[ii])
                    //    {
                    //        isFault = true;
                    //    }
                    //}

                    //for(int ii = 0; ii < data.Length; ii++)
                    //{
                    //    if(data[ii] == max)
                    //    {
                    //        ++iMax;
                    //        if(iMax>1)
                    //        {
                    //            isFault = true;
                    //            break;
                    //        }
                    //    }
                    //}

                    if (fillResult)
                {
                    Content[index] = new string[(int)row[row.Length - 1] + 2];
                    Content[index][0] = data[0].ToString();
                    Content[index][1] = data[1].ToString();
                   // Content[index][2] = data[2].ToString();
                   //Content[index][2] = data[3].ToString();
                   // Content[index][3] = data[3].ToString();
                   // Content[index][4] = data[4].ToString();
                   // Content[index][5] = data[5].ToString();
                   // Content[index][6] = data[6].ToString();
                   // Content[index][7] = data[7].ToString();
                   // Content[index][8] = data[8].ToString();
                   // Content[index][9] = data[9].ToString();
                    Content[index][3] = (row[row.Length - 1]).ToString();
                    //Console.WriteLine("final test:");
                    //foreach (float f in data)
                    //{
                        
                    //    Console.Write(f + "\t");
                    //}
                    //Console.WriteLine();
                }

                if (selected != (int)row[row.Length - 1]-1 || isFault)
                {
                    nErrs++;
                    //comErr[(int)row[row.Length - 1] - 1]++;
                    if (fillResult)
                        Content[index][4] = "BLAD";
                }
                else if (fillResult)
                    Content[index][4] = "OK";

                ++index;
            }

            return nErrs/(double)ldata.Length;//(nErrs>0) ? nErrs / (double)ldata.Length : 0;
            //return (nErrs > 0) ? nErrs / (double)ldata.Length : 0;
        }
        private float Activation(float sum)
        {
            //return 1f / (float)(1f + Math.Exp(-sum));

          // return ( (float)Math.Exp(sum) - (float)Math.Exp(-sum)) / ((float)Math.Exp(sum) + (float)Math.Exp(-sum));

             //return (1f - (float)Math.Exp(-2f * sum)) / (1f + (float)Math.Exp(-2f * sum));
          // float result = (float)Math.Tanh(sum);
          //return  result;
            //return Math.Sign(sum);
         return (float)Math.Tanh(sum);
           //return Math.Sign(sum);
        }

   
        
        
    }
}
