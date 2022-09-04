using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using OptimizatonMethods.Models;

using WPF_MVVM_Classes;

using ViewModelBase = OptimizatonMethods.Services.ViewModelBase;


namespace OptimizatonMethods.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
    #region Constructors

        public MainWindowViewModel()
        {
            Task = new Task
                {Alpha = 1, Beta = 1, Mu = 1, G = 1, H = 9, N = 10, TempSum = 12, TCoilMin = -2, TCoilMax = 15, TCasingMin = -2, TCasingMax = 12, Price = 100, Step = 0.01,};
        }

    #endregion



    #region Variables

        private IEnumerable<Method> _allMethods;
        private IEnumerable<Task> _allTasks;
        private Task _selectedTask;
        private RelayCommand? _calculateCommand;
        private IEnumerable _dataList;
        private List<Point3D> _point3D = new();

    #endregion


    #region Properties

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

        public IEnumerable<Task> AllTasks
        {
            get
            {
                return _allTasks;
            }
            set
            {
                _allTasks = value;
                OnPropertyChanged();
            }
        }

        public Task Task { get; set; }

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

    #endregion


    #region Command

        public RelayCommand CalculateCommand
        {
            get
            {
                return _calculateCommand ??= new RelayCommand(c =>
                {
                    var calc = new MathModel(Task);
                    calc.Calculate(out var points3D);
                    DataList = points3D;

                    var temp = new List<double>();

                    foreach (var item in points3D)
                    {
                        temp.Add(item.Z);
                    }

                    HandyControl.Controls.MessageBox.Show($"Максимальная прибыль, у.е.: {temp.Max()}\n " +
                                    $"Температура в змеевике Т1, С: {points3D.Find(x => x.Z == temp.Max()).X}\n " +
                                    $"Температура в рубашке Т2, С: {points3D.Find(x => x.Z == temp.Max()).Y}",
                                    "Ответ", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            }
        }

        public RelayCommand TwoDChartCommand
        {
            get
            {
                return new RelayCommand(r =>
                {
                    var test = new Chart2DWindow(DataList as List<Point3D>, Task);
                    test.Show();
                });
            }
        }
        public RelayCommand TaskInfo {
          get {
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
                "Информация о задании",MessageBoxButton.OK);
              });
           }
        }
        public RelayCommand ThreeDChartCommand
        {
            get
            {
                return new RelayCommand(r =>
                {
                    var test = new Chart3DWindow(DataList as List<Point3D>, Task);
                    test.Show();
                });
            }
        }

        public RelayCommand AboutCommand
        {
            get
            {
                return new RelayCommand(r =>
                {
                    HandyControl.Controls.MessageBox.Show("Курсовой проект. Вариант №19, Ветошкина Ульяна, 495 группа.",
                                    "Информация о программе", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            }
        }

        #endregion
    }
}
