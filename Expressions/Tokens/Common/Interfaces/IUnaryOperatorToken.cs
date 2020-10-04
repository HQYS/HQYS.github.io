namespace Expressions.Tokens
{
    internal interface IUnaryOperatorToken : IToken
    {

        byte Priority { get; }

        IConstantToken Calc(IConstantToken a);
    }
}
