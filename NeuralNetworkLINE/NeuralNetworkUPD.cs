using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLINE
{
    internal class NeuralNetworkUPD
    {
        List<float[,]> Weight; // Список связей между слоями двумя слоями, где каждый элемент - это
        // Двумерный массив. Каждый элемент массива - это значение веса между парой нейронов W1[i, j] в двух соседних слоях, где i - это номер нейрона в первом слою, а j - номер нейрона во втором слою
        List<float[,]> Layers; // Список слоев нейронной сети, где каждый элемент списка это - 
        // Двумерный массив, с двумя столбцами, где первый это непосредственно значений нейрона, а второй это ошибка слоя нейрона
        // При этом первый элемент списка - всегда входной слой, а последний - выходной
        float LearningRate = 0.3f;
        public NeuralNetworkUPD(int CountInPut = 16, int CountOutPut = 2, int CountHiddenLayers = 1, int CountHiddenNeural = 4)
        {
            // Создание слоев нейронов
            Layers = new List<float[,]>();
            Layers.Add(new float[CountInPut, CountInPut]);

            for (int i = 0; i < CountHiddenLayers; i++)
                Layers.Add(new float[CountHiddenNeural, CountHiddenNeural]);

            Layers.Add(new float[CountOutPut, CountOutPut]);

            // Создание весов между слоями
            Weight = new List<float[,]>();

            for (int i = 0; i < Weight.Count - 1; i++)
                Weight.Add(new float[Weight[i].GetLength(0), Weight[i+1].GetLength(0)]);

            foreach (float[,] weight in Weight)
                for (int i = 0; i < weight.GetLength(0); i++)
                    for (int j = 0; j < weight.GetLength(1); j++)
                        weight[i,j] = 0.5f; // Задание изначального веса
        }
        public NeuralNetworkUPD(int CountInPut = 16, int CountOutPut = 2, int CountHiddenLayers = 1, int[] HiddenNeural = null )
        {
            // Конструктор для задание нейронной сети с различным числом нейронов на каждом слое
            // Создание слоев нейронов
            Layers = new List<float[,]>();
            Layers.Add(new float[CountInPut, CountInPut]);

            if (HiddenNeural != null)
            {
                for (int i = 0; i < CountHiddenLayers; i++)
                    Layers.Add(new float[HiddenNeural[i], HiddenNeural[i]]);
            }
            else
            {
                for (int i = 0; i < CountHiddenLayers; i++)
                    Layers.Add(new float[4, 4]);
            }

            Layers.Add(new float[CountOutPut, CountOutPut]);

            // Создание весов между слоями
            Weight = new List<float[,]>();

            for (int i = 0; i < Weight.Count - 1; i++)
                Weight.Add(new float[Weight[i].GetLength(0), Weight[i + 1].GetLength(0)]);

            foreach (float[,] weight in Weight)
                for (int i = 0; i < weight.GetLength(0); i++)
                    for (int j = 0; j < weight.GetLength(1); j++)
                        weight[i, j] = 0.5f; // Задание изначального веса
        }

        float[,] LayerForward(float[,] FirstLayer, float[,] SecondLayer, float[,] LinksBetween) // Не тестировалось
        {
            // Возвращает новые значения нейронов для слоя из SecondLayer
            float[,] ResultLayer = new float[SecondLayer.GetLength(0), SecondLayer.GetLength(1)];
            for (int i = 0; i < SecondLayer.GetLength(1); i++) ResultLayer[0, i] = SecondLayer[0, i];

            for (int i = 0; i < ResultLayer.GetLength(0); i++)
            {
                ResultLayer[i, 0] = 0;

                for (int j = 0; j < FirstLayer.GetLength(0); j++)
                    ResultLayer[i, 0] += FirstLayer[j, 0] * LinksBetween[i, j];
            }

            return ResultLayer;
        }

        float[,] ErrorBetween(float[,] FirstLayer, float[,] SecondLayer, float[,] LinksBetween) // Не тестировалось
        {
            // Возвращает новые значения ошибки нейронов для слоя из FirstLayer
            float[,] ResultErrorLayer = new float[FirstLayer.GetLength(0), FirstLayer.GetLength(1)];
            for (int i = 0; i < FirstLayer.GetLength(0); i++) ResultErrorLayer[i, 0] = FirstLayer[i, 0];

            for (int i = 0; i < ResultErrorLayer.GetLength(1); i++)
            {
                ResultErrorLayer[0, i] = 0;

                for (int j = 0; j < SecondLayer.GetLength(1); j++)
                    ResultErrorLayer[0, i] += SecondLayer[0, j] * LinksBetween[i, j];
            }

            return ResultErrorLayer;
        }

        public void FindError(float[] ExpectedResult) // Не тестировалось
        {
            // Ошибка выходного слоя
            for (int i = 0; i < Layers[Layers.Count].GetLength(1); i++)
            {
                Layers[Layers.Count][0,i] = ExpectedResult[i] - Layers[0][i, 0];
            }

            // Ошибкb скрытого слоя
            for (int i = Layers.Count-1; i > 0; i--)
            {
                Layers[i] = ErrorBetween(Layers[i], Layers[i+1], Weight[i]);
            }
        }

    }
}