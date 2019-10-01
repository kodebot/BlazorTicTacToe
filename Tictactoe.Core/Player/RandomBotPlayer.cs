using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tictactoe.Core.Player
{
    public class RandomBotPlayer : Player<CellMarker>
    {
        public RandomBotPlayer(CellMarker marker) : base(marker)
        {
        }

        public override async Task<Coordinates> GetNextMove(Board<CellMarker> board)
        {
            await Task.Delay(2000);
            // find random empty cell
            var random = new Random();
            while (true)
            {
                var row = random.Next(0, 3);
                var col = random.Next(0, 3);
                if (board[row, col] == CellMarker.Empty)
                {
                    return new Coordinates(row, col);
                }
            }
        }
    }
}