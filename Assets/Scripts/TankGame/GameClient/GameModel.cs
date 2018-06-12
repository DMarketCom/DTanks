using SHLibrary.ObserverView;

namespace Game
{
    public enum GameMode
    {
        Online,
        Offline
    }

    public class GameModel : ObservableBase
    {
        public readonly GameMode Mode;
        
        public GameModel(GameMode mode)
        {
            Mode = mode;
        }
    }
}