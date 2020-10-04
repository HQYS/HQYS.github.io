using Expressions.Tokens;

namespace Expressions.Tokens
{
    internal interface IBinaryOperatorToken : IToken
    {
        string Ident { get; }

        byte Priority { get; }

        IConstantToken Calc(IConstantToken a, IConstantToken b);
    }
}
