using Expressions.Tokens.Math;

namespace Expressions.Tokens
{
    internal interface IDecimalConstantToken : IConstantToken
    {
        decimal Value { get; }
    }
}
