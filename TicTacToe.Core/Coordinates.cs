namespace TicTacToe.Core
{
    public struct Coordinates
    {
        private readonly int _x;
        private readonly int _y;

        public Coordinates(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int X => _x;
        public int Y => _y;
    }
}