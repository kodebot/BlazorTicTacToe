using System;
using System.Collections.Generic;

namespace TicTacToe.Core.PlayerBot
{
    public class RandomPlayerBot : IPlayerBot
    {
        public Coordinates GetNextMove<T>(Board<T> board, T playerMarker)
        {
            // find random empty cell
            var random = new Random();
            while (true)
            {
                var row = random.Next(0, 3);
                var col = random.Next(0, 3);
                if (EqualityComparer<T>.Default.Equals( board[row, col],default(T)))
                {
                    return new Coordinates(row, col);
                }
            }
        }
    }
}