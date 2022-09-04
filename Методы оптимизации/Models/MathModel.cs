using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;


namespace OptimizatonMethods.Models
{
    public class MathModel
    {
        private readonly Task _task;
        private readonly double _epsilon = 0.01;
        private readonly double _k = 10;
        private double _n = 2;
        private readonly double _r = 2;
        private double _step;
        private readonly double alpha;
        private readonly double beta;
        private readonly double tempSum;
        private readonly double H;
        public double tCoilMax;
        public double tCoilMin;
        private readonly double mu;
        private readonly double N;
        private readonly double G;
        private readonly double price;
        public double tCasingMax;
        public double tCasingMin;

        public MathModel(Task task)
        {
            _task = task;
            price = (double) _task.Price;
            alpha = (double) _task.Alpha;
            beta = (double) _task.Beta;
            mu = (double) _task.Mu;
            G = (double)_task.G;
            N = (double) _task.N;
            tCoilMin = (double) _task.TCoilMin;
            tCoilMax = (double) _task.TCoilMax;
            tCasingMin = (double) _task.TCasingMin;
            tCasingMax = (double) _task.TCasingMax;
            tempSum = (double) _task.TempSum;
            H = (double) _task.H;
        }

        public int CalculationCount { get; private set; }

        public double Function(double t1, double t2)
        {
            return price * alpha * G * (Math.Pow(t1 - t2, 2) + beta * (1 / H) * Math.Pow(t1 + t2 - mu * N, 2));
        }

        private bool Conditions(double t1, double t2)
        {
            return t1 >= tCoilMin && t1 <= tCoilMax && t2 >= tCasingMin && t2 <= tCasingMax && t1 + t2 <= tempSum;
        }

        public void Calculate(out List<Point3D> points3D)
        {
            var funcMax = double.MinValue;
            _step = Math.Pow(_k, _r) * _epsilon;
            points3D = new List<Point3D>();
            var p3D = new List<Point3D>();
            List<double> values;
            Point newMax;

            newMax = SearchMaxOnGrid(out p3D, out values);
            tCoilMin = newMax.X - _step;
            tCasingMin = newMax.Y - _step;

            tCoilMax = newMax.X + _step;
            tCasingMax = newMax.Y + _step;

            _step /= _k;
            points3D.AddRange(p3D);

            while (funcMax > values.Max())
            {
                newMax = SearchMaxOnGrid(out p3D, out values);

                tCoilMin = newMax.X - _step;
                tCasingMin = newMax.Y - _step;

                tCoilMax = newMax.X + _step;
                tCasingMax = newMax.Y + _step;

                _step /= _k;
                funcMax = values.Max();
                points3D.AddRange(p3D);
            }
        }

        private Point SearchMaxOnGrid(out List<Point3D> points3D, out List<double> values)
        {
            points3D = new List<Point3D>();

            for (var t1 = tCoilMin; t1 <= tCoilMax; t1 += _step)
            {
                for (var t2 = tCasingMin; t2 <= tCasingMax; t2 += _step)
                {
                    if (!Conditions(t1, t2))
                    {
                        continue;
                    }

                    CalculationCount++;
                    var value = Function(t1, t2);

                    if (value < 0)
                    {
                        MessageBox.Show($"t1 {t1} t2 {t2} Z {value}");
                    }

                    points3D.Add(new Point3D(Math.Round(t1, 2), Math.Round(t2, 2), Math.Round(value, 2)));
                }
            }

            var valuesListTemp = points3D.Select(item => item.Z).ToList();
            values = valuesListTemp;

            return new Point(points3D.Find(x => x.Z == valuesListTemp.Max()).X, points3D.Find(x => x.Z == valuesListTemp.Max()).Y);
        }
    }
}
