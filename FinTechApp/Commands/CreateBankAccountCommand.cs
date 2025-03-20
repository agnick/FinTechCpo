using System.Globalization;
using FinTechApp.DomainClasses;
using FinTechApp.Interfaces;

namespace FinTechApp.Commands;

/// <summary>
/// Команда для создания нового банковского счета.
/// </summary>
public class CreateBankAccountCommand : ICommand
{
    private readonly IFinTechManager _manager;

    /// <summary>
    /// Инициализирует команду с указанным менеджером финансовых данных.
    /// </summary>
    /// <param name="manager">Менеджер финансовых данных.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если менеджер равен null.</exception>
    public CreateBankAccountCommand(IFinTechManager manager)
    {
        _manager = manager ?? throw new ArgumentNullException(nameof(manager), "Менеджер не может быть null.");
    }

    /// <summary>
    /// Выполняет создание банковского счета с вводом данных пользователем.
    /// </summary>
    public void Execute()
    {
        try
        {
            Console.Write("Введите название счета: ");
            var name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Ошибка: Название счета не может быть пустым.");
                return;
            }

            Console.Write("Введите начальный баланс: ");
            var balanceInput = Console.ReadLine();
            if (!decimal.TryParse(balanceInput, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal balance) ||
                balance < 0)
            {
                Console.WriteLine("Ошибка: Начальный баланс должен быть неотрицательным числом.");
                return;
            }

            var account = new BankAccount(name, balance);
            _manager.AddBankAccount(account);
            Console.WriteLine($"Счет создан успешно с ID: {account.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при создании счета: {ex.Message}");
        }
    }
}