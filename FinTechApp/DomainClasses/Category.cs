using FinTechApp.Interfaces;
using FinTechApp.Types;

namespace FinTechApp.DomainClasses;

public class Category
{
    /// <summary>
    /// Уникальный идентификатор категории.
    /// </summary>
    public Guid Id { get; private set; }
    
    /// <summary>
    /// Тип категории: доход или расход.
    /// </summary>
    public OperationType Type { get; private set; }
    
    /// <summary>
    /// Название категории.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Создает новый экземпляр категории.
    /// </summary>
    /// <param name="id">Уникальный идентификатор.</param>
    /// <param name="type">Тип категории.</param>
    /// <param name="name">Название категории.</param>
    public Category(OperationType type, string name) 
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название категории не может быть пустым.", nameof(name));
        if (!Enum.IsDefined(type))
            throw new ArgumentException("Недопустимый тип категории.", nameof(type));

        Id = Guid.NewGuid();
        Type = type;
        Name = name;
    }
    
    [System.Text.Json.Serialization.JsonConstructor]
    public Category(Guid id, OperationType type, string name) : this(type, name)
    {
        Id = id;
    }
    
    /// <summary>
    /// Изменяет название категории.
    /// </summary>
    /// <param name="newName">Новое название категории.</param>
    public void UpdateName(string newName)
    {
        // Проверка: Новое название не может быть пустым.
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Название категории не может быть пустым.", nameof(newName));

        Name = newName;
    }

    public void UpdateType(OperationType newType)
    {
        if (!Enum.IsDefined(newType))
            throw new ArgumentException("Недопустимый тип операции.");
        
        Type = newType;
    }
    
    /// <summary>
    /// Принимает посетителя для экспорта данных.
    /// </summary>
    /// <param name="visitor">Объект посетителя.</param>
    public void Accept(IExportVisitor visitor)
    {
        visitor.VisitCategory(this);
    }

    // Переопределенный метод ToString.
    public override string ToString() => $"Категория: {Name}, Тип: {Type}, ID: {Id}";
}