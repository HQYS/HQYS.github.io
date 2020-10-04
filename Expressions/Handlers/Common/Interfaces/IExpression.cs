using System.Collections.Generic;

namespace Expressions
{
    public interface IExpression
    {
        string Ident { get; }

        ExpressionTypeEnum ExpressionType { get; }

        bool IsParsed { get; }

        IEnumerable<string> Errors { get; }

        IEnumerable<string> Variables { get; }

        bool TryCalc(IEnumerable<IVariable> variableValues, out IVariable value);

    }
}
