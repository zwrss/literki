using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using pl.edu.pk.NeuralNetwork;
using System.Diagnostics;

namespace sciecNeuronowa
{
    static class Program
    {
        public static Random random = new Random();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //NNTest();
        }

        static void Test()
        {
         //   Network n = new Network(2, 1, 1);
         //   double[] y = n.eval(new double[] { 1.0, 1.0 });
         //   System.Console.WriteLine("Output = " + y[0]);
            //n.teachMulti(new double[] { -1.0, 1.0 }, new double[] { -1.0 });
            /*for (int i = 0; i < 100000; i++)
            for (int i = 0; i < 10000; i++)
            {
                n.teach(new double[] { -1.0, -1.0 }, new double[] { -1.0 });
                n.teach(new double[] { -1.0, 1.0 }, new double[] { -1.0 });
                n.teach(new double[] { 1.0, -1.0 }, new double[] { -1.0 });
                n.teach(new double[] { 1.0, 1.0 }, new double[] { 1.0 });
            }*/
            /*n.setWeights(new double[] {
                0.098594092710998,
                0.142027248431928,
                0.168251298491528,
                -0.303751077743045,
                0.317479775149435,
                0.316428999146291,
                -0.282436690577179,
                0.251041846015736,
                0.892922405285977,
                0.203223224556291,
                1.555737942719387
            });*/
            /*{
                n.bruteForceTeach(
                    new double[][] {
                        new double[] { -1, -1 }, 
                        new double[] { -1, 1 },
                        new double[] { 1, -1 },
                        new double[] { 1, 1 }
                    },
                    new double[][] {
                        new double[] { -1 },
                        new double[] { 1 },
                        new double[] { 1 },
                        new double[] { -1 }
                    });
            }
            y = n.eval(new double[] { -1.0, -1.0 });
            System.Console.WriteLine("-1 -1 = " + y[0]);
            y = n.eval(new double[] { -1.0, 1.0 });
            System.Console.WriteLine("-1 1 = " + y[0]);
            y = n.eval(new double[] { 1.0, -1.0 });
            System.Console.WriteLine("1 -1 = " + y[0]);
            y = n.eval(new double[] { 1.0, 1.0 });
            System.Console.WriteLine("1 1 = " + y[0]);
            System.Console.WriteLine("");
            foreach (double d in n.getWeights())
            {
                System.Console.WriteLine(d);
            }*/
        }
    }
}
