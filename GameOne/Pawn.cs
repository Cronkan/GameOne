using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GameOne.Annotations;

namespace GameOne
{
    public class Pawn : INotifyPropertyChanged
    {
        private string color;
        private int _col;
        private int _row;
        private int _player;
        private int _health;

        public int health
        {
            get { return _health; }
            set
            {
                _health = value;
                OnPropertyChanged("health");
            }
        }

        public int col
        {
            get { return _col; }
            set
            {
                _col = value;
                OnPropertyChanged("col");
            }
        }

        public int row
        {
            get { return _row; }
            set
            {
                _row = value;
                OnPropertyChanged("row");
            }
        }

        public int player
        {
            get { return _player; }
            set
            {
                _player = value;
                OnPropertyChanged("player");
            }
        }

        public string Color
        {
            get { return color; }
            set
            {
                color = value;
                OnPropertyChanged("Color");
            }
        }       

        public Pawn(int player, string color, int health, int col, int row)
        {
            this.Color = color;
            this.health = health;
            this.col = col;
            this.row = row;
            this.player = player;

        }

   

        public void increaseHealth()
        {
            this.health++;
        }

     
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void resetColor()
        {
            Color = player == 1 ? "Pink" : "Teal";
        }
    }
}
