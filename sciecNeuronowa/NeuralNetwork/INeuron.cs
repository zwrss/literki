using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pl.edu.pk.NeuralNetwork
{
    public interface INeuron
    {
        double output();
        void calculate();
        List<double> getWeights();
        void setWeights(List<double> weights);
    }
}
