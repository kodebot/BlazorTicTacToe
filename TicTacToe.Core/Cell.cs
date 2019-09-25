namespace TicTacToe.Core
{
    public class Cell<T>
    {
        private T _value;
        private readonly Coordinates _coordinates;

        public Cell(int x, int y)
        {
            _value = default(T);
            _coordinates = new Coordinates(x, y);
        }

        public Coordinates Coordinates => _coordinates;
        public T Value => _value;

        public void Fill(T value)
        {
            _value = value;
        }
    }
}