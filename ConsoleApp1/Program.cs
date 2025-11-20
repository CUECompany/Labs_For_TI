using System;
using Ciphers; // подключаем твою библиотеку

class Program
{
    static void Main()
    {
        string text = "Привет мир!";
        string key = "ключ";
        int caesarKey = 5;

        Console.WriteLine("=== Оригинальный текст ===");
        Console.WriteLine(text);
        Console.WriteLine();

        // --- Caesar ---
        string caesarEnc = Code.Caesar(text, caesarKey);
        string caesarDec = DeCode.Caesar(caesarEnc, caesarKey);
        Console.WriteLine("[Caesar]");
        Console.WriteLine($"Зашифровано: {caesarEnc}");
        Console.WriteLine($"Расшифровано: {caesarDec}");
        Console.WriteLine();

        // --- Vigenere ---
        string vigEnc = Code.Vigenere(text, key);
        string vigDec = DeCode.Vigenere(vigEnc, key);
        Console.WriteLine("[Vigenere]");
        Console.WriteLine($"Зашифровано: {vigEnc}");
        Console.WriteLine($"Расшифровано: {vigDec}");
        Console.WriteLine();

        // --- Atbash ---
        string atbEnc = Code.Atbash(text);
        string atbDec = DeCode.Atbash(atbEnc);
        Console.WriteLine("[Atbash]");
        Console.WriteLine($"Зашифровано: {atbEnc}");
        Console.WriteLine($"Расшифровано: {atbDec}");
        Console.WriteLine();

        // --- Playfair ---
        string playEnc = Code.Playfair(text, key);
        string playDec = DeCode.Playfair(playEnc, key);
        Console.WriteLine("[Playfair]");
        Console.WriteLine($"Зашифровано: {playEnc}");
        Console.WriteLine($"Расшифровано: {playDec}");
        Console.WriteLine();

        // --- Vernam ---
        string verEnc = Code.Vernam(text, key);
        string verDec = DeCode.Vernam(verEnc, key);
        Console.WriteLine("[Vernam]");
        Console.WriteLine($"Зашифровано: {verEnc}");
        Console.WriteLine($"Расшифровано: {verDec}");
        Console.WriteLine();

        // --- DES (учебный) ---
        string desEnc = Code.DES(text, key);
        string desDec = DeCode.DES(desEnc, key);
        Console.WriteLine("[DES]");
        Console.WriteLine($"Зашифровано: {desEnc}");
        Console.WriteLine($"Расшифровано: {desDec}");
        Console.WriteLine();

        Console.WriteLine("=== Тест завершён ===");
        TestRSA();
    }
    static void TestRSA()
    {
        string text = "шифр";

        // Шифрование
        var (encrypted, e, d, n) = Code.RSA(text);

        Console.WriteLine("[RSA]");
        Console.WriteLine($"Открытый ключ: e={e}, n={n}");
        Console.WriteLine($"Закрытый ключ: d={d}");
        Console.WriteLine($"Зашифровано: {encrypted}");

        // Дешифрование
        string decrypted = DeCode.RSA(encrypted, d, n);
        Console.WriteLine($"Расшифровано: {decrypted}");
    }
}
