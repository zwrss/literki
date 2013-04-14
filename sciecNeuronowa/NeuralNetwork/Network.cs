using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pl.edu.pk.NeuralNetwork
{
    class Network
    {
        List<InputNeuron> _inputLayer;
        List<INeuron> _hiddenLayer, _outputLayer;
        List<double> _inputLayerWeights, _hiddenLayerWeights;

        public Network(int inputSize, int hiddenSize, int outputSize)
        {
            _inputLayer = new List<InputNeuron>(inputSize);
            _inputLayerWeights = new List<double>(inputSize);
            for (int i = 0; i < inputSize; i++)
            {
                _inputLayer.Add(new InputNeuron());
                _inputLayerWeights.Add(new Random().Next(0, 1));
            }

            _hiddenLayer = new List<INeuron>(hiddenSize);
            _hiddenLayerWeights = new List<double>(inputSize);
            for (int i = 0; i < hiddenSize; i++)
            {
                List<INeuron> tmp = new List<INeuron>();
                tmp.AddRange(_inputLayer);
                _hiddenLayer.Add(new Perceptron(tmp, _inputLayerWeights));
                _hiddenLayerWeights.Add(new Random().Next(0, 1));
            }

            _outputLayer = new List<INeuron>(outputSize);
            for (int i = 0; i < outputSize; i++)
                _outputLayer.Add(new Perceptron(_hiddenLayer, _hiddenLayerWeights));
        }


    }
}
