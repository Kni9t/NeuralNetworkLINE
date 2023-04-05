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
        public MainWindow()
        {
            InitializeComponent();
            RG = new RectGrid(BaseCanvas, 4);
        }
        public void CanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (e.ChangedButton == MouseButton.Left) BaseLabel.Content = C1.test();
        }
        public void Up(int b)
        {
            b++;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string Result = "";
            byte[][] buf = new byte[4][];
            for (int i = 0; i < buf.Length; i++) buf[i] = new byte[4];

            int Col = 0, Str = 0;

            foreach (Rectangle R in BaseCanvas.Children)
            {
                if (R.Fill == Brushes.White) buf[Str][Col] = 0;
                else buf[Str][Col] = 1;

                if (Str == 3)
                {
                    Col++;
                    Str = 0;
                }
                else Str++;
            }

            for (int i = 0; i < buf.Length; i++)
            {
                for (int j = 0; j < buf[i].Length; j++)
                {
                    Result += (buf[i][j] + " ");
                }
                Result += "\n";
            }
            MessageBox.Show(Result);
        }
    }
}
