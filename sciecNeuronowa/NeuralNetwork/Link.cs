
namespace pl.edu.pk.NeuralNetwork
{
    public class Link
    {
        double weight, signal;

        Perceptron input, output;

        public Link(double weight, Perceptron input, Perceptron output)
        {
            this.input = input;
            this.output = output;
            this.weight = weight;
            this.signal = 0;
        }

        public Perceptron getInput()
        {
            return this.input;
        }

        public Perceptron getOutput()
        {
            return this.output;
        }

        public void setWeight(double w)
        {
            this.weight = w;
        }

        public void setSignal(double s)
        {
            this.signal = s;
        }

        public double getWeight()
        {
            return this.weight;
        }

        public double getSignal()
        {
            return this.weight * this.signal;
        }

        public double getRawSignal()
        {
            return this.signal;
        }

    }
}
