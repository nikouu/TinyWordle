namespace TinyWordle
{
    public class GameUI
    {
        public void DisplayGame(GuessedWord[] guessedWords)
        {
            Console.Clear();
            Console.Write("TinyWordle\r\n");

            foreach (GuessedWord guessedWord in guessedWords)
            {
                if (string.IsNullOrEmpty(guessedWord.Word))
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
                            Console.Write(guessedLetter.Letter);
                        }
                        else if (guessedLetter.IsRightLetterWrongPlace)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                            Console.Write(guessedLetter.Letter);
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.Write(guessedLetter.Letter);                            
                        }
                    }

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("\r\n");
                }
            }
        }

        public void DisplayWonGame()
        {
            Console.Write("\r\nYou win!");
        }

        public void DisplayLostGame()
        {
            Console.Write("\r\nYou lose!");
        }

        public void ContinueScreen()
        {
            Console.Write("\r\nHit any key to play again. Hit 'q' to quit.");
        }
    }
}
