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
            System.Console.WriteLine("new publish");;
            if (_subscriberCallback != null)
            {
                System.Console.WriteLine("passing to subscriber");
                _subscriberCallback(coords);
            }else{
                System.Console.WriteLine("no subscriber");
            }
        }

        public void Subscribe(Action<Coordinates> callback)
        {
            Console.WriteLine("Subscription added");
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
            _userMoveStream.Subscribe(val => {
                Console.WriteLine("here1");
                taskSource.SetResult(val);});
            return taskSource.Task;
        }
    }
}