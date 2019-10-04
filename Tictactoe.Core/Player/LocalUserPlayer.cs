using System;
using System.Threading.Tasks;

namespace Tictactoe.Core.Player
{
    // Simple alternative until Reactive extension is working in webassembly
    // https://github.com/NuGet/Home/issues/8186
    public class UserMoveStream
    {
        private Action<Coordinates> _subscriberCallback;

        public void Publish(Coordinates coords)
        {
            _subscriberCallback?.Invoke(coords);
        }

        public void Subscribe(Action<Coordinates> callback)
        {
            _subscriberCallback = callback;
        }
    }

    public class LocalUserPlayer : Player<CellMarker>
    {
        private readonly UserMoveStream _userMoveStream;

        public LocalUserPlayer(
            UserMoveStream userMoveStream,
            CellMarker marker) : base(marker)
        {
            _userMoveStream = userMoveStream;
        }

        public override Task<Coordinates> GetNextMove(Board<CellMarker> board)
        {
            var taskSource = new TaskCompletionSource<Coordinates>();
            _userMoveStream.Subscribe(val =>
            {
                // ignore if users clicks a cell that is not empty
                if (board[val.X, val.Y] == CellMarker.Empty)
                {
                    taskSource.SetResult(val);    
                }
            });
            return taskSource.Task;
        }
    }
}