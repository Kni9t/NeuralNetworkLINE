using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NeuralNetworkLINE
{
    public class NeuralNetwork
    {
        float[,] W1, W2; // Двумерный массив, так как каждая строчка представляет число нейрон в первом слое, а столбец число нейронов во втором. Значение же в массивы - значения веса между этими нейронами
        float[] InPut, Hidden, OutPut;
        float[] ErrorHidden, ErrorOutPut;
        float LearningRate = 0.3f;

        public NeuralNetwork(float[] InPut, float[] OutPut)
        {
            this.InPut = InPut;
            this.OutPut = OutPut;

            Hidden = new float[4] {0, 0, 0, 0};

            W1 = new float[Hidden.Length, InPut.Length];

            for (int i = 0; i < Hidden.Length; i++)
                for (int j = 0; j < InPut.Length; j++) 
                    W1[i,j] = 0.5f;

            W2 = new float[OutPut.Length, Hidden.Length];

            for (int i = 0; i < OutPut.Length; i++)
                for (int j = 0; j < Hidden.Length; j++)
                    W2[i, j] = 0.5f;
        }

        float[] LayerForward(float[] FirstLayer, float[] SecondLayer, float[,] LinksBetween)
        {
            float[] ResultLayer = new float[SecondLayer.Length];
            Array.Copy(SecondLayer, ResultLayer, SecondLayer.Length);

            for (int i = 0; i < ResultLayer.Length; i++)
            {
                ResultLayer[i] = 0;
                for (int j = 0; j < FirstLayer.Length; j++)
                    ResultLayer[i] += FirstLayer[j] * LinksBetween[i, j];
            }

            return ResultLayer;
        }

        float[] ErrorBetween(float[] FirstLayer, float[] ErrorLayer, float[,] LinksBetween)
        {
            float[] ResultErrorLayer = new float[FirstLayer.Length];
            for (int i = 0; i < FirstLayer.Length; i++)
                ResultErrorLayer[i] = FirstLayer[i];

            for (int i = 0; i < ResultErrorLayer.Length; i++)
            {
                ResultErrorLayer[i] = 0;
                for (int j = 0; j < ErrorLayer.Length; j++)
                    ResultErrorLayer[i] += ErrorLayer[j] * LinksBetween[j, i];
            }

            return ResultErrorLayer;
        }

        void WeightСorrection(float[] FirstLayer, float[] FirstErrorLayer, float[] SecondLayer, float[,] LinksBetween) // Доделать формулу
        {
            float[,] NewLinksBetween = new float[LinksBetween.GetLength(0),LinksBetween.GetLength(0)];

            for (int i = 0; i< NewLinksBetween.GetLength(0); i++)
            {
                for (int j = 0; j < NewLinksBetween.GetLength(1); j++)
                {
                    // Формула вычисления нового веса следующая:
                    // Wн = Wc + C * E * x * (y * (1 - y))
                    // Где: Wн - новый вес, Wc - старый вес, C - коэф. обучения, E - ошибка правого нейрона, х - входное значение нейрона, у - выходное значение нейрона
                    NewLinksBetween[i, j] = LinksBetween[i, j] + LearningRate * FirstErrorLayer[j] * (SecondLayer[i] * LinksBetween[i,j]);
                }
            }
        }

        public float[] DoIt()
        {
            Hidden = LayerForward(InPut, Hidden, W1);
            OutPut = LayerForward(Hidden, OutPut, W2);

            return OutPut;
        }

        public void FindError(float[] ExpectedResult)
        {
            // Ошибка выходного слоя
            ErrorOutPut = new float[ExpectedResult.Length];

            for (int i = 0; i < OutPut.Length; i++)
            {
                ErrorOutPut[i] = ExpectedResult[i] - OutPut[i];
            }

            // Ошибка скрытого слоя
            ErrorHidden = ErrorBetween(Hidden, ErrorOutPut, W2);
        }

        public float[] GetHidden()
        {
            return Hidden;
        }
        public float[] GetOutPut()
        {
            return OutPut;
        }

        
    }
}
