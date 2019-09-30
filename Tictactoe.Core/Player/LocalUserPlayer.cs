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
    public class LocalUserPlayer : IPlayer
    {
        private readonly UserMoveStream _userMoveStream;

        public LocalUserPlayer(UserMoveStream userMoveStream)
        {
            _userMoveStream = userMoveStream;
        }

        public Task<Coordinates> GetNextMove<T>(Board<T> board, T playerMarker)
        {
            var taskSource = new TaskCompletionSource<Coordinates>();
            _userMoveStream.Subscribe(val => taskSource.SetResult(val));
            return taskSource.Task;
        }
    }
}