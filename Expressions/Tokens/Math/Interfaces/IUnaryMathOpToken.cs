using Expressions.Tokens;

namespace Expressions.Tokens.Math
{
    internal interface IUnaryMathOpToken : IUnaryOperatorToken
    {
        UnaryMathOpTypeEnum OperationType { get; }
    }
}
