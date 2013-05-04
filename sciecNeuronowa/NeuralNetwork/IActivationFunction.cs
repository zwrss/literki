using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pl.edu.pk.NeuralNetwork
{
    public interface IActivationFunction
    {
        double func(double x);
        double deriv(double x);
    }
    public class Tanh : IActivationFunction
    {
        double beta;
        public Tanh(double beta)
        {
            this.beta = beta;
        }

        public double func(double x)
        {
            return Math.Tanh(beta * x);
        }

        private double sech(double x)
        {
            return 2 / (Math.Exp(x) + Math.Exp(-x)); 
        }

        public double deriv(double x)
        {
            //return beta * func(x) * (1 - func(x)) * (1 + func(x));
            return beta * Math.Pow(sech(beta * x), 2);
        }
    }
}
