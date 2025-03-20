using System.Text.Json;
using FinTechApp.DomainClasses;
using FinTechApp.Managers;

namespace FinTechApp.Importers
{
    public class JsonDataImporter : DataImporter<JsonData>
    {
        private readonly FinTechManager _manager;

        public JsonDataImporter(FinTechManager manager = null)
        {
            _manager = manager;
        }

        protected override JsonData ParseData(string fileContent)
        {
            try
            {
                return JsonSerializer.Deserialize<JsonData>(fileContent);
            }
            catch (JsonException ex)
            {
                throw new Exception("Ошибка при парсинге JSON: " + ex.Message, ex);
            }
        }

        protected override void ProcessData(JsonData data)
        {
            if (_manager != null && data != null)
            {
                foreach (var account in data.Accounts ?? new List<BankAccount>()) _manager.AddBankAccount(account);
                foreach (var category in data.Categories ?? new List<Category>()) _manager.AddCategory(category);
                foreach (var operation in data.Operations ?? new List<Operation>()) _manager.AddOperation(operation);
            }

            Console.WriteLine("Импорт данных из JSON завершен.");
            Console.WriteLine(
                $"Счетов: {data.Accounts.Count}, Категорий: {data.Categories.Count}, Операций: {data.Operations.Count}");
        }
    }

    public class JsonData
    {
        public List<BankAccount> Accounts { get; set; }
        public List<Category> Categories { get; set; }
        public List<Operation> Operations { get; set; }
    }
}