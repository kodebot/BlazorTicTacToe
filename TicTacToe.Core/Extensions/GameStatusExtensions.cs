using System.Linq;

namespace TicTacToe.Core.Extensions
{
    public static class GameStatusExtensions
    {
        public static bool IsNew(this GameStatus status)
        {
            return status == GameStatus.New;
        }

        public static bool IsGameOver(this GameStatus status)
        {
            return new[] { GameStatus.Player1Won, GameStatus.Player2Won, GameStatus.Tie }.Contains(status);
        }

        public static bool IsInProgress(this GameStatus status){
            return new[] { GameStatus.Player1Turn, GameStatus.Player2Turn, }.Contains(status);
        }
    }
}