using Expressions.Tokens;

namespace Expressions.Tokens.Math
{
    internal class ArgumentSeparatorToken : IToken
    {
        TokenTypeEnum IToken.Type => TokenTypeEnum.ArgumentSeparator;

        string IToken.ToString()
        {
            return ";";
        }

        public override string ToString()
        {
            return (this as IToken).ToString();
        }

    }
}
