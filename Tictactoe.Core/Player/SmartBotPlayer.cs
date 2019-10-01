using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tictactoe.Core.Player
{
    public class SmartBotPlayer : IPlayer
    {
        public async Task<Coordinates> GetNextMove<T>(Board<T> board, T playerMarker)
        {
            await Task.Delay(2000);
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

        private bool CanIWin<T>(Board<T> board, T playerMarker)
        {
            foreach (var row in board.Rows)
            {
                
            }
        }

        private bool CanICreateMoreThanOneWinningOption()
        {
            
        }

        private bool CanICreateWinningOption()
        {
            
        }
        
        private bool CanOpponentWinInNextMove()
        {
            
        }

        private bool CanOpponentCreateMoreThanOneWinningOptionInNextMove()
        {
            
        }
        
        
    }
}