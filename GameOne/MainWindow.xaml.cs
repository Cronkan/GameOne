using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using GameOne.Annotations;

namespace GameOne
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
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
            game = new Game(boardSize);
            Ai = new AI(game);
            FillBoard();
            game.NewGame();
            //}

            Board.DataContext = new
            {
                game.grid,
                boardSize,
            };

            DataContext = this;
            watcher = new Watcher(game);
            watcher.Run();
            game.ContinueGame();
        }

        public static int boardSize { get; set; }
        public AI Ai { get; set; }

        public Game game
        {
            get { return _game; }
            set
            {
                _game = value;
                OnPropertyChanged("game");
            }
        }


        public Watcher watcher { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;


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


        private void rightClickRectangle(object sender, RoutedEventArgs e)
        {
            var obj = sender as ContentPresenter;


            if (obj.Content.GetType() == typeof (Pawn))
            {
                var thePawn = (Pawn) (from gridItem in game.grid
                    where gridItem.GetType() == typeof (Pawn) &&
                          ((Pawn) gridItem).col == Grid.GetColumn((UIElement) sender) &&
                          ((Pawn) gridItem).row == Grid.GetRow((UIElement) sender)
                    select gridItem).First();
                game.setPawnToSplit(thePawn);
            }
        }

        private void clickRectangle(object sender, RoutedEventArgs e)
        {
            //CODE CLEANUP - Validation done in Game-class
            //Boardtile -> movepawn -> splitpawn -> MoveOrSplitPawn()
            //pawn -> setcurrentpawn
            //pawn -> attackpawn
            var obj = sender as ContentPresenter;
    
                if (obj.Content.GetType() == typeof (Pawn))
                {
                    var thePawn = (Pawn) (from gridItem in game.grid
                        where gridItem.GetType() == typeof (Pawn) &&
                              ((Pawn) gridItem).col == Grid.GetColumn((UIElement) sender) &&
                              ((Pawn) gridItem).row == Grid.GetRow((UIElement) sender)
                        select gridItem).First();
                    game.setCurrentPawn(thePawn);
                    game.AttackPawn(thePawn);
                }
                else if (obj.Content.GetType() == typeof (BoardTile))
                {
                    game.MoveOrSplitPawn(Grid.GetColumn((UIElement) sender), Grid.GetRow((UIElement) sender));
                }
      
          
        }


        private void PlayAi(object sender, RoutedEventArgs e)
        {
            // game.RollDice();
            Ai.StartAi();
            //SaveGame();
        }

    

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ChangePlayer(object sender, RoutedEventArgs e)
        {
            game.ChangePlayer();
            game.RollDice();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            game.SaveState();
        }

        private void LoadClick(object sender, RoutedEventArgs e)
        {
            game.ContinueGame();
        }

        private void NewGameClick(object sender, RoutedEventArgs e)
        {
            game.EndGame();
            
        }
    }
}