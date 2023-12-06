using System.Runtime.CompilerServices;

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
                Console.Write("\r\nHit any key to play again. Hit 'q' to quit.");

                var shouldContinue = Console.ReadLine();

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
                var guessedWord = Console.ReadLine();

                if (guessedWord.Length != 5)
                {
                    continue;
                }

                var result = game.Guess(guessedWord);
                DisplayGame(game.GuessedWords);

                if (result == State.Won)
                {
                    Console.Write("\r\nYou win!");
                    break;
                }
                else if (result == State.Lost)
                {
                    Console.Write("\r\nYou lose!");
                    break;
                }
            }
        }

        public void DisplayGame(GuessedWord[] guessedWords)
        {
            Console.Clear();
            Console.Write("TinyWordle\r\n");

            foreach (GuessedWord guessedWord in guessedWords)
            {
                if (guessedWord.Word == null)
                {
                    Console.Write("#####\r\n");
                }
                else
                {
                    foreach (var guessedLetter in guessedWord.GuessedLetters)
                    {
                        if (guessedLetter.IsCorrect)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                            Console.Write(guessedLetter.Letter.ToString());
                        }
                        else if (guessedLetter.IsRightLetterWrongPlace)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                            Console.Write(guessedLetter.Letter.ToString());
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.Write(guessedLetter.Letter.ToString());
                        }
                    }

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("\r\n");
                }
            }
        }
    }
}
