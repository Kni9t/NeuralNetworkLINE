using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NeuralNetworkLINE
{
    internal class RectGrid
    {
        Rectangle[,] RectList;
        public RectGrid(Canvas canvas, int col, int str = -1)
        {
            if (str == -1) str = col;
            RectList = new Rectangle[col, str];
            
            for (int i = 0; i < col; i++)
                for (int j = 0; j < str; j++)
                {
                    Rectangle R = new Rectangle();
                    R.Stroke = Brushes.Black;
                    R.StrokeThickness = 1;
                    R.Width = 50;
                    R.Height = 50;
                    R.MouseDown += ChangeBack;
                    R.Fill = Brushes.White;
                    R.RenderTransform = new TranslateTransform() { X = i * 50, Y = j * 50 };
                    canvas.Children.Add(R);
                    RectList[j, i] = R;
                }
        }
        void ChangeBack(object sender, MouseButtonEventArgs e)
        {
            if (((Rectangle)sender).Fill == Brushes.White)
                ((Rectangle)sender).Fill = Brushes.Black;
            else
                ((Rectangle)sender).Fill = Brushes.White;
        }
        public string GetGridStateString()
        {
            string result = "";
      
            for (int i = 0; i < RectList.GetLength(0); i++)
            {
                for (int j = 0; j < RectList.GetLength(1); j++)
                {
                    if (RectList[i, j].Fill == Brushes.Black) result += " 1 ";
                    else result += " 0 ";
                }
                result += "\n";
            }

            return result;
        }
        public float[,] GetGridStateFloat()
        {
            float[,] result = new float[RectList.GetLength(0), RectList.GetLength(1)];

            for (int i = 0; i < RectList.GetLength(0); i++)
                for (int j = 0; j < RectList.GetLength(1); j++)
                    if (RectList[i, j].Fill == Brushes.Black) result[i, j] = 1;
                    else result[i, j] = 0;

            return result;
        }
    }
}