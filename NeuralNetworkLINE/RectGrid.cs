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
    public class RectGrid
    {
        List<Rectangle> RectList = new List<Rectangle>();
        public RectGrid(Canvas canvas, int col, int str = -1)
        {
            if (str == -1) str = col;
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
                    RectList.Add(R);
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
            int Incr = 1;
            foreach (Rectangle R in RectList)
            {
                if (R.Fill == Brushes.Black) result += " 1 ";
                else result += " 0 ";
                if (Incr % 4 == 0) result += "\n";
                Incr++;
            }

            return result;
        }
        public float[] GetGridStateFloat()
        {
            float[] result = new float[RectList.Count];
            int Incr = 0;
            foreach (Rectangle R in RectList)
            {
                if (R.Fill == Brushes.Black) result[Incr] = 1;
                else result[Incr] = 0;
                Incr++;
            }

            return result;
        }
    }
}
