namespace TinyWordle
{
    public class Game
    {
        private int _attempts;
        private string _word;
        public GuessedWord[] GuessedWords { get; private set; }

        public Game(string word)
        {
            _word = word;
            _attempts = 0;
            GuessedWords = new GuessedWord[6];
        }


        public State Guess(string guessedWord)
        {
            _attempts++;

            GuessedWords[_attempts - 1] = new GuessedWord
            {
                Word = guessedWord,
                GuessedLetters = new GuessedLetter[5]
                {
                    new GuessedLetter { Letter = guessedWord[0], IsRightLetterWrongPlace = _word.Contains(guessedWord[0]), IsCorrect = guessedWord[0] == _word[0] },
                    new GuessedLetter { Letter = guessedWord[1], IsRightLetterWrongPlace = _word.Contains(guessedWord[1]), IsCorrect = guessedWord[1] == _word[1] },
                    new GuessedLetter { Letter = guessedWord[2], IsRightLetterWrongPlace = _word.Contains(guessedWord[2]), IsCorrect = guessedWord[2] == _word[2] },
                    new GuessedLetter { Letter = guessedWord[3], IsRightLetterWrongPlace = _word.Contains(guessedWord[3]), IsCorrect = guessedWord[3] == _word[3] },
                    new GuessedLetter { Letter = guessedWord[4], IsRightLetterWrongPlace = _word.Contains(guessedWord[4]), IsCorrect = guessedWord[4] == _word[4] }
                }
            };

            if (guessedWord.ToLower() == _word)
            {
                return State.Won;
            }

            if (_attempts >= 6)
            {
                return State.Lost;
            }

            return State.Playing;
        }
    }
}
