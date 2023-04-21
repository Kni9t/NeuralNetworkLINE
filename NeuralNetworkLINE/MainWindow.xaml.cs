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
    }
}
