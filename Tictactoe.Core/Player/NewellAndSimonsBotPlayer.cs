using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tictactoe.Core.Player
{
    public class NewellAndSimonsBotPlayer : Player<CellMarker>
    {
        public NewellAndSimonsBotPlayer(CellMarker marker) : base(marker)
        {
        }
        
        public override async Task<Coordinates> GetNextMove(Board<CellMarker> board)
        {
            /*
            A player can play a perfect game of tic-tac-toe (to win or at least, draw) if each time it is their turn to play, they choose the first available move from the following list, as used in Newell and Simon's 1972 tic-tac-toe program.[16]

            Win: If the player has two in a row, they can place a third to get three in a row.

            Block: If the opponent has two in a row, the player must play the third themselves to block the opponent.

            Fork: Create an opportunity where the player has two ways to win (two non-blocked lines of 2).

            Blocking an opponent's fork: If there is only one possible fork for the opponent, the player should block it. Otherwise, the player should block all forks in any way that simultaneously allows them to create two in a row. Otherwise, the player should create a two in a row to force the opponent into defending, as long as it doesn't result in them creating a fork. For example, if "X" has two opposite corners and "O" has the center, "O" must not play a corner in order to win. (Playing a corner in this scenario creates a fork for "X" to win.)

            Center: A player marks the center. (If it is the first move of the game, playing on a corner gives the second player more opportunities to make a mistake and may therefore be the better choice; however, it makes no difference between perfect players.)

            Opposite corner: If the opponent is in the corner, the player plays the opposite corner.

            Empty corner: The player plays in a corner square.

            Empty side: The player plays in a middle square on any of the 4 sides.
             */

            await Task.Delay(1000);
                        
            var (canWin, winningCoordinates) = CanWinInNextMove(board, Marker);
            if (canWin)
            {
                return winningCoordinates;
            }

            var opponentPlayerMarker = GetOpponentMarker();
            var (canOpponentWin, blockingCoordinates) = CanWinInNextMove(board, opponentPlayerMarker);

            if (canOpponentWin)
            {
                return blockingCoordinates;
            }

            var (canCreateWinningBoard, winningBoardCoordinates) = CanCreateWinningOption(board, Marker);

            if (canCreateWinningBoard)
            {
                return winningBoardCoordinates;
            }

            return GetRandomCoordinates(board);
        }

        private (bool, Coordinates) CanWinInNextMove(Board<CellMarker> board, CellMarker marker)
        {
            foreach (var row in board.Rows)
            {
                if (IsInWinningPosition(row))
                {
                    return (true, row.First(cell => cell.Value == CellMarker.Empty).Coordinates);
                }
            }
            
            foreach (var column in board.Columns)
            {
                if (IsInWinningPosition(column))
                {
                    return (true, column.First(cell => cell.Value == CellMarker.Empty).Coordinates);
                }
            }

            if (IsInWinningPosition(board.LeftToRightDiagonal))
            {
                return (true, GetNextMoveCoordinates(board.LeftToRightDiagonal));
            }
            
            if (IsInWinningPosition(board.RightToLeftDiagonal))
            {
                return (true, GetNextMoveCoordinates(board.RightToLeftDiagonal));
            }
            
            return (false, default);

            bool IsInWinningPosition(Cell<CellMarker>[] cells)
            {
                return cells.Count(cell => cell.Value ==  marker) == 2 &&
                       cells.Any(cell => cell.Value  == CellMarker.Empty);
            }

            Coordinates GetNextMoveCoordinates(Cell<CellMarker>[] cells)
            {
                return cells.First(cell => cell.Value ==  CellMarker.Empty).Coordinates;
            }
        }

        private (bool, Coordinates) CanCreateWinningOption(Board<CellMarker> board, CellMarker marker)
        {
            foreach (var row in board.Rows)
            {
                if (IsWinningPositionPossible(row))
                {
                    return (true, row.First(cell => cell.Value == CellMarker.Empty).Coordinates);
                }
            }
            
            foreach (var column in board.Columns)
            {
                if (IsWinningPositionPossible(column))
                {
                    return (true, column.First(cell => cell.Value == CellMarker.Empty).Coordinates);
                }
            }

            if (IsWinningPositionPossible(board.LeftToRightDiagonal))
            {
                return (true, GetNextMoveCoordinates(board.LeftToRightDiagonal));
            }
            
            if (IsWinningPositionPossible(board.RightToLeftDiagonal))
            {
                return (true, GetNextMoveCoordinates(board.RightToLeftDiagonal));
            }
            
            return (false, default);

            bool IsWinningPositionPossible(Cell<CellMarker>[] cells)
            {
                return cells.Count(cell => cell.Value == CellMarker.Empty) == 2 &&
                       cells.Any(cell => cell.Value  == marker);
            }

            Coordinates GetNextMoveCoordinates(Cell<CellMarker>[] cells)
            {
                var random = new Random();
                var index = random.Next(0, 2);
                return cells.Where(cell => cell.Value ==  CellMarker.Empty).ElementAt(index).Coordinates;
            }
        }

        private Coordinates GetRandomCoordinates(Board<CellMarker> board)
        {
            var random = new Random();
            while (true)
            {
                var row = random.Next(0, 3);
                var col = random.Next(0, 3);
                if (board[row, col] == CellMarker.Empty)
                {
                    return new Coordinates(row, col);
                }
            }
        }

        private CellMarker GetOpponentMarker()
        {
            return Marker switch
            {
                CellMarker.Cross => CellMarker.Nought,
                CellMarker.Nought => CellMarker.Cross,
                _ => throw new InvalidOperationException("This is impossible!")
            };
        }
        
    }
}