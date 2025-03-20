using YamlDotNet.Serialization;
using FinTechApp.DomainClasses;
using FinTechApp.Interfaces;

namespace FinTechApp.Exporters;

public class YamlExportVisitor : IExportVisitor
{
    private readonly List<BankAccount> _accounts = new();
    private readonly List<Category> _categories = new();
    private readonly List<Operation> _operations = new();

    public void VisitBankAccount(BankAccount account) => _accounts.Add(account);
    public void VisitCategory(Category category) => _categories.Add(category);
    public void VisitOperation(Operation operation) => _operations.Add(operation);

    public string GetYaml()
    {
        var data = new
        {
            Accounts = _accounts,
            Categories = _categories,
            Operations = _operations
        };
        var serializer = new SerializerBuilder().Build();
        return serializer.Serialize(data);
    }
}