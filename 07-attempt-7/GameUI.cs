using System.Text;

namespace TinyWordle
{
    public class GameUI
    {
        public void DisplayGame(GuessedWord[] guessedWords)
        {
            Console.Clear();
            Console.WriteLine("TinyWordle");

            foreach (GuessedWord guessedWord in guessedWords)
            {
                if (string.IsNullOrEmpty(guessedWord.Word))
                {
                    Console.WriteLine("#####");
                }
                else
                {
                    var line = new StringBuilder();
                    foreach (var guessedLetter in guessedWord.GuessedLetters)
                    {
                        if (guessedLetter.IsCorrect)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(guessedLetter.Letter);
                            Console.ResetColor();
                        }
                        else if (guessedLetter.IsRightLetterWrongPlace)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(guessedLetter.Letter);
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(guessedLetter.Letter);
                            Console.ResetColor();
                        }
                    }

                    Console.Write("\r\n");
                }
            }
        }

        public void DisplayInvalidInput()
        {
            Console.WriteLine("Not a valid input.");
        }

        public void DisplayWonGame()
        {
            Console.WriteLine("You win!");
        }

        public void DisplayLostGame()
        {
            Console.WriteLine("You lose!");
        }

        public void ContinueScreen()
        {
            Console.WriteLine("Hit any key to play again. Hit 'q' to quit.");
        }
    }
}
