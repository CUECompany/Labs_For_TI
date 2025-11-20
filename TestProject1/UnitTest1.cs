using Microsoft.VisualStudio.TestTools.UnitTesting;
using BillShifor.WpCalculator;

namespace BillShifor.Tests
{
    [TestClass]
    public class WpCalculatorTests
    {
        [TestMethod]
        public void TestCaesarSecurityAnalysis()
        {
            string code = @"function caesar_encrypt(text, key)
    shift := key % 32
    result := """"
    for i from 0 to text.length - 1
        char := text[i]
        if char in alphabet
            idx := alphabet.indexOf(char)
            new_idx := (idx + shift) % alphabet.length
            result := result + alphabet[new_idx]
        end if
    end for
    return result
end function";

            string postcondition = "decrypt(encrypt(text, key), key) = text";

            var result = CryptoWpEngine.AnalyzeCryptoAlgorithm("цезарь", code, postcondition);

            Assert.IsTrue(result.Steps.Count > 0);
            Assert.IsTrue(result.FinalPrecondition.Contains("shift"));
        }

        [TestMethod]
        public void TestCodeWithTabsAndSpaces()
        {
            string codeWithTabs = "function test()\n\tx := 5\n\ty := x + 3\n\treturn y\nend function";

            string postcondition = "y > 0";

            var result = CryptoWpEngine.AnalyzeCryptoAlgorithm("цезарь", codeWithTabs, postcondition);

            Assert.IsTrue(result.Steps.Count > 0);
        }

        [TestMethod]
        public void TestSimpleAssignment()
        {
            string code = "x := 5\ny := x + 10";
            string postcondition = "y > 10";

            var result = CryptoWpEngine.AnalyzeCryptoAlgorithm("цезарь", code, postcondition);

            Assert.IsTrue(result.FinalPrecondition.Contains("5"));
        }

        [TestMethod]
        public void TestSecurityEvaluation()
        {
            string code = "shift := 0\nresult := encrypt(text, shift)";
            string postcondition = "decrypt(result, shift) = text";

            var result = CryptoWpEngine.AnalyzeCryptoAlgorithm("цезарь", code, postcondition);

            // Сдвиг 0 должен быть небезопасным
            Assert.IsFalse(result.IsSecure);
        }
    }
}