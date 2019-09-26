using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Tictactoe.Core.Player
{
    public class LocalUserPlayer : IPlayer
    {
        private readonly Subject<Coordinates> _userMoveRecorder;

        public LocalUserPlayer(Subject<Coordinates> userMoveRecorder)
        {
            _userMoveRecorder = userMoveRecorder;
        }
        
        public Task<Coordinates> GetNextMove<T>(Board<T> board, T playerMarker)
        {
            _userMoveRecorder.AsQbservable().
        }
    }
}