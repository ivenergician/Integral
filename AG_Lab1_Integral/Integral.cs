using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace AG_Lab1_Integral
{
    class Integral
    {
        double a;
        double b;
        double h;

        public Integral(double a, double b, double h)
        {
            this.a = a;
            this.b = b;
            this.h = h;
        }

        public double CalculatePosled()
        {
            return CalculateIntegral(this.a, this.b);
        }

        public double CalculateWithParallelFor()
        {
            double rez = 0.0;
            int N = Convert.ToInt32((this.b - this.a) / this.h);

            object obj = new object();

            Parallel.For(0, N, () => 0.0, (i, s, tmp) =>
            {
                tmp += (Function(this.a + h * i) + Function(this.a + h * (i + 1))) / 2.0;
                return tmp;
            }, tmp => { lock (obj) rez += tmp; });
            return rez * h;
        }

        public double CalculateParallel(int numThreads)
        {
            double rez = 0.0;
            int[] numOfIterationsPerThread = new int[numThreads];

            CalculateDotsCount(numThreads, ref numOfIterationsPerThread);

            Task<double>[] tasks = new Task<double>[numThreads];
            for (int i = 0; i < numThreads; i++)
            {
                double start = this.a + rez;
                double end = start + numOfIterationsPerThread[i] * h;
                tasks[i] = Task.Factory.StartNew<double>(() => CalculateIntegral(start, end));
                rez += numOfIterationsPerThread[i] * h;
            }
            Task.WaitAll(tasks);
            rez = 0.0;
            for (int i = 0; i < numThreads; i++)
            {
                rez += tasks[i].Result;
            }

            return rez;
        }


        public double CalculateWithThreads(int numThreads)
        {
            double rez = 0.0;
            int[] numOfIterationsPerThread = new int[numThreads];
            double[] results = new double[numThreads];

            CalculateDotsCount(numThreads, ref numOfIterationsPerThread);

            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < numThreads; i++)
            {
                int id = i;
                double start = this.a + rez;
                double end = start + numOfIterationsPerThread[id] * h;
                Thread thread = new Thread(() => { results[id] = CalculateIntegral(start, end); });
                rez += numOfIterationsPerThread[id] * h;
                thread.Start();
                threads.Add(thread);
            }

            foreach (var thread in threads)
                thread.Join();

            return results.Sum();
        }

        private void CalculateDotsCount(int numThreads, ref int[] numOfIterationsPerThread)
        {
            int n = Convert.ToInt32((this.b - this.a) / this.h);
            int ost = n % numThreads;

            for (int i = 0; i < numThreads; i++)
            {
                numOfIterationsPerThread[i] = Convert.ToInt32(n / numThreads);
                if (i < ost)
                    numOfIterationsPerThread[i]++;
            }
        }

        private double CalculateIntegral(double start, double end)
        {
            double rez = 0.0;
            int N = Convert.ToInt32((end - start) / this.h);
            double x = start;
            for (int i = 0; i < N; i++)
            {
                rez += (Function(x) + Function(x + h)) / 2.0;
                x += h;
            }
            return rez * h;
        }

        private double Function(double x)
        {
            return 2.0 * x - Math.Log10(7 * x) - 12;
        }
    }
}
