using Expressions.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressions.Boolean
{
    public class BoolExpression : IExpression
    {
        private readonly string _ident;
        private readonly List<IToken> _tokens;
        private readonly List<string> _errors = new List<string>();
        private readonly bool _isParsed;
        private readonly IEnumerable<string> _variables;

        public BoolExpression(string ident, List<IToken> tokens, List<string> errors)
        {
            _ident = ident;
            if (errors != null)
                _errors.AddRange(errors);
            _isParsed = !_errors.Any();
            _tokens = tokens;
            _variables = tokens.Where(t => t.Type == TokenTypeEnum.Variable).Select(v => (v as IVariableToken).Ident).Distinct();
        }

        bool IExpression.IsParsed => _isParsed;

        IEnumerable<string> IExpression.Errors => _errors;

        IEnumerable<string> IExpression.Variables => _variables;

        ExpressionTypeEnum IExpression.ExpressionType => ExpressionTypeEnum.Boolean;

        string IExpression.Ident => _ident;

        IEnumerable<IVariable> _variableValues;

        bool IExpression.TryCalc(IEnumerable<IVariable> variableValues, out IVariable value)
        {
            value = null;
            Stack<IToken> stack = new Stack<IToken>();
            Queue<IToken> queue = new Queue<IToken>(_tokens);

            _variableValues = variableValues;
            try
            {
                IToken token = queue.Dequeue();
                while (queue.Count >= 0)
                {
                    if (token.Type == TokenTypeEnum.UnaryOperator)
                    {
                        if (!TryGetConstantToken(stack.Pop(), out IConstantToken parametr))
                            break;
                        stack.Push((token as IUnaryOperatorToken).Calc(parametr));
                    }

                    else if (token.Type == TokenTypeEnum.BinaryOperator)
                    {
                        if (!TryGetConstantToken(stack.Pop(), out IConstantToken a) || !TryGetConstantToken(stack.Pop(), out IConstantToken b))
                            break;
                        stack.Push((token as IBinaryOperatorToken).Calc(b, a));
                    }

                    else // токены константы и переменные
                    {
                        stack.Push(token);
                    }

                    if (queue.Count > 0)
                        token = queue.Dequeue();
                    else
                        break;
                }

                // в стеке должно быть последние итоговое значение
                if (TryGetConstantToken(stack.Pop(), out IConstantToken constantToken))
                {
                    if (constantToken.DataType != DataTypeEnum.Boolean)
                    {
                        _errors.Add($"Значение последнего значения в стаке ожидалось типа 'boolean'.");
                        return false;
                    }
                    value = new BooleanExpressionResult(_ident, (constantToken as IBooleanConstantToken).Value);
                }
            }
            catch (Exception ex)
            {
                _errors.Add($"Во время вычисления выражения произошла ошибка. {ex.Message}");
            }

            return !_errors.Any();
        }

        private bool TryGetConstantToken(IToken token, out IConstantToken value)
        {
            if (token.Type == TokenTypeEnum.Constant)
            {
                value = token as IConstantToken;
                return true;
            }

            if (token.Type == TokenTypeEnum.Variable)
            {
                IVariableToken variableToken = token as IVariableToken;
                if (!variableToken.TryTransformToConstantToken(_variableValues, out value))
                {
                    _errors.Add($"Среди переданных значений переменных не найдена переменная '{variableToken.Ident}'.");
                    value = null;
                    return false;
                }
                return true;
            }

            _errors.Add($"Не возможно получить значение из токена ({token.Type}; {token}).");
            value = null;
            return false;
        }
    }

}
