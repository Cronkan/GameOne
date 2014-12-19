using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using GameOne.Annotations;

namespace GameOne
{
    public class Pawn : INotifyPropertyChanged
    {
        private int _col;
        private int _health;
        private int _player;
        private int _row;
        private string color;

        public Pawn(int player, string color, int health, int col, int row)
        {
            Color = color;
            this.health = health;
            this.col = col;
            this.row = row;
            this.player = player;
        }

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

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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


        public event PropertyChangedEventHandler PropertyChanged;

        public void increaseHealth()
        {
            health++;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void resetColor()
        {
            Color = player == 1 ? "Pink" : "Teal";
        }
    }
}