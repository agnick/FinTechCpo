using System.Text;
using FinTechApp.Interfaces;
using FinTechApp.Types;

namespace FinTechApp.DomainClasses;

public class Operation
{
    /// <summary>
    /// Уникальный идентификатор операции.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Тип операции: доход или расход.
    /// </summary>
    public OperationType Type { get; private set; }

    /// <summary>
    /// Идентификатор банковского счёта, к которому относится операция.
    /// </summary>
    public Guid BankAccountId { get; private set; }

    /// <summary>
    /// Сумма операции. Должна быть положительной.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Дата проведения операции.
    /// </summary>
    public DateTime Date { get; private set; }

    /// <summary>
    /// Описание операции (необязательное поле).
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Идентификатор категории, к которой относится операция.
    /// </summary>
    public Guid CategoryId { get; private set; }

    /// <summary>
    /// Создает новый экземпляр операции.
    /// </summary>
    /// <param name="id">Уникальный идентификатор операции.</param>
    /// <param name="type">Тип операции: доход или расход.</param>
    /// <param name="bankAccountId">Идентификатор банковского счёта, к которому относится операция.</param>
    /// <param name="amount">Сумма операции. Должна быть положительной.</param>
    /// <param name="date">Дата проведения операции.</param>
    /// <param name="description">Описание операции (необязательное поле).</param>
    /// <param name="categoryId">Идентификатор категории, к которой относится операция.</param>
    public Operation(OperationType type, Guid bankAccountId, decimal amount, DateTime date,
        string? description, Guid categoryId)
    {
        if (!Enum.IsDefined(type))
            throw new ArgumentException("Недопустимый тип операции.", nameof(type));
        if (bankAccountId == Guid.Empty)
            throw new ArgumentException("Идентификатор счета не может быть пустым.", nameof(bankAccountId));
        if (categoryId == Guid.Empty)
            throw new ArgumentException("Идентификатор категории не может быть пустым.", nameof(categoryId));
        if (amount < 0)
            throw new ArgumentException("Сумма операции не может быть отрицательной.", nameof(amount));
        
        Id = Guid.NewGuid();
        Type = type;
        BankAccountId = bankAccountId;
        Amount = amount;
        Date = date;
        Description = description;
        CategoryId = categoryId;
    }
    
    [System.Text.Json.Serialization.JsonConstructor]
    public Operation(Guid id, OperationType type, Guid bankAccountId, decimal amount, DateTime date, string description, Guid categoryId)
        : this(type, bankAccountId, amount, date, description, categoryId)
    {
        Id = id;
    }
    
    /// <summary>
    /// Обновляет параметры операции.
    /// </summary>
    /// <param name="newAmount">Новая сумма операции.</param>
    /// <param name="newDate">Новая дата операции.</param>
    /// <param name="newDescription">Новое описание операции.</param>
    /// <param name="newCategoryId">Новый идентификатор категории.</param>
    /// <exception cref="ArgumentException">Выбрасывается, если новая сумма отрицательна или категория пуста.</exception>
    public void Update(decimal newAmount, DateTime newDate, string? newDescription, Guid newCategoryId)
    {
        if (newAmount < 0)
            throw new ArgumentException("Сумма операции не может быть отрицательной.", nameof(newAmount));
        if (newCategoryId == Guid.Empty)
            throw new ArgumentException("Идентификатор категории не может быть пустым.", nameof(newCategoryId));

        Amount = newAmount;
        Date = newDate;
        Description = newDescription;
        CategoryId = newCategoryId;
    }
    
    /// <summary>
    /// Принимает посетителя для экспорта данных.
    /// </summary>
    /// <param name="visitor">Объект посетителя.</param>
    public void Accept(IExportVisitor visitor)
    {
        visitor.VisitOperation(this);
    }
    
    // Переопределенный метод ToString.
    public override string ToString() => $"Операция: {Id}, Тип: {Type}, Счет: {BankAccountId}, Сумма: {Amount:C}, Дата: {Date}";
}