using System.Globalization;
using System.Text;
using FinTechApp.DomainClasses;
using FinTechApp.Interfaces;

namespace FinTechApp.Exporters;

public class CsvExportVisitor : IExportVisitor
{
    private readonly StringBuilder _accountsCsv = new();
    private readonly StringBuilder _categoriesCsv = new();
    private readonly StringBuilder _operationsCsv = new();
    public string Delimiter { get; set; } = ",";

    public void VisitBankAccount(BankAccount account)
    {
        if (_accountsCsv.Length == 0)
            _accountsCsv.AppendLine($"Id{Delimiter}Name{Delimiter}Balance");
        var balanceStr = account.Balance.ToString("F2", CultureInfo.GetCultureInfo("ru-RU")); // Например, 1300,00
        _accountsCsv.AppendLine($"{account.Id}{Delimiter}{account.Name}{Delimiter}{balanceStr}");
    }

    public void VisitCategory(Category category)
    {
        if (_categoriesCsv.Length == 0)
            _categoriesCsv.AppendLine($"Id{Delimiter}Type{Delimiter}Name");
        _categoriesCsv.AppendLine($"{category.Id}{Delimiter}{category.Type}{Delimiter}{category.Name}");
    }

    public void VisitOperation(Operation operation)
    {
        if (_operationsCsv.Length == 0)
            _operationsCsv.AppendLine(
                $"Id{Delimiter}Type{Delimiter}BankAccountId{Delimiter}Amount{Delimiter}Date{Delimiter}Description{Delimiter}CategoryId");
        var amountStr = operation.Amount.ToString("F2", CultureInfo.GetCultureInfo("ru-RU")); // Например, 500,00
        var dateStr = operation.Date.ToString("yyyy-MM-dd HH:mm:ss");
        _operationsCsv.AppendLine(
            $"{operation.Id}{Delimiter}{operation.Type}{Delimiter}{operation.BankAccountId}{Delimiter}{amountStr}{Delimiter}{dateStr}{Delimiter}{operation.Description}{Delimiter}{operation.CategoryId}");
    }

    public string GetAccountsCsv() => _accountsCsv.ToString();
    public string GetCategoriesCsv() => _categoriesCsv.ToString();
    public string GetOperationsCsv() => _operationsCsv.ToString();
}