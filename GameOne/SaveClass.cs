using System.Collections.Generic;

namespace GameOne
{
    public class SaveClass
    {
        public SaveClass(List<Pawn> pawns, int currentMoves, int currentRollDice, Pawn currentPawn, int currentPlayer)
        {
            this.pawns = pawns;
            this.currentMoves = currentMoves;
            this.currentRollDice = currentRollDice;
            this.currentPawn = currentPawn;
            this.currentPlayer = currentPlayer;
        }

        public List<Pawn> pawns { get; set; }
        public int currentMoves { get; set; }
        public int currentRollDice { get; set; }
        public Pawn currentPawn { get; set; }
        public int currentPlayer { get; set; }
    }
}