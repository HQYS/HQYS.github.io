namespace Expressions
{
    public class BooleanExpressionResult : IBooleanVariable
    {
        private bool _value;
        private readonly string _ident;

        bool IBooleanVariable.Value { get => _value; set => _value = value; }

        DataTypeEnum IVariable.DataType => DataTypeEnum.Boolean;

        string IVariable.Ident => _ident;

        public BooleanExpressionResult(string ident, bool value)
        {
            _ident = ident;
            _value = value;
        }
    }
}
