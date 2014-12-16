using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOne
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<object> grid;
        public List<object> Grid
        {
            get
            {
                
                return grid;
            }
            set
            {
                if (grid == value)
                {
                    return;
                }
                grid = value;
                //raisePropertyChanged("CardList");
            }
        }

        public ObservableCollection<Rectangle> TestCollection;
        public Game game;
        public MainWindow()
        {
            InitializeComponent();
            game = new Game();
            TestCollection = new ObservableCollection<Rectangle>();
            grid = new List<object>();
            FillBoard();
            
        }

        public void FillBoard()
        {
            grid.Add(new Rectangle { Height = 60, Width = 15, Fill = Brushes.Blue });
        }
    }
}
