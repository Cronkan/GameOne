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
        public bool gameOver;

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
            gameOver = false;


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
            AddPawns();
            gameOver = false;
            StartGame();
        }
        public void AddPawns()
        {
            grid.Add(new Pawn(1, "Pink", 10, 4, 2));
            grid.Add(new Pawn(1, "Pink", 10, 4, 3));
            grid.Add(new Pawn(1, "Pink", 10, 4, 4));
            grid.Add(new Pawn(1, "Pink", 10, 4, 5));
            grid.Add(new Pawn(1, "Pink", 10, 4, 6));
            grid.Add(new Pawn(2, "Teal", 10, 5, 2));
            grid.Add(new Pawn(2, "Teal", 10, 5, 3));
            grid.Add(new Pawn(2, "Teal", 10, 5, 4));
            grid.Add(new Pawn(2, "Teal", 10, 5, 5));
            grid.Add(new Pawn(2, "Teal", 10, 5, 6));


        }
        public void EndGame()
        {
            gameOver = true;
            MessageBox.Show("Player " + currentPlayer + " Won! Starting new Game");
             if (currentPawn != null)
             {
                 currentPawn = null;
             }
            var removeList = (from gridItem in grid
                               where gridItem.GetType() == typeof(Pawn)
                               select gridItem).ToList();
            foreach (var pawn in removeList)
            {
                grid.Remove(pawn);
            }
           
            NewGame();
            
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
            //Debug.WriteLine("" + currentPawn.player + " " + currentPlayer + " " + currentPawn.player + " " + currentPlayer);
            if (currentPawn == null || enemy == null)
            {
                return;
            }
            Debug.WriteLine("Got "+ currentMoves + " enemy has: " + enemy.health);
            if (currentMoves > 0 &&currentPawn.player == currentPlayer && currentPawn.player != enemy.player &&
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
            CheckGameOver();
            


            //SaveState();
        }

        private void CheckGameOver()
        {
            var checkPlayer1 = from gridItem in grid
                             where gridItem.GetType() == typeof(Pawn) &&
                                   ((Pawn)gridItem).player == 1
                             select gridItem;
            var checkPlayer2 = from gridItem in grid
                               where gridItem.GetType() == typeof(Pawn) &&
                                     ((Pawn)gridItem).player == 2
                               select gridItem;
            if (!checkPlayer1.Any() || !checkPlayer2.Any())
            {
                EndGame();
            }
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
                Debug.WriteLine("Set current pawn");

            }
        }
        private void CheckMoves()
        {
            
            if (currentMoves <= 0)
            {
                ChangePlayer();
                if (currentPawn != null)
                {
                    currentPawn.resetColor();
                    currentPawn = null;
                }

                RollDice();
            }
        }

        public void ChangePlayer()
        {
            currentPlayer = currentPlayer != 1 ? 1 : 2;
        }

        private bool checkDirection(int col, int row)
        {

            //MessageBox.Show("" + currentPawn.col + " " + col + " " +currentPawn.row +" "+ row );
            if (

                   (currentPawn.col == col - 1 && currentPawn.row == row)
                || (currentPawn.col == col && currentPawn.row == row)
                || (currentPawn.col == col && currentPawn.row == row + 1)
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
            Debug.WriteLine("Got " + currentMoves);
            if (currentPawn == null || currentMoves <= 0 || Column - 1 < 0 || Row - 1 < 0 || Column + 1 > MainWindow.boardSize - 1 || Row + 1 > MainWindow.boardSize - 1)
            {
                return;

            }
            foreach (var o in grid)
            {
                if (o.GetType() == typeof(Pawn))
                {
                    // Debug.WriteLine(((Pawn)o).col + " " + Column + " " + ((Pawn)o).row + " " + Row);
                    if (((Pawn)o).col == Column && ((Pawn)o).row == Row)
                    {
                        Debug.WriteLine(((Pawn)o).col == Column && ((Pawn)o).row == Row);

                        return;
                    }
                }

            }

          
            var savedCol = currentPawn.col;
            var savedRow = currentPawn.row;
            var savedMoves = currentMoves;
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
            var checkTwo = from pawn in grid
                where
                    pawn.GetType() == typeof (Pawn) && ((Pawn) pawn).col == currentPawn.col &&
                    ((Pawn) pawn).row == currentPawn.row
                select pawn;
            if (checkTwo.Count() != 1)
            {
                currentPawn.col = savedCol;
                currentMoves = savedMoves;
            }

            savedMoves = currentMoves;

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
            var checkTwoAgain = from pawn in grid
                           where
                               pawn.GetType() == typeof(Pawn) && ((Pawn)pawn).col == currentPawn.col &&
                               ((Pawn)pawn).row == currentPawn.row
                           select pawn;
            if (checkTwoAgain.Count() != 1)
            {
                currentPawn.row = savedRow;
                currentMoves = savedMoves;
            }
            Debug.WriteLine("Moved Got " + currentMoves);
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

        public void StartGame()
        {
            RollDice();
            currentPlayer = currentRollDice%2+1;
            MessageBox.Show("Player " + currentPlayer + " Starts");
            
        }
    }
}
