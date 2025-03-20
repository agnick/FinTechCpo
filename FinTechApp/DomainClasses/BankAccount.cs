using System.Text;
using FinTechApp.Interfaces;

namespace FinTechApp.DomainClasses;

/// <summary>
/// Класс, представляющий банковский счет. 
/// </summary>
public class BankAccount
{
    /// <summary>
    /// Уникальный идентификатор счета.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Название счета. Это поле можно изменить.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Текущий баланс счета. Это поле можно изменить.
    /// </summary>
    public decimal Balance { get; private set; }

    /// <summary>
    /// Создает новый экземпляр банковского счета.
    /// </summary>
    /// <param name="id">Уникальный идентификатор.</param>
    /// <param name="name">Название счета.</param>
    /// <param name="balance">Начальный баланс.</param>
    public BankAccount(string name, decimal initialBalance = 0)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название счета не может быть пустым.", nameof(name));
        if (initialBalance < 0)
            throw new ArgumentException("Начальный баланс не может быть отрицательным.", nameof(initialBalance));

        Id = Guid.NewGuid();
        Name = name;
        Balance = initialBalance;
    }
    
    [System.Text.Json.Serialization.JsonConstructor]
    public BankAccount(Guid id, string name, decimal balance) : this(name, balance)
    {
        Id = id;
    }

    /// <summary>
    /// Метод для пополнения счета.
    /// </summary>
    /// <param name="amount">Сумма для пополнения.</param>
    public void Deposit(decimal amount)
    {
        // Проверка: Сумма для пополнения должна быть положительной.
        if (amount <= 0)
            throw new ArgumentException("Сумма для пополнения должна быть положительной.");

        Balance += amount;
    }

    /// <summary>
    /// Метод для списания средств со счета.
    /// </summary>
    /// <param name="amount">Сумма для списания.</param>
    public void Withdraw(decimal amount)
    {
        // Проверка: Сумма для списания должна быть положительной.
        if (amount <= 0)
            throw new ArgumentException("Сумма для списания должна быть положительной.");

        // Проверка: На счете должно быть достаточно средств.
        if (Balance < amount)
            throw new InvalidOperationException("Недостаточно средств для списания.");

        Balance -= amount;
    }

    /// <summary>
    /// Метод для обновления названия счета.
    /// </summary>
    /// <param name="newName">Новое название счета.</param>
    public void UpdateName(string newName)
    {
        // Проверка: Название счета не может быть пустым.
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Название счета не может быть пустым.");

        Name = newName;
    }
    
    /// <summary>
    /// Принимает посетителя для экспорта данных.
    /// </summary>
    /// <param name="visitor">Объект посетителя.</param>
    public void Accept(IExportVisitor visitor)
    {
        visitor.VisitBankAccount(this);
    }

    // Переопределенный метод ToString для получения информации о банковском аккаунте.
    public override string ToString() => $"Банковский счет: {Name} (ID: {Id}), Баланс: {Balance:C}";
}