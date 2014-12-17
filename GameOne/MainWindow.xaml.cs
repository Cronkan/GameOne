using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using GameOne.Annotations;
using GameOneDataLayer;

namespace GameOne
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        

        public int boardSize { get; set; }
        public Game game
        {
            get { return _game; }
            set
            {
                _game = value;
               OnPropertyChanged("game");
            }
        }


        private Game _game;


        public MainWindow()
        {
            InitializeComponent();
            boardSize = 10;
            /*
            Game lastGame = DataHandler.importFromJson<Game>("game.json") as Game;
            if (lastGame != null)
            {
                MessageBox.Show("" + lastGame.grid.ToString());
                game = lastGame;

            }
            else
            {*/
                game = new Game();
                FillBoard();
                AddPawns();
            //}

            Board.DataContext = new
            {
                grid = game.grid,
                boardSize = boardSize,
            };

            this.DataContext = this;
        }

        public void FillBoard()
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    game.grid.Add(new BoardTile(i, j));
                }

            }
        }

        public void AddPawns()
        {
            game.grid.Add(new Pawn(1, "Pink", 10, 4, 2));
            game.grid.Add(new Pawn(1, "Pink", 10, 4, 3));
            game.grid.Add(new Pawn(1, "Pink", 10, 4, 4));
            game.grid.Add(new Pawn(1, "Pink", 10, 4, 5));
            game.grid.Add(new Pawn(1, "Pink", 10, 4, 6));
            game.grid.Add(new Pawn(2, "Teal", 10, 5, 2));
            game.grid.Add(new Pawn(2, "Teal", 10, 5, 3));
            game.grid.Add(new Pawn(2, "Teal", 10, 5, 4));
            game.grid.Add(new Pawn(2, "Teal", 10, 5, 5));
            game.grid.Add(new Pawn(2, "Teal", 10, 5, 6));
           
          
        }

        private void rightClickRectangle(object sender, RoutedEventArgs e)
        {
            var obj = sender as ContentPresenter;


            if (obj.Content.GetType() == typeof(Pawn) && game.currentPawn == null)
            {

                Pawn thePawn = (Pawn)(from gridItem in game.grid
                                      where gridItem.GetType() == typeof(Pawn) &&
                                            ((Pawn)gridItem).col == Grid.GetColumn((UIElement)sender) &&
                                            ((Pawn)gridItem).row == Grid.GetRow((UIElement)sender)
                                      select gridItem).First();
                game.setPawnToSplit(thePawn);

            }
        }
        private void clickRectangle(object sender, RoutedEventArgs e)
        {
            var obj = sender as ContentPresenter;
            if (obj.Content.GetType() == typeof(BoardTile) && game.pawnToSplit != null)
            {
                game.SplitPawn(Grid.GetColumn((UIElement)sender), Grid.GetRow((UIElement)sender));

            }
            else
            {
                if (obj.Content.GetType() == typeof (Pawn) && game.currentPawn == null)
                {

                    Pawn thePawn = (Pawn) (from gridItem in game.grid
                        where gridItem.GetType() == typeof (Pawn) &&
                              ((Pawn) gridItem).col == Grid.GetColumn((UIElement) sender) &&
                              ((Pawn) gridItem).row == Grid.GetRow((UIElement) sender)
                        select gridItem).First();
                    game.setCurrentPawn(thePawn);

                }
                else if (obj.Content.GetType() == typeof (BoardTile) && game.currentPawn != null)
                {

                    game.MovePawn(Grid.GetColumn((UIElement) sender), Grid.GetRow((UIElement) sender));

                }
                if (game.currentPawn != null && obj.Content.GetType() == typeof (Pawn))
                {
                    Pawn enemyPawn = (Pawn) (from gridItem in game.grid
                        where gridItem.GetType() == typeof (Pawn) &&
                              ((Pawn) gridItem).col == Grid.GetColumn((UIElement) sender) &&
                              ((Pawn) gridItem).row == Grid.GetRow((UIElement) sender)
                        select gridItem).First();
                    game.setCurrentPawn(enemyPawn);
                    game.AttackPawn(enemyPawn);
                 
                }

            }


        }

        

        private void DiceRoll(object sender, RoutedEventArgs e)
        {
           game.RollDice();
          
          SaveGame();
            
        }

        public void SaveGame()
        {
       
            using (var ctx = new Context())
            {

                //ctx.Game.Add(game);
                ctx.Pawns.Add(new Pawn(2,"pink",2,2,2));
                ctx.SaveChanges();
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
