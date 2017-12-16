using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger
{
    public class Methods
    {
        public static int GenerateAvailablePrime(List<int> primes)
        {
            if (primes.Count == 0) { return 1; }
            primes.Sort();
            
            return GenerateNextPrime(primes[primes.Count - 1]);
        }


        public static bool IsPrime(int n)
        {
            if (n < 2) return false;
            if (n == 2) return true;
            if (n % 2 == 0) return false;
            for (int a = 3; a <= Math.Sqrt(n); a += 2)
            {
                if (n % a == 0) return false;
            }
            return true;
        }


        public static int GreatestCommonDivisor(params int[] Numbers)
        {
            for (int a = 0; a < Numbers.Length; a++)
                if (Numbers[a] == 1) return 1;
            int min = Math.Min(Numbers[0], Numbers[1]);
            int max = Numbers[0] + Numbers[1] - min;
            int swapper = 0;

            if (Numbers.Length == 2)
            {
                while (min != 0)
                {

                    swapper = max % min;
                    max = min;
                    min = swapper;
                }
                return max;
            }

            for (int a = 1; a < Numbers.Length; a++)
            {
                while (min != 0)
                {

                    swapper = max % min;
                    max = min;
                    min = swapper;
                }
                if (a == Numbers.Length - 1)
                {
                    return max;
                }
                // now min = 0   and   max = GCD
                swapper = Math.Max(max, Numbers[a + 1]);
                min = Math.Min(max, Numbers[a + 1]);
                max = swapper;
            }

            return max;
        }

        public static int SmallestMultipleOf(int a, int b = 0)
        {
            if (b == 0)
            { return -1; }

            return (a / GreatestCommonDivisor(a, b)) * b;
        }

        public static int SmallestMultipleOf(params int[] Numbers)
        {
            int SMO = SmallestMultipleOf(Numbers[0], Numbers[1]);
            for (int a = 3; a < Numbers.Length; a++)
            {
                SMO = SmallestMultipleOf(SMO, Numbers[a]);
            }
            return SMO;
        }


        public static List<int> GetPrimeFactors(int n) // When there is one factor of each
        {
            int m = n;
            List<int> output = new List<int>();
            if (m % 2 == 0)
            {
                output.Add(2);
                m /= 2;
            }

            for (int a = 3; m != 1; a += 2)
            {
                if (m % a == 0)
                {
                    output.Add(a);
                    m /= a;
                }
            }
            return output;
        }



        public static int GenerateNextPrime(int x)
        {
            bool isPrime;
            //int y = x+1;
            for (int y = x + 1; true; y++)
            {
                isPrime = true;
                for (int a = 2; a <= Math.Sqrt(y); a++)
                {
                    if (y % a == 0) { isPrime = false; break; }
                }
                if (isPrime) { return y; }
            }
            //return 0;
        }

        public static double[] MergeSorted(double[] arr1, double[] arr2)
        {
            int n = arr1.Length + arr2.Length;
            double[] merged = new double[n];
            int k1 = 0; int k2 = 0;
            for (int a = 0; a < n; a++)
            {
                if (arr1[k1] > arr2[k2])
                {
                    merged[a] = arr1[k1];
                    k1++;
                }
                else
                {
                    merged[a] = arr2[k2];
                    k2++;
                }

                if (k1 == arr1.Length)
                {

                    while (k1 + k2 < n)
                    {
                        merged[k1 + k2] = arr2[k2];
                        k2++;
                    }
                    break;
                }

                else if (k2 == arr2.Length)
                {

                    while (k1 + k2 < n)
                    {
                        merged[k1 + k2] = arr1[k1];
                        k1++;
                    }
                    break;
                }


            }
            return merged;
        }

        public static double[] SortByMerge(double[] arr)
        {

            if (arr.Length == 1)
            { return arr; }

            double[] result = new double[arr.Length];

            double[] L = new double[arr.Length / 2];
            double[] R = new double[arr.Length - L.Length];
            Array.Copy(arr, 0, L, 0, L.Length);
            Array.Copy(arr, arr.Length / 2, R, 0, R.Length);

            L = SortByMerge(L);
            R = SortByMerge(R);
            result = MergeSorted(L, R);
            return result;
        }

        public static int[] SortIntByCount(int[] arr)
        {
            int[] output = new int[arr.Length];
            int min = arr[0];
            int max = arr[0];
            for (int a = 1; a < arr.Length; a++)
            {
                if (arr[a] > max) { max = arr[a]; }
                else if (arr[a] < min) { min = arr[a]; }
            }

            int[] CountArray = new int[max - min + 1];
            for (int a = 0; a < arr.Length; a++)
            {
                CountArray[arr[a] - min]++;
            }
            for (int a = CountArray.Length - 1, c = 0; a > -1; a--)
            {
                for (int b = 0; b < CountArray[a]; b++)
                {
                    output[c] = a + min;
                    c++;
                }
            }
            return output;
        }

        public static double AngleRespectToX(double x1, double y1, double x2, double y2)
        {
            double Angle = 0;
            Angle = Math.Acos((x2 - x1) / Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)));
            if (y2 < y1) Angle = 2 * Math.PI - Angle;
            return Angle;
        }

        static public string StringReverse(string str)
        {
            int l = str.Length;
            char[] charArray = new char[l];


            for (int a = 0; a < l / 2; a++)
            {

                charArray[a] = str[l - 1 - a];
                charArray[l - 1 - a] = str[a];
            }
            if (l % 2 == 1) { charArray[l / 2] = str[l / 2]; }
            string output = new string(charArray);
            return output;
        }   //Works

        static public string cutNullOut(string str)
        {
            for (int a = 0; a < str.Length; a++)
            {
                if (str[a] == '\0')
                {
                    str = str.Remove(a, str.Length - a);
                    return str;
                }
            }
            return str;
        }           //Works

        //Solve equations matrix n x (n+1) input      returns single array of solutions
        //x+2y=2    1 2 2
        //2x+y=4    2 1 4
        public static double[] SolveEquationSet(double[,] arr)
        {
            //n   = number of Lines
            //n+1 = number of items in the line
            int n = Convert.ToInt32(Math.Sqrt(4 * arr.Length + 1) - 1) / 2;
            double[,] solved = new double[n, n + 1];
            double[,] back = new double[n, n + 1];
            solved = arr;
            double[] solution = new double[n];
            double k;
            // Console.WriteLine(n);



            for (int a = n - 2; a >= 0; a--)
            {
                for (int b = 0; b < a + 2; b++)
                {
                    double o = solved[b, a + 1];
                    for (int c = 0; c < n + 1; c++)
                    {
                        solved[b, c] /= o;
                        //   Console.WriteLine(solved[b, c]);
                    }

                }




                for (int i = 0; i < n + 1; i++)
                    back[n - a - 2, i] = solved[0, i];


                for (int b = 0; b < a + 1; b++)
                {
                    for (int c = 0; c < n + 1; c++)
                    {
                        solved[b, c] = back[n - a - 2, c] - solved[b + 1, c];
                        //      Console.WriteLine(solved[b, c]);
                    }

                }



                //Last Line of main "a" cycle

            }



            k = solved[0, n] / (solved[0, 0]);
            solution[0] = k;

            for (int a = 1; a < n; a++)
            {
                double mult = 0;
                for (int b = 0; b < a; b++)
                {
                    mult += back[n - a - 1, b] * solution[b];
                    //    Console.WriteLine(mult);
                }
                solution[a] = back[n - a - 1, n] - mult;

            }


            return solution;

        }

        // Find polinomial magnitudes of using points (2 arrays)  returns single array of magnitudes
        public static double[] FindPol(double[] x, double[] y)
        {
            int n = x.Length;
            double[,] equation = new double[n, n + 1];


            for (int a = 0; a < n; a++)
            {
                for (int b = n - 1; b >= 0; b--)
                {
                    equation[a, n - 1 - b] = Math.Pow(x[a], b);
                }
                equation[a, n] = y[a];
            }


            return SolveEquationSet(equation);
        }


        public static double[] SortDoubleIncrease(double[] Numbers, bool increase = true)
        {
            double[] Values = new double[Numbers.Length];
            Array.Copy(Numbers, Values, Numbers.Length);
            for (int a = 0; a < Numbers.Length; a++)
            {

                for (int b = a + 1; b < Numbers.Length; b++)
                {
                    if (Values[b] < Values[a])
                    {
                        double k = Values[a];
                        Values[a] = Values[b];
                        Values[b] = k;
                    }
                }

            }

            if (increase) return Values;
            else
            {
                for (int a = 0; a < Values.Length / 2; a++)
                {
                    double k = Values[a];
                    Values[a] = Values[Values.Length - a - 1];
                    Values[Values.Length - a - 1] = k;
                }
                return Values;
            }
        }


        public static int[] SortDoubleIndexesIncrease(double[] Numbers, bool increase = true)
        {
            int[] output = new int[Numbers.Length];
            double[] Sorted = new double[Numbers.Length];
            Array.Copy(Numbers, Sorted, Numbers.Length);
            Sorted = SortDoubleIncrease(Numbers);

            for (int a = 0; a < Numbers.Length; a++)
            {
                for (int b = 0; b < Numbers.Length; b++)
                {
                    if (Sorted[a] == Numbers[b] && !IntBelongsToArray(b, output, a))
                    {

                        //Console.WriteLine(b);
                        output[a] = b;
                        break;
                    }
                }
            }

            return output;
        }


        public static bool IntBelongsToArray(int n, int[] ArrayN, int max, int min = 0)
        {

            for (int a = min; a < max; a++)
            {
                if (n == ArrayN[a]) return true;

            }
            return false;
        }



        public static int SearchListForIndex(List<double> list, int length, double x)
        {
            int low = 0;
            int high = length - 1;
            int m = new int();
            if (x >= list[high]) return length;
            else if (x <= list[0]) return 0;
            do
            {
                m = (low + high) / 2;
                if (x > list[m])
                {
                    low = m + 0;
                }
                else
                {
                    high = m + 0;
                }
            }
            while (high - low != 1);
            return high;
        }

        public static double[] InsertionSort(double[] Numbers)

        {
            double[] output = new double[Numbers.Length];
            output[0] = Numbers[0];
            List<double> list = new List<double>();
            if (Numbers[0] < Numbers[1])
            {
                list.Add(Numbers[0]);
                list.Add(Numbers[1]);
            }
            else
            {
                list.Add(Numbers[1]);
                list.Add(Numbers[0]);
            }
            output[0] = list[0];
            output[1] = list[1];
            for (int a = 2; a < Numbers.Length; a++)
            {
                list.Insert(SearchListForIndex(list, a, Numbers[a]), Numbers[a]);
                output[a] = list[a];
            }

            return output;
        }


    }
}
