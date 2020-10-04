using Expressions.Tokens.Boolean;
using Expressions.Tokens.Math;
using System.Collections.Generic;

namespace Expressions.Tokens
{
    internal static class TokensRepository
    {
        private static readonly Dictionary<string, IBinaryMathOpToken> _binaryMathOps = new Dictionary<string, IBinaryMathOpToken>()
        {
            { AdditionBinaryMathOpToken.Ident, new AdditionBinaryMathOpToken() },
            { SubtractionBOperatorToken.Ident, new SubtractionBOperatorToken() },
            { MultiplicationBinaryMathOpToken.Ident, new MultiplicationBinaryMathOpToken() },
            { DivisionBinaryMathOpToken.Ident, new DivisionBinaryMathOpToken() },
            { ExponentBinaryMathOpToken.Ident, new ExponentBinaryMathOpToken() },
        };

        private static readonly Dictionary<string, IComparisionOperatorToken> _comparisionOperations = new Dictionary<string, IComparisionOperatorToken>()
        {
            { EqualComparisionOperatorToken.Ident, new EqualComparisionOperatorToken() },
            { MoreComparisionOperatorToken.Ident, new MoreComparisionOperatorToken() },
            { LessComparisionOperatorToken.Ident, new LessComparisionOperatorToken() },
            { MoreMoreOrEqualComparisionOperatorToken.Ident, new MoreMoreOrEqualComparisionOperatorToken() },
            { LessOrEqualComparisionOperatorToken.Ident, new LessOrEqualComparisionOperatorToken() },
            { NotEqualComparisionOperatorToken.Ident, new NotEqualComparisionOperatorToken() },
        };

        internal static IConstantToken GetConstantToken(IVariable variableValue)
        {
            switch (variableValue.DataType)
            {
                case DataTypeEnum.Boolean:
                    return GetBooleanConstantToken((variableValue as IBooleanVariable).Value);

                case DataTypeEnum.Decimal:
                    return GetDecimalConstantToken((variableValue as IDecimalVariable).Value, false);
            }
            return null;
        }

        private static readonly Dictionary<decimal, IDecimalConstantToken> _decimalConstants = new Dictionary<decimal, IDecimalConstantToken>();

        private static readonly Dictionary<string, IVariableToken> _variables = new Dictionary<string, IVariableToken>();

        private static readonly Dictionary<string, IFunctionToken> _functions = new Dictionary<string, IFunctionToken>()
        {
            { MaxFunctionToken.Name, new MaxFunctionToken() },
            { MinFunctionToken.Name, new MinFunctionToken() },
            { MultipleConditionFunctionToken.Name, new MultipleConditionFunctionToken() }
        };

        private static readonly Dictionary<string, IBinaryBoolOpToken> _binaryBoolOps = new Dictionary<string, IBinaryBoolOpToken>()
        {
            { AndBinaryBoolOpToken.Ident, new AndBinaryBoolOpToken() },
            { OrBinaryBoolOpToken.Ident, new OrBinaryBoolOpToken() },
            { XorBinaryBoolOpToken.Ident, new XorBinaryBoolOpToken() },
        };



        public static IToken ArgumentSeparartor { get; } = new ArgumentSeparatorToken();
        public static IToken OpenBracket { get; } = new OpenBracketToken();
        public static IToken CloseBracket { get; } = new CloseBracketToken();
        public static IToken SubtractionPrefixMathOperator { get; } = new SubtractionPrefixMathOpToken();
        public static IBooleanConstantToken TrueConstant { get; } = new TrueBoolConstantToken();
        public static IBooleanConstantToken FalseConstant { get; } = new FalseBoolConstantToken();
        public static IToken NotPrefixUnaryBoolOpToken { get; } = new NotPrefixUnaryBoolOpToken();

        public static IBinaryMathOpToken GetBinaryMathOpToken(string operation)
        {
            _binaryMathOps.TryGetValue(operation, out IBinaryMathOpToken result);
            return result;
        }

        public static IBinaryBoolOpToken GetBinaryBoolOpToken(string operation)
        {
            _binaryBoolOps.TryGetValue(operation, out IBinaryBoolOpToken result);
            return result;
        }

        public static IComparisionOperatorToken GetComparisionOperatorToken(string operation)
        {
            _comparisionOperations.TryGetValue(operation, out IComparisionOperatorToken result);
            return result;
        }

        public static IDecimalConstantToken GetDecimalConstantToken(decimal value, bool isCache = true)
        {
            if (!_decimalConstants.TryGetValue(value, out IDecimalConstantToken result))
            {
                result = new DecimalConstantToken(value);
                if (isCache)
                    _decimalConstants.Add(value, result);
            }
            return result;
        }

        public static IBooleanConstantToken GetBooleanConstantToken(bool value)
        {
            return value ? TrueConstant : FalseConstant;
        }

        public static IVariableToken GetVariableToken(string name)
        {
            if (!_variables.TryGetValue(name, out IVariableToken result))
            {
                result = new VariableToken(name);
                _variables.Add(name, result);
            }
            return result;
        }

        public static IFunctionToken GetFunctionToken(string name)
        {
            _functions.TryGetValue(name, out IFunctionToken result);
            return result;
        }
    }
}
