using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tictactoe.Core.Player
{
    public class RandomBotPlayer : Player<CellMarker>
    {
        public RandomBotPlayer(
            Board<CellMarker> board,
            CellMarker playerMarker,
            CellMarker opponentPlayerMarker) : base(board, playerMarker, opponentPlayerMarker)
        {
        }

        public override async Task<Coordinates> GetNextMove()
        {
            await Task.Delay(2000);
            // find random empty cell
            var random = new Random();
            while (true)
            {
                var row = random.Next(0, 3);
                var col = random.Next(0, 3);
                if (Board[row, col] == CellMarker.Empty)
                {
                    return new Coordinates(row, col);
                }
            }
        }
    }
}