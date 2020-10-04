using Expressions.Tokens.Math;
using Expressions.Tokens;
using System.Collections.Generic;
using System.Linq;

namespace Expressions
{
    internal static class TokenUtils
    {
        public static byte GetPriority(this IToken token)
        {
            return
                token.Type == TokenTypeEnum.BinaryOperator ? (token as IBinaryOperatorToken).Priority :
                token.Type == TokenTypeEnum.UnaryOperator ? (token as IUnaryOperatorToken).Priority :
                token.Type == TokenTypeEnum.ComparisonOperator ? (token as IComparisionOperatorToken).Priority : byte.MinValue;
        }


        public static bool TryTransformToConstantToken(this IVariableToken variable, IEnumerable<IVariable> variableValues, out IConstantToken value)
        {
            IVariable variableValue = variableValues.FirstOrDefault(v => string.Equals(v.Ident, variable.Ident));
            if (variableValue == null)
            {
                value = null;
                return false;
            }

            value = TokensRepository.GetConstantToken(variableValue);
            return true;
        }

    }
}
