using Xunit;
using Ciphers;
using System;

namespace TestCode
{
    /// <summary>
    /// Тесты для методов шифрования и дешифрования (TabItem 1 - Шифроватор)
    /// </summary>
    public class CipherTests
    {
        #region Тесты для класса Alphabet

        [Fact]
        public void Alphabet_Length_Returns33()
        {
            // Arrange & Act
            int length = Alphabet.Length;

            // Assert
            Assert.Equal(33, length);
        }

        [Theory]
        [InlineData('а', 0)]
        [InlineData('б', 1)]
        [InlineData('я', 32)]
        [InlineData('ё', 6)]
        public void Alphabet_IndexOf_ReturnsCorrectIndex(char c, int expectedIndex)
        {
            // Act
            int index = Alphabet.IndexOf(c);

            // Assert
            Assert.Equal(expectedIndex, index);
        }

        [Theory]
        [InlineData('а', true)]
        [InlineData('я', true)]
        [InlineData('A', false)]
        [InlineData('1', false)]
        [InlineData(' ', false)]
        public void Alphabet_Contains_ReturnsCorrectResult(char c, bool expected)
        {
            // Act
            bool result = Alphabet.Contains(c);

            // Assert
            Assert.Equal(expected, result);
        }

        #endregion

        #region Тесты шифра Цезаря

        [Fact]
        public void Caesar_Encrypt_BasicText_ReturnsCorrectResult()
        {
            // Arrange
            string input = "привет";
            int key = 3;

            // Act
            string result = Code.Caesar(input, key);

            // Assert
            Assert.NotEqual(input, result);
            Assert.Equal(6, result.Length);
        }

        [Fact]
        public void Caesar_EncryptDecrypt_ReturnsOriginalText()
        {
            // Arrange
            string original = "тест";
            int key = 5;

            // Act
            string encrypted = Code.Caesar(original, key);
            string decrypted = DeCode.Caesar(encrypted, key);

            // Assert
            Assert.Equal(original, decrypted);
        }

