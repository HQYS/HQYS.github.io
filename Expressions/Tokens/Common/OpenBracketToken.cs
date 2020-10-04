namespace Expressions.Tokens
{
    internal class OpenBracketToken : IToken
    {
        TokenTypeEnum IToken.Type => TokenTypeEnum.OpenBracket;

        string IToken.ToString()
        {
            return "(";
        }

        public override string ToString()
        {
            return (this as IToken).ToString();
        }

    }
}
