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
            NW = new NeuralNetwork();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //NW.InPut(RG.GetGridStateFloat());
            float[,] f1 = new float[4, 4]
            {
                {1,1,1,1 },
                {0,0,0,0 },
                {0,0,0,0 },
                {0,0,0,0 }
            };
            float[] e1 = new float[2] { 0.8f, 0.2f };

            float[,] f2 = new float[4, 4]
            {
                {1,0,0,0 },
                {1,0,0,0 },
                {1,0,0,0 },
                {1,0,0,0 }
            };
            float[] e2 = new float[2] { 0.2f, 0.8f };

            float[,] f3 = new float[4, 4]
            {
                {1,1,1,1 },
                {1,0,0,0 },
                {1,0,0,0 },
                {1,0,0,0 }
            };
            float[] e3 = new float[2] { 0.8f, 0.8f };

            float[,] f4 = new float[4, 4]
            {
                {1,1,1,1 },
                {0,0,0,0 },
                {1,1,1,1 },
                {0,0,0,0 }
            };
            float[] e4 = new float[2] { 0.8f, 0.2f };

            NW.Loop(f1, e1);
            Show(NW.GetResult());

            NW.Loop(f2, e2);
            Show(NW.GetResult());

            NW.Loop(f3, e3);
            Show(NW.GetResult());

            NW.Loop(f4, e4);
            Show(NW.GetResult());



            //NW.Correct(new float[2] { 0.8f, 0.2f }); // Первое число - горизонтальная линия, второе число - вертикальная линия | >0,8 есть линия, <0.2 нету
        }

        void Show(float[] Mass)
        {
            string buf = "";
            foreach (float f in Mass) buf += f + " ";
            MessageBox.Show(buf);
        }

        private void ShowNetworkButton(object sender, RoutedEventArgs e)
        {
            // Переделать под двумерный массив
            /*NetworkWindow networkWindow = new NetworkWindow();
            networkWindow.Owner = this;

            float[] buf= RG.GetGridStateFloat();

            // Отрисовка входного слоя нейронов
            for (int i = 0; i < buf.Length; i++)
            {
                Grid G = new Grid();
                Ellipse E = new Ellipse();
                E.Stroke = Brushes.Black;
                E.StrokeThickness = 1;
                E.Width = 30;
                E.Height = 30;
                E.Fill = Brushes.White;
                G.RenderTransform = new TranslateTransform() { X = 30, Y = (i+1) * 30 };
                G.Children.Add(E);
                TextBlock TB = new TextBlock();
                TB.Text = buf[i].ToString();
                TB.RenderTransform = new TranslateTransform() { X = 12, Y = 9 };
                G.Children.Add(TB);
                networkWindow.NetworkCanvas.Children.Add(G);
            }

            // Отрисовка скрытого слоя нейронов
            buf = NW.GetHidden();
            for (int i = 0; i < 4; i++)
            {
                Grid G = new Grid();
                Ellipse E = new Ellipse();
                E.Stroke = Brushes.Black;
                E.StrokeThickness = 1;
                E.Width = 30;
                E.Height = 30;
                E.Fill = Brushes.White;
                G.RenderTransform = new TranslateTransform() { X = 150, Y = (i + 1) * ((RG.GetGridStateFloat().Length * 30 / 4) - 15) };
                G.Children.Add(E);
                TextBlock TB = new TextBlock();
                TB.Text = buf[i].ToString();
                TB.RenderTransform = new TranslateTransform() { X = 12, Y = 9 };
                G.Children.Add(TB);
                networkWindow.NetworkCanvas.Children.Add(G);
            }

            for (int i = 0; i < buf.Length * 4; i++) // Отрисовка линий, доработать
            {
                Line L = new Line();
                L.X1 = 45;
                L.X2 = 165;
                L.Y1 = (i + 1) * 30 + 15;
                L.Y2 = (i + 1) * ((RG.GetGridStateFloat().Length * 30 / 4) - 15) + 15;
                L.Stroke = Brushes.Black;
                L.StrokeThickness = 1;
                networkWindow.NetworkCanvas.Children.Add(L);
            }

            // Отрисовка выходного слоя нейронов
            buf = NW.GetOutPut();
            for (int i = 0; i < 2; i++)
            {
                Grid G = new Grid();
                Ellipse E = new Ellipse();
                E.Stroke = Brushes.Black;
                E.StrokeThickness = 1;
                E.Width = 30;
                E.Height = 30;
                E.Fill = Brushes.White;
                G.RenderTransform = new TranslateTransform() { X = 270, Y = (i + 1) * ((RG.GetGridStateFloat().Length * 30 / 2) - 65) };
                G.Children.Add(E);
                TextBlock TB = new TextBlock();
                TB.Text = buf[i].ToString();
                TB.RenderTransform = new TranslateTransform() { X = 12, Y = 9 };
                G.Children.Add(TB);
                networkWindow.NetworkCanvas.Children.Add(G);
            }

            networkWindow.Show();*/
        }
    }
}
