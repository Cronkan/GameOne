using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using GameOne.Annotations;

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

        public static Pawn currentPawn { get; set; }
      
        private Game _game;

        public MainWindow()
        {
            InitializeComponent();
            boardSize = 10;
            Game lastGame = DataHandler.importFromJson<Game>("game.json") as Game;
            if (lastGame != null)
            {
                MessageBox.Show("" + lastGame.grid.ToString());
                game = lastGame;

            }
            else
            {
                game = new Game();
                FillBoard();
                AddPawns();
            }

            

          
            

           
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
            game.grid.Add(new Pawn(1, "Red", 10, 0, 3));
            game.grid.Add(new Pawn(2, "Blue", 10, 6, 3));
            game.grid.Add(new Pawn(1, "Pink", 10, 3, 3));
          
        }


        private void clickRectangle(object sender, RoutedEventArgs e)
        {
            var obj = sender as ContentPresenter;


            if (obj.Content.GetType() == typeof(Pawn) && currentPawn == null)
            {

                Pawn thePawn = (Pawn)(from gridItem in game.grid
                                      where gridItem.GetType() == typeof(Pawn) &&
                                            ((Pawn)gridItem).col == Grid.GetColumn((UIElement)sender) &&
                                            ((Pawn)gridItem).row == Grid.GetRow((UIElement)sender)
                                      select gridItem).First();

                thePawn.Color = "Green";
                currentPawn = thePawn;


            }
            else if (obj.Content.GetType() == typeof(BoardTile) && currentPawn != null)
            {

                currentPawn.col = Grid.GetColumn((UIElement)sender);
                currentPawn.row = Grid.GetRow((UIElement)sender);
                
                currentPawn = null;
                

            }
            else if(currentPawn != null && obj.Content.GetType() == typeof(Pawn))
            {
                Pawn enemyPawn = (Pawn)(from gridItem in game.grid
                                      where gridItem.GetType() == typeof(Pawn) &&
                                            ((Pawn)gridItem).col == Grid.GetColumn((UIElement)sender) &&
                                            ((Pawn)gridItem).row == Grid.GetRow((UIElement)sender)
                                      select gridItem).First();
                game.AttackPawn(enemyPawn);   
            }




        }

        

        private void DiceRoll(object sender, RoutedEventArgs e)
        {
           game.RollDice();
          
          
            
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
