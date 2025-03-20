using System.Globalization;
using System.Text.RegularExpressions;
using FinTechApp.DomainClasses;
using FinTechApp.Managers;
using FinTechApp.Types;

namespace FinTechApp.Importers
{
    public class CsvDataImporter : DataImporter<CsvData>
    {
        private readonly FinTechManager _manager;

        public CsvDataImporter(FinTechManager manager = null)
        {
            _manager = manager;
        }

        protected override CsvData ParseData(string fileContent)
        {
            var exportData = new CsvData
            {
                Accounts = new List<BankAccount>(),
                Categories = new List<Category>(),
                Operations = new List<Operation>()
            };

            var lines = fileContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines.Skip(1)) // Пропускаем заголовок
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Разбиваем строку с учётом экранирования запятых в числах
                var cells = SplitCsvLine(line);
                if (cells.Length == 0)
                    continue;

                try
                {
                    // BankAccount: Id,Name,Balance (3 поля, Balance — число)
                    if (cells.Length == 3 && decimal.TryParse(cells[2].Trim(), NumberStyles.Any,
                            CultureInfo.GetCultureInfo("ru-RU"), out _))
                    {
                        var id = Guid.Parse(cells[0].Trim());
                        var name = cells[1].Trim();
                        var balance = decimal.Parse(cells[2].Trim(), CultureInfo.GetCultureInfo("ru-RU"));
                        exportData.Accounts.Add(new BankAccount(id, name, balance));
                    }
                    // Category: Id,Type,Name (3 поля, Type — Income/Expense)
                    else if (cells.Length == 3 && Enum.TryParse<OperationType>(cells[1].Trim(), true, out _))
                    {
                        var id = Guid.Parse(cells[0].Trim());
                        var type = (OperationType)Enum.Parse(typeof(OperationType), cells[1].Trim(), true);
                        var name = cells[2].Trim();
                        exportData.Categories.Add(new Category(id, type, name));
                    }
                    // Operation: Id,Type,BankAccountId,Amount,Date,Description,CategoryId (7 полей)
                    else if (cells.Length >= 7)
                    {
                        var id = Guid.Parse(cells[0].Trim());
                        var type = (OperationType)Enum.Parse(typeof(OperationType), cells[1].Trim(), true);
                        var bankAccountId = Guid.Parse(cells[2].Trim());
                        var amount = decimal.Parse(cells[3].Trim(), CultureInfo.GetCultureInfo("ru-RU"));
                        var date = DateTime.ParseExact(cells[4].Trim(), "yyyy-MM-dd HH:mm:ss",
                            CultureInfo.InvariantCulture);
                        var description = cells[5].Trim();
                        var categoryId = Guid.Parse(cells[6].Trim());
                        exportData.Operations.Add(new Operation(id, type, bankAccountId, amount, date, description,
                            categoryId));
                    }
                }
                catch (Exception ex)
                {
                    throw new FormatException($"Ошибка при парсинге строки: '{line}'. Подробности: {ex.Message}", ex);
                }
            }

            return exportData;
        }

        private string[] SplitCsvLine(string line)
        {
            // Регулярное выражение для разделения CSV с учётом чисел с запятой
            var pattern = @",(?=(?:[^""]*""[^""]*"")*[^""]*$)(?<!\d)";
            var parts = Regex.Split(line, pattern);

            // Убираем кавычки, если они есть, и возвращаем массив
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = parts[i].Trim('"');
            }

            // Объединяем числа с запятой (например, "500", "00" → "500,00")
            List<string> correctedParts = new List<string>();
            for (int i = 0; i < parts.Length; i++)
            {
                if (i + 1 < parts.Length && decimal.TryParse(parts[i] + "," + parts[i + 1], NumberStyles.Any,
                        CultureInfo.GetCultureInfo("ru-RU"), out _))
                {
                    correctedParts.Add(parts[i] + "," + parts[i + 1]);
                    i++; // Пропускаем следующую часть
                }
                else
                {
                    correctedParts.Add(parts[i]);
                }
            }

            return correctedParts.ToArray();
        }

        protected override void ProcessData(CsvData data)
        {
            if (_manager != null && data != null)
            {
                foreach (var account in data.Accounts ?? new List<BankAccount>()) _manager.AddBankAccount(account);
                foreach (var category in data.Categories ?? new List<Category>()) _manager.AddCategory(category);
                foreach (var operation in data.Operations ?? new List<Operation>()) _manager.AddOperation(operation);
            }

            Console.WriteLine("Импорт данных из CSV завершен.");
            Console.WriteLine(
                $"Счетов: {data.Accounts.Count}, Категорий: {data.Categories.Count}, Операций: {data.Operations.Count}");
        }
    }

    public class CsvData
    {
        public List<BankAccount> Accounts { get; set; }
        public List<Category> Categories { get; set; }
        public List<Operation> Operations { get; set; }
    }
}