using System.Threading.Tasks;

namespace Tictactoe.Core.Player
{
    public abstract class Player<T>
    {
        protected Player(T marker)
        {
            Marker = marker;
        }

        public T Marker { get; }

        public abstract Task<Coordinates> GetNextMove(Board<T> board);
    }
}