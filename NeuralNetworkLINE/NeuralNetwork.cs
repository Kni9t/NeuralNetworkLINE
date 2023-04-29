using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNetworkLINE
{
    internal class NeuralNetwork
    {
        Random R = new Random();
        List<float[,]> Weight; // Список связей между слоями двумя слоями, где каждый элемент - это
        // Двумерный массив. Каждый элемент массива - это значение веса между парой нейронов W1[i, j] в двух соседних слоях, где i - это номер нейрона в первом слою, а j - номер нейрона во втором слою
        List<float[,]> Layers; // Список слоев нейронной сети, где каждый элемент списка это - 
        // Двумерный массив, с двумя столбцами, где первый это непосредственно значений нейрона, а второй это ошибка слоя нейрона
        // При этом первый элемент списка - всегда входной слой, а последний - выходной
        float LearningRate = 0.1f;
        public NeuralNetwork(int CountInPut = 16, int CountOutPut = 2, int CountHiddenLayers = 1, int CountHiddenNeural = 4)
        {
            // Создание слоев нейронов
            Layers = new List<float[,]>();
            Layers.Add(new float[CountInPut, 2]);

            for (int i = 0; i < CountHiddenLayers; i++)
                Layers.Add(new float[CountHiddenNeural, 2]);

            Layers.Add(new float[CountOutPut, 2]);

            // Создание весов между слоями
            Weight = new List<float[,]>();

            for (int l = 0; l < Layers.Count - 1; l++)
            {
                float[,] buf = new float[Layers[l].GetLength(0), Layers[l + 1].GetLength(0)];

                for (int i = 0; i < buf.GetLength(0); i++)
                    for (int j = 0; j < buf.GetLength(1); j++)
                        buf[i, j] = ((float)R.Next(-30, 30) / 100f );

                Weight.Add(buf);
            }
        }
        public NeuralNetwork(int[] HiddenNeural, int CountInPut = 16, int CountOutPut = 2, int CountHiddenLayers = 1)
        {
            // Конструктор для задание нейронной сети с различным числом нейронов на каждом слое
            // Создание слоев нейронов
            Layers = new List<float[,]>();
            Layers.Add(new float[CountInPut, CountInPut]);

            for (int i = 0; i < CountHiddenLayers; i++)
                Layers.Add(new float[HiddenNeural[i], HiddenNeural[i]]);

            Layers.Add(new float[CountOutPut, CountOutPut]);

            // Создание весов между слоями
            Weight = new List<float[,]>();

            for (int l = 0; l < Layers.Count - 1; l++)
            {
                float[,] buf = new float[Layers[l].GetLength(0), Layers[l + 1].GetLength(0)];

                for (int i = 0; i < buf.GetLength(0); i++)
                    for (int j = 0; j < buf.GetLength(1); j++)
                        buf[i, j] = ((float)R.Next(-5, 5) / 10f);

                Weight.Add(buf);
            }
        }
        public void InPut(float[] InPutMass)
        {
            for (int i = 0; i < InPutMass.Length; i++)
            {
                Layers[0][i, 0] = InPutMass[i];
            }
        }
        public void InPut(float[,] InPutMass)
        {
            int i = 0;

            foreach (float f in InPutMass)
            {
                Layers[0][i, 0] = f;
                i++;
            }
        }

        float[,] LayerForward(float[,] FirstLayer, float[,] SecondLayer, float[,] LinksBetween)
        {
            // Возвращает новые значения нейронов для слоя из SecondLayer
            float[,] ResultLayer = new float[SecondLayer.GetLength(0), 2];
            for (int i = 0; i < SecondLayer.GetLength(0); i++) ResultLayer[i, 0] = SecondLayer[i, 0];

            for (int i = 0; i < ResultLayer.GetLength(0); i++)
            {
                ResultLayer[i, 0] = 0;

                for (int j = 0; j < FirstLayer.GetLength(0); j++)
                    ResultLayer[i, 0] += FirstLayer[j, 0] * LinksBetween[j, i];
            }

            return ResultLayer;
        }

        float[,] ErrorBetween(float[,] FirstLayer, float[,] SecondLayer, float[,] LinksBetween)
        {
            // Возвращает новые значения ошибки нейронов для слоя из FirstLayer
            float[,] ResultErrorLayer = new float[FirstLayer.GetLength(0), FirstLayer.GetLength(1)];
            for (int i = 0; i < FirstLayer.GetLength(0); i++) ResultErrorLayer[i, 0] = FirstLayer[i, 0];

            for (int i = 0; i < ResultErrorLayer.GetLength(0); i++)
            {
                ResultErrorLayer[i, 1] = 0;

                for (int j = 0; j < SecondLayer.GetLength(0); j++)
                    ResultErrorLayer[i, 1] += SecondLayer[j, 1] * LinksBetween[i, j];
            }

            return ResultErrorLayer;
        }

        public void FindError(float[] ExpectedResult)
        {
            // Ошибка выходного слоя
            for (int i = 0; i < Layers[Layers.Count - 1].GetLength(0); i++)
            {
                Layers[Layers.Count - 1][i, 1] = ExpectedResult[i] - Layers[Layers.Count - 1][i, 0];
            }

            // Ошибка скрытого слоя
            for (int i = Layers.Count-2; i > 0; i--)
            {
                Layers[i] = ErrorBetween(Layers[i], Layers[i+1], Weight[i]);
            }
        }
        float[,] WeightСorrection(float[,] FirstLayer, float[,] SecondLayer, float[,] LinksBetween) // Подумать над формулой расчетов
        {
            float[,] NewLinksBetween = new float[LinksBetween.GetLength(0), LinksBetween.GetLength(1)];

            for (int i = 0; i < NewLinksBetween.GetLength(0); i++)
            {
                for (int j = 0; j < NewLinksBetween.GetLength(1); j++)
                {
                    // Формула вычисления нового веса следующая:
                    // Wн = Wc + C * E * x * (y * (1 - y))
                    // Где: Wн - новый вес, Wc - старый вес, C - коэф. обучения, E - ошибка правого нейрона, х - входное значение нейрона, у - выходное значение нейрона
                    NewLinksBetween[i, j] = LinksBetween[i, j] + LearningRate * SecondLayer[j, 1] * FirstLayer[j, 0] * (SecondLayer[j, 0] * (1 - SecondLayer[j, 0]));
                }
            }
            return NewLinksBetween;
        }
        public void Correct(float[] ExpectedResult) // Тестить
        {
            FindError(ExpectedResult);

            for (int i = Weight.Count-1; i >= 0; i--)
            {
                Weight[i] = WeightСorrection(Layers[i], Layers[i + 1], Weight[i]);
            }
        }

        public float[] DoIt() // Тестовая функция вывода результата
        {
            float[] Result = new float[Layers[Layers.Count - 1].GetLength(0)];

            for (int i = 0; i < Weight.Count; i++)
            {
                for (int j = 0; j < Layers[i + 1].GetLength(0); j++)
                {
                    for (int g = 0; g < Layers[i].GetLength(0); g++)
                    {
                        Layers[i + 1] = LayerForward(Layers[i], Layers[i + 1], Weight[i]);
                    }
                }
            }

            for (int i = 0; i < Result.Length; i++)
                Result[i] = Layers[Layers.Count - 1][i, 0];
            return Result;
        }
        public void Loop(float[,] InPutMass, float[] ExpectedResult)
        {
            InPut(InPutMass);

            for (int i = 0; i < Weight.Count; i++)
            {
                for (int j = 0; j < Layers[i + 1].GetLength(0); j++)
                {
                    for (int g = 0; g < Layers[i].GetLength(0); g++)
                    {
                        Layers[i + 1] = LayerForward(Layers[i], Layers[i + 1], Weight[i]);
                    }
                }
            }

            Correct(ExpectedResult);
        }
        public float[] GetResult()
        {
            float[] Result = new float[Layers[Layers.Count - 1].GetLength(0)];

            for (int i = 0; i < Result.Length; i++)
                Result[i] = Layers[Layers.Count - 1][i, 0];
            return Result;
        }

        /*public void SaveWeights() Не поддерживается сериализация матрицы. Сделать
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "json file (*.json)|*.json|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FilePath;
                if ((FilePath = saveFileDialog1.FileName) != null)
                {
                    FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate);
                    var Json = JsonSerializer.Serialize(W1);
                    File.WriteAllText(FilePath, Json);
                }
            }
        }*/
    }
}