using System;
using System.Collections.Generic;
using sciecNeuronowa;

namespace pl.edu.pk.NeuralNetwork
{
    public class Perceptron
    {
        List<Link> _inputs;
        List<Link> _outputs;
        IActivationFunction func;

        public Perceptron(IActivationFunction func)
        {
            this.func = func;
            this._inputs = new List<Link>();
            this._outputs = new List<Link>();
            //Adding bias
            double w = Program.random.NextDouble() * 2 - 1;
            Link bias = new Link(w, null, this); //TODO Waga!
            bias.setSignal(1.0);
            this._inputs.Add(bias);
        }

        double activationFunction(double x)
        {
            return func.func(x);
        }

        public void work()
        {
            //Calculating input signal
            double input = 0.0;
            foreach (Link l in _inputs)
            {
                input += l.getSignal();
            }
            //Calculating output signal
            double output = activationFunction(input);
            //Propagating output signal
            foreach (Link l in _outputs)
            {
                l.setSignal(output);
            }
        }

        public void addInput(Link l)
        {
            this._inputs.Add(l);
        }

        public void addOutput(Link l)
        {
            this._outputs.Add(l);
        }

        public void linkWith(Perceptron p)
        {
            double w = Program.random.NextDouble() * 2 - 1;
            Link l = new Link(w, this, p); //TODO Waga!
            this.addOutput(l);
            p.addInput(l);
        }

        public List<Link> getInputs()
        {
            return this._inputs;
        }

        public List<Link> getOutputs()
        {
            return this._outputs;
        }

    }
}
