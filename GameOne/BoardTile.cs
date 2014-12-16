using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOne
{
    public class BoardTile : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private String setColor()
        {
            return ((PosX + PosY) % 2 == 0) ? "DarkRed" : "Tan";
        }


        public BoardTile(int posX, int posY)
        {
            this.PosX = posX;
            this.PosY = posY;
            this.Color = setColor();
            this.Coords = posX.ToString() + "," + posY.ToString();
        }

        String Name;
        String Color;
        String Coords;
        int PosX;
        int PosY;

        public String name { get { return this.Name; } set { this.Name = value; } }
        public String color { get { return this.Color; } }
        public String coords { get { return this.Coords; } }

        public void setName(String n)
        {
            this.Name = n;
            OnPropertyChanged("name");
        }

        public String getName()
        {
            return this.Name;
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
