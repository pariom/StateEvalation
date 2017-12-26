﻿using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StateEvaluation.BioColor
{
    class Function
    {
        static int mid = Settings.Default.mid,
               square = Settings.Default.square,
               topline = Settings.Default.topline,
               step = Settings.Default.step,
               range = 2;
        static double alpha = Settings.Default.alpha,
               screen_W = SystemParameters.PrimaryScreenWidth,
               screen_H = SystemParameters.PrimaryScreenHeight;
        internal static Color ColorMix(Color c1, Color c2, Color c3)
        {
            const int DIVIDER_3 = 4;
            const int DIVIDER_2 = 4;
            const int SPLACER = 128;

            if (c1.A > SPLACER && c2.A > SPLACER && c3.A > SPLACER)
            {
                return Color.FromArgb(
                    (byte)((c1.R + c2.R + c3.R) / DIVIDER_3),
                    (byte)((c1.G + c2.G + c3.G) / DIVIDER_3),
                    (byte)((c1.B + c2.B + c3.B) / DIVIDER_3)
                );
            }
            else if (c1.A > SPLACER && c2.A > SPLACER ||
                    c2.A > SPLACER && c3.A > SPLACER ||
                    c3.A > SPLACER && c1.A > SPLACER)
            {
                return Color.FromArgb(
                    (byte)((c1.R + c2.R + c3.R) / DIVIDER_2),
                    (byte)((c1.G + c2.G + c3.G) / DIVIDER_2),
                    (byte)((c1.B + c2.B + c3.B) / DIVIDER_2)
                );
            }
            else
            {
                if (c1.A > SPLACER) return c1;
                else if (c2.A > SPLACER) return c2;
                else if (c3.A > SPLACER) return c3;
                else return Color.FromArgb(0, 0, 0, 0);
            }
        }


        static public void DrawClear(Grid myGrid)
        {
            // myGrid.Children.Clear();
            /*
            System.Windows.Shapes.Rectangle rectangle = new System.Windows.Shapes.Rectangle
            {
                Fill = System.Windows.Media.Brushes.White,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            Thickness margin = rectangle.Margin;
            margin.Top = topline;
            rectangle.Margin = margin;
            rectangle.Width = screen_W;
            rectangle.Height = screen_H;
            myGrid.Children.Add(rectangle);
            */
            Line myLine = new Line
            {
                Stroke = System.Windows.Media.Brushes.Black,
                X1 = mid,
                X2 = mid,
                Y1 = topline,
                Y2 = screen_H,
                StrokeThickness = 1
            };
            myGrid.Children.Add(myLine);

        }
   
        public static void GetCanvasImage(String a, int x, int Delta, Grid myGrid)
        {
            try
            {
                BitmapImage theImage = new BitmapImage();

                theImage.BeginInit();
                theImage.CacheOption = BitmapCacheOption.OnLoad;
                theImage.UriSource = new Uri(a, UriKind.Relative);
                theImage.EndInit();

                System.Windows.Media.ImageBrush myImageBrush = new System.Windows.Media.ImageBrush(theImage);

                for (int i = -range; i <= range; ++i)
                {
                    myGrid.Children.Add( new Canvas
                    {
                        Width = theImage.Width,
                        Height = theImage.Height,
                        Background = myImageBrush,
                        Opacity = alpha,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(i * square * x - Delta % x * square + mid, 0, 0, 0)
                    });
                }
            }

            catch (System.IO.FileNotFoundException)
            {
                Main.Generate();
                GetCanvasImage(a, x, Delta, myGrid);
            }
            finally
            {
                
            }
        }
        
    }
}
