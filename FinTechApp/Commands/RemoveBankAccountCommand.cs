using FinTechApp.Interfaces;

namespace FinTechApp.Commands;

/// <summary>
/// Команда для удаления банковского счета.
/// </summary>
public class RemoveBankAccountCommand : ICommand
{
    private readonly IFinTechManager _manager;

    /// <summary>
    /// Инициализирует команду с указанным менеджером финансовых данных.
    /// </summary>
    /// <param name="manager">Менеджер финансовых данных.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если менеджер равен null.</exception>
    public RemoveBankAccountCommand(IFinTechManager manager)
    {
        _manager = manager ?? throw new ArgumentNullException(nameof(manager), "Менеджер не может быть null.");
    }

    /// <summary>
    /// Выполняет удаление банковского счета по введённому ID.
    /// </summary>
    public void Execute()
    {
        try
        {
            var accounts = _manager.GetBankAccounts().ToList();
            if (!accounts.Any())
            {
                Console.WriteLine("Ошибка: Нет доступных счетов.");
                return;
            }

            Console.WriteLine("Доступные счета:");
            foreach (var account in accounts)
                Console.WriteLine($"{account.Id} - {account.Name} (Баланс: {account.Balance:C})");
            
            Console.Write("Введите ID счета для удаления: ");
            var input = Console.ReadLine();
            if (!Guid.TryParse(input, out Guid id))
            {
                Console.WriteLine("Ошибка: Неверный формат ID счета.");
                return;
            }

            _manager.RemoveBankAccount(id);
            Console.WriteLine("Счет удалён успешно.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении счета: {ex.Message}");
        }
    }
}