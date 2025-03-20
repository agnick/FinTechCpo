using FinTechApp.DomainClasses;
using FinTechApp.Types;

namespace FinTechApp.Managers;

/// <summary>
/// Прокси для FinTechManager, реализующий кэширование данных для ускорения доступа.
/// </summary>
public class FinTechManagerProxy : IFinTechManager
{
    private readonly IFinTechManager _realManager;
    private IEnumerable<BankAccount> _cachedAccounts;
    private DateTime _accountsCacheTimestamp;
    private IEnumerable<Category> _cachedCategories;
    private DateTime _categoriesCacheTimestamp;
    private IEnumerable<Operation> _cachedOperations;
    private DateTime _operationsCacheTimestamp;

    /// <summary>
    /// Конструктор прокси, принимающий реальный менеджер.
    /// </summary>
    /// <param name="realManager">Реальный менеджер, к которому прокси будет обращаться за данными.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если реальный менеджер равен null.</exception>
    public FinTechManagerProxy(IFinTechManager realManager)
    {
        _realManager = realManager ??
                       throw new ArgumentNullException(nameof(realManager), "Реальный менеджер не может быть null.");
    }

    /// <summary>
    /// Добавляет новый банковский счет в систему и сбрасывает кэш счетов.
    /// </summary>
    /// <param name="account">Экземпляр банковского счета для добавления.</param>
    public void AddBankAccount(BankAccount account)
    {
        _realManager.AddBankAccount(account);
        InvalidateAccountsCache();
    }

    /// <summary>
    /// Удаляет банковский счет по его идентификатору и сбрасывает кэш счетов.
    /// </summary>
    /// <param name="accountId">Уникальный идентификатор счета.</param>
    public void RemoveBankAccount(Guid accountId)
    {
        _realManager.RemoveBankAccount(accountId);
        InvalidateAccountsCache();
    }

    /// <summary>
    /// Получает список всех банковских счетов. Использует кэш, если он актуален (менее 30 секунд).
    /// </summary>
    /// <returns>Коллекция всех банковских счетов.</returns>
    public IEnumerable<BankAccount> GetBankAccounts()
    {
        if (_cachedAccounts != null && (DateTime.Now - _accountsCacheTimestamp).TotalSeconds < 30)
            return _cachedAccounts;

        _cachedAccounts = _realManager.GetBankAccounts().ToList();
        _accountsCacheTimestamp = DateTime.Now;
        return _cachedAccounts;
    }

    /// <summary>
    /// Редактирует название существующего банковского счета и сбрасывает кэш счетов.
    /// </summary>
    /// <param name="accountId">Уникальный идентификатор счета.</param>
    /// <param name="newName">Новое название счета.</param>
    public void EditBankAccount(Guid accountId, string newName)
    {
        _realManager.EditBankAccount(accountId, newName);
        InvalidateAccountsCache();
    }

    /// <summary>
    /// Добавляет новую категорию в систему и сбрасывает кэш категорий.
    /// </summary>
    /// <param name="category">Экземпляр категории для добавления.</param>
    public void AddCategory(Category category)
    {
        _realManager.AddCategory(category);
        InvalidateCategoriesCache();
    }

    /// <summary>
    /// Удаляет категорию по её идентификатору и сбрасывает кэш категорий.
    /// </summary>
    /// <param name="categoryId">Уникальный идентификатор категории.</param>
    public void RemoveCategory(Guid categoryId)
    {
        _realManager.RemoveCategory(categoryId);
        InvalidateCategoriesCache();
    }

    /// <summary>
    /// Получает список всех категорий. Использует кэш, если он актуален (менее 30 секунд).
    /// </summary>
    /// <returns>Коллекция всех категорий.</returns>
    public IEnumerable<Category> GetCategories()
    {
        if (_cachedCategories != null && (DateTime.Now - _categoriesCacheTimestamp).TotalSeconds < 30)
            return _cachedCategories;

        _cachedCategories = _realManager.GetCategories().ToList();
        _categoriesCacheTimestamp = DateTime.Now;
        return _cachedCategories;
    }

    /// <summary>
    /// Редактирует существующую категорию и сбрасывает кэш категорий.
    /// </summary>
    /// <param name="categoryId">Уникальный идентификатор категории.</param>
    /// <param name="newName">Новое название категории.</param>
    /// <param name="newType">Новый тип категории (доход или расход).</param>
    public void EditCategory(Guid categoryId, string newName, OperationType newType)
    {
        _realManager.EditCategory(categoryId, newName, newType);
        InvalidateCategoriesCache();
    }

