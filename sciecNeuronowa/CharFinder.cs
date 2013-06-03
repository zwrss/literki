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
        // szerokosc przeskalowanej litery
        private const int SCALED_IMAGE_WIDTH = 12;
        // wysokosc przeskalowanej litery
        private const int SCALED_IMAGE_HEIGHT = 12;
        // krok x, aby nie sprawdzac pixeli przy wyszukiwaniu co jeden
        // mozemy ustawic krok co ile ma przeskakiwac sprawdzanie.
        // Im wiecej tym program bedzie szybciej dzialac, 
        // ale wykrywanie bedzie dzialalo gorzej.
        private const int X_STEP = 5;
        private const int Y_STEP = 5;
        // minimalna szerokosc litery
        // ustawiamy w zaleznosci od wielkosci danych
        // pozwala na unikniecie uznacia kropki badz jakis smieci
        // za litere
        private const int MIN_FOUND_LETTER_WIDTH = 15;
        // minimalna szerokosc
        private const int MIN_FOUND_LETTER_HEIGHT = 15;

        private List<Point> globalVisitedPixels;
        private Bitmap img;
        private Graphics g;
        int foundLettersCount = 0;
        private List<int[]> vectors = new List<int[]>();


        /*
         * Konstruktor, jako argument podajemy obrazek
         * na ktorym wykonujemy ooperacje
        */
        public CharFinder(Bitmap imgToParse)
        {
            // inicjujemy liste odwiedzonych punktow
            // ma ona za zadanie zapobiegac sprawdzaniu 
            // kilka razy tego samego pixela
            globalVisitedPixels = new List<Point>();

            // przypisujemy obrazek 
            img = imgToParse;
            // operacje na obrazku
            g = Graphics.FromImage(imgToParse);
        }

        /**
         * Funkcja wyszukuje litery na obrazku. 
         * Dziala ona w magiczny sposob i jej dzialanie jest 
         * znane tylko jej autorowi wiec lepiej nie proujcie 
         * jej zrozumiec.
        */
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

                            // jak wejdziemy do tego ifa to jestesmy szczesliwi
                            // bo znalezlismy literke i mozemy juz bic brawo
                            if (min_x != max_x  // nie wiem czemu czasami szerokosc lub wysokosc =0
                                && max_y!= min_y
                                && Math.Abs(min_x-max_x) > MIN_FOUND_LETTER_WIDTH // zabezpieczamy sie przed jakimis malymi smieciami
                                && Math.Abs(max_y - min_y) > MIN_FOUND_LETTER_HEIGHT
                                )
                            {
                                Rectangle srcRect = new Rectangle(min_x, min_y, (max_x - min_x), (max_y - min_y));
                                Bitmap letter = (Bitmap)img.Clone(srcRect, img.PixelFormat);

                                Bitmap result = new Bitmap(SCALED_IMAGE_WIDTH, SCALED_IMAGE_HEIGHT);
                                result.SetResolution(letter.HorizontalResolution, letter.VerticalResolution);

                                // skalowanie znalezionej litery to spojnego rozmiaru
                                using (Graphics graphics = Graphics.FromImage(result))
                                {
                                    //set the resize quality modes to high quality
                                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                    //draw the image into the target bitmap
                                    graphics.DrawImage(letter, 0, 0, result.Width, result.Height);
                                }
                                
                                result.Save("tmp " + foundLettersCount + ".bmp",ImageFormat.Bmp);

                                // zamian obrazka na vektor 0/1
                                ImageToVect imageToVect = new ImageToVect();
                                imageToVect.loadImage(result);
                                vectors.Add(imageToVect.getVect());

                                // oznaczamy wykryte litery na obrazku
                                g.DrawRectangle(Pens.Red, min_x, min_y, (max_x - min_x), (max_y - min_y));
                            }
                            
                        }
                    }
                }
            }
            //MessageBox.Show("GlobalVisitedPixels " + globalVisitedPixels.Count + " foundLettersCount" + foundLettersCount);
            g.Dispose();
        }

        /**
         * Funkcja sprawdzajaca czy w liscie pixeli znajduje sie dany pixel 
         */
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

        /*
         * Funkcja sprawdza najblizsze otoczenie pixela
         */
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
                            // nalezaloby sie zastanowic czy sprawdzanie po skosie jest 
                            // na pewno potrzebne
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

        /*
         * zwracamy wektory znalezionych liter 
         */
        public List<int[]> getVectors()
        {
            return vectors;
        }
    }
}
