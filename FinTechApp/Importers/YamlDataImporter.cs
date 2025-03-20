using System;
using System.Collections.Generic;
using System.Linq;
using FinTechApp.DomainClasses;
using FinTechApp.Managers;
using FinTechApp.Types;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FinTechApp.Importers
{
    public class YamlDataImporter : DataImporter<YamlData>
    {
        private readonly FinTechManager _manager;

        public YamlDataImporter(FinTechManager manager = null)
        {
            _manager = manager;
        }

        protected override YamlData ParseData(string fileContent)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            // Десериализуем YAML как объект общего типа
            var rawObject = deserializer.Deserialize<object>(fileContent);

            IDictionary<string, object> rootDict = null;

            // Преобразование в словарь с ключами-строками
            if (rawObject is IDictionary<object, object> dict)
            {
                rootDict = dict.ToDictionary(k => k.Key.ToString(), v => v.Value);
            }
            else if (rawObject is IDictionary<string, object> stringDict)
            {
                rootDict = stringDict;
            }
            else
            {
                throw new FormatException("Некорректный формат YAML (ожидался словарь).");
            }

            var exportData = new YamlData
            {
                Accounts = new List<BankAccount>(),
                Categories = new List<Category>(),
                Operations = new List<Operation>()
            };

            // Обработка счетов
            if (rootDict.TryGetValue("Accounts", out object accountsObj) && accountsObj is IList<object> accountsList)
            {
                foreach (var item in accountsList)
                {
                    if (item is IDictionary<object, object> itemDictObj)
                    {
                        var itemDict = itemDictObj.ToDictionary(k => k.Key.ToString(), v => v.Value);
                        var id = Guid.Parse(itemDict["Id"].ToString());
                        var name = itemDict["Name"].ToString();
                        var balance = decimal.Parse(itemDict["Balance"].ToString());
                        exportData.Accounts.Add(new BankAccount(id, name, balance));
                    }
                }
            }

            // Обработка категорий
            if (rootDict.TryGetValue("Categories", out object categoriesObj) &&
                categoriesObj is IList<object> categoriesList)
            {
                foreach (var item in categoriesList)
                {
                    if (item is IDictionary<object, object> itemDictObj)
                    {
                        var itemDict = itemDictObj.ToDictionary(k => k.Key.ToString(), v => v.Value);
                        var id = Guid.Parse(itemDict["Id"].ToString());
                        var type = (OperationType)Enum.Parse(typeof(OperationType), itemDict["Type"].ToString(), true);
                        var name = itemDict["Name"].ToString();
                        exportData.Categories.Add(new Category(id, type, name));
                    }
                }
            }

            // Обработка операций
            if (rootDict.TryGetValue("Operations", out object operationsObj) &&
                operationsObj is IList<object> operationsList)
            {
                foreach (var item in operationsList)
                {
                    if (item is IDictionary<object, object> itemDictObj)
                    {
                        var itemDict = itemDictObj.ToDictionary(k => k.Key.ToString(), v => v.Value);
                        var id = Guid.Parse(itemDict["Id"].ToString());
                        var type = (OperationType)Enum.Parse(typeof(OperationType), itemDict["Type"].ToString(), true);
                        var bankAccountId = Guid.Parse(itemDict["BankAccountId"].ToString());
                        var amount = decimal.Parse(itemDict["Amount"].ToString());
                        var date = DateTime.Parse(itemDict["Date"].ToString());
                        var description = itemDict["Description"]?.ToString();
                        var categoryId = Guid.Parse(itemDict["CategoryId"].ToString());
                        exportData.Operations.Add(new Operation(id, type, bankAccountId, amount, date, description,
                            categoryId));
                    }
                }
            }

            return exportData;
        }

        protected override void ProcessData(YamlData data)
        {
            if (_manager != null && data != null)
            {
                foreach (var account in data.Accounts ?? new List<BankAccount>()) _manager.AddBankAccount(account);
                foreach (var category in data.Categories ?? new List<Category>()) _manager.AddCategory(category);
                foreach (var operation in data.Operations ?? new List<Operation>()) _manager.AddOperation(operation);
            }

            Console.WriteLine("Импорт данных из YAML завершен.");
            Console.WriteLine(
                $"Счетов: {data.Accounts.Count}, Категорий: {data.Categories.Count}, Операций: {data.Operations.Count}");
        }
    }

    public class YamlData
    {
        public List<BankAccount> Accounts { get; set; }
        public List<Category> Categories { get; set; }
        public List<Operation> Operations { get; set; }
    }
}