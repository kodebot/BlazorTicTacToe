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
            await Task.Delay(1000);
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


            var (canWin, winningCoordinates) = CanWinInNextMove(board, Marker);
            if (canWin)
            {
                Console.WriteLine("Can Win");
                return winningCoordinates[0];
            }

            var opponentPlayerMarker = GetOpponentMarker();
            var (canOpponentWin, blockingCoordinates) = CanWinInNextMove(board, opponentPlayerMarker);

            if (canOpponentWin)
            {
                Console.WriteLine("Block");
                return blockingCoordinates[0];
            }

            var (canFork, forkCoordinates) = CanFork(board, Marker);

            if (canFork)
            {
                Console.WriteLine("can fork");
                return forkCoordinates[0];
            }

            var (canOpponentFork, opponentForkCoordinates) = CanFork(board, opponentPlayerMarker);

            if (canOpponentFork)
            {
                Console.WriteLine("block fork");
                var (canCreateWinningOptionToBlock, coordinates) = CanCreateWinningOption(board, Marker);
                if (canCreateWinningOptionToBlock)
                {
                    // use one of the forkable coordiante to force the opponent to play diffenrent move
                    var coordinatesToBlock = coordinates.Where(coordinate => opponentForkCoordinates.Contains(coordinate));
                    if (coordinatesToBlock.Any())
                    {
                        return coordinatesToBlock.First();
                    }
                }
                else
                {
                    return opponentForkCoordinates[0];
                }
            }

            // Center
            var row = (int)Math.Floor(board.GridSize / 2.0);
            var col = row;
            Console.WriteLine($"center {row} {col}");
            if (board.Rows[row][col].Value == CellMarker.Empty)
            {
                Console.WriteLine("center");
                return board.Rows[row][col].Coordinates; // center
            }

            // Opposite Corner
            if (board.Corners[0].Value == opponentPlayerMarker && board.Corners[2].Value == CellMarker.Empty)
            {
                Console.WriteLine("opposite corner");
                return board.Corners[2].Coordinates;
            }

            if (board.Corners[1].Value == opponentPlayerMarker && board.Corners[3].Value == CellMarker.Empty)
            {
                Console.WriteLine("opposite corner");
                return board.Corners[3].Coordinates;
            }

            if (board.Corners[2].Value == opponentPlayerMarker && board.Corners[0].Value == CellMarker.Empty)
            {
                Console.WriteLine("opposite corner");
                return board.Corners[0].Coordinates;
            }

            if (board.Corners[3].Value == opponentPlayerMarker && board.Corners[1].Value == CellMarker.Empty)
            {
                Console.WriteLine("opposite corner");
                return board.Corners[1].Coordinates;
            }

            // Empty Corner
            if (board.Corners.Any(corner => corner.Value == CellMarker.Empty))
            {
                Console.WriteLine("empty corner");
                return board.Corners.First(corner => corner.Value == CellMarker.Empty).Coordinates;
            }

            // Empty Side
            var midCellIndex = (int)Math.Floor(board.GridSize / 2.0);
            if (board.Rows[0].All(cell => cell.Value == CellMarker.Empty))
            {
                Console.WriteLine("empty side");
                return board.Rows[0][midCellIndex].Coordinates;
            }

            if (board.Columns[2].All(cell => cell.Value == CellMarker.Empty))
            {
                Console.WriteLine("empty side");
                return board.Columns[2][midCellIndex].Coordinates;
            }

            if (board.Rows[2].All(cell => cell.Value == CellMarker.Empty))
            {
                Console.WriteLine("empty side");
                return board.Rows[2][midCellIndex].Coordinates;
            }

            if (board.Columns[0].All(cell => cell.Value == CellMarker.Empty))
            {
                Console.WriteLine("empty side");
                return board.Columns[0][midCellIndex].Coordinates;
            }

            Console.WriteLine("random");
            return GetRandomCoordinates(board);
        }

        private (bool, Coordinates[]) CanWinInNextMove(Board<CellMarker> board, CellMarker marker)
        {
            var winningCoordinates = new List<Coordinates>();

            foreach (var row in board.Rows)
            {
                if (IsInWinningPosition(row))
                {
                    winningCoordinates.Add(row.First(cell => cell.Value == CellMarker.Empty).Coordinates);
                }
            }

            foreach (var column in board.Columns)
            {
                if (IsInWinningPosition(column))
                {
                    winningCoordinates.Add(column.First(cell => cell.Value == CellMarker.Empty).Coordinates);
                }
            }

            if (IsInWinningPosition(board.LeftToRightDiagonal))
            {
                winningCoordinates.Add(GetNextMoveCoordinates(board.LeftToRightDiagonal));
            }

            if (IsInWinningPosition(board.RightToLeftDiagonal))
            {
                winningCoordinates.Add(GetNextMoveCoordinates(board.RightToLeftDiagonal));
            }

            winningCoordinates.Distinct();

            return (winningCoordinates.Any(), winningCoordinates.ToArray());

            bool IsInWinningPosition(Cell<CellMarker>[] cells)
            {
                return cells.Count(cell => cell.Value == marker) == 2 &&
                       cells.Any(cell => cell.Value == CellMarker.Empty);
            }

            Coordinates GetNextMoveCoordinates(Cell<CellMarker>[] cells)
            {
                return cells.First(cell => cell.Value == CellMarker.Empty).Coordinates;
            }
        }

        private (bool, List<Coordinates>) CanFork(Board<CellMarker> board, CellMarker marker)
        {
            var forkOptions = new List<(int, Coordinates)>();

            var clonedBoard = board.Clone();

            for (var row = 0; row < clonedBoard.GridSize; row++)
            {
                for (var col = 0; col < clonedBoard.GridSize; col++)
                {
                    var cell = clonedBoard.Rows[row][col];
                    if (cell.Value == CellMarker.Empty)
                    {
                        // place the player marker in the empty cell and record the coordinate if it has more than one winning possibility
                        clonedBoard.Fill(cell.Coordinates, marker);
                        var (canWin, coordinates) = CanWinInNextMove(clonedBoard, marker);
                        if (canWin && coordinates.Count() > 1)
                        {
                            forkOptions.Add((coordinates.Count(), cell.Coordinates));
                        }
                        clonedBoard.Fill(cell.Coordinates, CellMarker.Empty); // clear the cell to original
                    }
                }
            }

            if (forkOptions.Any())
            {
                return (true, forkOptions.Select(x => x.Item2).Distinct().ToList());
            }

            return (false, default);
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



        private (bool, Coordinates[]) CanCreateWinningOption(Board<CellMarker> board, CellMarker marker)
        {
            var winningOptions = new List<Coordinates>();

            foreach (var row in board.Rows)
            {
                if (IsWinningPositionPossible(row))
                {
                    winningOptions.AddRange(row.Where(cell => cell.Value == CellMarker.Empty).Select(cell => cell.Coordinates));
                }
            }

            foreach (var column in board.Columns)
            {
                if (IsWinningPositionPossible(column))
                {
                    winningOptions.AddRange(column.Where(cell => cell.Value == CellMarker.Empty).Select(cell => cell.Coordinates));
                }
            }

            if (IsWinningPositionPossible(board.LeftToRightDiagonal))
            {
                winningOptions.AddRange(board.LeftToRightDiagonal.Where(cell => cell.Value == CellMarker.Empty).Select(cell => cell.Coordinates));
            }

            if (IsWinningPositionPossible(board.RightToLeftDiagonal))
            {
                winningOptions.AddRange(board.RightToLeftDiagonal.Where(cell => cell.Value == CellMarker.Empty).Select(cell => cell.Coordinates));
            }

            if (winningOptions.Any())
            {
                return (true, winningOptions.ToArray());
            }

            return (false, default);

            bool IsWinningPositionPossible(Cell<CellMarker>[] cells)
            {
                return cells.Count(cell => cell.Value == CellMarker.Empty) == 2 &&
                       cells.Any(cell => cell.Value == marker);
            }
        }
    }
}