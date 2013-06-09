using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pl.edu.pk.NeuralNetwork
{
    public class Neuron
    {
        public Neuron[] _inputs = new Neuron[0];
        public double[] _weights = new double[0];
        double _signal = 0;

        public Neuron()
        {
        }

        static public double f(double x)
        {
            double b = 20;
            //double e = Math.E;
            //return 1/(1 + Math.Pow(e, -b*x));
            return Math.Tanh(b * x);
        }

        static public double d(double x)
        {
            double b = 20;
            //return b * f(x) * (1 - f(x));
            return 1.0 * b * f(x) * (1 - f(x)) * (1 + f(x));
        }

        public double x(int i)
        {
            return _inputs[i].y();
        }

        public double s()
        {
            double sum = 0.0;
            for (int i = 0; i < _weights.Length; i++)
            {
                sum += x(i) * w(i);
            }
            return sum;
        }

        public double w(int inputNeuron)
        {
            return this._weights[inputNeuron];
        }

        public double y()
        {
            return this._signal;
        }
        
        public void setInputs(Neuron[] neurons)
        {
            this._inputs = neurons;
            this._weights = new double[_inputs.Length];
            for (int i = 0; i < _inputs.Length; i++)
            {
                this._weights[i] = Network.rand.NextDouble() * 2 - 1;
            }
        }

        public void setSignal(double signal)
        {
            this._signal = signal;
        }

        public void setWeight(int inputNeuron, double w)
        {
            _weights[inputNeuron] = w;
        }

        public void think()
        {
            _signal = f(s());
        }
    }

    public class Layer
    {
        public Neuron[] _neurons;
        public Layer(int neurons)
        {
            this._neurons = new Neuron[neurons + 1];
            for (int i = 0; i < neurons + 1; i++)
            {
                this._neurons[i] = new Neuron();
            }
            this._neurons[0].setSignal(1); // strobe
        }

        public void linkInputLayer(Layer l)
        {
            for (int i = 1; i < _neurons.Length; i++)
            {
                _neurons[i].setInputs(l._neurons);
            }
        }

        public void setSignals(double[] s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                this._neurons[i+1].setSignal(s[i]);
            }
        }

        public double y(int neuron)
        {
            return _neurons[neuron].y();
        }

        public double[] getSignals()
        {
            double[] signals = new double[_neurons.Length - 1];
            for (int i = 0; i < signals.Length; i++)
            {
                signals[i] = _neurons[i+1].y();
            }
            return signals;
        }

        public double w(int neuron, int inputNeuron)
        {
            return _neurons[neuron].w(inputNeuron);
        }

        public double s(int neuron)
        {
            return _neurons[neuron].s();
        }

        public void setWeight(int neuron, int inputNeuron, double w)
        {
            _neurons[neuron].setWeight(inputNeuron, w);
        }

        public void think()
        {
            for (int i = 1; i < _neurons.Length; i++)
            {
                _neurons[i].think();
            }
        }
    }

    public class Network
    {
        Layer[] _layers;
        int _lastLayer, _inputs, _outputs;
        double[] _err;
        double mi = 0.7;

        public static Random rand = new Random();

        public Network(int inputs, int outputs, int hiddens)
        {
            this._inputs = inputs;
            this._outputs = outputs;
            this._layers = new Layer[4];
            this._lastLayer = 3;

            // preinput layer
            this._layers[0] = new Layer(inputs);
                        
            // input layer
            this._layers[1] = new Layer(inputs);
            this._layers[1].linkInputLayer(this._layers[0]);

            // hidden layer
            this._layers[2] = new Layer(hiddens);
            this._layers[2].linkInputLayer(this._layers[1]);

            // output layer
            this._layers[3] = new Layer(outputs);
            this._layers[3].linkInputLayer(this._layers[2]);

            this._err = new double[outputs];
            for (int i = 0; i < outputs; i++)
            {
                _err[i] = Double.MaxValue;
            }
        }

        public void think()
        {
            for (int i = 1; i < _layers.Length; i++)
            {
                _layers[i].think();
            }
        }

        public double[] eval(double[] input)
        {
            if (input.Length != _inputs)
            {
                throw new ArgumentException();
            }
            _layers[0].setSignals(input);
            think();
            return _layers[_lastLayer].getSignals();
        }

        public double f(double x)
        {
            return Neuron.f(x);
        }

        public double d(double x)
        {
            return Neuron.d(x);
        }

        public double w(int layer, int neuron, int inputNeuron)
        {
            return _layers[layer].w(neuron, inputNeuron);
        }

        public double y(int layer, int neuron)
        {
            return _layers[layer].y(neuron);
        }

        public double s(int layer, int neuron)
        {
            return _layers[layer].s(neuron);
        }

        public double x(int layer, int neuron)
        {
            return _layers[layer - 1].y(neuron);
        }

        public double eps(int layer, int neuron)
        {
            if (layer == _lastLayer)
            {
                return _err[neuron - 1];
            }
            else
            {
                double sum = 0.0;
                int i = neuron;
                int kp1 = layer + 1;
                int Nkp1 = _layers[kp1]._neurons.Length;
                for (int m = 1; m < Nkp1; m++)
                {
                    sum += sig(kp1, m) * w(kp1, m, i);
                }
                return sum;
            }
        }

        public double sig(int layer, int neuron)
        {
            return Neuron.d(s(layer, neuron)) * eps(layer, neuron);
        }

        public void setWeight(int layer, int neuron, int inputNeuron, double w)
        {
            _layers[layer].setWeight(neuron, inputNeuron, w);
        }

        public void teachMulti(double[] input, double[] modelOutput)
        {
            double error = 0.0;
            do
            {
                error = 0.0;
                foreach (double e in _err)
                {
                    error += e * e;
                }
                //Console.WriteLine("error = " + error);
                teach(input, modelOutput);
            }
            while (error > 0.001);
        }

        public void teach(double[] input, double[] modelOutput)
        {
            double[] output = eval(input);
            for (int i = 0; i < output.Length; i++)
            {
                _err[i] = output[i] - modelOutput[i];
                _err[i] = _err[i] * _err[i];
            }
            for (int k = 0; k < _layers.Length; k++)
            {
                for (int i = 0; i < _layers[k]._neurons.Length; i++)
                {
                    for (int j = 0; j < _layers[k]._neurons[i]._weights.Length; j++)
                    {
                        double newWeight = w(k, i, j) + 2 * mi * sig(k, i) * x(k, j);
                        setWeight(k, i, j, newWeight);
                    }
                }
            }
        }

        private double errorFunc(double[][] inputs, double[][] modelOutputs)
        {
            double error = 0.0;
            for (int j = 0; j < inputs.Length; j++)
            {
                double[] o = eval(inputs[j]);
                for (int i = 0; i < o.Length; i++)
                {
                    error += (o[i] - modelOutputs[j][i]) * (o[i] - modelOutputs[j][i]);
                }
            }
            return error;
        }

        private void randomizeWeights()
        {
            double[] w = getWeights();
            for (int i = 0; i < w.Length; i++)
            {
                w[i] = Network.rand.NextDouble() * 2 - 1;
            }
            setWeights(w);
        }

        public void fakeTeach(int[][] inputs, int[] outputs)
        {
            
        }

        public void bruteForceTeach(double[][] inputs, double[][] modelOutputs)
        {
            double y = 1.0;
            int iters = 0;
            while (y > 0.0001 && iters < 10000)
            {
                iters += 1;
                double k_start = Network.rand.NextDouble() * 10;
                double k = k_start;
                randomizeWeights();
                double[] weights = getWeights();
                y = errorFunc(inputs, modelOutputs);
                while (k > 0.00001)
                    for (int i = 0; i < weights.Length; i++)
                    {
                        double w_back = weights[i];
                        double y_back = y;

                        weights[i] = w_back + k;
                        y = errorFunc(inputs, modelOutputs);
                        if (y < y_back)
                        {
                            k = k_start;
                            continue;
                        }

                        weights[i] = w_back - k;
                        y = errorFunc(inputs, modelOutputs);
                        if (y < y_back)
                        {
                            k = k_start;
                            continue;
                        }

                        k = k * 0.9;
                        weights[i] = w_back;
                        y = y_back;
                    }
            }
        }

        public double[] getWeights()
        {
            double[] weights = new double[0];
            for (int i = 1; i < _layers.Length; i++)
            {
                for (int j = 1; j < _layers[i]._neurons.Length; j++)
                {
                    for (int k = 0; k < _layers[i]._neurons[j]._inputs.Length; k++)
                    {
                        weights = extend(weights, _layers[i]._neurons[j]._weights[k]);
                    }
                }
            }
            return weights;
        }

        public void setWeights(double[] w)
        {
            int counter = 0;
            for (int i = 1; i < _layers.Length; i++)
            {
                for (int j = 1; j < _layers[i]._neurons.Length; j++)
                {
                    for (int k = 0; k < _layers[i]._neurons[j]._inputs.Length; k++)
                    {
                        setWeight(i, j, k, w[counter]);
                        counter += 1;
                    }
                }
            }
        }

        double[] extend(double[] array, double x)
        {
            double[] newArray = new double[array.Length + 1];
            for (int i = 0; i < array.Length; i++)
            {
                newArray[i] = array[i];
            }
            newArray[array.Length] = x;
            return newArray;
        }

    }
}
