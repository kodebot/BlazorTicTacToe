using System.Threading.Tasks;

namespace Tictactoe.Core.Player
{
    public abstract class Player<T>
    {
        protected readonly Board<T> Board;
        protected readonly T PlayerMarker;
        protected readonly T OpponentPlayerMarker;

        protected Player(Board<T> board, T playerMarker, T opponentPlayerMarker)
        {
            Board = board;
            PlayerMarker = playerMarker;
            OpponentPlayerMarker = opponentPlayerMarker;
        }

        public T Marker => PlayerMarker;
        public abstract Task<Coordinates> GetNextMove();
    }
}