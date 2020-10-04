using System;

namespace Expressions.Tokens.Math
{
    internal class MoreMoreOrEqualComparisionOperatorToken : IComparisionOperatorToken
    {
        public static string Ident => ">=";

        string IBinaryOperatorToken.Ident => Ident;
        
        TokenTypeEnum IToken.Type => TokenTypeEnum.ComparisonOperator;

        ComparisonMathOpTypeEnum IComparisionOperatorToken.OperationType => ComparisonMathOpTypeEnum.MoreOrEqual;

        byte IBinaryOperatorToken.Priority => 1;

        string IToken.ToString()
        {
            return Ident;
        }

        public override string ToString()
        {
            return (this as IToken).ToString();
        }

        IConstantToken IBinaryOperatorToken.Calc(IConstantToken a, IConstantToken b)
        {
            if (a.DataType != DataTypeEnum.Decimal || b.DataType != DataTypeEnum.Decimal)
            {
                throw new Exception("Для операции сравнения 'больше либо равно' ожидались параметры типа 'decimal'.");
            }
            bool result = (a as IDecimalConstantToken).Value >= (b as IDecimalConstantToken).Value;
            return TokensRepository.GetBooleanConstantToken(result);
        }
    }
}
