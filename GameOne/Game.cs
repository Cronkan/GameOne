using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GameOne.Annotations;

namespace GameOne
{
    public class Game : INotifyPropertyChanged
    {
        public List<Pawn> pawns { get; set; }
        public String[] gameState { get; set; }
        private Random rand;
        public int currentRollDice { get; set; }

        public Game()
        {
            pawns = new List<Pawn>();
            rand = new Random();
            SaveState();
        
        }
           public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }
        public void NewGame()
        {
            
        }

        public void EndGame()
        {
            
        }

        public void ContinueGame()
        {
            
        }

        public void RollDice()
        {
            currentRollDice = rand.Next(6) + 1;
            
        }

        public void SplitPawn()
        {
            
        }

        public void AttackPawn()
        {
            
        }

        public void RemovePawn()
        {
            
        }

        public void UpdatePawns()
        {
            
        }

        public void SaveState()
        {
            DataHandler.exportToJson(this,"game.json");

        }

        public void ReloadState()
        {
            
        }

     
    }
}
