using System.Linq;

namespace GameOne
{
    public class AI
    {
        public AI(Game game)
        {
            this.game = game;
        }

        public Game game { get; set; }

        public void StartAi()
        {
            Pawn myPawn;
            var checkMyPawn = (from gridItem in game.grid
                where gridItem.GetType() == typeof (Pawn) &&
                      ((Pawn) gridItem).player == game.currentPlayer
                select gridItem);
            if (checkMyPawn.Any())
            {
                myPawn = (Pawn)checkMyPawn.First();
                game.setCurrentPawn(myPawn);                          
            }
            else
            {
                return;
            }
            var infLoopCount = 0;
            var moveAround = 0;
            Pawn enemyPawn;
            while ((myPawn.player == game.currentPlayer && game.currentMoves > 0) && game.gameOver != true)
            {
                
                var findEnemyPawn = from gridItem in game.grid
                                    where gridItem.GetType() == typeof(Pawn) &&
                                          ((Pawn)gridItem).player != game.currentPlayer
                                    select gridItem;
                if (!findEnemyPawn.Any())
                {
                    break;
                }
                enemyPawn = (Pawn) findEnemyPawn.First();
                game.AttackPawn(enemyPawn);
         
                switch (moveAround)
                {
                    case 0:
                    {
                        game.MoveOrSplitPawn(enemyPawn.col, enemyPawn.row - 1);
                        moveAround++;
                        break;
                    }
                    case 1:
                    {
                        game.MoveOrSplitPawn(enemyPawn.col, enemyPawn.row + 1);
                        moveAround++;
                        break;
                    }
                    case 2:
                    {
                        game.MoveOrSplitPawn(enemyPawn.col - 1, enemyPawn.row );
                        moveAround++;
                        break;
                    }
                    case 3:
                    {
                        game.MoveOrSplitPawn(enemyPawn.col + 1, enemyPawn.row);
                        moveAround++;
                        break;
                    }    
                    case 4:
                    {
                        game.MoveOrSplitPawn(enemyPawn.col - 1, enemyPawn.row + 1);
                        moveAround++;
                        break;
                    }
                    case 5:
                    {
                        game.MoveOrSplitPawn(enemyPawn.col - 1, enemyPawn.row - 1);
                        moveAround++;
                        break;
                    }
                    case 6:
                    {
                        game.MoveOrSplitPawn(enemyPawn.col-1, enemyPawn.row - 1);
                        moveAround++;
                        break;
                    }
                    case 7:
                    {
                        game.MoveOrSplitPawn(enemyPawn.col+1, enemyPawn.row + 1);
                        moveAround++;
                        break;
                    }
                    default:
                    {
                       
                        game.setCurrentPawn(myPawn);
                        moveAround = 0;
                        infLoopCount++;
                        break;
                    }
                }
                if (infLoopCount > 10)
                {
                    game.ChangePlayer();
                }
                /*
                infLoopCount++;
                if (infLoopCount > 10)
                {
                    game.setPawnToSplit(myPawn);
                    game.SplitPawn(myPawn.col-1,myPawn.row-1);
                    /*
                    if (enemyPawn.row > myPawn.row && enemyPawn.col > myPawn.col)
                    {
                        game.MovePawn(enemyPawn.col - 1, enemyPawn.row - 1);
                    }
                    else
                    {
                        game.MovePawn(myPawn.row-1, myPawn.col - 1);
                        
                    }
                 
                    infLoopCount = 0;
                }
            */
            }
            
           

        }


    }
}
