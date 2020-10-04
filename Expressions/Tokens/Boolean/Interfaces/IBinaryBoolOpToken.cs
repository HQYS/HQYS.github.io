namespace Expressions.Tokens.Boolean
{
    internal interface IBinaryBoolOpToken : IBinaryOperatorToken
    {
        BinaryBoolOpTypeEnum OperationType { get; }
    }
}
