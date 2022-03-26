namespace TinyWordle
{
    public readonly record struct GuessedWord(string Word, GuessedLetter[] GuessedLetters);
}
