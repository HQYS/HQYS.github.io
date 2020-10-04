namespace Expressions
{
    public class DecimalExpressionResult : IDecimalVariable
    {
        private decimal _value;
        private readonly string _ident;

        decimal IDecimalVariable.Value { get => _value; set => _value = value; }

        DataTypeEnum IVariable.DataType => DataTypeEnum.Boolean;

        string IVariable.Ident => _ident;

        public DecimalExpressionResult(string ident, decimal value)
        {
            _ident = ident;
            _value = value;
        }
    }
}
