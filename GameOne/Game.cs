using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Xml.Serialization;
using GameOne.Annotations;

namespace GameOne
{
    public class Game : INotifyPropertyChanged
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GameId { get; set; }
        public int currentMoves
        {
            get { return _currentMoves; }
            set
            {
                _currentMoves = value;
                OnPropertyChanged("currentMoves");
            }
        }
    
        public Pawn pawnToSplit { get; set; }

        private Random rand;
        [NotMapped]
        public Pawn currentPawn
        {
            get { return _currentPawn; }
            set
            {
                _currentPawn = value;
                OnPropertyChanged("currentPawn");
            }
        }

        public int currentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                _currentPlayer = value;
                OnPropertyChanged("currentPlayer");
            }
        }

        private int _currentRollDice;
        private int _currentPlayer;
        private int _currentMoves;
        private Pawn _currentPawn;
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
            currentPlayer = 1;
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
            currentMoves = currentRollDice;
        }

        public void SplitPawn(int col, int row)
        {
            currentMoves = 0;
            pawnToSplit.health = pawnToSplit.health / 2;
            Pawn newPawn = new Pawn(currentPlayer, "Blue", pawnToSplit.health, col, row);
            newPawn.resetColor();
            Add(newPawn);
            pawnToSplit = null;
            CheckMoves();
        }

        public void AttackPawn(Pawn enemy)
        {
            Debug.WriteLine("" + currentPawn.player + " " + currentPlayer + " " + currentPawn.player + " " + currentPlayer);
            if (currentPawn.player == currentPlayer && currentPawn.player != enemy.player &&
                checkDirection(enemy.col, enemy.row))
            {
                enemy.health -= 1;
                currentMoves -= 1;
            }

            if (enemy.health <= 0)
            {
                RemovePawn(enemy);
            }
            CheckMoves();


            //SaveState();
        }
        public void setCurrentPawn(Pawn thePawn)
        {
            if (thePawn.player == currentPlayer)
            {

                if (currentPawn != null)
                {
                    currentPawn.resetColor();
                }


                currentPawn = thePawn;
                currentPawn.Color = "Blue";

            }
        }
        private void CheckMoves()
        {
            if (currentMoves <= 0)
            {
                currentPlayer = currentPlayer != 1 ? 1 : 2;
                if (currentPawn != null)
                {
                    currentPawn.resetColor();
                    currentPawn = null;
                }

                RollDice();
            }
        }

        private bool checkDirection(int? col, int? row)
        {
            Debug.WriteLine("" + col + row + currentPawn.col + currentPawn.row);
            Debug.WriteLine((currentPawn.col == col + 1 && currentPawn.row == row));
            Debug.WriteLine((currentPawn.col == col - 1 && currentPawn.row == row));
            Debug.WriteLine((currentPawn.col == col + 1 && currentPawn.row == row + 1));
            Debug.WriteLine((currentPawn.col == col + 1 && currentPawn.row == row - 1));
            Debug.WriteLine((currentPawn.col == col - 1 && currentPawn.row == row - 1));
            Debug.WriteLine((currentPawn.col == col - 1 && currentPawn.row == row + 1));
            //MessageBox.Show("" + currentPawn.col + " " + col + " " +currentPawn.row +" "+ row );
            if (
                
                   (currentPawn.col == col - 1 && currentPawn.row == row)
                || (currentPawn.col == col && currentPawn.row == row)
                || (currentPawn.col == col && currentPawn.row == row + 1 )
                || (currentPawn.col == col && currentPawn.row == row - 1)
                || (currentPawn.col == col - 1 && currentPawn.row == row + 1)
                || (currentPawn.col == col - 1 && currentPawn.row == row - 1)
                || (currentPawn.col == col + 1 && currentPawn.row == row + 1)
                || (currentPawn.col == col + 1 && currentPawn.row == row - 1)
                || (currentPawn.col == col + 1 && currentPawn.row == row)
             

                )
            {
                return true;
            }

            return false;
        }

      


        public void UpdatePawns()
        {

        }

        public void SaveState()
        {
           

            //DataHandler.exportToJson(this,"game.json");

        }



        public void MovePawn(int Column, int Row)
        {
            /*
             * TODO: Add better movement
            MessageBox.Show("" + (currentPawn.row -Row) + " " + (currentPawn.col - Column));
            if (Row > currentPawn.row && Column < currentPawn.col && (Row - currentPawn.row) <= currentMoves)
            {
                currentMoves -=  Row - currentPawn.row;
                currentPawn.row = Row;
                currentPawn.col = Column;
            }
            else if (Row < currentPawn.row && Column < currentPawn.col && (currentPawn.col - Column) <= currentMoves)
            {
                currentMoves -= currentPawn.col - Column;
                currentPawn.row = Row;
                currentPawn.col = Column;
            }
            else if (Row > currentPawn.row && Column > currentPawn.col && ( currentPawn.row - Row) <= currentMoves)
            {
                currentMoves -= currentPawn.row - Row;
                currentPawn.row = Row;
                currentPawn.col = Column;
            }
                else if (Row < currentPawn.row && Column > currentPawn.col && (currentPawn.col - Column) <= currentMoves)
            {
                currentMoves -= Row - currentPawn.row;
                currentPawn.row = Row;
                currentPawn.col = Column;
            }
          
            
            else{
                    */
            if (Column < currentPawn.col && currentPawn.col - Column <= currentMoves)
            {
                currentMoves -= currentPawn.col - Column;
                currentPawn.col = Column;
            }
            else if (Column > currentPawn.col && Column - currentPawn.col <= currentMoves)
            {
                currentMoves -= Column - currentPawn.col;
                currentPawn.col = Column;
            }
            if (Row < currentPawn.row && currentPawn.row - Row <= currentMoves)
            {
                currentMoves -= currentPawn.row - Row;
                currentPawn.row = Row;
            }
            else if (Row > currentPawn.row && Row - currentPawn.row <= currentMoves)
            {
                currentMoves -= Row - currentPawn.row;
                currentPawn.row = Row;
            }
            CheckMoves();
        }

        public void setPawnToSplit(Pawn thePawn)
        {
            if (thePawn.player == currentPlayer)
            {
                pawnToSplit = thePawn;
            }

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
