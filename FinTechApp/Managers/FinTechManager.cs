using FinTechApp.DomainClasses;
using FinTechApp.Exporters;
using FinTechApp.Importers;
using FinTechApp.Types;

namespace FinTechApp.Managers
{
    /// <summary>
    /// Фасад для управления финансовыми данными: счетами, категориями, операциями, а также импортом и экспортом.
    /// </summary>
    public class FinTechManager : IFinTechManager
    {
        private readonly List<BankAccount> _accounts = new();
        private readonly List<Category> _categories = new();
        private readonly List<Operation> _operations = new();

        /// <summary>
        /// Добавляет новый банковский счет в систему.
        /// </summary>
        /// <param name="account">Экземпляр банковского счета для добавления.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если счет равен null.</exception>
        public void AddBankAccount(BankAccount account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account), "Банковский счет не может быть null.");

            _accounts.Add(account);
        }

        /// <summary>
        /// Удаляет банковский счет по его идентификатору.
        /// </summary>
        /// <param name="accountId">Уникальный идентификатор счета.</param>
        /// <exception cref="InvalidOperationException">Выбрасывается, если счет с указанным ID не найден.</exception>
        public void RemoveBankAccount(Guid accountId)
        {
            var account = _accounts.FirstOrDefault(a => a.Id == accountId);
            if (account == null)
                throw new InvalidOperationException($"Банковский счет с ID {accountId} не найден.");

            _accounts.Remove(account);
        }

        /// <summary>
        /// Получает список всех банковских счетов.
        /// </summary>
        /// <returns>Коллекция всех банковских счетов.</returns>
        public IEnumerable<BankAccount> GetBankAccounts()
        {
            return _accounts.AsReadOnly();
        }

        /// <summary>
        /// Редактирует название существующего банковского счета.
        /// </summary>
        /// <param name="accountId">Уникальный идентификатор счета.</param>
        /// <param name="newName">Новое название счета.</param>
        /// <exception cref="InvalidOperationException">Выбрасывается, если счет не найден.</exception>
        /// <exception cref="ArgumentException">Выбрасывается, если новое название пустое.</exception>
        public void EditBankAccount(Guid accountId, string newName)
        {
            var account = _accounts.FirstOrDefault(a => a.Id == accountId);
            if (account == null)
                throw new InvalidOperationException($"Банковский счет с ID {accountId} не найден.");

            account.UpdateName(newName);
        }

        /// <summary>
        /// Добавляет новую категорию в систему.
        /// </summary>
        /// <param name="category">Экземпляр категории для добавления.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если категория равна null.</exception>
        public void AddCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category), "Категория не может быть null.");

            _categories.Add(category);
        }

        /// <summary>
        /// Удаляет категорию по её идентификатору.
        /// </summary>
        /// <param name="categoryId">Уникальный идентификатор категории.</param>
        /// <exception cref="InvalidOperationException">Выбрасывается, если категория с указанным ID не найдена.</exception>
        public void RemoveCategory(Guid categoryId)
        {
            var category = _categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
                throw new InvalidOperationException($"Категория с ID {categoryId} не найдена.");

            _categories.Remove(category);
        }

        /// <summary>
        /// Получает список всех категорий.
        /// </summary>
        /// <returns>Коллекция всех категорий.</returns>
        public IEnumerable<Category> GetCategories()
        {
            return _categories.AsReadOnly();
        }

        /// <summary>
        /// Редактирует существующую категорию.
        /// </summary>
        /// <param name="categoryId">Уникальный идентификатор категории.</param>
        /// <param name="newName">Новое название категории.</param>
        /// <param name="newType">Новый тип категории (доход или расход).</param>
        /// <exception cref="InvalidOperationException">Выбрасывается, если категория не найдена.</exception>
        /// <exception cref="ArgumentException">Выбрасывается, если новое название пустое или тип недопустим.</exception>
        public void EditCategory(Guid categoryId, string newName, OperationType newType)
        {
            var category = _categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
                throw new InvalidOperationException($"Категория с ID {categoryId} не найдена.");

            category.UpdateName(newName);
            category.UpdateType(newType);
        }

        /// <summary>
        /// Добавляет новую финансовую операцию в систему.
        /// </summary>
        /// <param name="operation">Экземпляр операции для добавления.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если операция равна null.</exception>
        /// <exception cref="InvalidOperationException">Выбрасывается, если счет не найден или недостаточно средств для расхода.</exception>
        public void AddOperation(Operation operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation), "Операция не может быть null.");

            var account = _accounts.FirstOrDefault(a => a.Id == operation.BankAccountId);
            if (account == null)
                throw new InvalidOperationException($"Счет с ID {operation.BankAccountId} не найден.");

            if (operation.Type == OperationType.Expense && operation.Amount > account.Balance)
                throw new InvalidOperationException("Недостаточно средств на счете для выполнения расходной операции.");

            if (operation.Type == OperationType.Income)
                account.Deposit(operation.Amount);
            else
                account.Withdraw(operation.Amount);

            _operations.Add(operation);
        }

        /// <summary>
        /// Удаляет операцию по её идентификатору.
        /// </summary>
        /// <param name="operationId">Уникальный идентификатор операции.</param>
        /// <exception cref="InvalidOperationException">Выбрасывается, если операция с указанным ID не найдена.</exception>
        public void RemoveOperation(Guid operationId)
        {
            var operation = _operations.FirstOrDefault(o => o.Id == operationId);
            if (operation == null)
                throw new InvalidOperationException($"Операция с ID {operationId} не найдена.");

            _operations.Remove(operation);
        }

        /// <summary>
        /// Получает список всех операций.
        /// </summary>
        /// <returns>Коллекция всех операций.</returns>
        public IEnumerable<Operation> GetOperations()
        {
            return _operations.AsReadOnly();
        }

        /// <summary>
        /// Редактирует существующую операцию.
        /// </summary>
        /// <param name="operationId">Уникальный идентификатор операции.</param>
        /// <param name="newAmount">Новая сумма операции.</param>
        /// <param name="newDate">Новая дата операции.</param>
        /// <param name="newDescription">Новое описание операции.</param>
        /// <param name="newCategoryId">Новый идентификатор категории.</param>
        /// <exception cref="InvalidOperationException">Выбрасывается, если операция или счет не найдены, или недостаточно средств.</exception>
        /// <exception cref="ArgumentException">Выбрасывается, если новая сумма отрицательна.</exception>
        public void EditOperation(Guid operationId, decimal newAmount, DateTime newDate, string newDescription,
            Guid newCategoryId)
        {
            var operation = _operations.FirstOrDefault(o => o.Id == operationId);
            if (operation == null)
                throw new InvalidOperationException($"Операция с ID {operationId} не найдена.");

            var account = _accounts.FirstOrDefault(a => a.Id == operation.BankAccountId);
            if (account == null)
                throw new InvalidOperationException($"Счет с ID {operation.BankAccountId} не найден.");

            if (newAmount < 0)
                throw new ArgumentException("Сумма операции не может быть отрицательной.", nameof(newAmount));

            // Откатываем старую сумму
            if (operation.Type == OperationType.Income)
                account.Withdraw(operation.Amount);
            else
                account.Deposit(operation.Amount);

            // Обновляем операцию
            operation.Update(newAmount, newDate, newDescription, newCategoryId);

            // Применяем новую сумму
            if (operation.Type == OperationType.Income)
                account.Deposit(newAmount);
            else if (newAmount > account.Balance)
                throw new InvalidOperationException(
                    "Недостаточно средств на счете для обновленной расходной операции.");
            else
                account.Withdraw(newAmount);
        }

        /// <summary>
        /// Вычисляет разницу между доходами и расходами за указанный период.
        /// </summary>
        /// <param name="start">Начальная дата периода.</param>
        /// <param name="end">Конечная дата периода.</param>
        /// <returns>Разница между доходами и расходами (доходы минус расходы).</returns>
        public decimal GetIncomeExpenseDifference(DateTime start, DateTime end)
        {
            var income = _operations
                .Where(o => o.Date >= start && o.Date <= end && o.Type == OperationType.Income)
                .Sum(o => o.Amount);
            var expense = _operations
                .Where(o => o.Date >= start && o.Date <= end && o.Type == OperationType.Expense)
                .Sum(o => o.Amount);
            return income - expense;
        }

        /// <summary>
        /// Группирует операции по категориям за указанный период.
        /// </summary>
        /// <param name="start">Начальная дата периода.</param>
        /// <param name="end">Конечная дата периода.</param>
        /// <returns>Словарь, где ключ — ID категории, значение — сумма операций в этой категории.</returns>
        public Dictionary<Guid, decimal> GroupOperationsByCategory(DateTime start, DateTime end)
        {
            return _operations
                .Where(o => o.Date >= start && o.Date <= end)
                .GroupBy(o => o.CategoryId)
                .ToDictionary(g => g.Key, g => g.Sum(o => o.Amount));
        }

        /// <summary>
        /// Экспортирует данные в указанный формат (CSV, JSON, YAML).
        /// </summary>
        /// <param name="format">Формат экспорта: "csv", "json" или "yaml" (регистр не учитывается).</param>
        /// <param name="directoryOrFilePath">Путь к директории (для CSV) или файлу (для JSON/YAML).</param>
        /// <exception cref="ArgumentException">Выбрасывается, если формат или путь пусты либо формат не поддерживается.</exception>
        /// <exception cref="IOException">Выбрасывается при ошибках записи в файл.</exception>
        public void ExportData(string format, string directoryOrFilePath)
        {
            if (string.IsNullOrWhiteSpace(format))
                throw new ArgumentException("Формат экспорта не может быть пустым.", nameof(format));
            if (string.IsNullOrWhiteSpace(directoryOrFilePath))
                throw new ArgumentException("Путь не может быть пустым.", nameof(directoryOrFilePath));

            switch (format.ToLower())
            {
                case "csv":
                    ExportToCsv(directoryOrFilePath);
                    break;
                case "json":
                    ExportToJson(directoryOrFilePath);
                    break;
                case "yaml":
                    ExportToYaml(directoryOrFilePath);
                    break;
                default:
                    throw new ArgumentException(
                        $"Формат '{format}' не поддерживается. Используйте 'csv', 'json' или 'yaml'.", nameof(format));
            }
        }

        private void ExportToCsv(string directory)
        {
            var visitor = new CsvExportVisitor();
            foreach (var account in _accounts) account.Accept(visitor);
            foreach (var category in _categories) category.Accept(visitor);
            foreach (var operation in _operations) operation.Accept(visitor);

            Directory.CreateDirectory(directory);
            File.WriteAllText(Path.Combine(directory, "accounts.csv"), visitor.GetAccountsCsv());
            File.WriteAllText(Path.Combine(directory, "categories.csv"), visitor.GetCategoriesCsv());
            File.WriteAllText(Path.Combine(directory, "operations.csv"), visitor.GetOperationsCsv());
        }

        private void ExportToJson(string filePath)
        {
            var visitor = new JsonExportVisitor();
            foreach (var account in _accounts) account.Accept(visitor);
            foreach (var category in _categories) category.Accept(visitor);
            foreach (var operation in _operations) operation.Accept(visitor);
            
            File.WriteAllText(filePath, visitor.GetJson());
        }

        private void ExportToYaml(string filePath)
        {
            var visitor = new YamlExportVisitor();
            foreach (var account in _accounts) account.Accept(visitor);
            foreach (var category in _categories) category.Accept(visitor);
            foreach (var operation in _operations) operation.Accept(visitor);
            
            File.WriteAllText(filePath, visitor.GetYaml());
        }

        /// <summary>
        /// Импортирует данные из указанного формата (CSV, JSON, YAML).
        /// </summary>
        /// <param name="format">Формат импорта: "csv", "json" или "yaml" (регистр не учитывается).</param>
        /// <param name="directoryOrFilePath">Путь к директории (для CSV) или файлу (для JSON/YAML).</param>
        /// <exception cref="ArgumentException">Выбрасывается, если формат или путь пусты либо формат не поддерживается.</exception>
        /// <exception cref="Exception">Выбрасывается при ошибках импорта данных.</exception>
        public void ImportData(string format, string directoryOrFilePath)
        {
            if (string.IsNullOrWhiteSpace(format))
                throw new ArgumentException("Формат импорта не может быть пустым.", nameof(format));
            if (string.IsNullOrWhiteSpace(directoryOrFilePath))
                throw new ArgumentException("Путь не может быть пустым.", nameof(directoryOrFilePath));

            switch (format.ToLower())
            {
                case "csv":
                    ImportFromCsv(directoryOrFilePath);
                    break;
                case "json":
                    ImportFromJson(directoryOrFilePath);
                    break;
                case "yaml":
                    ImportFromYaml(directoryOrFilePath);
                    break;
                default:
                    throw new ArgumentException(
                        $"Формат '{format}' не поддерживается. Используйте 'csv', 'json' или 'yaml'.", nameof(format));
            }
        }

        private void ImportFromCsv(string directory)
        {
            var importer = new CsvDataImporter(this);
            importer.ImportData(Path.Combine(directory, "accounts.csv"));
            importer.ImportData(Path.Combine(directory, "categories.csv"));
            importer.ImportData(Path.Combine(directory, "operations.csv"));
        }

        private void ImportFromJson(string filePath)
        {
            var importer = new JsonDataImporter(this);
            importer.ImportData(filePath);
        }

        private void ImportFromYaml(string filePath)
        {
            var importer = new YamlDataImporter(this);
            importer.ImportData(filePath);
        }
    }
}