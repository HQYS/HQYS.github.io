using Expressions.Boolean;
using Expressions.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ExpressionsTest
{
    [TestClass]
    public class BoolConvertorTest 
    {
        [DataTestMethod]

        [DataRow("ИСТИНА", "ИСТИНА", DisplayName = "Парсим выражение: 'ИСТИНА'.")]
        [DataRow("ЛОЖЬ", "ЛОЖЬ", DisplayName = "Парсим выражение: 'ЛОЖЬ'.")]

        [DataRow("not ИСТИНА", "ИСТИНА NOT", DisplayName = "Парсим выражение: 'not  ИСТИНА'.")]
        [DataRow("not ЛОЖЬ", "ЛОЖЬ NOT", DisplayName = "Парсим выражение: 'not ЛОЖЬ'.")]

        [DataRow("ИСТИНА ANd ЛОЖЬ", "ИСТИНА ЛОЖЬ AND", DisplayName = "Парсим выражение: 'ИСТИНА ANd ЛОЖЬ'.")]
        [DataRow("ИСТИНА OR ЛОЖЬ", "ИСТИНА ЛОЖЬ OR", DisplayName = "Парсим выражение: 'ИСТИНА OR ЛОЖЬ'.")]
        [DataRow("ИСТИНА XOR ЛОЖЬ", "ИСТИНА ЛОЖЬ XOR", DisplayName = "Парсим выражение: 'ИСТИНА XOR ЛОЖЬ'.")]

        [DataRow("ИСТИНА ANd NOT ЛОЖЬ", "ИСТИНА ЛОЖЬ NOT AND", DisplayName = "Парсим выражение: 'ИСТИНА ANd NOT ЛОЖЬ'.")]

        [DataRow("ИСТИНА ANd NOT (ЛОЖЬ OR ИСТИНА)", "ИСТИНА ЛОЖЬ ИСТИНА OR NOT AND", DisplayName = "Парсим выражение: 'ИСТИНА ANd NOT (ЛОЖЬ OR ИСТИНА)'.")]


        public void TestConvertor(string expression, string trueResult)
        {
            BoolConvertor convertor = new BoolConvertor();            
            convertor.TryConvert(expression, out List<IToken> parsedFormula);

            string result = string.Join(" ", parsedFormula );
            string errMsg = $"Ожидали: '{trueResult}'; Получили: '{result}'; Ошибки: '{string.Join("; ", convertor.Errors)}'.";

            Assert.IsTrue(string.Equals(result, trueResult), errMsg);
        }
    }
}
