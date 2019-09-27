using System.Threading.Tasks;

namespace Tictactoe.Core.Player
{
    public interface IPlayer
    {
        Task<Coordinates> GetNextMove<T>(Board<T> board, T playerMarker);
    }
}