using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace sciecNeuronowa
{
    class ImageToVect
    {
        Bitmap imgToConvert = null;
        public const double BLACK = 0.5;

        public void loadImage(Bitmap image)
        {
            imgToConvert = image;
        }

        public int[] getVect()
        {
            int[] vector = new int[imgToConvert.Size.Width * imgToConvert.Size.Height];

            if (imgToConvert != null)
            {
                int i = 0;
                for (int y = 0; y < imgToConvert.Size.Height; y++)
                {
                    for (int x = 0; x < imgToConvert.Size.Width; x++)
                    {
                        float c = imgToConvert.GetPixel(x, y).GetBrightness();
                        if (c > BLACK)
                        {
                            vector[i++] = 1;
                        }
                        else
                        {
                            vector[i++] = 0;
                        }
                    }
                }
            }
            else
            {
                throw new Exception("Zaladuj wczesniej obrazek");
            }

            return vector;
        }

    }
}
