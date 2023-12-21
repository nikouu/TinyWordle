namespace TinyWordle
{
    public class GameManager
    {
        private string[] _wordList;
        private GameUI _gameUI;

        public GameManager(string[] wordList)
        {
            _wordList = wordList;
            _gameUI = new GameUI();
        }

        public void Menu()
        {
            while (true)
            {
                GameLoop();
                _gameUI.ContinueScreen();

                var shouldContinue = Console.ReadKey();

                if (shouldContinue.KeyChar == 'q')
                {
                    return;
                }
            }
        }

        private void GameLoop()
        {
            var random = new Random();
            var wordIndex = random.Next(_wordList.Length);
            var word = _wordList[wordIndex];

            var game = new Game(word);

            while (true)
            {
                _gameUI.DisplayGame(game.GuessedWords);

                var guessedWord = Console.ReadLine()?.ToLower();

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

        // see if the ? brings in way more code
        // also see if its worth turning that off to reduce overhead code?
        private bool IsValidGuess(string? guess)
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
