using Expressions.Tokens;

namespace Expressions.Tokens.Math
{
    internal interface IBinaryMathOpToken : IBinaryOperatorToken
    {
        BinaryMathOpTypeEnum OperationType { get; }
    }
}
