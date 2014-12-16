using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using GameOne.Annotations;

namespace GameOne
{
    public class Game : INotifyPropertyChanged
    {
        private Random rand;
        private int _currentRollDice;
        private static ObservableCollection<object> _grid;
        public ObservableCollection<object> grid
        {
            get { return _grid; }
            set
            {
                _grid = value;
            }
        }
        public int currentRollDice
        {
            get { return _currentRollDice; }
            set
            {
                _currentRollDice = value; 
                OnPropertyChanged("currentRollDice");
            }
        }

        public Game()
        {
          rand = new Random();
     
            currentRollDice = 0;
            grid = new ObservableCollection<object>();
            

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

        public void AttackPawn(Pawn enemy)
        {
            if (MainWindow.currentPawn.player != enemy.player)
            {
                enemy.health -= currentRollDice;
            }
            if (enemy.health <= 0)
            {
                RemovePawn(enemy);
            }
            SaveState();
        }

 

        public void UpdatePawns()
        {
            
        }

        public void SaveState()
        {
            DataHandler.exportToJson(this,"game.json");

        }

      

        public static event NotifyCollectionChangedEventHandler CollectionChanged;
        public void Add(Pawn item)
        {
            grid.Add(item);
            OnCollectionChanged(
               new NotifyCollectionChangedEventArgs(
                 NotifyCollectionChangedAction.Add, item));
        }

        

        public void RemovePawn(Pawn item)
        {
            grid.Remove(item);
            OnCollectionChanged(
              new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Remove, item));
        }
        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            var handler = CollectionChanged;
            if (handler != null)
                handler(this, args);
        }
     
    }
}
