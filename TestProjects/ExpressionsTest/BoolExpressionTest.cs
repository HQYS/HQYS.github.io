using Expressions;
using Expressions.Boolean;
using Expressions.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ExpressionsTest
{
    [TestClass]
    public class BoolExpressionTest
    {
        [DataTestMethod]

        [DataRow("ИСТИНА", true, DisplayName = "Считаем выражение: 'ИСТИНА'.")]
        [DataRow("ЛОЖЬ", false, DisplayName = "Считаем выражение: 'ЛОЖЬ'.")]

        [DataRow("not ИСТИНА", false, DisplayName = "Считаем выражение: 'not  ИСТИНА'.")]
        [DataRow("not ЛОЖЬ", true, DisplayName = "Считаем выражение: 'not ЛОЖЬ'.")]

        [DataRow("ИСТИНА ANd ЛОЖЬ", false, DisplayName = "Считаем выражение: 'ИСТИНА ANd ЛОЖЬ'.")]
        [DataRow("ИСТИНА OR ЛОЖЬ", true, DisplayName = "Считаем выражение: 'ИСТИНА OR ЛОЖЬ'.")]
        [DataRow("ИСТИНА XOR ЛОЖЬ", true, DisplayName = "Считаем выражение: 'ИСТИНА XOR ЛОЖЬ'.")]

        [DataRow("ИСТИНА ANd NOT ЛОЖЬ", true, DisplayName = "Считаем выражение: 'ИСТИНА ANd NOT ЛОЖЬ'.")]

        [DataRow("ИСТИНА ANd NOT (ЛОЖЬ OR ИСТИНА)", false, DisplayName = "Считаем выражение: 'ИСТИНА ANd NOT (ЛОЖЬ OR ИСТИНА)'.")]


        public void TestCalculation(string formula, bool validResult)
        {
            IConvertor convertor = new BoolConvertor();
            if (convertor.TryConvert(formula, out List<IToken> parsedFormula))
            {
                IExpression expression = new BoolExpression("Test", parsedFormula, null);
                if (expression.TryCalc(new List<IVariable>(), out IVariable result))
                {
                    string errMsg = $"Ожидали: '{validResult}'; Получили: '{result}'.";

                    Assert.IsTrue((result as IBooleanVariable).Value == validResult, errMsg);
                }
                else
                {
                    Assert.Fail(string.Join("; ", expression.Errors));
                }
            }
            else
            {
                Assert.Fail(string.Join("; ", convertor.Errors));
            }
        }

        [DataTestMethod]
        [DataRow("МКС( 1, 2)", DisplayName = "Считаем выражение: 'МКС( 1, 2)'.")]

        public void TestNotCorrectCalculation(string formula)
        {
            IConvertor convertor = new BoolConvertor();
            bool flag = convertor.TryConvert(formula, out List<IToken> parsedFormula);
            Assert.IsFalse(flag, "Ожидали ошибку.");
            if (!flag)
                return;
            IExpression expression = new BoolExpression("Test", parsedFormula, null);
            Assert.IsFalse(expression.TryCalc(new List<IVariable>(), out _), "Ожидали ошибку.");
        }

    }
}