    /// <summary>
    /// Добавляет новую финансовую операцию в систему и сбрасывает кэш операций.
    /// </summary>
    /// <param name="operation">Экземпляр операции для добавления.</param>
    public void AddOperation(Operation operation)
    {
        _realManager.AddOperation(operation);
        InvalidateOperationsCache();
    }

    /// <summary>
    /// Удаляет операцию по её идентификатору и сбрасывает кэш операций.
    /// </summary>
    /// <param name="operationId">Уникальный идентификатор операции.</param>
    public void RemoveOperation(Guid operationId)
    {
        _realManager.RemoveOperation(operationId);
        InvalidateOperationsCache();
    }

    /// <summary>
    /// Получает список всех операций. Использует кэш, если он актуален (менее 30 секунд).
    /// </summary>
    /// <returns>Коллекция всех операций.</returns>
    public IEnumerable<Operation> GetOperations()
    {
        if (_cachedOperations != null && (DateTime.Now - _operationsCacheTimestamp).TotalSeconds < 30)
            return _cachedOperations;

        _cachedOperations = _realManager.GetOperations().ToList();
        _operationsCacheTimestamp = DateTime.Now;
        return _cachedOperations;
    }

    /// <summary>
    /// Редактирует существующую операцию и сбрасывает кэш операций.
    /// </summary>
    /// <param name="operationId">Уникальный идентификатор операции.</param>
    /// <param name="newAmount">Новая сумма операции.</param>
    /// <param name="newDate">Новая дата операции.</param>
    /// <param name="newDescription">Новое описание операции.</param>
    /// <param name="newCategoryId">Новый идентификатор категории.</param>
    public void EditOperation(Guid operationId, decimal newAmount, DateTime newDate, string newDescription,
        Guid newCategoryId)
    {
        _realManager.EditOperation(operationId, newAmount, newDate, newDescription, newCategoryId);
        InvalidateOperationsCache();
    }

    /// <summary>
    /// Вычисляет разницу между доходами и расходами за указанный период.
    /// </summary>
    /// <param name="start">Начальная дата периода.</param>
    /// <param name="end">Конечная дата периода.</param>
    /// <returns>Разница между доходами и расходами (доходы минус расходы).</returns>
    public decimal GetIncomeExpenseDifference(DateTime start, DateTime end)
    {
        return _realManager.GetIncomeExpenseDifference(start, end);
    }

    /// <summary>
    /// Группирует операции по категориям за указанный период.
    /// </summary>
    /// <param name="start">Начальная дата периода.</param>
    /// <param name="end">Конечная дата периода.</param>
    /// <returns>Словарь, где ключ — ID категории, значение — сумма операций в этой категории.</returns>
    public Dictionary<Guid, decimal> GroupOperationsByCategory(DateTime start, DateTime end)
    {
        return _realManager.GroupOperationsByCategory(start, end);
    }

    /// <summary>
    /// Экспортирует данные в указанный формат (CSV, JSON, YAML).
    /// </summary>
    /// <param name="format">Формат экспорта: "csv", "json" или "yaml".</param>
    /// <param name="directoryOrFilePath">Путь к директории (для CSV) или файлу (для JSON/YAML).</param>
    public void ExportData(string format, string directoryOrFilePath)
    {
        _realManager.ExportData(format, directoryOrFilePath);
    }

    /// <summary>
    /// Импортирует данные из указанного формата (CSV, JSON, YAML) и сбрасывает все кэши.
    /// </summary>
    /// <param name="format">Формат импорта: "csv", "json" или "yaml".</param>
    /// <param name="directoryOrFilePath">Путь к директории (для CSV) или файлу (для JSON/YAML).</param>
    public void ImportData(string format, string directoryOrFilePath)
    {
        _realManager.ImportData(format, directoryOrFilePath);
        InvalidateAllCaches();
    }

    /// <summary>
    /// Сбрасывает кэш банковских счетов.
    /// </summary>
    private void InvalidateAccountsCache() => _cachedAccounts = null;

    /// <summary>
    /// Сбрасывает кэш категорий.
    /// </summary>
    private void InvalidateCategoriesCache() => _cachedCategories = null;

    /// <summary>
    /// Сбрасывает кэш операций.
    /// </summary>
    private void InvalidateOperationsCache() => _cachedOperations = null;

    /// <summary>
    /// Сбрасывает все кэши (счета, категории, операции).
    /// </summary>
    private void InvalidateAllCaches()
    {
        InvalidateAccountsCache();
        InvalidateCategoriesCache();
        InvalidateOperationsCache();
    }
}