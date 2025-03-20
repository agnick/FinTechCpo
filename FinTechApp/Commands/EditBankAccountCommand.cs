using FinTechApp.Interfaces;

namespace FinTechApp.Commands;

/// <summary>
/// Команда для редактирования банковского счета.
/// </summary>
public class EditBankAccountCommand : ICommand
{
    private readonly IFinTechManager _manager;

    /// <summary>
    /// Инициализирует команду с указанным менеджером финансовых данных.
    /// </summary>
    /// <param name="manager">Менеджер финансовых данных.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если менеджер равен null.</exception>
    public EditBankAccountCommand(IFinTechManager manager)
    {
        _manager = manager ?? throw new ArgumentNullException(nameof(manager), "Менеджер не может быть null.");
    }

    /// <summary>
    /// Выполняет редактирование банковского счета по введённому ID.
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
            
            Console.Write("Введите ID счета для редактирования: ");
            var input = Console.ReadLine();
            if (!Guid.TryParse(input, out Guid id))
            {
                Console.WriteLine("Ошибка: Неверный формат ID счета.");
                return;
            }

            Console.Write("Введите новое название счета: ");
            var newName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newName))
            {
                Console.WriteLine("Ошибка: Название счета не может быть пустым.");
                return;
            }

            _manager.EditBankAccount(id, newName);
            Console.WriteLine("Счет отредактирован успешно.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при редактировании счета: {ex.Message}");
        }
    }
}