namespace Expressions.Tokens
{
    internal class CloseBracketToken : IToken
    {
        TokenTypeEnum IToken.Type => TokenTypeEnum.CloseBracket;

        string IToken.ToString()
        {
            return ")";
        }

        public override string ToString()
        {
            return (this as IToken).ToString();
        }

    }
}
