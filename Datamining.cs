
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Console_kmeans
{
    public class Kmeans : Datamining
    {
        public const string FILE_NAME = "rfm.data";
        public const string FILE_NAME2 = "Result.data";
        public double[,] Customers = new double[numCustomers, 3];
        public double[,] Centroid = new double[3, 3];
        public double[,] distance = new double[numCustomers, 3];

        public bool[,] Groups = new bool[numCustomers, 3];
        public bool[,] Group1 = new bool[numCustomers, 3];
        public bool[,] Group2 = new bool[numCustomers, 3];
        public double[] G1 = new double[numCustomers * 2];
        public double[] G2 = new double[numCustomers * 2];
        public double[] G3 = new double[numCustomers * 2];

        public object[] MinArr = new object[numCustomers];
        public object MaxMonetary = new object();
        public object MaxFrequency = new object();
        public object[] SetArray = new object[numCustomers];

        bool isStillMoving = true;
        public int numMemInG1, numMemInG2, numMemInG3 = 0;
        public int[] A = new int[numCustomers];
        public int[] B = new int[numCustomers];
        public double MaxNumTrue;
        public int[] col = new int[3];

        public double totalG1 = 0;
        public double totalG2 = 0;
        public double totalG3 = 0;
        public double Gold = 0;
        public double Bronze = 0;


        //Recomputation Centroids
        public void ReCentroid()
        {

            for (int k = 0; k < 3; k++)
            {
                col[k] = 0;
            }
            for (int r = 0; r < numCustomers; r++)
            {
                for (int Y = 0; Y < 3; Y++)
                {
                    if ((double)MinArr[r] == distance[r, Y])
                    {
                        A[r] = r;
                        B[Y] = Y;
                        switch (Y)
                        {
                            case 0:
                                col[0]++;
                                break;
                            case 1:
                                col[1]++;
                                break;
                            case 2:
                                col[2]++;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            MaxNumTrue = (double)max(col[0], col[1], col[2], 0);

            double[] buff = new double[(int)numCustomers * 3];
            double[] buffEQ = new double[(((int)numCustomers * (int)Math.Pow(MaxNumTrue, 2))) + 4];

            int i = 0;
            for (int Y = 0; Y < 3; Y++)
            {
                for (int r = 0; r < numCustomers; r++)
                {
                    if ((double)MinArr[r] == distance[r, Y])
                    {
                        if (MaxNumTrue == col[Y])
                        {
                            A[r] = r;
                            B[Y] = Y;

                            if (((col[0] == MaxNumTrue) && (col[1] == MaxNumTrue)) || ((col[0] == MaxNumTrue) && (col[2] == MaxNumTrue)) || ((col[1] == MaxNumTrue) && (col[2] == MaxNumTrue)))
                            {
                                if (numCustomers == 3)
                                    break;

                                if (Y == 0)
                                {
                                    buffEQ[i] = Customers[A[r], B[Y]];
                                    buffEQ[i + 1] = Customers[A[r], B[Y] + 1];
                                    buffEQ[i + 2] = Customers[A[r], B[Y] + 2];
                                }
                                else if (Y == 1)
                                {
                                    buffEQ[i] = Customers[A[r], B[Y] - 1];
                                    buffEQ[i + 1] = Customers[A[r], B[Y]];
                                    buffEQ[i + 2] = Customers[A[r], B[Y] + 1];
                                }
                                else
                                {
                                    buffEQ[i] = Customers[A[r], B[Y] - 2];
                                    buffEQ[i + 1] = Customers[A[r], B[Y] - 1];
                                    buffEQ[i + 2] = Customers[A[r], B[Y]];
                                }
                                Console.WriteLine("buffEQ[{0}]= {1}\t buffEQ[{2}]= {3}\t buffEQ[{4}]= {5}\n", i, buffEQ[i], i + 1, buffEQ[i + 1], i + 2, buffEQ[i + 2]);
                                i += 3;
                            }
                            else
                            {

                                if (i == MaxNumTrue * 3)
                                {
                                    i = 0;
                                }
                                if (Y == 0)
                                {
                                    buff[i] = Customers[A[r], B[Y]];
                                    buff[i + 1] = Customers[A[r], B[Y] + 1];
                                    buff[i + 2] = Customers[A[r], B[Y] + 2];
                                }
                                else if (Y == 1)
                                {
                                    buff[i] = Customers[A[r], B[Y] - 1];
                                    buff[i + 1] = Customers[A[r], B[Y]];
                                    buff[i + 2] = Customers[A[r], B[Y] + 1];
                                }
                                else
                                {
                                    buff[i] = Customers[A[r], B[Y] - 2];
                                    buff[i + 1] = Customers[A[r], B[Y] - 1];
                                    buff[i + 2] = Customers[A[r], B[Y]];
                                }
                                Console.WriteLine("buff[{0}]= {1}\t buff[{2}]= {3}\t buff[{4}]= {5}\n", i, buff[i], i + 1, buff[i + 1], i + 2, buff[i + 2]);
                                i += 3;
                            }
                        }
                    }
                }
            }

            for (int Y = 0; Y < 3; Y++)
            {
                for (int r = 0; r < numCustomers; r++)
                {
                    if ((double)MinArr[r] == distance[r, Y])
                    {
                        if (MaxNumTrue == col[Y])
                        {
                            A[r] = r;
                            B[Y] = Y;
                            if (((col[0] == MaxNumTrue) && (col[1] == MaxNumTrue)) || ((col[0] == MaxNumTrue) && (col[2] == MaxNumTrue)) || ((col[1] == MaxNumTrue) && (col[2] == MaxNumTrue)))
                            {
                                if (numCustomers == 3)
                                    break;

                                if (Y == 0)
                                {
                                    for (int p = 0; p < 3; p++)
                                    {
                                        for (int h = 0; h < (MaxNumTrue * 10); h += 3)
                                        {
                                            if (p == 0)
                                            {
                                                if (h == 0)
                                                    Centroid[0, p] = (buffEQ[h] + buffEQ[h + 3]);
                                                else if (MaxNumTrue > 2)
                                                    Centroid[0, p] = (Centroid[0, p] + buffEQ[h + 3]);
                                            }
                                            else if (p == 1)
                                            {
                                                if (h == 0)
                                                    Centroid[0, p] = (buffEQ[h + 1] + buffEQ[h + 4]);
                                                else if (MaxNumTrue > 2)
                                                    Centroid[0, p] = (Centroid[0, p] + buffEQ[h + 4]);
                                            }
                                            else if (p == 2)
                                            {
                                                if (h == 0)
                                                    Centroid[0, p] = (buffEQ[h + 2] + buffEQ[h + 5]);
                                                else if (MaxNumTrue > 2)
                                                    Centroid[0, p] = (Centroid[0, p] + buffEQ[h + 5]);
                                            }
                                        }
                                    }
                                    for (int p = 0; p < 3; p++)
                                    {
                                        Centroid[0, p] /= MaxNumTrue;
                                    }
                                }
                                else if (Y == 1)
                                {
                                    for (int p = 0; p < 3; p++)
                                    {
                                        for (int h = 0; h < (MaxNumTrue * 10); h += 3)
                                        {
                                            if (p == 0)
                                            {
                                                if ((col[0] == MaxNumTrue) && (col[1] == MaxNumTrue))
                                                {
                                                    if (h == 6)
                                                        Centroid[1, p] = (buffEQ[h] + buffEQ[h + 3]);
                                                }
                                                else if (h == 0)
                                                {
                                                    Centroid[1, p] = (buffEQ[h] + buffEQ[h + 3]);
                                                }
                                                else if (MaxNumTrue > 2)
                                                    Centroid[1, p] = (Centroid[1, p] + buffEQ[h + 3]);
                                            }
                                            else if (p == 1)
                                            {
                                                if ((col[0] == MaxNumTrue) && (col[1] == MaxNumTrue))
                                                {
                                                    if (h == 6)
                                                        Centroid[1, p] = (buffEQ[h + 1] + buffEQ[h + 4]);
                                                }
                                                else if (h == 0)
                                                {
                                                    Centroid[1, p] = (buffEQ[h + 1] + buffEQ[h + 4]);
                                                }
                                                else if (MaxNumTrue > 2)
                                                    Centroid[1, p] = (Centroid[1, p] + buffEQ[h + 4]);
                                            }
                                            else if (p == 2)
                                            {
                                                if ((col[0] == MaxNumTrue) && (col[1] == MaxNumTrue))
                                                {
                                                    if (h == 6)
                                                        Centroid[1, p] = (buffEQ[h + 2] + buffEQ[h + 5]);
                                                }
                                                else if (h == 0)
                                                {
                                                    Centroid[1, p] = (buffEQ[h + 2] + buffEQ[h + 5]);
                                                }
                                                else if (MaxNumTrue > 2)
                                                    Centroid[1, p] = (Centroid[1, p] + buffEQ[h + 5]);
                                            }
                                        }
                                    }
                                    for (int p = 0; p < 3; p++)
                                    {
                                        Centroid[1, p] /= MaxNumTrue;
                                    }
                                }
                                else
                                {
                                    for (int p = 0; p < 3; p++)
                                    {
                                        for (int h = 0; h < (MaxNumTrue * 10); h += 3)
                                        {
                                            if (p == 0)
                                            {
                                                if (((col[0] == MaxNumTrue) && (col[2] == MaxNumTrue)) || ((col[1] == MaxNumTrue) && (col[2] == MaxNumTrue)))
                                                {
                                                    if (h == 6)
                                                        Centroid[2, p] = (buffEQ[h] + buffEQ[h + 3]);
                                                }
                                                else if (h == 0)
                                                {
                                                    Centroid[2, p] = (buffEQ[h] + buffEQ[h + 3]);
                                                }
                                                else if (MaxNumTrue > 2)
                                                    Centroid[2, p] = (Centroid[2, p] + buffEQ[h + 3]);
                                            }
                                            else if (p == 1)
                                            {
                                                if (((col[0] == MaxNumTrue) && (col[2] == MaxNumTrue)) || ((col[1] == MaxNumTrue) && (col[2] == MaxNumTrue)))
                                                {
                                                    if (h == 6)
                                                        Centroid[2, p] = (buffEQ[h + 1] + buffEQ[h + 4]);
                                                }
                                                else if (h == 0)
                                                {
                                                    Centroid[2, p] = (buffEQ[h + 1] + buffEQ[h + 4]);
                                                }
                                                else if (MaxNumTrue > 2)
                                                    Centroid[2, p] = (Centroid[2, p] + buffEQ[h + 4]);
                                            }
                                            else if (p == 2)
                                            {
                                                if (((col[0] == MaxNumTrue) && (col[2] == MaxNumTrue)) || ((col[1] == MaxNumTrue) && (col[2] == MaxNumTrue)))
                                                {
                                                    if (h == 6)
                                                        Centroid[2, p] = (buffEQ[h + 2] + buffEQ[h + 5]);
                                                }
                                                else if (h == 0)
                                                {
                                                    Centroid[2, p] = (buffEQ[h + 2] + buffEQ[h + 5]);
                                                }

                                                else if (MaxNumTrue > 2)
                                                    Centroid[2, p] = (Centroid[2, p] + buffEQ[h + 5]);
                                            }
                                        }
                                    }
                                    for (int p = 0; p < 3; p++)
                                    {
                                        Centroid[2, p] /= MaxNumTrue;
                                    }
                                }
                            }//end if (Equal)
                            else
                            {
                                if (Y == 0)
                                {
                                    for (int x = 0; x < 3; x++)
                                        for (int y = 0; y < 3; y++)
                                            Centroid[1, y] = Customers[1, y];
                                    for (int x = 0; x < 3; x++)
                                        for (int y = 0; y < 3; y++)
                                            Centroid[2, y] = Customers[2, y];

                                    for (int p = 0; p < 3; p++)
                                    {
                                        for (int h = 0; h < (MaxNumTrue * 3); h += 3)
                                        {
                                            if (p == 0)
                                            {
                                                if (h == 0)
                                                    Centroid[0, p] = (buff[h] + buff[h + 3]);
                                                else
                                                    Centroid[0, p] = (Centroid[0, p] + buff[h + 3]);
                                            }
                                            else if (p == 1)
                                            {
                                                if (h == 0)
                                                    Centroid[0, p] = (buff[h + 1] + buff[h + 4]);
                                                else
                                                    Centroid[0, p] = (Centroid[0, p] + buff[h + 4]);
                                            }
                                            else if (p == 2)
                                            {
                                                if (h == 0)
                                                    Centroid[0, p] = (buff[h + 2] + buff[h + 5]);
                                                else
                                                    Centroid[0, p] = (Centroid[0, p] + buff[h + 5]);
                                            }
                                        }
                                    }
                                    for (int p = 0; p < 3; p++)
                                    {
                                        Centroid[0, p] /= MaxNumTrue;
                                    }
                                }
                                else if (Y == 1)
                                {
                                    for (int x = 0; x < 3; x++)
                                        for (int y = 0; y < 3; y++)
                                            Centroid[0, y] = Customers[0, y];

                                    for (int x = 0; x < 3; x++)
                                        for (int y = 0; y < 3; y++)
                                            Centroid[2, y] = Customers[2, y];

                                    for (int p = 0; p < 3; p++)
                                    {
                                        for (int h = 0; h < (MaxNumTrue * 3); h += 3)
                                        {
                                            if (p == 0)
                                            {
                                                if (h == 0)
                                                    Centroid[1, p] = (buff[h] + buff[h + 3]);
                                                else
                                                    Centroid[1, p] = (Centroid[1, p] + buff[h + 3]);
                                            }
                                            else if (p == 1)
                                            {
                                                if (h == 0)
                                                    Centroid[1, p] = (buff[h + 1] + buff[h + 4]);
                                                else
                                                    Centroid[1, p] = (Centroid[1, p] + buff[h + 4]);
                                            }
                                            else if (p == 2)
                                            {
                                                if (h == 0)
                                                    Centroid[1, p] = (buff[h + 2] + buff[h + 5]);
                                                else
                                                    Centroid[1, p] = (Centroid[1, p] + buff[h + 5]);
                                            }
                                        }
                                    }
                                    for (int p = 0; p < 3; p++)
                                    {
                                        Centroid[1, p] /= MaxNumTrue;
                                    }
                                }
                                else
                                {
                                    for (int x = 0; x < 3; x++)
                                        for (int y = 0; y < 3; y++)
                                            Centroid[0, y] = Customers[0, y];

                                    for (int x = 0; x < 3; x++)
                                        for (int y = 0; y < 3; y++)
                                            Centroid[1, y] = Customers[1, y];

                                    for (int p = 0; p < 3; p++)
                                    {
                                        for (int h = 0; h < (MaxNumTrue * 3); h += 3)
                                        {
                                            if (p == 0)
                                            {
                                                if (h == 0)
                                                    Centroid[2, p] = (buff[h] + buff[h + 3]);
                                                else
                                                    Centroid[2, p] = (Centroid[2, p] + buff[h + 3]);
                                            }
                                            else if (p == 1)
                                            {
                                                if (h == 0)
                                                    Centroid[2, p] = (buff[h + 1] + buff[h + 4]);
                                                else
                                                    Centroid[2, p] = (Centroid[2, p] + buff[h + 4]);
                                            }
                                            else if (p == 2)
                                            {
                                                if (h == 0)
                                                    Centroid[2, p] = (buff[h + 2] + buff[h + 5]);
                                                else
                                                    Centroid[2, p] = (Centroid[2, p] + buff[h + 5]);
                                            }
                                        }
                                    }
                                    for (int p = 0; p < 3; p++)
                                    {
                                        Centroid[2, p] /= MaxNumTrue;
                                    }
                                }
                            }//end else (!Equal)                            
                        }
                    }
                }
            }
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    Console.Write(" {0} \t", (float)Centroid[x, y]);
                }
                Console.WriteLine("\n");
            }
            Console.WriteLine("\n");
        }

        // return minimum value between three numbers(distance)
        public object min(double d0, double d1, double d2)
        {
            object functionReturnValue = null;
            // Declare and set array element values
            double[] Ardist = new double[] { d0, d1, d2 };
            double m = d0;
            for (int i = 0; i < 3; i++)
            {
                if (Ardist[i] < m)
                    m = Ardist[i];
            }
            functionReturnValue = m;
            return functionReturnValue;
        }        

        // return maximum value between four numbers
        public object max(double d0, double d1, double d2, double d3)
        {
            object functionReturnValue = null;
            // Declare and set array element values
            object[] Ardist = new object[] { d0, d1, d2, d3 };
            object m = d0;
            for (int i = 0; i < 4; i++)
            {
                if ((double)Ardist[i] > (double)m)
                    m = Ardist[i];
            }
            functionReturnValue = m;
            return functionReturnValue;
        }

        //  Write.cs -        
        public void WritingRFM(String args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Must include file name.");
            }
            else
            {
                StreamWriter myFile = new StreamWriter(FILE_NAME);
                Random rnd = new Random();
                int p = 100000;

                // Write data to rfm.data.
                for (int i = 0; i < numCustomers; i++)
                {
                    myFile.WriteLine(rnd.Next(0, 2));
                    myFile.WriteLine(rnd.Next(1, 8));
                    myFile.WriteLine((int)(p * rnd.NextDouble()));
                }
                myFile.Close();
            }
        }

        //  Read.cs - 
        public void ReadingRFM(String args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Must include file name.");
            }
            else
            {
                StreamReader myFile = File.OpenText(FILE_NAME);
                object[,] buffer = new object[numCustomers, 3];

                // Read data 
                for (int x = 0; x < numCustomers; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        buffer[x, y] = myFile.ReadLine();
                    }
                }

                Console.Write("\n");
                Console.WriteLine("Moving data from {0} to Customers array....\n", FILE_NAME);
                for (long i = 0; i <= 10000000.98; i++)
                {
                    // just to introduce some delay 
                }

                Console.WriteLine("Num\t R\t F\t M \n");
                for (int x = 0; x < numCustomers; x++)
                {
                    Console.Write("{0}:\t", x + 1);
                    for (int y = 0; y < 3; y++)
                    {
                        Customers[x, y] = Convert.ToDouble(buffer[x, y]);
                        Console.Write(" {0} \t", Customers[x, y]);
                    }
                    Console.WriteLine("\n");
                }
                Console.WriteLine("....Done Moving.");                
                
                myFile.Close();
            }
        }

        //WritingResult
        public void WritingResult(String args)
        {

            StreamWriter myFile = new StreamWriter(FILE_NAME2);

            totalG1 = 0;
            totalG2 = 0;
            totalG3 = 0;

            // Write result to Result.data. 
            for (int X = 0; X < numCustomers; X++)
            {
                A[X] = X;
                if ((bool)((Groups[X, 0]) == true))
                {
                    totalG1 = totalG1 + (Customers[X, 1] * Customers[X, 2]);
                    myFile.WriteLine(" Customer[{0}] belong Cluster[{1}] ", A[X] + 1, 1);
                    numMemInG1++;
                }
            }
            myFile.WriteLine("\n The number memberships Of Cluster1: {0} \n", (numMemInG1 / 2));
            myFile.WriteLine("\n");

            /////
            for (int X = 0; X < numCustomers; X++)
            {
                A[X] = X;
                if ((bool)((Groups[X, 1]) == true))
                {
                    totalG2 = totalG2 + (Customers[X, 1] * Customers[X, 2]);
                    myFile.WriteLine(" Customer[{0}] belong Cluster[{1}] ", A[X] + 1, 2);
                    numMemInG1++;
                }
            }
            myFile.WriteLine(" The number memberships Of Cluster2: {0} \n", numMemInG2);
            myFile.WriteLine("\n");

            /////
            for (int X = 0; X < numCustomers; X++)
            {
                A[X] = X;
                if ((bool)((Groups[X, 2]) == true))
                {
                    totalG3 = totalG3 + (Customers[X, 1] * Customers[X, 2]);
                    myFile.WriteLine(" Customer[{0}] belong Cluster[{1}] ", A[X] + 1, 3);
                    numMemInG2++;
                }
            }
            myFile.WriteLine(" The number memberships Of Cluster3: {0} \n", numMemInG3);
            myFile.WriteLine("***********************************************************");

            myFile.WriteLine("\ntotalG1: {0}", totalG1);
            myFile.WriteLine("totalG2: {0}", totalG2);
            myFile.WriteLine("totalG3: {0}", totalG3);

            myFile.WriteLine("\n");

            Gold = (double)max(totalG1, totalG2, totalG3, 0);
            Bronze = (double)min(totalG1, totalG2, totalG3);

            if (Gold == totalG1)
                myFile.WriteLine("\nCluster1--> Golden");
            else if (Gold == totalG2)
                myFile.WriteLine("Cluster2--> Golden");
            else if (Gold == totalG3)
                myFile.WriteLine("Cluster3--> Golden");

            if (((int)Bronze < (int)totalG1) && ((int)totalG1 < (int)Gold))
                myFile.WriteLine("Cluster1--> Silver");
            else if (((int)Bronze < (int)totalG2) && ((int)totalG2 < (int)Gold))
                myFile.WriteLine("Cluster2--> Silver");
            else if (((int)Bronze < (int)totalG3) && ((int)totalG3 < (int)Gold))
                myFile.WriteLine("Cluster3--> Silver");

            if (Bronze == totalG1)
                myFile.WriteLine("Cluster1--> Bronze");
            else if (Bronze == totalG2)
                myFile.WriteLine("Cluster2--> Bronze");
            else if (Bronze == totalG3)
                myFile.WriteLine("Cluster3--> Bronze"); 

            myFile.Close();
        }        

        //k-means clustering
        public void KmeansCluster()
        {
            Console.WriteLine("***********************************************************");
            Console.Read();
            //The K means algorithm will do the three steps below until convergence:
            //Iterate until stable (= no object move group):
            // 1. Determine the centroid coordinate
            // 2. Determine the distance of each object to the centroids
            // 3. Group the object based on minimum distance

            short MaxIteration = 0;   //Number Of Iteration

            while (isStillMoving)
            {
                MaxIteration++;
                isStillMoving = true;
                //determine centroids 
                Console.WriteLine("Determine Centroids....\n");
                if (numMemInG1 == 0)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            Centroid[x, y] = Customers[x, y];
                            Console.Write(" {0} \t", Centroid[x, y]);
                        }
                        Console.WriteLine("\n");
                    } Console.WriteLine("....Done determining Centroids.");
                }
                else
                {
                    ReCentroid();
                    Console.WriteLine("....Recomputation Centroids Done.");
                }
                Console.WriteLine("***********************************************************");

                //فاصله داده های مشتریان از ثقل ها                
                for (int t = 0; t < numCustomers; t++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        distance[t, i] = Math.Sqrt(Math.Pow((Customers[t, 0] - Centroid[i, 0]), 2) +
                                                   Math.Pow((Customers[t, 1] - Centroid[i, 1]), 2) +
                                                   Math.Pow((Customers[t, 2] - Centroid[i, 2]), 2));
                    }
                }

                Console.ReadLine();

                Console.WriteLine("Determine the distance of each object to the centroids....\n");
                for (int k = 0; k < numCustomers; k++)
                {
                    for (int L = 0; L < 3; L++)
                    {
                        Console.Write(" {0} \t", (int)distance[k, L]);
                    }
                    Console.WriteLine("\n");
                } Console.WriteLine("....Done Determine the distance.");
                Console.Write("***********************************************************");
                Console.ReadLine();

                //ریست کردن گروه یا خوشه مربوطه                
                for (int k = 0; k < numCustomers; k++)
                {
                    for (int L = 0; L < 3; L++)
                    {
                        Groups[k, L] = Group1[k, L] = Group2[k, L] = false;
                    }
                }

                //Objects clustering or Objects grouping
                //در هر سطر از ماتریس فاصله ها پس از تعیین کمترین آنها
                //مکان معادل آنها در ماتریس کلاسترینگ را با یک(1) ست می کنیم
                //The element of Group matrix below is True(1) if and only if the object is assigned to that group.                 
                for (int r = 0; r < numCustomers; r++)
                {
                    MinArr[r] = min(distance[r, 0], distance[r, 1], distance[r, 2]);
                    for (int Y = 0; Y < 3; Y++)
                    {
                        if ((bool)((double)MinArr[r] == distance[r, Y]))
                        {
                            Groups[r, Y] = true;
                        }
                    }
                }

                Console.WriteLine("Distance minimum:");
                for (int L = 0; L < numCustomers; L++)
                {
                    Console.Write(" {0} \t", MinArr[L]);
                }
                Console.WriteLine("\n***********************************************************");
                Console.ReadLine();

                //نمایش اعضا در هر گروه یا خوشه مربوطه
                Console.WriteLine(" Groups:\n");
                for (int k = 0; k < numCustomers; k++)
                {
                    for (int L = 0; L < 3; L++)
                    {
                        Console.Write(" {0} \t", (bool)Groups[k, L]);
                    }
                    Console.WriteLine("\n");
                }                
                Console.WriteLine("\n***********************************************************");
                Console.ReadLine();

                numMemInG1 = 0;
                numMemInG2 = 0;
                numMemInG3 = 0;
                //Clustering or Grouping                 

                for (int Y = 0; Y < 3; Y++)
                {
                    for (int X = 0; X < numCustomers; X++)
                    {
                        A[X] = X;
                        if ((Groups[X, Y]) == true)
                        {
                            if ((Y == 0))
                            {
                                Console.Write(" Customer[{0}]  belong Cluster[{1}] ", A[X] + 1, Y + 1);
                                G1[X] = (A[X] + 1);
                                G1[X + 1] = 1;
                                Console.Write("\t{0}--> ", G1[X]);
                                Console.WriteLine(G1[X + 1]);
                                numMemInG1++;
                            }
                            else if (Y == 1)
                            {
                                Console.Write(" Customer[{0}]  belong Cluster[{1}] ", A[X] + 1, Y + 1);
                                G2[X] = (A[X] + 1);
                                G2[X + 1] = 2;
                                Console.Write("\t{0}--> ",G2[X]);
                                Console.WriteLine(G2[X + 1]);
                                numMemInG2++;
                            }
                            else if (Y == 2)
                            {
                                Console.Write(" Customer[{0}]  belong Cluster[{1}] ", A[X] + 1, Y + 1);
                                G3[X] = (A[X] + 1);
                                G3[X + 1] = 3;
                                Console.Write("\t{0}--> ", G3[X]);
                                Console.WriteLine(G3[X + 1]);
                                numMemInG3++;
                            }
                        }
                    }
                    Console.WriteLine("\n");
                }                
                Console.WriteLine(" The number memberships Of Cluster1: {0} \t", numMemInG1);

                Console.WriteLine(" The number memberships Of Cluster2: {0} \t", numMemInG2);

                Console.WriteLine(" The number memberships Of Cluster3: {0} \t", numMemInG3);
                Console.WriteLine("***********************************************************");

                if (numCustomers == 3)
                {
                    isStillMoving = false;
                    Console.ReadLine();
                }

                if (MaxIteration < 3)
                {
                    for (int s = 0; s < numCustomers; s++)
                    {
                        for (int e = 0; e < 3; e++)
                        {
                            Group1[s, e] = Groups[s, e];
                        }
                    }
                }
                else if (MaxIteration == 3)
                {
                    for (int s = 0; s < numCustomers; s++)
                    {
                        for (int e = 0; e < 3; e++)
                        {
                            Group2[s, e] = Groups[s, e];
                        }
                    }
                }

                //Kmeans شرط توقف حلقه درالگوریتم 
                int count = 0;
                for (int z = 0; z < numCustomers; z++)
                {
                    for (int f = 0; f < 3; f++)
                    {
                        if (Group1[z, f] == Group2[z, f])
                        {
                            count++;
                        }
                    }
                }

                if (count == numCustomers * 3)
                {
                    isStillMoving = false;
                    Console.WriteLine("count= {0}(numCustomers * 3) or G{1}==G{2}\n", count, (MaxIteration - 1), MaxIteration);
                    for (long g = 0; g <= 100000000; g++)
                    { /* just to introduce some delay */ }

                    Console.WriteLine("Comparing grouping in bottommost rehearsal will illustrated that,");
                    for (long g = 0; g <= 100000000; g++)
                    { /* just to introduce some delay */ }

                    Console.WriteLine("don't move to other 'Cluster' Customers, thus while's loop will stoped!");
                }
                else if ((count < numCustomers * 3) && (numCustomers != 3))
                {
                    Console.WriteLine("\nCount= {0},or G{1} != G{2}, if(count == {3})the while's loop will stoped!\n", count, (MaxIteration - 1), MaxIteration, numCustomers * 3);
                    Console.WriteLine("*******************************");
                }
            }
        }

        //Customers Color
        public void CustColor()
        {
            for (int Y = 0; Y < 3; Y++)
            {
                for (int X = 0; X < numCustomers; X++)
                {
                    if ((Groups[X, Y]) == true)
                    {
                        if ((Y == 0))
                            totalG1 = totalG1 + (Customers[X, 1] * Customers[X, 2]);

                        else if (Y == 1)
                            totalG2 = totalG2 + (Customers[X, 1] * Customers[X, 2]);

                        else if (Y == 2)
                            totalG3 = totalG3 + (Customers[X, 1] * Customers[X, 2]);
                    }
                }
            }            
            Console.WriteLine("\ntotalG1: {0}", totalG1);
            Console.WriteLine("totalG2: {0}", totalG2);
            Console.WriteLine("totalG3: {0}", totalG3);
            double Gold = (double)max(totalG1, totalG2, totalG3, 0);
            double Bronze = (double)min(totalG1, totalG2, totalG3);

            Console.Write("\n");

            //Golden
            if (Gold == totalG1)
                Console.WriteLine("Cluster1--> Golden");

            else if (Gold == totalG2)
                Console.WriteLine("Cluster2--> Golden");

            else if (Gold == totalG3)
                Console.WriteLine("Cluster3--> Golden");

            //Silver
            if (((int)Bronze < (int)totalG1) && ((int)totalG1 < (int)Gold))
                Console.WriteLine("Cluster1--> Silver");

            else if (((int)Bronze < (int)totalG2) && ((int)totalG2 < (int)Gold))
                Console.WriteLine("Cluster2--> Silver");

            else if (((int)Bronze < (int)totalG3) && ((int)totalG3 < (int)Gold))
                Console.WriteLine("Cluster3--> Silver");

            //bronze
            if (Bronze == totalG1)
                Console.WriteLine("Cluster1--> Bronze");

            else if (Bronze  == totalG2)
                Console.WriteLine("Cluster2--> Bronze");

            else if (Bronze == totalG3)
                Console.WriteLine("Cluster3--> Bronze");                         
        }

    }//end of Kmeans class

    public partial class Datamining
    {
        public static int numCustomers = 0;
        public static void Main()
        {
        iterator:
            Console.Write("Enter the number customers: ");
            try
            {
                numCustomers = Convert.ToInt32(Console.ReadLine());
                while ((numCustomers < 3) || (numCustomers <= 0))
                {
                    Console.WriteLine("\nPlease enter a value greater of '3'.");
                    Console.Write("Enter the number customers: ");
                    numCustomers = Convert.ToInt32(Console.ReadLine());
                }
            }
            catch (ArgumentException)
            { Console.WriteLine("\nNo value was entered..."); goto iterator; }
            catch (OutOfMemoryException)
            { Console.WriteLine("\nYou entered a value that is out of memory."); goto iterator; }
            catch (OverflowException)
            { Console.WriteLine("\nYou entered a value that is too big or too small."); goto iterator; }
            catch (FormatException)
            {
                Console.WriteLine("\nYou didn't enter a valid number.");
                goto iterator;
            }
            catch (Exception e)
            {
                Console.WriteLine("\nSomething went wrong with the conversion.");
                throw (e);                
            }

            Kmeans kmeans = new Kmeans();

            kmeans.WritingRFM(Kmeans.FILE_NAME);
            kmeans.ReadingRFM(Kmeans.FILE_NAME);

            kmeans.KmeansCluster();
            kmeans.CustColor();

            kmeans.WritingResult(Kmeans.FILE_NAME2);

            Console.ReadLine();
            Console.WriteLine("\n'Microsoft Billkav' closing....");
            Console.WriteLine("Please press Enter key! ");
            Console.ReadLine();
        }
    }
}
