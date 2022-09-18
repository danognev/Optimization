using ChartDirector;
using OptimizatonMethods.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using Task = OptimizatonMethods.ViewModels.MainWindowViewModel;

namespace OptimizatonMethods
{
  public partial class MainWindow
  {
    private List<string> methodList = new List<string>();
    private ContourLayer contourLayer;
    private double m_elevationAngle;
    private bool m_isDragging;
    private int m_lastMouseX;
    private int m_lastMouseY;
    private double m_rotationAngle;
    public MainWindow()
    {
      InitializeComponent();
      methodList.Add("Метод сканирования с переменным шагом");
      methodList.Add("Метод деления отрезка пополам");
      methodList.Add("Метод золотого сечения");
      methodList.Add("Метод Фибоначчи");
      methodBox.ItemsSource = methodList;
      methodBox.SelectedIndex = 0;
      m_elevationAngle = 30;
      m_rotationAngle = 45;
      m_isDragging = false;
      m_lastMouseX = -1;
      m_lastMouseY = -1;
      WPFChartViewer1.updateViewPort(true, false);
    }

    private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
    {
      if (e.PropertyDescriptor is PropertyDescriptor descriptor)
      {
        e.Column.Header = descriptor.DisplayName ?? descriptor.Name;
      }
    }

    private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      const int CHART_2D = 1, CHART_3D = 2;
      var tab = sender as TabControl;
      switch (tab?.SelectedIndex)
      {
        case CHART_2D:
          {
            DrawChart(WPFChartViewer);
            break;
          }
        case CHART_3D:
          {
            CreateChart(WPFChartViewer1, 1);
            break;
          }
      }
    }
    public void CreateChart(WPFChartViewer viewer, int chartIndex)
    {
      var dataX = new List<double>();
      var dataY = new List<double>();
      var step = 1;

      var mathModel = new MathModel();
      for (double i = Task.T1Min - step; i < Task.T1Max + step; i += step)
      {
        dataX.Add(i);
      }
      for (double i = Task.T2Min - step; i < Task.T2Max + step; i += step)
      {
        dataY.Add(i);
      }
      var dataZ = new List<double>();

      for (int i = 0; i < dataX.Count; i++)
      {
        for (int j = 0; j < dataY.Count; j++)
        {
          dataZ.Add(0);
        }
      }
      var k = 0;

      for (int i = 0; i < dataX.Count; i++)
      {
        for (int j = 0; j < dataY.Count; j++)
        {
          dataZ[j * dataX.Count + i] = mathModel.Function(dataX[i], dataY[j]);
        }
      }
      var c = new SurfaceChart(680, 580);
      c.setPlotRegion(310, 280, 320, 320, 240);
      c.setViewAngle(m_elevationAngle, m_rotationAngle);

      if (m_isDragging && DrawFrameOnRotate.IsChecked.Value)
      {
        c.setShadingMode(Chart.RectangularFrame);
      }
      c.setData(dataX.ToArray(), dataY.ToArray(), dataZ.ToArray());
      c.setInterpolation(80, 80);
      var majorGridColor = unchecked((int)0xc0000000);
      var minorGridColor = c.dashLineColor(majorGridColor, Chart.DotLine);
      c.setSurfaceAxisGrid(majorGridColor, majorGridColor, minorGridColor, minorGridColor);
      c.addXYProjection();
      c.setContourColor(0x7fffffff);
      c.setColorAxis(620, 250, Chart.Left, 200, Chart.Right);
      c.xAxis().setTitle("Температура в 1 теплообменнике, С", "Arial Bold Italic", 12);
      c.yAxis().setTitle("Температура во 2 теплообменнике, С", "Arial Bold Italic", 12);
      c.zAxis().setTitle("Максимальные затраты", "Arial Bold", 12);
      viewer.Chart = c;
      viewer.ImageMap = c.getHTMLImageMap("clickable", "",
                                          "title='<*cdml*>X: {x|2}<*br*>Y: {y|2}<*br*>Z: {z|2}'");
    }
    private void DrawChart(WPFChartViewer viewer)
    {
      var dataX = new List<double>();
      var dataY = new List<double>();
      var step = 1;

      var mathModel = new MathModel();
      for (double i = Task.T1Min - step; i < Task.T1Max + step; i += step)
      {
        dataX.Add(i);
      }
      for (double i = Task.T2Min - step; i < Task.T2Max + step; i += step)
      {
        dataY.Add(i);
      }
      var dataZ = new List<double>();

      for (int i = 0; i < dataX.Count; i++)
      {
        for (int j = 0; j < dataY.Count; j++)
        {
          dataZ.Add(0);
        }
      }

      for (int i = 0; i < dataX.Count; i++)
      {
        for (int j = 0; j < dataY.Count; j++)
        {
          dataZ[j * dataX.Count + i] = mathModel.Function(dataX[i], dataY[j]);
        }
      }
      var c = new XYChart(680, 580);
      var p = c.setPlotArea(75, 30, 450, 450, -1, -1, -1, c.dashLineColor(unchecked((int)0xaf000000), Chart.DotLine), -1);
      c.xAxis().setTitle("Температура в 1 теплообменнике, С", "Arial Bold Italic", 10);
      c.yAxis().setTitle("Температура во 2 теплообменнике, С", "Arial Bold Italic", 10);
      c.setYAxisOnRight();
      c.xAxis().setLabelStyle("Arial", 10);
      c.yAxis().setLabelStyle("Arial", 10);
      c.xAxis().setLinearScale(-3, 3, 1);
      c.yAxis().setLinearScale(3, 6, 1);
      contourLayer = c.addContourLayer(dataX.ToArray(), dataY.ToArray(), dataZ.ToArray());
      contourLayer.setContourLabelFormat("<*font=Arial Bold,size=10*>{value}<*/font*>");
      contourLayer.setZBounds(0);
      c.getPlotArea().moveGridBefore(contourLayer);
      var cAxis = contourLayer.setColorAxis(0, p.getTopY(), Chart.TopLeft,
                                            p.getHeight(), Chart.Right);
      cAxis.setColorGradient(true);
      cAxis.setTitle("Чем краснее тем выше затраты", "Arial Bold Italic", 10);
      cAxis.setLabelStyle("Arial", 10);
      viewer.Chart = c;
      viewer.ImageMap = c.getHTMLImageMap("", "",
                                          "title='<*cdml*><*font=Arial Bold*>X={x|2}<*br*>Y={y|2}<*br*>Z={z|2}'");
    }

    private void WPFChartViewer1_ViewPortChanged(object sender, WPFViewPortEventArgs e)
    {
      if (e.NeedUpdateChart)
      {
        CreateChart((WPFChartViewer)sender, 1);
      }
    }

    private void WPFChartViewer1_MouseMoveChart(object sender, MouseEventArgs e)
    {
      var mouseX = WPFChartViewer1.ChartMouseX;
      var mouseY = WPFChartViewer1.ChartMouseY;
      if (Mouse.LeftButton == MouseButtonState.Pressed)
      {
        if (m_isDragging)
        {
          m_rotationAngle += (m_lastMouseX - mouseX) * 90.0 / 360;
          m_elevationAngle += (mouseY - m_lastMouseY) * 90.0 / 270;
          WPFChartViewer1.updateViewPort(true, false);
        }
        m_lastMouseX = mouseX;
        m_lastMouseY = mouseY;
        m_isDragging = true;
      }
    }

    private void WPFChartViewer1_MouseUpChart(object sender, MouseButtonEventArgs e)
    {
      m_isDragging = false;
      WPFChartViewer1.updateViewPort(true, false);
    }
  }
}
