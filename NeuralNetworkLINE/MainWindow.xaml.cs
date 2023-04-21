using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NeuralNetworkLINE
{
    public partial class MainWindow : Window
    {
        RectGrid RG;
        NeuralNetwork NW;
        public MainWindow()
        {
            InitializeComponent();
            RG = new RectGrid(BaseCanvas, 4);
        }
        public void CanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (e.ChangedButton == MouseButton.Left) BaseLabel.Content = C1.test();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            float[] bufInput = RG.GetGridStateFloat(), bufOutput = new float[2] { 0, 0 }, result;
            NW = new NeuralNetwork(bufInput, bufOutput);
            result = NW.DoIt();

            string buf = "";
            foreach (float f in result) buf += f + "";
            MessageBox.Show(buf);

            NW.FindError(new float[2] { 0.8f, 0.2f });
        }

        private void ShowNetworkButton(object sender, RoutedEventArgs e)
        {
            NetworkWindow networkWindow = new NetworkWindow();
            networkWindow.Owner = this;

            float[] inputLayer= RG.GetGridStateFloat();

            // Отрисовка входного слоя нейронов
            for (int i = 0; i < inputLayer.Length; i++)
            {
                Ellipse E = new Ellipse();
                E.Stroke = Brushes.Black;
                E.StrokeThickness = 1;
                E.Width = 30;
                E.Height = 30;
                E.Fill = Brushes.White;
                E.RenderTransform = new TranslateTransform() { X = 30, Y = (i+1) * 30 };
                networkWindow.NetworkCanvas.Children.Add(E);
            }

            // Отрисовка скрытого слоя нейронов
            for (int i = 0; i < 4; i++)
            {
                Ellipse E = new Ellipse();
                E.Stroke = Brushes.Black;
                E.StrokeThickness = 1;
                E.Width = 30;
                E.Height = 30;
                E.Fill = Brushes.White;
                E.RenderTransform = new TranslateTransform() { X = 150, Y = (i + 1) * ((inputLayer.Length * 30 / 4) - 15) };
                networkWindow.NetworkCanvas.Children.Add(E);
            }

            // Отрисовка выходного слоя нейронов
            for (int i = 0; i < 2; i++)
            {
                Ellipse E = new Ellipse();
                E.Stroke = Brushes.Black;
                E.StrokeThickness = 1;
                E.Width = 30;
                E.Height = 30;
                E.Fill = Brushes.White;
                E.RenderTransform = new TranslateTransform() { X = 270, Y = (i + 1) * ((inputLayer.Length * 30 / 2) - 65) };
                networkWindow.NetworkCanvas.Children.Add(E);
            }

            networkWindow.Show();
        }
    }
}
