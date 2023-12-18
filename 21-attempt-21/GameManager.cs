using System.Runtime.CompilerServices;
using System.Text;

namespace TinyWordle
{
    public class GameManager
    {
        private string[] _wordList;
        private Random _random;

        public GameManager(string[] wordList)
        {
            _wordList = wordList;
            _random = new Random((uint)Environment.TickCount64);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Menu()
        {
            while (true)
            {
                GameLoop();
                TinyConsole.printf("\r\nHit any key to play again. Hit 'q' to quit.");

                var shouldContinue = TinyConsole.ReadLine();


                if (shouldContinue == "q")
                {
                    return;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GameLoop()
        {
            var index = _random.Next() % _wordList.Length;
            var word = _wordList[index];

            var game = new Game(word);

            DisplayGame(game.GuessedWords);

            while (true)
            {
                var guessedWord = TinyConsole.ReadLine();

                if (guessedWord.Length != 5)
                {
                    continue;
                }

                var result = game.Guess(guessedWord);
                DisplayGame(game.GuessedWords);

                if (result == State.Won)
                {
                    TinyConsole.printf("\r\nYou win!");
                    break;
                }
                else if (result == State.Lost)
                {
                    TinyConsole.printf("\r\nYou lose!");
                    break;
                }
            }
        }

        public void DisplayGame(GuessedWord[] guessedWords)
        {
            TinyConsole.system("cls");
            TinyConsole.printf("TinyWordle\r\n");

            foreach (GuessedWord guessedWord in guessedWords)
            {
                if (guessedWord.Word == null)
                {
                    TinyConsole.printf("#####\r\n");
                }
                else
                {
                    foreach (var guessedLetter in guessedWord.GuessedLetters)
                    {
                        if (guessedLetter.IsCorrect)
                        {
                            TinyConsole.printf($"\u001b[48;2;0;127;0m{guessedLetter.Letter}\x1B[0m");
                        }
                        else if (guessedLetter.IsRightLetterWrongPlace)
                        {
                            TinyConsole.printf($"\u001b[48;2;128;128;0m{guessedLetter.Letter}\x1B[0m");
                        }
                        else
                        {
                            TinyConsole.printf($"\u001b[48;2;128;128;127m{guessedLetter.Letter}\x1B[0m");
                        }
                    }

                    TinyConsole.printf("\r\n");
                }
            }
        }
    }
}
