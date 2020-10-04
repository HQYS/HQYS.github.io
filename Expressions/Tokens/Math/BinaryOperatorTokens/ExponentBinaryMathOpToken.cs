using System;

namespace Expressions.Tokens.Math
{
    internal class ExponentBinaryMathOpToken : IBinaryMathOpToken
    {
        public static string Ident => "^";

        string IBinaryOperatorToken.Ident => Ident;

        TokenTypeEnum IToken.Type => TokenTypeEnum.BinaryOperator;

        BinaryMathOpTypeEnum IBinaryMathOpToken.OperationType => BinaryMathOpTypeEnum.Exponentiation;

        byte IBinaryOperatorToken.Priority => 4;

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
                throw new Exception("Для операции возведения в степень ожидались параметры типа 'decimal'.");
            }
            decimal result = Convert.ToDecimal(System.Math.Pow(
                decimal.ToDouble((a as IDecimalConstantToken).Value),
                decimal.ToDouble((b as IDecimalConstantToken).Value)
                ));
            return TokensRepository.GetDecimalConstantToken(result, false);
        }

    }
}
