using System.Runtime.CompilerServices;

namespace TinyWordle
{
    public class GameManager
    {
        private string[] _wordList;
        private int _attempts;
        private string _word;
        private GuessedWord[] _guessedWords;

        public GameManager(string[] wordList)
        {
            _wordList = wordList;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Menu()
        {
            while (true)
            {
                GameLoop();

                TinyConsole.Write("\r\nHit any key to play again. Hit 'q' to quit.");

                var shouldContinue = TinyConsole.ReadLine();
                if (shouldContinue.Length != 0 && shouldContinue[0] == 'q')
                {
                    return;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GameLoop()
        {
            var index = Random.Next() % _wordList.Length;
            _word = _wordList[index];
            _attempts = 0;
            _guessedWords = new GuessedWord[6];

            DisplayGame();

#if DEBUG
            Console.WriteLine(_word);
#endif

            while (true)
            {
                var guessedWord = TinyConsole.ReadLine();

                if (guessedWord.Length != 5)
                {
                    continue;
                }

                var result = Guess(guessedWord);
                DisplayGame();

                if (result == State.Won)
                {
                    TinyConsole.Write("\r\nYou win!");
                    break;
                }
                else if (result == State.Lost)
                {
                    TinyConsole.Write("\r\nYou lose!");
                    break;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public State Guess(string guessedWord)
        {
            _attempts++;
            _guessedWords[_attempts - 1] = new GuessedWord
            {
                Word = guessedWord,
                GuessedLetters =
                [
                    new GuessedLetter(guessedWord[0]),
                    new GuessedLetter(guessedWord[1]),
                    new GuessedLetter(guessedWord[2]),
                    new GuessedLetter(guessedWord[3]),
                    new GuessedLetter(guessedWord[4])
                ]
            };

            if (guessedWord == _word)
            {
                return State.Won;
            }

            if (_attempts >= 6)
            {
                return State.Lost;
            }

            return State.Playing;
        }

        public void DisplayGame()
        {
            TinyConsole.Clear();
            TinyConsole.Write("TinyWordle\r\n");

            foreach (GuessedWord guessedWord in _guessedWords)
            {
                if (guessedWord.Word == null)
                {
                    TinyConsole.Write("#####\r\n");
                }
                else
                {                    
                    for (int i = 0; i < 5; i++)
                    {
                        var guessedLetter = guessedWord.GuessedLetters[i];
                        var ansiColour = "\u001b[48;2;128;128;127m";

                        if (guessedLetter.IsCorrect(_word[i]))
                        {
                            ansiColour = "\u001b[48;2;0;127;0m";
                        }
                        else if (guessedLetter.IsRightLetterWrongPlace(_word))
                        {
                            ansiColour = "\u001b[48;2;128;128;0m";
                        }

                        // Doing the interpolation simplification brings in methods to append chars
                        // taking a ToString() makes the concatenation very simple code
                        TinyConsole.Write($"{ansiColour}{guessedLetter.Letter.ToString()}\u001b[0m");
                    }

                    TinyConsole.Write("\r\n");
                }
            }
        }
    }
}
