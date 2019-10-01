using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tictactoe.Core.Player
{
    public class SmartBotPlayer : Player<CellMarker>
    {
        public SmartBotPlayer(CellMarker marker) : base(marker)
        {
        }
        
        public override async Task<Coordinates> GetNextMove(Board<CellMarker> board)
        {
            await Task.Delay(2000);
            // find random empty cell

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