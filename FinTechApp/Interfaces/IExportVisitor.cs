using FinTechApp.DomainClasses;

namespace FinTechApp.Interfaces;

public interface IExportVisitor
{
    void VisitBankAccount(BankAccount account);
    void VisitCategory(Category category);
    void VisitOperation(Operation operation);
}