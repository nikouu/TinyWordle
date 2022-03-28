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
