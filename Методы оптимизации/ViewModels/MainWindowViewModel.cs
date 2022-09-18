using OptimizatonMethods.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WPF_MVVM_Classes;
using ViewModelBase = OptimizatonMethods.Services.ViewModelBase;


namespace OptimizatonMethods.ViewModels
{
  public class MainWindowViewModel : ViewModelBase
  {
    public static double Alpha { get; set; }
    public static double Beta { get; set; }
    public static double Mu { get; set; }
    public static double G { get; set; }
    public static double A { get; set; }
    public static double V { get; set; }
    public static double T1Min { get; set; }
    public static double T1Max { get; set; }
    public static double T2Min { get; set; }
    public static double T2Max { get; set; }
    public static double Price { get; set; }
    public static double Step { get; set; }
    public static double TempSum { get; set; }
    public MainWindowViewModel()
    {
      SetDefaultValues();
    }
    private IEnumerable<Method> _allMethods;
    private RelayCommand? _calculateCommand;
    private IEnumerable _dataList;
    public IEnumerable<Method> AllMethods
    {
      get
      {
        return _allMethods;
      }
      set
      {
        _allMethods = value;
        OnPropertyChanged();
      }
    }

    public IEnumerable DataList
    {
      get
      {
        return _dataList;
      }
      set
      {
        _dataList = value;
        OnPropertyChanged();
      }
    }
    public RelayCommand CalculateCommand
    {
      get
      {
        SetDefaultValues();
        return _calculateCommand ??= new RelayCommand(c =>
        {
          var calc = new MathModel();
          calc.Calculate(out var points3D);
          DataList = points3D;

          var temp = new List<double>();

          foreach (var item in points3D)
          {
            temp.Add(item.Z);
          }

          HandyControl.Controls.MessageBox.Show($"Минимальные затраты, у.е.: {temp.Max()}\n " +
                                  $"Температура Т1, С: {points3D.Find(x => x.Z == temp.Max()).X}\n " +
                                  $"Температура Т2, С: {points3D.Find(x => x.Z == temp.Max()).Y}",
                                  "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
        });
      }
    }
    public static RelayCommand TaskInfo
    {
      get
      {
        return new RelayCommand(r =>
        {
          HandyControl.Controls.MessageBox.Show("" +
              "Объектом оптимизации является химический реактор, в котором помимо целевого компонента  образуется побочный (вредный) продукт." +
              "\nВ силу этого необходимы существенные затраты на очистку реакционной массы.  Известно, что   количество  побочного  продукта    С (кг)  связано  с условиями проведения процесса  следующим выражением:" +
              "\r\n                C = α*(G*  µ *( (T2- T1)^V +    (β *A-T1) ^V )),    \r\n" +
              "где    G –  количество  реакционной массы в реакторе (2 т);\r\n" +
              "       V -  рабочий объем реактора, равный 2 м^3;\r\n" +
              "      T1 и T2 – температуры хладагента в двух теплообменных устройствах,   °C;\r\n" +
              "       α, µ, β -  нормирующие множители, равные 1;\r\n" +
              "       А – давление в реакторе (1 КПа).\r\n" +
              "   Для нормального протекания технологического процесса необходимо, чтобы температура в первом теплообменнике   была  не меньше -3°C и не  больше 3°C" +
              ",\n а во втором- не   меньше -3°C и не превосходила 6°C. Кроме того,   регламентом  установлено, что  обязательно должно выполняться условие  Т2-Т1   1°C. \r\n" +
              "  Необходимо определить такие значения температуры хладагента в теплообменных устройствах реактора ( Т1 и Т2 ), при которых будут минимальны затраты на очистку реакционной массы от вредного продукта." +
              " \nИзвестно, что затраты на очистку реакционной массы от 1 кг побочного продукта составляют 100 у.е. Точность решения – 0,01°C \r\n",
              "Информация о задании", MessageBoxButton.OK);
        });
      }
    }
    public static RelayCommand AboutCommand
    {
      get
      {
        return new RelayCommand(r =>
        {
          HandyControl.Controls.MessageBox.Show("Курсовой проект. Вариант №13, выполнил Огнев Даниил, 494 группа.",
                                  "Информация о программе", MessageBoxButton.OK, MessageBoxImage.Information);
        });
      }
    }
    private void SetDefaultValues()
    {
      Alpha = 1;
      Beta = 1;
      Mu = 1;
      G = 2;
      A = 1;
      V = 2;
      TempSum = 1;
      T1Min = -3;
      T1Max = 3;
      T2Min = -3;
      T2Max = 6;
      Price = 100;
      Step = 0.01;
    }
  }
}
