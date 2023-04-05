using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLINE
{
    public class NeuralNetwork
    {
        float[,] W1, W2; // Первое число обозначает выходной номер, второе входной
        float[] InPut, Hidden, OutPut;

        public NeuralNetwork(float[] InPut, float[] OutPut)
        {
            this.InPut = InPut;
            this.OutPut = OutPut;

            Hidden = new float[4] {0, 0, 0, 0};

            W1 = new float[Hidden.Length, InPut.Length];

            for (int i = 0; i < Hidden.Length; i++)
                for (int j = 0; j < InPut.Length; j++) 
                    W1[i,j] = 0.5f;

            W1 = new float[OutPut.Length, Hidden.Length];

            for (int i = 0; i < OutPut.Length; i++)
                for (int j = 0; j < Hidden.Length; j++)
                    W2[i, j] = 0.5f;
        }

        public float[] LayerForward(float[] FirstLayer, float[] SecondLayer, float[,] LinksBetween)
        {
            float[] ResultLayer = SecondLayer;

            for (int i = 0; i < ResultLayer.Length; i++)
            {
                ResultLayer[i] = 0;
                for (int j = 0; j < FirstLayer.Length; j++)
                    ResultLayer[i] += FirstLayer[j] * LinksBetween[i, j];
            }

            return ResultLayer;
        }

        public float[] DoIt()
        {
            float[] ResultLayer = new float[InPut.Length];

            Hidden = LayerForward(InPut, Hidden, W1);
            OutPut = LayerForward(Hidden, OutPut, W2);

            return ResultLayer;
        }
    }
}
