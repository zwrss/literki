using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace sciecNeuronowa
{
    class CharFinder
    {
        List<Point> globalVisitedPixels;
        Bitmap img;
        Graphics g;
        int foundLettersCount = 0;

        public CharFinder(Bitmap imgToParse)
        {
            globalVisitedPixels = new List<Point>();

            img = imgToParse;
            g = Graphics.FromImage(imgToParse);
        }

        public void search()
        {
            for (int x = 0; x < img.Size.Width; x++)
            {
                for (int y = 0; y < img.Size.Height; y++) 
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

                            g.DrawRectangle(Pens.Red, min_x, min_y, (max_x - min_x), (max_y - min_y));
                            //g.FillRectangle(Brushes.Red, new Rectangle(new Point(min_x, min_y), new Size(max_x - min_x, max_y - min_y)));
                            localBlackPoints.Clear();
                            //MessageBox.Show("min_x: " + min_x + " min_y " + min_y + " max_x " + max_x + " height " + max_y );
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

                if (jasnosc < 0.2)
                {
                    localBlackPoints.Add(point);
            

                    if (point.X > 0 
                        && point.Y > 0 
                        && point.X < img.Size.Width 
                        && point.Y < img.Size.Height
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
    }
}
