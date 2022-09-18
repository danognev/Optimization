using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Task = OptimizatonMethods.ViewModels.MainWindowViewModel;

namespace OptimizatonMethods.Models
{
  public class MathModel
  {
    private readonly double _epsilon = 0.01;
    private readonly double _k = 10;
    private readonly double _r = 2;
    private double _step;
    public int CalculationCount { get; private set; }

    public double Function(double t1, double t2)
    {
      return Task.Price * Task.Alpha * (Task.G * Task.Mu * ((Math.Pow(t2 - t1, Task.V) + Math.Pow(Task.Beta * Task.A - t1, Task.V))));
    }

    private static bool Conditions(double t1, double t2)
    {
      return t1 >= Task.T1Min && t1 <= Task.T1Max && t2 >= Task.T2Min && t2 <= Task.T2Max && t1 - t2 >= Task.TempSum;
    }

    public void Calculate(out List<Point3D> points3D)
    {
      var funcMin = double.MaxValue;
      _step = Math.Pow(_k, _r) * _epsilon;
      points3D = new List<Point3D>();
      var p3D = new List<Point3D>();
      List<double> values;
      Point newMin;

      newMin = SearchMinOnGrid(out p3D, out values);
      Task.T1Min = newMin.X - _step;
      Task.T2Min = newMin.Y - _step;

      Task.T1Max = newMin.X + _step;
      Task.T2Max = newMin.Y + _step;

      _step /= _k;
      points3D.AddRange(p3D);

      while (funcMin < values.Min())
      {
        newMin = SearchMinOnGrid(out p3D, out values);

        Task.T1Min = newMin.X - _step;
        Task.T2Min = newMin.Y - _step;

        Task.T1Max = newMin.X + _step;
        Task.T2Max = newMin.Y + _step;

        _step /= _k;
        funcMin = values.Min();
        points3D.AddRange(p3D);
      }
    }

    private Point SearchMinOnGrid(out List<Point3D> points3D, out List<double> values)
    {
      points3D = new List<Point3D>();

      for (var t1 = Task.T1Min; t1 <= Task.T1Max; t1 += _step)
      {
        for (var t2 = Task.T2Min; t2 <= Task.T2Max; t2 += _step)
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

      return new Point(points3D.Find(x => x.Z == valuesListTemp.Min()).X, points3D.Find(x => x.Z == valuesListTemp.Min()).Y);
    }
  }
}
