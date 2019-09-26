namespace Tictactoe.Core.PlayerBot
{
    public interface IPlayerBot
    {
        Coordinates GetNextMove<T>(Board<T> board, T playerMarker);
    }
}