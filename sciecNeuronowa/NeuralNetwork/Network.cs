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

        public Network(int inputSize, int hiddenSize, int outputSize)
        {
            _inputLayer = new List<InputNeuron>(inputSize);
            List<double> _inputLayerWeights = new List<double>(inputSize);
            for (int i = 0; i < inputSize; i++)
            {
                _inputLayer.Add(new InputNeuron());
                _inputLayerWeights.Add(new Random().Next(0, 1));
            }

            _hiddenLayer = new List<INeuron>(hiddenSize);
            List<double> _hiddenLayerWeights = new List<double>(inputSize);
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

        public void setWeights(List<double> weights)
        {
            for (int i = 0; i < _hiddenLayer.Count; i++)
                _hiddenLayer[i].setWeights(weights.GetRange(i * _inputLayer.Count, _inputLayer.Count));
            for (int i = 0; i < _outputLayer.Count; i++)
                _hiddenLayer[i].setWeights(weights.GetRange(_inputLayer.Count * _hiddenLayer.Count + i * _hiddenLayer.Count, _hiddenLayer.Count));
        }

        public List<double> getWeights()
        {
            List<double> weights = new List<double>(_hiddenLayer.Count * _inputLayer.Count + _outputLayer.Count * _hiddenLayer.Count);
            foreach (INeuron n in _hiddenLayer)
                weights.AddRange(n.getWeights());
            foreach (INeuron n in _outputLayer)
                weights.AddRange(n.getWeights());
            return weights;
        }

        public List<double> classify(List<double> input)
        {
            List<double> output = new List<double>(_outputLayer.Count);
            foreach (INeuron n in _hiddenLayer)
                n.calculate();
            foreach (INeuron n in _outputLayer)
            {
                n.calculate();
                output.Add(n.output());
            }
            return output;
        }

        public void teach(List<double> input, List<double> output)
        {
            //TODO Implementacja nauczania!
        }

    }
}
