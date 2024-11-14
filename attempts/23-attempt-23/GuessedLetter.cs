namespace TinyWordle
{
    public struct GuessedLetter
    {
        public char Letter;
        public bool IsCorrect(char wordLetter) => wordLetter == Letter;

        public bool IsRightLetterWrongPlace(string word) => Contains(word, Letter);

        public GuessedLetter(char letter)
        {
            Letter = letter;
        }

        // https://stackoverflow.com/a/54129140
        private bool Contains(string stringToSearch, char characterToFind)
        {
            for (int i = 0; i < 5; i++)
            {
                if (stringToSearch[i] == characterToFind)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
