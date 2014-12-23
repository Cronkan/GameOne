using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using GameOne.Annotations;

namespace GameOne
{
    public class Game : INotifyPropertyChanged
    {
        private static ObservableCollection<object> _grid;
        private readonly Random rand;
        public SaveClass LastSaveClass;
        private int _currentMoves;
        private Pawn _currentPawn;
        private int _currentPlayer;
        private int _currentRollDice;
        public bool gameOver;
        private string _winnerText;
        private Visibility _showWinner;

        public Visibility showWinner
        {
            get { return _showWinner; }
            set
            {
                _showWinner = value;
                OnPropertyChanged("showWinner");
            }
        }

        public string winnerText
        {
            get { return _winnerText; }
            set
            {
                _winnerText = value;
                OnPropertyChanged("winnerText");
            }
        }


        public Game(int boardSize)
        {
            rand = new Random();
            currentPlayer = 1;
            currentRollDice = 0;
            grid = new ObservableCollection<object>();
            gameOver = false;
            this.boardSize = boardSize;
            showWinner = Visibility.Hidden;

        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        public ObservableCollection<object> grid
        {
            get { return _grid; }
            set { _grid = value; }
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

        public int boardSize { get; set; }
        public bool saving { get; set; }

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
            OnPropertyChanged(propertyName);
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
            
            if (currentPawn != null)
            {
                currentPawn = null;
            }
            List<object> removeList = (from gridItem in grid
                where gridItem.GetType() == typeof (Pawn)
                select gridItem).ToList();
            foreach (object pawn in removeList)
            {
                grid.Remove(pawn);
            }

            NewGame();
            SaveState();
        }


        public void RollDice()
        {
            currentRollDice = rand.Next(6) + 1;
            currentMoves = currentRollDice;
        }
        public void MoveOrSplitPawn(int Column, int Row)
        {
            //Validation for pawn-split
            if (pawnToSplit != null)
            {
                SplitPawn(Column,Row);
                CheckGameOver();
            }
            //Validation for valid move
            if (currentPawn == null || currentMoves <= 0 || Column - 1 < 0 || Row - 1 < 0 || Column + 1 > boardSize - 1 ||
                Row + 1 > boardSize - 1)
            {
                return;
            }
            foreach (object o in grid)
            {
                if (o.GetType() == typeof(Pawn))
                {
                    if (((Pawn)o).col == Column && ((Pawn)o).row == Row)
                    {
                        return;
                    }
                }
            }
            //Actually move pawn
            MovePawn(Column,Row);

            CheckMoves();
            CheckGameOver();
        }

        private void MovePawn(int Column, int Row)
        {

            int savedCol = currentPawn.col;
            int savedRow = currentPawn.row;
            int savedMoves = currentMoves;
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
            IEnumerable<object> checkTwo = from pawn in grid
                                           where
                                               pawn.GetType() == typeof(Pawn) && ((Pawn)pawn).col == currentPawn.col &&
                                               ((Pawn)pawn).row == currentPawn.row
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
            IEnumerable<object> checkTwoAgain = from pawn in grid
                                                where
                                                    pawn.GetType() == typeof(Pawn) && ((Pawn)pawn).col == currentPawn.col &&
                                                    ((Pawn)pawn).row == currentPawn.row
                                                select pawn;
            if (checkTwoAgain.Count() != 1)
            {
                currentPawn.row = savedRow;
                currentMoves = savedMoves;
            }

        }
        public void setPawnToSplit(Pawn thePawn)
        {
            if (thePawn.player == currentPlayer && currentPawn == null)
            {
                pawnToSplit = thePawn;
            }
        }
        public void SplitPawn(int col, int row)
        {
            currentMoves = 0;
            pawnToSplit.health = pawnToSplit.health/2;
            var newPawn = new Pawn(currentPlayer, "Blue", pawnToSplit.health, col, row);
            newPawn.resetColor();
            Add(newPawn);
            pawnToSplit = null;
            CheckMoves();
        }

        public void AttackPawn(Pawn enemy)
        {
            if (currentPawn == null || enemy == null)
            {
                return;
            }
            if (currentMoves > 0 && currentPawn.player == currentPlayer && currentPawn.player != enemy.player &&
                checkDirection(enemy.col, enemy.row))
            {
                //Created private function to attack pawn 
               AttackThePawn(enemy);
            }

       
            CheckMoves();
            CheckGameOver();
        }

        private void AttackThePawn(Pawn enemy)
        {
            enemy.health -= 1;
            currentMoves -= 1;
            if (enemy.health <= 0)
            {
                RemovePawn(enemy);
            }
        }

        

        private void CheckGameOver()
        {
            IEnumerable<object> checkPlayer1 = from gridItem in grid
                where gridItem.GetType() == typeof (Pawn) &&
                      ((Pawn) gridItem).player == 1
                select gridItem;
            IEnumerable<object> checkPlayer2 = from gridItem in grid
                where gridItem.GetType() == typeof (Pawn) &&
                      ((Pawn) gridItem).player == 2
                select gridItem;
            if (!checkPlayer1.Any() || !checkPlayer2.Any())
            {
               // MessageBox.Show("Player " + currentPlayer + " Won! Starting new Game");
                ShowWinner();
                EndGame();
            }
            SaveState();
        }

        public void ShowWinner()
        {
            winnerText = "Player " + currentPlayer + " Won!";
            showWinner = showWinner == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;

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
            else
            {
                thePawn.resetColor();
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
            pawnToSplit = null;
            currentPawn = null;
            CheckGameOver();
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


        public void SaveState()
        {
            try
            {
                saving = true;
                List<Pawn> pawns = (from pawn in grid
                    where pawn.GetType() == typeof (Pawn)
                    select (Pawn) pawn).ToList();
                var saveGame = new SaveClass(pawns, currentMoves, currentRollDice, currentPawn, currentPlayer);

                DataHandler.exportToJson(saveGame, "game.json");
                saving = false;
            }
            catch (Exception)
            {
            }
        }


        public void ContinueGame()
        {
            try
            {
                if (!saving)
                {
                    var lastgame = (SaveClass) DataHandler.importFromJson<SaveClass>("game.json");
                    if (lastgame == null)
                    {
                        EndGame();
                        return;
                    }
                    if (grid.Count() != 0)
                    {
                        List<object> removeList = (from gridItem in grid
                                                   where gridItem.GetType() == typeof(Pawn)
                                                   select gridItem).ToList();
                        foreach (object pawn in removeList)
                        {
                            grid.Remove(pawn);
                        }  
                    }
                  
                    foreach (Pawn pawn in lastgame.pawns)
                    {
                        grid.Add(pawn);
                    }
                    currentMoves = lastgame.currentMoves;
                    currentRollDice = lastgame.currentRollDice;
                    if (lastgame.currentPawn != null)
                    {
                        object lastCurrentPawn = (from gridItem in grid
                                                  where
                                                      gridItem.GetType() == typeof(Pawn) && lastgame.currentPawn.row == ((Pawn)gridItem).row &&
                                                      lastgame.currentPawn.col == ((Pawn)gridItem).col
                                                  select gridItem).First();
                        setCurrentPawn((Pawn)lastCurrentPawn);
                    }
                    
                    currentPlayer = lastgame.currentPlayer;
                }
            }
            catch (Exception e)
            {
              //  MessageBox.Show("What" + e);
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
            NotifyCollectionChangedEventHandler handler = CollectionChanged;
            if (handler != null)
                handler(this, args);
        }

        public void StartGame()
        {
            RollDice();

            currentPlayer = currentRollDice%2 + 1;
            // MessageBox.Show("Player " + currentPlayer + " Starts");
        }
    }
}