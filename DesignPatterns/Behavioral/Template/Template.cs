namespace DesignPatterns.Behavioral.Template;

public static class Template
{
    public abstract class Game
    {
        public void Run()
        {
            Start();
            while (!HaveWinner)
                TakeTurn();
            Console.WriteLine($"Player {WinningPlayer} wins");
        }

        protected int _currentPlayer;
        protected readonly int _numberOfPlayers;

        protected Game(int numberOfPlayers)
        {
            _numberOfPlayers = numberOfPlayers;
        }

        protected abstract void Start();
        protected abstract void TakeTurn();
        protected abstract bool HaveWinner { get; }
        protected abstract int WinningPlayer { get; }
    }

    public class Chess : Game
    {
        public Chess() : base(2)
        {
        }

        protected override bool HaveWinner => _turn == _maxTurns;

        protected override int WinningPlayer => _currentPlayer;

        protected override void Start()
        {
            Console.WriteLine($"Starting of game of chess with {_numberOfPlayers} players.");
        }

        protected override void TakeTurn()
        {
            Console.WriteLine($"Turn {_turn++} tkaen by player {_currentPlayer}");
            _currentPlayer = (_currentPlayer + 1) % _numberOfPlayers;
        }

        private int _turn = 1;
        private int _maxTurns = 10;
    }

    public static void Render()
    {
        var chess = new Chess();
        chess.Run();

        
    }
}
