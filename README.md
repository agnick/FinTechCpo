# -FinTech_KPO_IDZ-1

# Отчёт по проекту FinTechApp

## Общая идея решения

**FinTechApp** — консольное приложение для управления личными финансами. Функционал:
- Управление счетами, категориями, операциями (создание, редактирование, удаление).
- Выполнение операций (доходы/расходы) с обновлением баланса.
- Аналитика: разница доходов/расходов, группировка операций по категориям.
- Импорт/экспорт данных (CSV, JSON, YAML).
- Выполнение существующих операций с отображением счетов.

**Изменения:**
- Обновляемое меню (`Console.Clear()` в `Program.cs`).
- Команда `PerformAccountOperationCommand` для выполнения существующих операций.
- Кэширование через `FinTechManagerProxy` (30 секунд).

## Применённые принципы SOLID и GRASP

### SOLID
- **SRP:** `BankAccount`, `Category`, `Operation` — каждый класс отвечает за свою сущность. Команды (`CreateBankAccountCommand`, `PerformAccountOperationCommand`) выполняют одну задачу.
- **OCP:** Экспорт через Visitor (`CsvExportVisitor`, `JsonExportVisitor`, `YamlExportVisitor`) — легко добавить новый формат.
- **LSP:** `IFinTechManager` реализован в `FinTechManager` и `FinTechManagerProxy`.
- **ISP:** Интерфейсы `IExportVisitor`, `ICommand` содержат только нужные методы.
- **DIP:** DI в `Program.cs` для внедрения `IFinTechManager` в команды.

### GRASP
- **Low Coupling:** `FinTechManager` как фасад минимизирует зависимости.
- **High Cohesion:** `BankAccount`, `FinTechManager`, `CsvExportVisitor` имеют чёткие обязанности.
- **Information Expert:** `BankAccount` управляет балансом, `Operation` — своими данными.
- **Creator:** `DomainFactory` создаёт объекты.
- **Controller:** `Program.cs` управляет взаимодействием.

## Применённые паттерны GoF

- **Factory Method:** `DomainFactory` (`CreateBankAccount`, `CreateCategory`, `CreateOperation`) — централизованное создание с валидацией.
- **Facade:** `FinTechManager` — единый интерфейс для работы с системой.
- **Proxy:** `FinTechManagerProxy` — кэширование для оптимизации.
- **Visitor:** `CsvExportVisitor`, `JsonExportVisitor`, `YamlExportVisitor` — расширяемый экспорт.
- **Command:** `CreateBankAccountCommand`, `PerformAccountOperationCommand` — инкапсуляция действий.
- **Dependency Injection:** `Program.cs` — внедрение зависимостей через DI.

## Инструкция по запуску

### Требования
- .NET 8.0 SDK.

### Запуск приложения
1. Склонируйте проект.
2. В терминале:

cd FinTechApp
dotnet run

3. Следуйте инструкциям в консоли.

### Запуск тестов
1. В терминале:

cd FinTechApp.Tests
dotnet test
