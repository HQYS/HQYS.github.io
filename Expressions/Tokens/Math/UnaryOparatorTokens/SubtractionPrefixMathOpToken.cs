using System;

namespace Expressions.Tokens.Math
{
    internal class SubtractionPrefixMathOpToken : IUnaryMathOpToken
    {
        TokenTypeEnum IToken.Type => TokenTypeEnum.UnaryOperator;

        byte IUnaryOperatorToken.Priority => 5;

        UnaryMathOpTypeEnum IUnaryMathOpToken.OperationType => UnaryMathOpTypeEnum.PrefixSubtraction;

        string IToken.ToString()
        {
            return "¬";
        }

        public override string ToString()
        {
            return (this as IToken).ToString();
        }

        IConstantToken IUnaryOperatorToken.Calc(IConstantToken a)
        {
            if (a.DataType != DataTypeEnum.Decimal)
            {
                throw new Exception("Для операции математического префиксного унарного отрицания ожидался параметр типа 'decimal'.");
            }
            decimal result = (a as IDecimalConstantToken).Value * -1;
            return TokensRepository.GetDecimalConstantToken(result, false);
        }
    }
}
