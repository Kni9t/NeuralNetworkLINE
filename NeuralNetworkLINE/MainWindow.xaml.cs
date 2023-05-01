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
        struct TestUnit
        {
            public float[,] TestMass;
            public float[] ExpectedMass;
            public void SetMass(float[,] bufMass)
            {
                TestMass = new float[bufMass.GetLength(0), bufMass.GetLength(1)];

                for (int i = 0; i < bufMass.GetLength(0); i++)
                    for (int j = 0; j < bufMass.GetLength(1); j++)
                        TestMass[i, j] = bufMass[i, j];
            }
            public void SetExpected(float[] bufMass)
            {
                ExpectedMass = new float[bufMass.Length];

                for (int i = 0; i < bufMass.Length; i++)
                    ExpectedMass[i] = bufMass[i];
            }
        }

        private void SetUp(object sender, RoutedEventArgs e)
        {
            List<TestUnit> TestList = new List<TestUnit>();
            // Генерация тестового набора данных
            for (int g = 0; g < 4; g++) // Тестовые горизонтальные линии
            {
                float[,] Buf = new float[4, 4];

                for (int i = 0; i < 4; i++)
                {
                    Buf[g, i] = 1;
                }
                TestUnit bufUnit = new TestUnit();

                bufUnit.SetMass(Buf);
                bufUnit.SetExpected(new float[2] { 0.8f, 0.2f });

                TestList.Add(bufUnit);
            }
            for (int g = 0; g < 4; g++) // Тестовые вертикальные линии
            {
                float[,] Buf = new float[4, 4];

                for (int i = 0; i < 4; i++)
                {
                    Buf[i, g] = 1;
                }

                TestUnit bufUnit = new TestUnit();

                bufUnit.SetMass(Buf);
                bufUnit.SetExpected(new float[2] { 0.2f, 0.8f });

                TestList.Add(bufUnit);
            }
            for (int g = 0; g < 4; g++) // Две тестовых горизонтальных линий
            {
                float[,] Buf = new float[4, 4];

                for (int i = 0; i < 4; i++)
                {
                    Buf[g, i] = 1;
                    if ((g + 2) < 3) Buf[g + 2, i] = 1;
                }
                TestUnit bufUnit = new TestUnit();

                bufUnit.SetMass(Buf);
                bufUnit.SetExpected(new float[2] { 0.8f, 0.2f });

                TestList.Add(bufUnit);
            }
            for (int g = 0; g < 4; g++) // Две тестовых вертикальных линий
            {
                float[,] Buf = new float[4, 4];

                for (int i = 0; i < 4; i++)
                {
                    Buf[i, g] = 1;
                    if ((g + 2) < 3) Buf[i, g + 2] = 1;
                }

                TestUnit bufUnit = new TestUnit();

                bufUnit.SetMass(Buf);
                bufUnit.SetExpected(new float[2] { 0.2f, 0.8f });

                TestList.Add(bufUnit);
            }
            for (int g = 0; g < 4; g++) // Горизонтальная линия, вместе с вертикальной
            {
                float[,] Buf = new float[4, 4];

                for (int i = 0; i < 4; i++)
                {
                    Buf[i, g] = 1;
                    Buf[g, i] = 1;
                }

                TestUnit bufUnit = new TestUnit();

                bufUnit.SetMass(Buf);
                bufUnit.SetExpected(new float[2] { 0.8f, 0.8f });

                TestList.Add(bufUnit);
            }
            for (int g = 0; g < 3; g++) // Несколько запросов без линий
            {
                float[,] Buf = new float[4, 4];

                TestUnit bufUnit = new TestUnit();

                bufUnit.SetMass(Buf);
                bufUnit.SetExpected(new float[2] { 0.2f, 0.2f });

                TestList.Add(bufUnit);
            }

            string res = "";

            for (int l = 0; l < 1000; l++) // Число прогонов по тестовому набору
            {
                if (l % 100 == 0) 
                    for (int i = TestList.Count - 1; i >= 1; i--) // Перемешивание тестового набора
                {
                    int j = new Random().Next(i + 1);
                    var temp = TestList[j];
                    TestList[j] = TestList[i];
                    TestList[i] = temp;
                }

                foreach (TestUnit TU in TestList)
                    NW.Loop(TU.TestMass, TU.ExpectedMass);
                //if (l % 100 == 0)  res += "Итерация: " + l + "\nЗначение ошибки: " + NW.GetSummError().ToString("F") + "\n";
            }
            MessageBox.Show("Обучение завершено!");
            //MessageBox.Show(res);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            float[] Result = NW.Execute(RG.GetGridStateFloat());
            BaseLabel.Content = Result[0].ToString("F") + " " + Result[1].ToString("F");

            //NW.Loop(f1, e1);
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
            /*
            NetworkWindow networkWindow = new NetworkWindow();
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

            networkWindow.Show();
            */
        }
    }
}
