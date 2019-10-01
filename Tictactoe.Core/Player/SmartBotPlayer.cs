using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tictactoe.Core.Player
{
    public class SmartBotPlayer : Player<CellMarker>
    {
        public SmartBotPlayer(
            Board<CellMarker> board,
            CellMarker playerMarker,
            CellMarker opponentPlayerMarker) : base(board, playerMarker, opponentPlayerMarker)
        {
        }
        
        public override async Task<Coordinates> GetNextMove()
        {
            await Task.Delay(2000);
            // find random empty cell

            var (canWin, winningCoordinates) = CanWinInNextMove(PlayerMarker);
            if (canWin)
            {
                return winningCoordinates;
            }

            var (canOpponentWin, blockingCoordinates) = CanWinInNextMove(OpponentPlayerMarker);

            if (canOpponentWin)
            {
                return blockingCoordinates;
            }

            var (canCreateWinningBoard, winningBoardCoordinates) = CanCreateWinningOption(PlayerMarker);

            if (canCreateWinningBoard)
            {
                return winningCoordinates;
            }

            return GetRandomCoordinates();

        }

        private (bool, Coordinates) CanWinInNextMove(CellMarker marker)
        {
            foreach (var row in Board.Rows)
            {
                if (IsInWinningPosition(row))
                {
                    return (true, row.First(cell => cell.Value == CellMarker.Empty).Coordinates);
                }
            }
            
            foreach (var column in Board.Columns)
            {
                if (IsInWinningPosition(column))
                {
                    return (true, column.First(cell => cell.Value == CellMarker.Empty).Coordinates);
                }
            }

            if (IsInWinningPosition(Board.LeftToRightDiagonal))
            {
                return (true, GetNextMoveCoordinates(Board.LeftToRightDiagonal));
            }
            
            if (IsInWinningPosition(Board.RightToLeftDiagonal))
            {
                return (true, GetNextMoveCoordinates(Board.RightToLeftDiagonal));
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

        private bool CanICreateMoreThanOneWinningOption()
        {
            // todo
            return false;
        }

        private (bool, Coordinates) CanCreateWinningOption(CellMarker marker)
        {
            foreach (var row in Board.Rows)
            {
                if (IsWinningPositionPossible(row))
                {
                    return (true, row.First(cell => cell.Value == CellMarker.Empty).Coordinates);
                }
            }
            
            foreach (var column in Board.Columns)
            {
                if (IsWinningPositionPossible(column))
                {
                    return (true, column.First(cell => cell.Value == CellMarker.Empty).Coordinates);
                }
            }

            if (IsWinningPositionPossible(Board.LeftToRightDiagonal))
            {
                return (true, GetNextMoveCoordinates(Board.LeftToRightDiagonal));
            }
            
            if (IsWinningPositionPossible(Board.RightToLeftDiagonal))
            {
                return (true, GetNextMoveCoordinates(Board.RightToLeftDiagonal));
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
        
        private bool CanOpponentCreateMoreThanOneWinningOptionInNextMove()
        {
            // todo: 
            return false;
        }

        private Coordinates GetRandomCoordinates()
        {
            var random = new Random();
            while (true)
            {
                var row = random.Next(0, 3);
                var col = random.Next(0, 3);
                if (Board[row, col] == CellMarker.Empty)
                {
                    return new Coordinates(row, col);
                }
            }
        }
        
    }
}