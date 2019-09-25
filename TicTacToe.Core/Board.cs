using System;

namespace TicTacToe.Core
{

    public class Board<T>
    {
        private readonly int _gridSize;
        private Cell<T>[][] _cells;
        private Cell<T>[][] _colMajorCells;

        public Board(int gridSize = 3)
        {
            _gridSize = gridSize;
            InitCells();
        }

        public T this[int row, int col]
        {
            get
            {
                return _cells[row][col].Value;
            }
        }

        public int GridSize => _gridSize;

        public Cell<T>[][] Rows => _cells;

        public Cell<T>[][] Columns => _colMajorCells;

        public Cell<T>[] LeftToRightDiagonal
        {
            get
            {
                var result = new Cell<T>[3];
                for (var rowIndex = 0; rowIndex < _gridSize; rowIndex++)
                {
                    result[rowIndex] = _cells[rowIndex][rowIndex];
                }
                return result;
            }
        }

        public Cell<T>[] RightToLeftDiagonal
        {
            get
            {
                var result = new Cell<T>[3];
                for (var rowIndex = 0; rowIndex < _gridSize; rowIndex++)
                {
                    result[rowIndex] = _cells[rowIndex][(_gridSize - 1) - rowIndex];
                }
                return result;
            }
        }

        public void Fill(Coordinates coordinates, T value)
        {
            if (coordinates.X < 0 || coordinates.X >= _gridSize)
            {
                throw new ArgumentOutOfRangeException(nameof(coordinates.X), $"{nameof(coordinates.X)} must be between 0 and 2");
            }

            if (coordinates.Y < 0 || coordinates.Y >= _gridSize)
            {
                throw new ArgumentOutOfRangeException(nameof(coordinates.Y), $"{nameof(coordinates.Y)} must be between 0 and 2");
            }

            _cells[coordinates.X][coordinates.Y].Fill(value);
        }
        private void InitCells()
        {
            // create empty array
            _cells = new Cell<T>[_gridSize][];
            _colMajorCells = new Cell<T>[_gridSize][];
            for (var x = 0; x < _gridSize; x++)
            {
                _cells[x] = new Cell<T>[_gridSize];
                _colMajorCells[x] = new Cell<T>[_gridSize];
            }

            // fill with cells
            for (var x = 0; x < _gridSize; x++)
            {
                for (var y = 0; y < _gridSize; y++)
                {
                    _cells[x][y] = new Cell<T>(x, y);
                    _colMajorCells[y][x] = _cells[x][y];
                }
            }
        }
    }
}