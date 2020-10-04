namespace Expressions.Tokens
{
    public interface IToken
    {
        TokenTypeEnum Type { get; }

        string ToString();
    }
}
