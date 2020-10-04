using Expressions.Tokens;

namespace Expressions.Tokens.Math
{
    internal interface IComparisionOperatorToken : IBinaryOperatorToken
    {
        ComparisonMathOpTypeEnum OperationType { get; }
    }
}
