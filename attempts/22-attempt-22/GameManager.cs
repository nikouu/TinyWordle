using System.Runtime.CompilerServices;

namespace TinyWordle
{
    public class GameManager
    {
        private string[] _wordList;

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
            var word = _wordList[index];

            var game = new Game(word);

            DisplayGame(game.GuessedWords);

#if DEBUG
            Console.WriteLine(word);
#endif

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

        public void DisplayGame(GuessedWord[] guessedWords)
        {
            TinyConsole.Clear();
            TinyConsole.Write("TinyWordle\r\n");

            foreach (GuessedWord guessedWord in guessedWords)
            {
                if (guessedWord.Word == null)
                {
                    TinyConsole.Write("#####\r\n");
                }
                else
                {
                    foreach (var guessedLetter in guessedWord.GuessedLetters)
                    {
                        var ansiColour = "\u001b[48;2;128;128;127m";

                        if (guessedLetter.IsCorrect)
                        {
                            ansiColour = "\u001b[48;2;0;127;0m";
                        }
                        else if (guessedLetter.IsRightLetterWrongPlace)
                        {
                            ansiColour = "\u001b[48;2;128;128;0m";
                        }

                        TinyConsole.Write($"{ansiColour}{guessedLetter.Letter}\u001b[0m");
                    }

                    TinyConsole.Write("\r\n");
                }
            }
        }
    }
}
