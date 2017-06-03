using System;
using System.Collections.Generic;
using System.IO;

namespace RegressionConsApp
{
    class Program
    {
		//path should be a text or csv format , wich have 2 columns, every row is seperated by ,(comma)
        static void Main(string[] args)
        {
            string path = Console.ReadLine();
            run( path);
        }

        
        static void run(string path)
        {
            List<double[]> points = new List<double[]>();
            var fs = File.OpenRead(path);
            var reader = new StreamReader(fs);
            while (!reader.EndOfStream)
            {
                var values = reader.ReadLine().Split(',');
                double tmp1 = Convert.ToDouble(values[0]);
                double tmp2 = Convert.ToDouble(values[1]);
                points.Add(new double[] { tmp1, tmp2 });
            }
            double learning_rate = 0.0001;
            double initial_b = 0;
            double initial_m = 0;
            int num_iterations = 1000;
            double b;
            double m;
            (b, m) = gradient_decent_runner(points.ToArray(), initial_b, initial_m, learning_rate, num_iterations);
            Console.WriteLine("After {0} iterations b = {1}, m = {2}, error = {3}", num_iterations, b, m, compute_error(b, m, points.ToArray()));
        }

        static double compute_error(double b, double m, double[][] points)
        {
            double total_error = 0;
            for (int i = 0; i < points.Length; i++)
            {
                double x = points[i][0];
                double y = points[i][1];
                total_error +=  (y - (m * x + b))* (y - (m * x + b));
            }
            return total_error/points.Length;
        }

        static (double, double) gradient_decent_runner(double[][] points, double starting_b, double starting_m, double learning_rate, int num_iterations)
        {
            double b = starting_b;
            double m = starting_m;
            for (int i = 0; i < num_iterations; i++)
                (b, m) = step_gradient(b, m, points, learning_rate);
            return (b, m);
        }

        static (double, double) step_gradient(double b_current, double m_current, double[][] points, double learning_rate)
        {
            double b_gradient = 0;
            double m_gradient = 0;
            double N = points.Length;
            for (int i = 0; i < points.Length; i++)
            {
                double x = points[i][0];
                double y = points[i][1];
                b_gradient += -(2 / N) * (y - ((m_current * x) + b_current));
                m_gradient += -(2 / N) * x * (y - ((m_current * x) + b_current));
            }
            double new_b = b_current - (learning_rate * b_gradient);
            double new_m = m_current - (learning_rate * m_gradient);
            return (new_b, new_m);
        }
    }
}