using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;


namespace OptimizatonMethods
{
    public partial class MainWindow
    {
        private List<string> methodList = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            methodList.Add("Метод сканирования с переменным шагом");
            methodList.Add("Метод деления отрезка пополам");
            methodList.Add("Метод золотого сечения");
            methodList.Add("Метод Фибоначчи");
            methodBox.ItemsSource = methodList;
            methodBox.SelectedIndex = 0;
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyDescriptor is PropertyDescriptor descriptor)
            {
                e.Column.Header = descriptor.DisplayName ?? descriptor.Name;
            }
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // FontSize = (ActualHeight + ActualHeight / ActualWidth * ActualWidth) / 75;
        }
    }
}
