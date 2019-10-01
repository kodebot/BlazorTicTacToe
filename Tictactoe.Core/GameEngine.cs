using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tictactoe.Core.Extensions;
using Tictactoe.Core.Player;

namespace Tictactoe.Core
{
    public class GameEngine
    {
        private readonly Board<CellMarker> _board;
        private GameStatus _status;
        private Coordinates[] _winningCoordinates;
        private readonly Player<CellMarker> _player1;
        private readonly Player<CellMarker> _player2;

        public event Action<GameStatus> GameStatusChanged;

        public GameEngine(Player<CellMarker> player1, Player<CellMarker> player2)
        {
            _player1 = player1;
            _player2 = player2;
            _board = new Board<CellMarker>();
        }

        public GameStatus Status => _status;

        public Board<CellMarker> Board => _board;

        public Coordinates[] WinningCoordinates => _winningCoordinates;

        public void Start()
        {
            if (_status == GameStatus.New)
            {
                _status = GameStatus.Player1Turn;
                Run();
                return;
            }

            throw new InvalidOperationException("Cannot start a game that have already been started");
        }

        private async void Run()
        {
            while (_status.IsInProgress())
            {
                var currentPlayer = GetCurrentPlayer();
                if (currentPlayer == null)
                {
                    throw new InvalidOperationException(
                        "Something is wrong, current player cannot be null at this point!");
                }

                var coords = await currentPlayer.GetNextMove(_board);
                _board.Fill(coords, currentPlayer.Marker);
                RecalculateGameStatus();
            }
        }

        private Player<CellMarker> GetCurrentPlayer()
        {
            return _status switch
            {
                GameStatus.Player1Turn => _player1,
                GameStatus.Player2Turn => _player2,
                _ => null
            };
        }

        private void RecalculateGameStatus()
        {
            var newStatus = GetNewGameStatus();

            if(newStatus != _status){
                _status = newStatus;

                GameStatusChanged?.Invoke(_status);
            }
        }

        private GameStatus GetNewGameStatus()
        {
            var (hasWinner, winningCells) = GetWinner();

            if (hasWinner)
            {
                _winningCoordinates = winningCells.Select(cell => cell.Coordinates).ToArray();
                return GetWinnerGameStatus(winningCells.ToArray());
            }

            // any moves available?
            foreach (var row in _board.Rows)
            {
                if (row.Any(r => r.Value == CellMarker.Empty))
                {
                    return RotateTurn();
                }
            }

            return GameStatus.Tie;

            GameStatus GetWinnerGameStatus(Cell<CellMarker>[] row)
            {
                if (_player1.Marker == row[0].Value)
                {
                    return GameStatus.Player1Won;
                }
                
                if (_player2.Marker == row[0].Value)
                {
                    return GameStatus.Player2Won;
                }

                throw new InvalidOperationException("This is impossible!");
            }

            GameStatus RotateTurn()
            {
                return _status switch
                {
                    GameStatus.Player1Turn => GameStatus.Player2Turn,
                    GameStatus.Player2Turn => GameStatus.Player1Turn,
                    _ => _status // ignore
                };
            }
        }

        private (bool, Cell<CellMarker>[]) GetWinner()
        {
            var winningCells = new List<Cell<CellMarker>>();

            // horizontals
            foreach (var row in _board.Rows)
            {
                if (IsAllMarkedWithSameNonEmptyValue(row))
                {
                    winningCells = row.ToList();
                    return (true, winningCells.ToArray());
                }
            }

            // verticals
            foreach (var col in _board.Columns)
            {
                if (IsAllMarkedWithSameNonEmptyValue(col))
                {
                    winningCells = col.ToList();
                    return (true, winningCells.ToArray());
                }
            }

            // diagonal - top left to bottom right
            if (IsAllMarkedWithSameNonEmptyValue(_board.LeftToRightDiagonal))
            {
                winningCells = _board.LeftToRightDiagonal.ToList();
                return (true, winningCells.ToArray());
            }

            // diagonal - top right to bottom left
            if (!IsAllMarkedWithSameNonEmptyValue(_board.RightToLeftDiagonal))
            {
                return (false, winningCells.ToArray()); // no winner
            }

            winningCells = _board.RightToLeftDiagonal.ToList();
            return (true, winningCells.ToArray());
        }

        private static bool IsAllMarkedWithSameNonEmptyValue(Cell<CellMarker>[] cells)
        {
            return cells.All(c => c.Value != CellMarker.Empty) && cells.GroupBy(c => c.Value).Count() == 1;
        }
    }
}