using Cloudcrate.AspNetCore.Blazor.Browser.Storage;

namespace Tictactoe.Model
{
    public class Settings
    {
        public static class Keys
        {
            public const string ComputerPlayerMode = "ComputerPlayerMode";
        }
        
        private readonly LocalStorage _storage;

        public Settings(LocalStorage storage)
        {
            _storage = storage;
        }
        
        public ComputerPlayerMode ComputerPlayerMode
        {
            get => _storage.GetItem<ComputerPlayerMode>(Keys.ComputerPlayerMode);
            set => _storage.SetItem(Keys.ComputerPlayerMode, value);
        }
    }
}