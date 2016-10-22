using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            int N = Convert.ToInt32((this.b - this.a) / this.h);
            int ost = N % numThreads;
            int[] numOfIterationsPerThread = new int[numThreads];

            for( int i = 0; i < numThreads; i++)
            {
                numOfIterationsPerThread[i] = Convert.ToInt32(N / numThreads);
                if (i < ost)
                    numOfIterationsPerThread[i]++;
            }

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
