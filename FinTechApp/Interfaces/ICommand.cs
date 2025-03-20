namespace FinTechApp.Interfaces;

/// <summary>
/// Определяет интерфейс для команд, которые выполняют действия в системе.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Выполняет действие команды.
    /// </summary>
    void Execute();
}