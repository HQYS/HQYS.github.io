using System;

namespace Expressions.Tokens.Math
{
    internal class SubtractionBOperatorToken : IBinaryMathOpToken
    {
        public static string Ident => "-";

        string IBinaryOperatorToken.Ident => Ident;

        TokenTypeEnum IToken.Type => TokenTypeEnum.BinaryOperator;

        BinaryMathOpTypeEnum IBinaryMathOpToken.OperationType => BinaryMathOpTypeEnum.Subtraction;

        byte IBinaryOperatorToken.Priority => 2;

        string IToken.ToString()
        {
            return Ident.ToString();
        }

        public override string ToString()
        {
            return (this as IToken).ToString();
        }

        IConstantToken IBinaryOperatorToken.Calc(IConstantToken a, IConstantToken b)
        {
            if (a.DataType != DataTypeEnum.Decimal || b.DataType != DataTypeEnum.Decimal)
            {
                throw new Exception("Для операции математического вычитания ожидались параметры типа 'decimal'.");
            }
            decimal result = (a as IDecimalConstantToken).Value - (b as IDecimalConstantToken).Value;
            return TokensRepository.GetDecimalConstantToken(result, false);
        }
    }
}
