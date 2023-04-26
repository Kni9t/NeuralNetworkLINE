using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLINE
{
    internal class NeuralNetworkUPD
    {
        float[,] W1, W2; // Двумерный массив. Каждый элемент массива - это значение веса между парой нейронов W1[i, j] в двух соседних слоях, где i - это номер нейрона в первом слою, а j - номер нейрона во втором слою
        float[,] InPutLayer, HiddenLayer, OutPutLayer; // Двумерный массив.
        float LearningRate = 0.3f;
    }
}