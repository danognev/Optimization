using System.ComponentModel;


namespace OptimizatonMethods.Models
{
  public class Point3D
  {
    public Point3D(double x, double y, double z)
    {
      X = x;
      Y = y;
      Z = z;
    }

    [DisplayName("Температура в 1 теплообменнике")]
    public double X { get; set; }

    [DisplayName("Температура во 2 теплообменнике")]
    public double Y { get; set; }

    [DisplayName("Затраты на очистку")]
    public double Z { get; set; }
  }
}
