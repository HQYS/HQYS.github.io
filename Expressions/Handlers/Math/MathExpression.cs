using Expressions.Tokens;
using Expressions.Tokens.Math;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressions.Math
{
    public class MathExpression : IExpression
    {
        private readonly string _ident;
        private readonly List<IToken> _tokens;
        private readonly List<string> _errors = new List<string>();
        private readonly bool _isParsed;
        private readonly IEnumerable<string> _variables;

        public MathExpression(string ident, List<IToken> tokens, List<string> errors)
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

        ExpressionTypeEnum IExpression.ExpressionType => ExpressionTypeEnum.Math;

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
                        stack.Push((token as IUnaryMathOpToken).Calc(parametr));
                    }

                    else if (token.Type == TokenTypeEnum.BinaryOperator)
                    {                        
                        if (!TryGetConstantToken(stack.Pop(), out IConstantToken a) || !TryGetConstantToken(stack.Pop(), out IConstantToken b))
                            break;
                        stack.Push((token as IBinaryOperatorToken).Calc(b, a));
                    }

                    else if (token.Type == TokenTypeEnum.ComparisonOperator)
                    {
                        if (!TryGetConstantToken(stack.Pop(), out IConstantToken a) || !TryGetConstantToken(stack.Pop(), out IConstantToken b))
                            break;
                        stack.Push((token as IComparisionOperatorToken).Calc(b, a));
                    }

                    else if (token.Type == TokenTypeEnum.Function)
                    {
                        // сколько аргуметов у функции?
                        IDecimalConstantToken countArgument = stack.Pop() as IDecimalConstantToken;

                        int count = (int)System.Math.Round(countArgument.Value);
                        List<IConstantToken> arguments = new List<IConstantToken>();
                    
                        // соберем все аргументы
                        for (int i = 0; i < count; i++)
                        {
                            if (!TryGetConstantToken(stack.Pop(), out IConstantToken arg))
                                break;
                            arguments.Insert(0, arg);
                        }
                        
                        // были ошибки?
                        if (_errors.Any())
                            break;
                                                
                        // полужим занчение функции в стэк
                        stack.Push((token as IFunctionToken).Calc(arguments));
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
                    if (constantToken.DataType != DataTypeEnum.Decimal)
                    {
                        _errors.Add($"Тип последнего значения в стэке ожидался типа 'decimal'.");
                        return false;
                    }
                    value = new DecimalExpressionResult(_ident, (constantToken as IDecimalConstantToken).Value);
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
