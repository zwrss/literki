using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pl.edu.pk.NeuralNetwork
{
    public class InputNeuron : INeuron
    {
        double _input;

        public InputNeuron()
        {
            this._input = 0.0;
        }

        public void input(double inp)
        {
            this._input = inp;
        }

        public double output()
        {
            return _input;
        }

        public List<double> getWeights()
        {
            return new List<double>(0);
        }

        public void setWeights(List<double> weights)
        {
        }

        public void calculate()
        { 
        }
    }
}
