using Expressions.Boolean;
using Expressions.Math;
using Expressions.Tokens;
using System.Collections.Generic;

namespace Expressions
{
    public class ExpressionHandler
    {
        private readonly Dictionary<string, List<IToken>> _parsedMathFormulas = new Dictionary<string, List<IToken>>();
        private readonly Dictionary<string, List<IToken>> _parsedBoolFormulas = new Dictionary<string, List<IToken>>();

        #region Статические члены

        private static readonly ExpressionHandler _instance = new ExpressionHandler();

        public static bool TryGetExpression(string ident, string formula, ExpressionTypeEnum expressionType, out IExpression expression)
        {
            return expressionType == ExpressionTypeEnum.Math
                    ? _instance.GetMathExpression(ident, formula, out expression)
                    : _instance.GetBoolExpression(ident, formula, out expression);
        }

        #endregion

        #region private Math functions

        private bool GetMathExpression(string ident, string formula, out IExpression expression)
        {
            if ( !_parsedMathFormulas.TryGetValue(formula, out List<IToken> tokens)) 
            {
                // еще не парсили такую функцию
                IConvertor convertor = GetMathConverter();
                if (!convertor.TryConvert(formula, out tokens))
                {
                    expression = GetMathExpression(ident, tokens, convertor.Errors);
                    return false;
                }
                _parsedMathFormulas.Add(formula, tokens);
            }
            expression = GetMathExpression(ident, tokens);
            return true;
        }

        private IConvertor GetMathConverter()
        {
            return new MathConvertor();
        }

        private IExpression GetMathExpression(string ident, List<IToken> parsedFormula, List<string> errors = null)
        {
            return new MathExpression(ident, parsedFormula, errors);
        }

        #endregion

        #region private Bool functions

        private bool GetBoolExpression(string ident, string formula, out IExpression expression)
        {
            if (!_parsedBoolFormulas.TryGetValue(formula, out List<IToken> tokens))
            {
                IConvertor convertor = GetBoolConverter();
                if (!convertor.TryConvert(formula, out tokens))
                {
                    expression = GetBoolExpression(ident, tokens, convertor.Errors);
                    return false;
                }
                _parsedBoolFormulas.Add(formula, tokens);
            }
            expression = GetBoolExpression(ident, tokens);
            return true;
        }

        private IConvertor GetBoolConverter()
        {
            return new BoolConvertor();
        }

        private IExpression GetBoolExpression(string ident, List<IToken> parsedFormula, List<string> errors = null)
        {
            return new BoolExpression(ident, parsedFormula, errors);
        }

        #endregion

    }
}
