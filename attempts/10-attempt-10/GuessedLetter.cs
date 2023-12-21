namespace TinyWordle
{
    public readonly record struct GuessedLetter(char Letter, bool IsRightLetterWrongPlace, bool IsCorrect);
}
