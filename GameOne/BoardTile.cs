﻿using System;
using System.ComponentModel;
using System.Windows.Media;

namespace GameOne
{
    public class BoardTile : INotifyPropertyChanged
    {
        public BoardTile(int row, int col)
        {
            this.row = row;
            this.col = col;

            color = setColor();
        }

        public int row { get; set; }

        public int col { get; set; }

        public Brush color { get; set; }
        public String coords { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private Brush setColor()
        {
            return ((row + col)%2 == 0)
                ? new SolidColorBrush(Color.FromArgb(255, 165, 245, 255))
                : new SolidColorBrush(Color.FromArgb(255, 183, 165, 255));
        }


        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}