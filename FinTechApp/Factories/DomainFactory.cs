using FinTechApp.DomainClasses;
using FinTechApp.Types;

namespace FinTechApp.Factories;

/// <summary>
/// Фабрика для создания доменных объектов модуля "Учет финансов".
/// Гарантирует централизованное создание и валидацию объектов.
/// </summary>
public static class DomainFactory
{
    /// <summary>
    /// Создает новый банковский счет с заданным названием и начальным балансом.
    /// </summary>
    /// <param name="name">Название банковского счета.</param>
    /// <param name="initialBalance">Начальный баланс счета.</param>
    /// <returns>Экземпляр BankAccount.</returns>
    public static BankAccount CreateBankAccount(string name, decimal initialBalance = 0)
    {
        return new BankAccount(name, initialBalance);
    }

    /// <summary>
    /// Создает новую категорию с указанным типом и названием.
    /// </summary>
    /// <param name="type">Тип категории (доход или расход).</param>
    /// <param name="name">Название категории.</param>
    /// <returns>Экземпляр Category.</returns>
    public static Category CreateCategory(OperationType type, string name)
    {
        return new Category(type, name);
    }

    /// <summary>
    /// Создает новую операцию с заданными параметрами.
    /// </summary>
    /// <param name="type">Тип операции (доход или расход).</param>
    /// <param name="bankAccount">Банковский счет.</param>
    /// <param name="amount">Сумма операции.</param>
    /// <param name="date">Дата проведения операции.</param>
    /// <param name="description">Описание операции (необязательное поле).</param>
    /// <param name="category">Категория.</param>
    /// <returns>Экземпляр Operation.</returns>
    public static Operation CreateOperation(
        OperationType type,
        BankAccount bankAccount,
        decimal amount,
        DateTime date,
        string? description,
        Category category)
    {
        if (type == OperationType.Expense && amount > bankAccount.Balance)
            throw new InvalidOperationException("Недостаточно средств на счете для операции c типом расход");
        
        return new Operation(type, bankAccount.Id, amount, date, description, category.Id);
    }
}