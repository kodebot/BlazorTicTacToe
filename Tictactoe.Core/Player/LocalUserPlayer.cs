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
            Board<CellMarker> board,
            CellMarker playerMarker,
            CellMarker opponentPlayerMarker) : base(board, playerMarker, opponentPlayerMarker)
        {
            _userMoveStream = userMoveStream;
        }

        public override Task<Coordinates> GetNextMove()
        {
            var taskSource = new TaskCompletionSource<Coordinates>();
            _userMoveStream.Subscribe(val => taskSource.SetResult(val));
            return taskSource.Task;
        }
    }
}