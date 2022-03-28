namespace TinyWordle
{
    public class GameManager
    {
        private string[] _wordList;
        private GameUI _gameUI;
        private Random _random;

        public GameManager(string[] wordList)
        {
            _wordList = wordList;
            _gameUI = new GameUI();
            _random = new Random((uint)Environment.TickCount64);
        }

        public void Menu()
        {
            while (true)
            {
                GameLoop();
                _gameUI.ContinueScreen();

                var shouldContinue = Console.ReadLine();

                if (shouldContinue == "q")
                {
                    return;
                }
            }
        }

        private void GameLoop()
        {
            var index = _random.Next() % _wordList.Length;
            var word = _wordList[index];

            var game = new Game(word);

            while (true)
            {
                _gameUI.DisplayGame(game.GuessedWords);

                var guessedWord = Console.ReadLine();

                if (!IsValidGuess(guessedWord))
                {
                    continue;
                }

                var result = game.Guess(guessedWord);

                if (result == State.Playing)
                {
                    continue;
                }
                else if (result == State.Won)
                {
                    _gameUI.DisplayGame(game.GuessedWords);
                    _gameUI.DisplayWonGame();
                    break;
                }
                else if (result == State.Lost)
                {
                    _gameUI.DisplayGame(game.GuessedWords);
                    _gameUI.DisplayLostGame();
                    break;
                }
            }
        }

        private bool IsValidGuess(string guess)
        {
            if (guess == null)
            {
                return false;
            }
            else if (guess.Length != 5)
            {
                return false;
            }

            return true;
        }
    }
}
