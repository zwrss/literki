using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace sciecNeuronowa
{
    class CharFinder
    {
        private List<Point> globalVisitedPixels;
        private Bitmap img;
        private Graphics g;
        int foundLettersCount = 0;
        private List<int[]> vectors = new List<int[]>();
        private const int SCALED_IMAGE_WIDTH = 12;
        private const int SCALED_IMAGE_HEIGHT = 12;
        private const int X_STEP = 5;
        private const int Y_STEP = 5;
        private const int MIN_FOUND_LETTER_WIDTH = 15;

        public CharFinder(Bitmap imgToParse)
        {
            globalVisitedPixels = new List<Point>();

            img = imgToParse;
            g = Graphics.FromImage(imgToParse);
        }

        public void search()
        {
            for (int x = 0; x < (img.Size.Width - (X_STEP+2)); x += X_STEP)
            {
                for (int y = 0; y < (img.Size.Height - (Y_STEP+2)); y += Y_STEP)
                {
                    

                    Point pointToCheck = new Point(x,y);
                    if(!isListContainsPixel(pointToCheck, globalVisitedPixels))
                    {
                        List<Point> localBlackPoints = new List<Point>();
                        checkPixels(pointToCheck, localBlackPoints);
                    
                    
                    // wyszukiwanie rogów
                        if (localBlackPoints.Count() > 0)
                        {
                            
                            foundLettersCount++;
                            int min_x = 999999;
                            int min_y = 999999;
                            int max_x = 0;
                            int max_y = 0;

                            foreach (var element in localBlackPoints)
                            {
                                if (element.X < min_x)
                                {
                                    min_x = element.X;
                                }

                                if (element.X > max_x)
                                {
                                    max_x = element.X;
                                }

                                if (element.Y < min_y)
                                {
                                    min_y = element.Y;
                                }

                                if (element.Y > max_y)
                                {
                                    max_y = element.Y;
                                }
                            }

                            
                            if (min_x != max_x  // nie wiem czemu czasami szerokosc lub wysokosc =0
                                && max_y!= min_y
                                && Math.Abs(min_x-max_x) > MIN_FOUND_LETTER_WIDTH // zabezpieczamy sie przed jakimis malymi smieciami
                                )
                            {
                                Rectangle srcRect = new Rectangle(min_x, min_y, (max_x - min_x), (max_y - min_y));
                                Bitmap letter = (Bitmap)img.Clone(srcRect, img.PixelFormat);

                                Bitmap result = new Bitmap(SCALED_IMAGE_WIDTH, SCALED_IMAGE_HEIGHT);
                                result.SetResolution(letter.HorizontalResolution, letter.VerticalResolution);

                                using (Graphics graphics = Graphics.FromImage(result))
                                {
                                    //set the resize quality modes to high quality
                                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                    //draw the image into the target bitmap
                                    graphics.DrawImage(letter, 0, 0, result.Width, result.Height);
                                }
                                
                                result.Save("dupa " + foundLettersCount + ".bmp",ImageFormat.Bmp);

                                ImageToVect imageToVect = new ImageToVect();
                                imageToVect.loadImage(result);
                                vectors.Add(imageToVect.getVect());
                                g.DrawRectangle(Pens.Red, min_x, min_y, (max_x - min_x), (max_y - min_y));
                            }
                            
                        }
                    }
                }
            }
            //MessageBox.Show("GlobalVisitedPixels " + globalVisitedPixels.Count + " foundLettersCount" + foundLettersCount);
            g.Dispose();
        }

        private bool isListContainsPixel(Point point, List<Point> list)
        {
            foreach (var element in list)
            {
                if (element.X == point.X && element.Y == point.Y)
                {
                    return true;
                }
            }
            return false;
        }

        private void checkPixels(Point point, List<Point> localBlackPoints) 
        {
            if(!isListContainsPixel(point,globalVisitedPixels))
            {
                globalVisitedPixels.Add(point);
        
                    Color pixelColor = img.GetPixel(point.X, point.Y);
                    float jasnosc = pixelColor.GetBrightness();

                    if (jasnosc > 0.2)
                    {
                        localBlackPoints.Add(point);


                        if (point.X > 0
                            && point.Y > 0
                            && point.X < img.Size.Width-1
                            && point.Y < img.Size.Height-1
                            //&& !isListContainsPixel(point, globalVisitedPixels)
                            )
                        {

                            checkPixels(new Point(point.X - 1, point.Y - 1), localBlackPoints);
                            checkPixels(new Point(point.X, point.Y - 1), localBlackPoints);
                            checkPixels(new Point(point.X + 1, point.Y - 1), localBlackPoints);
                            checkPixels(new Point(point.X + 1, point.Y), localBlackPoints);
                            checkPixels(new Point(point.X + 1, point.Y + 1), localBlackPoints);
                            checkPixels(new Point(point.X, point.Y + 1), localBlackPoints);
                            checkPixels(new Point(point.X - 1, point.Y + 1), localBlackPoints);
                            checkPixels(new Point(point.X - 1, point.Y), localBlackPoints);
                        }
                    }
      
                     
            }

        }

        public List<int[]> getVectors()
        {
            return vectors;
        }
    }
}
