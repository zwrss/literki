using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pl.edu.pk.NeuralNetwork
{
    public class Network
    {

        List<Perceptron> _layer1 = new List<Perceptron>();
        List<Perceptron> _layer2 = new List<Perceptron>();
        List<Perceptron> _layer3 = new List<Perceptron>();
        List<Link> _input = new List<Link>();
        List<Link> _output = new List<Link>();
        double mi = 5;
        IActivationFunction func = new Tanh(5);

        public Network(int l1, int l2, int l3)
        {
            _layer1 = new List<Perceptron>();
            _layer2 = new List<Perceptron>();
            _layer3 = new List<Perceptron>();
            _input = new List<Link>();
            _output = new List<Link>();
            for (int i = 0; i < l1; i++)
            {
                //Creating new neuron and adding the link
                Perceptron neuron = new Perceptron(func);
                //Creating a link representing (partial) input signal 
                asInput(neuron);
                //Adding neuron to layer
                _layer1.Add(neuron);
            }
            for (int i = 0; i < l2; i++)
            {
                //Creating new neuron and adding it to the layer
                Perceptron neuron = new Perceptron(func);
                _layer2.Add(neuron);
                //Linking it with all neurons of the previous layer
                foreach (Perceptron prevNeuron in _layer1)
                {
                    prevNeuron.linkWith(neuron);
                }
            }
            for (int i = 0; i < l3; i++)
            {
                //Creating new neuron and adding it to the layer
                Perceptron neuron = new Perceptron(func);
                //Creating and adding a link representing (partial) output signal
                asOutput(neuron);
                //Adding neuron to layer
                _layer3.Add(neuron);
                //Linking it with all neurons of the previous layer
                foreach (Perceptron prevNeuron in _layer2)
                {
                    prevNeuron.linkWith(neuron);
                }
            }
            if (_input.Count != _layer1.Count)
            {
                throw new Exception("Internal exception! Input count is " + _input.Count + ", expected " + _layer1.Count);
            }
            if (_output.Count != _layer3.Count)
            {
                throw new Exception("Internal exception! Output count is " + _output.Count + ", expected " + _layer3.Count);
            }
        }

        void asInput(Perceptron n)
        {
            Link link = new Link(1, null, n);
            this._input.Add(link);
            n.addInput(link);
        }

        void asOutput(Perceptron n)
        {
            Link link = new Link(1, n, null);
            this._output.Add(link);
            n.addOutput(link);
        }

        public List<double> output(List<double> input)
        {
            if (input.Count != _input.Count)
            {
                throw new ArgumentException("Bad input vector size! Expected " + _input.Count + ", actual " + input.Count);
            }
            //Applying input signals
            for (int i = 0; i < input.Count; i++)
            {
                _input[i].setSignal(input[i]);
            }
            //Working each layer
            foreach (Perceptron n in _layer1)
            {
                n.work();
            }
            foreach (Perceptron n in _layer2)
            {
                n.work();
            }
            foreach (Perceptron n in _layer3)
            {
                n.work();
            }
            //Reading output signals
            List<double> output = new List<double>();
            for (int i = 0; i < _output.Count; i++)
            {
                output.Add(_output[i].getRawSignal());
            }
            return output;
        }

        public void teachOne(List<double> input, List<double> expectedOutput)
        {
            if (input.Count != _input.Count)
            {
                throw new ArgumentException("Bad input vector size! Expected " + _input.Count + ", actual " + input.Count);
            }
            if (expectedOutput.Count != _output.Count)
            {
                throw new ArgumentException("Bad output vector size! Expected " + _output.Count + ", actual " + expectedOutput.Count);
            }

            output(input);

            for (int i = 0; i < _output.Count; i++)
            {
                _output[i].error = _output[i].getRawSignal() - expectedOutput[i];
            }

            //Weights between hidden and output layers
            foreach (Perceptron neuronL3 in _layer3)
            {
                double y = neuronL3.getOutputs()[0].getRawSignal();
                double error = neuronL3.getOutputs()[0].error;
                foreach (Link linkL2L3 in neuronL3.getInputs())
                {
                    double w = linkL2L3.getWeight();
                    double h = linkL2L3.getRawSignal();
                    double dw = mi * error * func.deriv(y) * h;
                    linkL2L3.setWeight(w + dw);
                }
            }

            //Weights between input and hidden layers
            foreach (Perceptron neuronL2 in _layer2)
            {
                foreach (Link linkL1L2 in neuronL2.getInputs())
                {
                    double w = linkL1L2.getWeight();
                    double x = linkL1L2.getRawSignal();
                    double h = linkL1L2.getOutput().getOutputs()[0].getRawSignal();
                    double sum = 0.0;
                    foreach (Link linkL2L3 in linkL1L2.getOutput().getOutputs())
                    {
                        double y = linkL2L3.getOutput().getOutputs()[0].getRawSignal();
                        double error = linkL2L3.getOutput().getOutputs()[0].error;
                        double wmk = linkL2L3.getWeight();
                        sum += error * func.deriv(y) * wmk * func.deriv(h) * x;
                    }
                    double dw = mi * sum;
                    linkL1L2.setWeight(w + dw);
                }
            }

        }

        public void printWeights()
        {
            foreach (Perceptron n in _layer1)
            {
                foreach (Link l in _output)
                {
                    System.Console.Write(l.getWeight() + ", ");
                }
            }
            foreach (Perceptron n in _layer2)
            {
                foreach (Link l in _output)
                {
                    System.Console.Write(l.getWeight() + ", ");
                }
            }
        }
    }
}
