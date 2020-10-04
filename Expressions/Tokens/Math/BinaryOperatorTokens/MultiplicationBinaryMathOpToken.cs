using System;

namespace Expressions.Tokens.Math
{
    internal class MultiplicationBinaryMathOpToken : IBinaryMathOpToken
    {
        public static string Ident => "*";

        string IBinaryOperatorToken.Ident => Ident;

        TokenTypeEnum IToken.Type => TokenTypeEnum.BinaryOperator;

        BinaryMathOpTypeEnum IBinaryMathOpToken.OperationType => BinaryMathOpTypeEnum.Multiplication;

        byte IBinaryOperatorToken.Priority => 3;

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
                throw new Exception("Для операции математического умножения ожидались параметры типа 'decimal'.");
            }
            decimal result = (a as IDecimalConstantToken).Value * (b as IDecimalConstantToken).Value;
            return TokensRepository.GetDecimalConstantToken(result, false);
        }
    }
}
