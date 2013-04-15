using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pl.edu.pk.NeuralNetwork
{
    public class Perceptron : INeuron
    {
        List<INeuron> _inputs;
        List<double> _weights;
        double _output;

        public Perceptron(List<INeuron> inputs, List<double> weights)
        {
            this._inputs = inputs;
            this._weights = weights;
            this._output = 0.0;
        }

        public double output()
        {
            return this._output;
        }

        public void calculate()
        {
            double input = 0.0;

            for (int i = 0; i < _inputs.Count; i++)
                input += _inputs[i].output() * _weights[i];

            this._output = activationFunc(input);
        }

        private double activationFunc(double x)
        {
            double beta = 10.0;
            return 1.0 / (1 + Math.Exp(-beta * x));
        }

        public List<double> getWeights()
        {
            return this._weights;
        }

        public void setWeights(List<double> weights)
        {
            this._weights = weights;
        }
    }
}
