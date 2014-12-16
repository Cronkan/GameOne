using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOne
{
    public class Pawn
    {
        public string color { get; set; }
        public int health { get; set; }
        public int x { get; set; }
        public int y { get; set; }


        public Pawn(string color,int health, int x, int y)
        {
            this.color = color;
            this.health = health;
            this.x = x;
            this.y = y;

        }

        public void increaseHealth()
        {
            this.health++;
        }


    }
}
