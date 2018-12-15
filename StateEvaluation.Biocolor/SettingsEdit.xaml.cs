﻿using System;
using System.Windows;

namespace StateEvaluation.BioColor
{
    /// <summary>
    /// Interaction logic for SetingsEdit.xaml
    /// </summary>
    public partial class SettingsEdit : Window
    {
        private ImageGenerator imageGenerator;

        public SettingsEdit()
        {
            InitializeComponent();
            imageGenerator = new ImageGenerator();
        }
        private string FilePicker()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); ;
            dlg.Filter = "PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                return dlg.FileName;
            }
            else
            {
                throw new ArgumentNullException("File was NOT selected!");
            }
        }

        private void Br_click(object sender, RoutedEventArgs e)
        {
            try
            {
                pr.Text = FilePicker();
            }
            catch
            {

            }
        }
        private void Bg_click(object sender, RoutedEventArgs e)
        {
            try
            {
                pg.Text = FilePicker();
            }
            catch
            {

            }
        }
        private void Bb_click(object sender, RoutedEventArgs e)
        {
            try
            {
                pb.Text = FilePicker();
            }
            catch
            {

            }
        }
        private void Open_color(object sender, RoutedEventArgs e)
        {
            Colors c = new Colors();
            c.ShowDialog();
            c.Save();

            imageGenerator.Generate(23);
            imageGenerator.Generate(28);
            imageGenerator.Generate(33);
        }
    }
}