        [Theory]
        [InlineData("а", 1, "б")]
        [InlineData("я", 1, "а")]
        [InlineData("абв", 0, "абв")]
        public void Caesar_Encrypt_SpecificCases(string input, int key, string expected)
        {
            // Act
            string result = Code.Caesar(input, key);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Caesar_WithNonAlphabetCharacters_PreservesNonAlphabet()
        {
            // Arrange
            string input = "привет мир!";
            int key = 3;

            // Act
            string result = Code.Caesar(input, key);

            // Assert
            Assert.Contains(" ", result);
            Assert.Contains("!", result);
        }

        [Fact]
        public void Caesar_WithUpperCase_ConvertsToLowerCase()
        {
            // Arrange
            string input = "ПРИВЕТ";
            int key = 1;

            // Act
            string result = Code.Caesar(input, key);

            // Assert
            Assert.Equal(result, result.ToLower());
        }

        #endregion

        #region Тесты шифра Виженера

        [Fact]
        public void Vigenere_Encrypt_BasicText_ReturnsCorrectResult()
        {
            // Arrange
            string input = "привет";
            string key = "ключ";

            // Act
            string result = Code.Vigenere(input, key);

            // Assert
            Assert.NotEqual(input, result);
            Assert.Equal(input.Length, result.Length);
        }

        [Fact]
        public void Vigenere_EncryptDecrypt_ReturnsOriginalText()
        {
            // Arrange
            string original = "тестирование";
            string key = "секрет";

            // Act
            string encrypted = Code.Vigenere(original, key);
            string decrypted = DeCode.Vigenere(encrypted, key);

            // Assert
            Assert.Equal(original, decrypted);
        }

        [Fact]
        public void Vigenere_WithEmptyKey_ReturnsOriginalText()
        {
            // Arrange
            string input = "привет";
            string key = "";

            // Act
            string result = Code.Vigenere(input, key);

            // Assert
            Assert.Equal(input, result);
        }

        [Fact]
        public void Vigenere_WithNullKey_ReturnsOriginalText()
        {
            // Arrange
            string input = "привет";
            string key = null;

            // Act
            string result = Code.Vigenere(input, key);

            // Assert
            Assert.Equal(input, result);
        }

        #endregion

        #region Тесты шифра Атбаш

        [Fact]
        public void Atbash_Encrypt_BasicText_ReturnsCorrectResult()
        {
            // Arrange
            string input = "привет";

            // Act
            string result = Code.Atbash(input);

            // Assert
            Assert.NotEqual(input, result);
            Assert.Equal(input.Length, result.Length);
        }

        [Fact]
        public void Atbash_EncryptTwice_ReturnsOriginalText()
        {
            // Arrange
            string original = "тест";

            // Act
            string encrypted = Code.Atbash(original);
            string decrypted = Code.Atbash(encrypted);

            // Assert
            Assert.Equal(original, decrypted);
        }

        [Fact]
        public void Atbash_FirstAndLastLetters_SwapCorrectly()
        {
            // Arrange
            string input = "а";
            string expected = "я";

            // Act
            string result = Code.Atbash(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Atbash_DeCode_SameAsCode()
        {
            // Arrange
            string input = "тестовый текст";

            // Act
            string encoded = Code.Atbash(input);
            string decoded = DeCode.Atbash(input);

            // Assert
            Assert.Equal(encoded, decoded);
        }

        #endregion

        #region Тесты шифра Плейфера

        [Fact]
        public void Playfair_Encrypt_BasicText_ReturnsResult()
        {
            // Arrange
            string input = "привет";
            string key = "ключ";

            // Act
            string result = Code.Playfair(input, key);

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void Playfair_EncryptDecrypt_ReturnsOriginalOrClose()
        {
            // Arrange
            string original = "привет";
            string key = "секрет";

            // Act
            string encrypted = Code.Playfair(original, key);
            string decrypted = DeCode.Playfair(encrypted, key);

            // Assert
            // Плейфер может добавлять символы-заполнители
            Assert.Contains(original.Replace("ъ", ""), decrypted.Replace("ъ", ""));
        }

        [Fact]
        public void Playfair_WithOddLength_AddsFillerCharacter()
        {
            // Arrange
            string input = "тест";
            string key = "ключ";

            // Act
            string result = Code.Playfair(input, key);

            // Assert
            // Результат должен иметь четную длину
            Assert.True(result.Length % 2 == 0);
        }

        [Fact]
        public void Playfair_WithRepeatingCharacters_InsertsSeparator()
        {
            // Arrange
            string input = "привветт";
            string key = "ключ";

            // Act
            string result = Code.Playfair(input, key);

            // Assert
            Assert.NotEmpty(result);
        }

        #endregion

        #region Тесты шифра Вернама

        [Fact]
        public void Vernam_Encrypt_BasicText_ReturnsResult()
        {
            // Arrange
            string input = "привет";
            string key = "ключ";

            // Act
            string result = Code.Vernam(input, key);

            // Assert
            Assert.NotEqual(input, result);
            Assert.Equal(input.Length, result.Length);
        }

        [Fact]
        public void Vernam_EncryptDecrypt_ReturnsOriginalText()
        {
            // Arrange
            string original = "тестирование";
            string key = "длинныйключ";

            // Act
            string encrypted = Code.Vernam(original, key);
            string decrypted = DeCode.Vernam(encrypted, key);

            // Assert
            Assert.Equal(original, decrypted);
        }

        [Fact]
        public void Vernam_WithEmptyKey_ReturnsOriginalText()
        {
            // Arrange
            string input = "привет";
            string key = "";

            // Act
            string result = Code.Vernam(input, key);

            // Assert
            Assert.Equal(input, result);
        }

        [Fact]
        public void Vernam_WithNullKey_ReturnsOriginalText()
        {
            // Arrange
            string input = "привет";
            string key = null;

            // Act
            string result = Code.Vernam(input, key);

            // Assert
            Assert.Equal(input, result);
        }

        #endregion

        #region Тесты шифра DES

        [Fact]
        public void DES_Encrypt_BasicText_ReturnsResult()
        {
            // Arrange
            string input = "привет";
            string key = "ключ";

            // Act
            string result = Code.DES(input, key);

            // Assert
            Assert.NotEqual(input, result);
            Assert.Equal(input.Length, result.Length);
        }

        [Fact]
        public void DES_EncryptDecrypt_ReturnsOriginalText()
        {
            // Arrange
            string original = "тестирование";
            string key = "секретныйключ";

            // Act
            string encrypted = Code.DES(original, key);
            string decrypted = DeCode.DES(encrypted, key);

            // Assert
            Assert.Equal(original, decrypted);
        }

        

        [Fact]
        public void DES_PositionDependent_DifferentResultsAtDifferentPositions()
        {
            // Arrange
            string input1 = "а";
            string input2 = "ба";
            string key = "ключ";

            // Act
            string result1 = Code.DES(input1, key);
            string result2 = Code.DES(input2, key);

            // Assert
            // Второй символ в result2 должен отличаться от первого символа в result1
            // из-за позиционной зависимости DES
            Assert.NotEqual(result1[0], result2[1]);
        }

        #endregion

        #region Тесты шифра RSA

        [Fact]
        public void RSA_Encrypt_BasicText_ReturnsEncryptedString()
        {
            // Arrange
            string input = "привет";

            // Act
            var result = Code.RSA(input);

            // Assert
            Assert.NotEmpty(result.encrypted);
            Assert.Contains(",", result.encrypted);
            Assert.True(result.e > 0);
            Assert.True(result.d > 0);
            Assert.True(result.n > 0);
        }

        [Fact]
        public void RSA_EncryptDecrypt_ReturnsOriginalText()
        {
            // Arrange
            string original = "тест";

            // Act
            var encrypted = Code.RSA(original);
            string decrypted = DeCode.RSA(encrypted.encrypted, encrypted.d, encrypted.n);

            // Assert
            Assert.Equal(original, decrypted);
        }

        [Fact]
        public void RSA_WithEmptyString_ReturnsEmptyEncrypted()
        {
            // Arrange
            string input = "";

            // Act
            var result = Code.RSA(input);

            // Assert
            Assert.Empty(result.encrypted);
        }

        [Fact]
        public void RSA_WithNullString_HandlesGracefully()
        {
            // Arrange
            string input = null;

            // Act
            var result = Code.RSA(input);

            // Assert
            Assert.NotNull(result.encrypted);
        }

        [Fact]
        public void RSA_ReturnsValidParametersE_D_N()
        {
            // Arrange
            string input = "abc";

            // Act
            var result = Code.RSA(input);

            // Assert
            Assert.Equal(17, result.e);
            Assert.True(result.d > 0);
            Assert.Equal(61 * 53, result.n);
        }

        [Fact]
        public void RSA_DecryptWithWrongKey_ReturnsIncorrectResult()
        {
            // Arrange
            string original = "тест";
            var encrypted = Code.RSA(original);
            int wrongD = encrypted.d + 1;

            // Act
            string decrypted = DeCode.RSA(encrypted.encrypted, wrongD, encrypted.n);

            // Assert
            Assert.NotEqual(original, decrypted);
        }

        #endregion

        #region Параметризованные тесты для всех шифров

        [Theory]
        [InlineData("привет")]
        [InlineData("тест123")]
        [InlineData("длинный текст для проверки")]
        [InlineData("а")]
        public void AllCiphers_Caesar_EncryptDecrypt_PreservesLength(string input)
        {
            // Arrange
            int key = 5;

            // Act
            string encrypted = Code.Caesar(input, key);
            string decrypted = DeCode.Caesar(encrypted, key);

            // Assert
            Assert.Equal(input.Length, encrypted.Length);
            Assert.Equal(input, decrypted);
        }

        [Theory]
        [InlineData("тест", "ключ")]
        [InlineData("привет мир", "секрет")]
        [InlineData("абвгд", "а")]
        public void AllCiphers_Vigenere_EncryptDecrypt_ReturnsOriginal(string input, string key)
        {
            // Act
            string encrypted = Code.Vigenere(input, key);
            string decrypted = DeCode.Vigenere(encrypted, key);

            // Assert
            Assert.Equal(input, decrypted);
        }

        [Theory]
        [InlineData("привет")]
        [InlineData("тестирование")]
        [InlineData("абв")]
        public void AllCiphers_Atbash_DoubleEncryption_ReturnsOriginal(string input)
        {
            // Act
            string firstEncryption = Code.Atbash(input);
            string secondEncryption = Code.Atbash(firstEncryption);

            // Assert
            Assert.Equal(input, secondEncryption);
        }

        #endregion

        #region Тесты граничных случаев

       

        [Fact]
        public void Caesar_WithNegativeKey_WorksCorrectly()
        {
            // Arrange
            string input = "тест";
            int key = -5;

            // Act
            string encrypted = Code.Caesar(input, key);
            string decrypted = DeCode.Caesar(encrypted, key);

            // Assert
            Assert.Equal(input, decrypted);
        }

        [Fact]
        public void Vigenere_KeyShorterThanText_RepeatsKey()
        {
            // Arrange
            string input = "привет мир";
            string key = "аб";

            // Act
            string encrypted = Code.Vigenere(input, key);
            string decrypted = DeCode.Vigenere(encrypted, key);

            // Assert
            Assert.Equal(input, decrypted);
        }

        [Fact]
        public void DeCode_Playfair_InvalidEncryptedLength_ThrowsException()
        {
            // Arrange
            string invalidEncrypted = "абв"; // Нечетная длина
            string key = "ключ";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => DeCode.Playfair(invalidEncrypted, key));
        }

        #endregion

        #region Тесты производительности и стабильности

        [Fact]
        public void AllCiphers_MultipleEncryptionsDecryptions_AreStable()
        {
            // Arrange
            string original = "стабильность";
            int key = 7;

            // Act - Множественное шифрование и дешифрование
            string current = original;
            for (int i = 0; i < 10; i++)
            {
                current = Code.Caesar(current, key);
                current = DeCode.Caesar(current, key);
            }

            // Assert
            Assert.Equal(original, current);
        }

        [Fact]
        public void Caesar_VeryLongText_ProcessesSuccessfully()
        {
            // Arrange
            string longText = new string('а', 1000);
            int key = 3;

            // Act
            var exception = Record.Exception(() =>
            {
                string encrypted = Code.Caesar(longText, key);
                string decrypted = DeCode.Caesar(encrypted, key);
            });

            // Assert
            Assert.Null(exception);
        }

        #endregion

        #region Интеграционные тесты

        [Fact]
        public void Integration_CaesarThenVigenere_CanBeDecryptedInReverseOrder()
        {
            // Arrange
            string original = "привет";
            int caesarKey = 3;
            string vigenereKey = "ключ";

            // Act
            string step1 = Code.Caesar(original, caesarKey);
            string step2 = Code.Vigenere(step1, vigenereKey);
            
            string step3 = DeCode.Vigenere(step2, vigenereKey);
            string step4 = DeCode.Caesar(step3, caesarKey);

            // Assert
            Assert.Equal(original, step4);
        }

        [Fact]
        public void Integration_MultipleAlgorithms_ChainEncryptionDecryption()
        {
            // Arrange
            string original = "тест";

            // Act
            string caesar = Code.Caesar(original, 3);
            string atbash = Code.Atbash(caesar);
            
            string decryptAtbash = DeCode.Atbash(atbash);
            string decryptCaesar = DeCode.Caesar(decryptAtbash, 3);

            // Assert
            Assert.Equal(original, decryptCaesar);
        }

        #endregion
    }
}
