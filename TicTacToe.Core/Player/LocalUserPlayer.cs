using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tictactoe.Core.Player
{
    public class LocalUserPlayer : IPlayer
    {
        private readonly Action<int, int> _userMoveRecorder;


        public LocalUserPlayer(Action<int, int> userMoveRecorder)
        {
            _userMoveRecorder = userMoveRecorder;
        }
        
        public async Task<Coordinates> GetNextMove<T>(Board<T> board, T playerMarker)
        {
            
        }
    }
}