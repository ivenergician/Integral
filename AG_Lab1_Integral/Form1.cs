using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace AG_Lab1_Integral
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double start = 0.0;
            double end = 0.0;
            double h = 0.0;
            int numThreads = 0;
            try
            {
                GetParams(out start, out end, out h, out numThreads);
            }
            catch
            {
                textBox4.Text = "Указаны неверные параметры";
                return;
            }

            Integral I = new Integral(start, end, h);

            Stopwatch t1 = new Stopwatch();
            t1.Start();
            double rez = I.CalculatePosled();
            t1.Stop();
            textBox4.Text = "Последовательный:" + Environment.NewLine;
            textBox4.Text += ComposeStringResult(t1.Elapsed.Milliseconds, rez);
            t1.Reset();
            t1.Start();
            rez = I.CalculateParallel(numThreads);
            t1.Stop();
            textBox4.Text += "Параллельный с использованем Tasks:" + Environment.NewLine;
            textBox4.Text += ComposeStringResult(t1.Elapsed.Milliseconds, rez);
            t1.Reset();
            t1.Start();
            rez = I.CalculateWithParallelFor();
            t1.Stop();
            textBox4.Text += "Параллельный с использованем ParallelFor:" + Environment.NewLine;
            textBox4.Text += ComposeStringResult(t1.Elapsed.Milliseconds, rez);
        }

        static string ComposeStringResult(int time, double resault)
        {
            string s = "";
            return s = "Time: " + time.ToString() + " millisec, Resault: " + resault.ToString() + Environment.NewLine;
        }

        private void btnDrawGraphs_Click(object sender, EventArgs e)
        {
            DrawGraphs();
        }

        private void DrawGraphs()
        {
            ChartTimeH();
            ChartTimeNumTherads();
            ChartTimeParallelVSPosled();
            ChartTimeTreeSeries();
            ChartTimeVSh();
        }

        private void ChartTimeNumTherads()
        {
            ch2.Series[0].Points.Clear();

            double start = 0.0;
            double end = 0.0;
            double h = 0.0;
            int numThreads = 0;

            try
            {
                GetParams(out start, out end, out h);
            }
            catch
            {
                textBox4.Text = "Указаны неверные параметры";
                return;
            }

            for (numThreads = 1; numThreads <= 10; numThreads++)
            {
                Integral I = new Integral(start, end, h);

                Stopwatch t1 = new Stopwatch();
                t1.Start();
                double rez = I.CalculateParallel(numThreads);
                t1.Stop();
                ch2.Series[0].Points.AddXY(numThreads, t1.Elapsed.Milliseconds);
            }
        }

        private void ChartTimeH()
        {
            ch1.Series[0].Points.Clear();
            ch1.Series[1].Points.Clear();

            double start = 0.0;
            double end = 0.0;
            double h = 0.0;
            int numThreads = 0;

            try
            {
                GetParams(out start, out end, out numThreads);
            }
            catch
            {
                textBox4.Text = "Указаны неверные параметры";
                return;
            }

            for (h = 0.1; h > 0.00001; h *= 0.1)
            {
                Integral I = new Integral(start, end, h);

                Stopwatch t1 = new Stopwatch();
                t1.Start();
                double rez = I.CalculatePosled();
                t1.Stop();
                ch1.Series[0].Points.AddXY(Convert.ToInt32((end - start) / h), t1.Elapsed.Milliseconds);
                t1.Reset();
                t1.Start();
                rez = I.CalculateParallel(numThreads);
                t1.Stop();
                ch1.Series[1].Points.AddXY(Convert.ToInt32((end - start) / h), t1.Elapsed.Milliseconds);
            }
        }
      
        private void ChartTimeParallelVSPosled()
        {
            ch3.Series[0].Points.Clear();
            ch3.Series[1].Points.Clear();

            double start = 0.0;
            double end = 0.0;
            double h = 0.0;
            int numThreads = 0;
            try
            {
                GetParams(out start, out end, out h, out numThreads);
            }
            catch
            {
                textBox4.Text = "Указаны неверные параметры";
                return;
            }

            Integral I = new Integral(start, end, h);

            Stopwatch t1 = new Stopwatch();
            t1.Start();
            double rez = I.CalculatePosled();
            t1.Stop();
            ch3.Series[0].Points.AddXY(1, t1.Elapsed.Milliseconds);
            t1.Reset();
            t1.Start();
            rez = I.CalculateParallel(numThreads);
            t1.Stop();
            ch3.Series[1].Points.AddXY(numThreads, t1.Elapsed.Milliseconds);
        }

        private void ChartTimeTreeSeries()
        {
            ch4.Series[0].Points.Clear();
            ch4.Series[1].Points.Clear();
            ch4.Series[2].Points.Clear();

            double start = 0.0;
            double end = 0.0;
            double h = 0.0;
            int numThreads = 0;
            try
            {
                GetParams(out start, out end, out h, out numThreads);
            }
            catch
            {
                textBox4.Text = "Указаны неверные параметры";
                return;
            }

            Integral I = new Integral(start, end, h);

            Stopwatch t1 = new Stopwatch();
            t1.Start();
            double rez = I.CalculatePosled();
            t1.Stop();
            ch4.Series[0].Points.AddXY(1, t1.Elapsed.Milliseconds);
            t1.Reset();
            t1.Start();
            rez = I.CalculateParallel(numThreads);
            t1.Stop();
            ch4.Series[1].Points.AddXY(numThreads, t1.Elapsed.Milliseconds);
            t1.Reset();
            t1.Start();
            rez = I.CalculateWithParallelFor();
            t1.Stop();
            ch4.Series[2].Points.AddXY(Environment.ProcessorCount, t1.Elapsed.Milliseconds);
            
        }

        private void ChartTimeVSh()
        {
            ch5.Series[0].Points.Clear();
            ch5.Series[1].Points.Clear();

            double start = 0.0;
            double end = 0.0;
            double h = 0.0;
            int numThreads = 0;

            try
            {
                GetParams(out start, out end, out numThreads);
            }
            catch
            {
                textBox4.Text = "Указаны неверные параметры";
                return;
            }

            for (h = 0.01; h > 0.00001; h *= 0.1)
            {
                Integral I = new Integral(start, end, h);

                Stopwatch t1 = new Stopwatch();
                t1.Start();
                double rez = I.CalculatePosled();
                t1.Stop();
                ch5.Series[0].Points.AddXY(h, t1.Elapsed.Milliseconds);
                t1.Reset();
                t1.Start();
                rez = I.CalculateParallel(numThreads);
                t1.Stop();
                ch5.Series[1].Points.AddXY(h, t1.Elapsed.Milliseconds);
            }
        }

        private void GetParams(out double start, out double end, out double h, out int numThreads)
        {
            start = Convert.ToDouble(textBox1.Text);
            end = Convert.ToDouble(textBox2.Text);
            h = Convert.ToDouble(textBox3.Text);
            numThreads = Convert.ToInt32(textBox5.Text);
        }

        private void GetParams(out double start, out double end, out int numThreads)
        {
            start = Convert.ToDouble(textBox1.Text);
            end = Convert.ToDouble(textBox2.Text);
            numThreads = Convert.ToInt32(textBox5.Text);
        }

        private void GetParams(out double start, out double end, out double h)
        {
            start = Convert.ToDouble(textBox1.Text);
            end = Convert.ToDouble(textBox2.Text);
            h = Convert.ToDouble(textBox3.Text);
        }
    }
}