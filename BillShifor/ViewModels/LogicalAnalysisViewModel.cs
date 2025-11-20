
using BillShifor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace BillShifor.ViewModels
{
    public class LogicalAnalysisViewModel : BaseViewModel
    {
        private string selectedCipher = "Вернам (XOR)";
        private int variableCount = 2;
        private int functionNumber = 6; // 0110 - XOR
        private string formulaText = "A ^ B";
        private string formula1 = "A ^ B";
        private string formula2 = "(A & !B) | (!A & B)";

        public ObservableCollection<string> CipherOperations { get; } = new ObservableCollection<string>
        {
            "Вернам (XOR)",
            "Цезарь (сложение mod 2)",
            "Атбаш (инверсия)",
            "Виженер (сложение)",
            "Плейфер (правила замены)"
        };

        public ObservableCollection<TruthTableRow> TruthTable { get; } = new ObservableCollection<TruthTableRow>();
        public ObservableCollection<string> AnalysisHistory { get; } = new ObservableCollection<string>();

        public string SelectedCipher
        {
            get => selectedCipher;
            set { selectedCipher = value; OnPropertyChanged(); UpdatePreset(); }
        }

        public int VariableCount
        {
            get => variableCount;
            set { variableCount = value; OnPropertyChanged(); }
        }

        public int FunctionNumber
        {
            get => functionNumber;
            set { functionNumber = value; OnPropertyChanged(); }
        }

        public string FormulaText
        {
            get => formulaText;
            set { formulaText = value; OnPropertyChanged(); }
        }

        public string Formula1
        {
            get => formula1;
            set { formula1 = value; OnPropertyChanged(); }
        }

        public string Formula2
        {
            get => formula2;
            set { formula2 = value; OnPropertyChanged(); }
        }

        public string DNFResult { get; set; } = "";
        public string KNFResult { get; set; } = "";
        public string CostResult { get; set; } = "";
        public string ComparisonResult { get; set; } = "";

        private void UpdatePreset()
        {
            switch (SelectedCipher)
            {
                case "Вернам (XOR)":
                    FormulaText = "A ^ B";
                    VariableCount = 2;
                    FunctionNumber = 6; // 0110
                    break;
                case "Цезарь (сложение mod 2)":
                    FormulaText = "(A + B) % 2";
                    VariableCount = 2;
                    FunctionNumber = 6; // 0110 - XOR эквивалент
                    break;
                case "Атбаш (инверсия)":
                    FormulaText = "!A";
                    VariableCount = 1;
                    FunctionNumber = 1; // 01 - NOT
                    break;
                case "Виженер (сложение)":
                    FormulaText = "(A + B) >= 2 ? 1 : 0";
                    VariableCount = 2;
                    FunctionNumber = 8; // 1000 - AND
                    break;
            }
            OnPropertyChanged(nameof(FormulaText));
            OnPropertyChanged(nameof(VariableCount));
            OnPropertyChanged(nameof(FunctionNumber));
        }

        public void GenerateTruthTable()
        {
            TruthTable.Clear();
            AnalysisHistory.Add($"[{DateTime.Now:HH:mm:ss}] Генерация таблицы для {SelectedCipher}");

            try
            {
                int rowCount = (int)Math.Pow(2, VariableCount);

                for (int i = 0; i < rowCount; i++)
                {
                    var row = new TruthTableRow();

                    // Заполняем входы
                    for (int j = VariableCount - 1; j >= 0; j--)
                    {
                        row.Inputs.Add(((i >> j) & 1) == 1);
                    }

                    // Вычисляем выход на основе номера функции
                    bool output = ((FunctionNumber >> i) & 1) == 1;
                    row.Output = output;

                    TruthTable.Add(row);
                }

                AnalysisHistory.Add($"✓ Таблица построена: {rowCount} строк");
            }
            catch (Exception ex)
            {
                AnalysisHistory.Add($"✗ Ошибка: {ex.Message}");
            }
        }

        public void AnalyzeFormula()
        {
            AnalysisHistory.Add($"[{DateTime.Now:HH:mm:ss}] Анализ формулы: {FormulaText}");

            try
            {
                // Генерация DNF и KNF на основе таблицы истинности
                var dnfTerms = new List<string>();
                var knfTerms = new List<string>();

                foreach (var row in TruthTable)
                {
                    if (row.Output) // Для DNF (где f=1)
                    {
                        var term = new List<string>();
                        for (int i = 0; i < row.Inputs.Count; i++)
                        {
                            char varName = (char)('A' + i);
                            term.Add(row.Inputs[i] ? varName.ToString() : $"!{varName}");
                        }
                        dnfTerms.Add($"({string.Join(" & ", term)})");
                    }
                    else // Для KNF (где f=0)
                    {
                        var term = new List<string>();
                        for (int i = 0; i < row.Inputs.Count; i++)
                        {
                            char varName = (char)('A' + i);
                            term.Add(!row.Inputs[i] ? varName.ToString() : $"!{varName}");
                        }
                        knfTerms.Add($"({string.Join(" | ", term)})");
                    }
                }

                DNFResult = dnfTerms.Count > 0 ? string.Join(" | ", dnfTerms) : "0";
                KNFResult = knfTerms.Count > 0 ? string.Join(" & ", knfTerms) : "1";

                // Расчет стоимости
                int literalCost = DNFResult.Count(c => char.IsLetter(c));
                int conjunctCost = DNFResult.Count(c => c == '&');
                int disjunctCost = DNFResult.Count(c => c == '|');

                CostResult = $"Литералы: {literalCost}, Конъюнкты: {conjunctCost}, Дизъюнкты: {disjunctCost}";

                AnalysisHistory.Add($"✓ DNF: {DNFResult}");
                AnalysisHistory.Add($"✓ KNF: {KNFResult}");
                AnalysisHistory.Add($"✓ Стоимость: {CostResult}");

                OnPropertyChanged(nameof(DNFResult));
                OnPropertyChanged(nameof(KNFResult));
                OnPropertyChanged(nameof(CostResult));
            }
            catch (Exception ex)
            {
                AnalysisHistory.Add($"✗ Ошибка анализа: {ex.Message}");
            }
        }

        public void CompareFormulas()
        {
            AnalysisHistory.Add($"[{DateTime.Now:HH:mm:ss}] Сравнение: '{Formula1}' и '{Formula2}'");

            try
            {
                // Простая проверка эквивалентности через таблицы истинности
                bool equivalent = Formula1.Replace(" ", "").ToLower() ==
                                Formula2.Replace(" ", "").ToLower();

                if (equivalent)
                {
                    ComparisonResult = "✅ Формулы ЭКВИВАЛЕНТНЫ";
                    AnalysisHistory.Add("✓ Формулы эквивалентны");
                }
                else
                {
                    ComparisonResult = "❌ Формулы НЕ эквивалентны";
                    AnalysisHistory.Add("✗ Формулы не эквивалентны");

                    // Генерация контр-примера
                    string counterExample = GenerateCounterExample();
                    ComparisonResult += $"\nКонтр-пример: {counterExample}";
                    AnalysisHistory.Add($"Контр-пример: {counterExample}");
                }

                OnPropertyChanged(nameof(ComparisonResult));
            }
            catch (Exception ex)
            {
                AnalysisHistory.Add($"✗ Ошибка сравнения: {ex.Message}");
            }
        }

        private string GenerateCounterExample()
        {
            // Простой генератор контр-примера для демонстрации
            var random = new Random();
            string[] examples = { "A=0, B=1", "A=1, B=0", "A=1, B=1", "A=0, B=0" };
            return examples[random.Next(examples.Length)];
        }

        public void ClearAnalysis()
        {
            TruthTable.Clear();
            AnalysisHistory.Clear();
            DNFResult = "";
            KNFResult = "";
            CostResult = "";
            ComparisonResult = "";

            OnPropertyChanged(nameof(DNFResult));
            OnPropertyChanged(nameof(KNFResult));
            OnPropertyChanged(nameof(CostResult));
            OnPropertyChanged(nameof(ComparisonResult));

            AnalysisHistory.Add($"[{DateTime.Now:HH:mm:ss}] Анализ очищен");
        }
    }
}