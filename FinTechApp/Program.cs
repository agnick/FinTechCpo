using FinTechApp.Commands;
using FinTechApp.Interfaces;
using FinTechApp.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace FinTechApp
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        private static string _currentMenu = "main"; // Текущее меню: "main", "accounts", "categories", "operations"

        /// <summary>
        /// Точка входа в приложение.
        /// </summary>
        public static void Main()
        {
            // Настройка DI-контейнера
            _serviceProvider = ConfigureServices();

            // Запуск основного цикла приложения
            RunApplication();
        }

        /// <summary>
        /// Настраивает DI-контейнер и возвращает поставщика сервисов.
        /// </summary>
        /// <returns>Поставщик сервисов DI.</returns>
        private static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            // Регистрация FinTechManager и прокси
            serviceCollection.AddSingleton<FinTechManager>();
            serviceCollection.AddSingleton<IFinTechManager>(provider =>
                new FinTechManagerProxy(provider.GetRequiredService<FinTechManager>()));

            // Регистрация команд как Transient
            serviceCollection.AddTransient<CreateBankAccountCommand>();
            serviceCollection.AddTransient<EditBankAccountCommand>();
            serviceCollection.AddTransient<RemoveBankAccountCommand>();
            serviceCollection.AddTransient<PerformAccountOperationCommand>(); // Новая команда
            serviceCollection.AddTransient<CreateCategoryCommand>();
            serviceCollection.AddTransient<EditCategoryCommand>();
            serviceCollection.AddTransient<RemoveCategoryCommand>();
            serviceCollection.AddTransient<CreateOperationCommand>();
            serviceCollection.AddTransient<EditOperationCommand>();
            serviceCollection.AddTransient<RemoveOperationCommand>();
            serviceCollection.AddTransient<ListDataCommand>();
            serviceCollection.AddTransient<AnalyticsCommand>();
            serviceCollection.AddTransient<ImportExportCommand>();

            return serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Запускает основной цикл приложения с обновляемым меню.
        /// </summary>
        private static void RunApplication()
        {
            while (true)
            {
                Console.Clear(); // Очищаем консоль для обновления меню
                DisplayMenu();

                Console.Write("Ваш выбор: ");
                var choice = Console.ReadLine();

                if (!HandleChoice(choice))
                    break; // Выход из приложения
            }
        }

        /// <summary>
        /// Отображает текущее меню в зависимости от состояния.
        /// </summary>
        private static void DisplayMenu()
        {
            switch (_currentMenu)
            {
                case "main":
                    Console.WriteLine("=== Главное меню ===");
                    Console.WriteLine("1. Работа со счетами");
                    Console.WriteLine("2. Работа с категориями");
                    Console.WriteLine("3. Работа с операциями");
                    Console.WriteLine("4. Аналитика");
                    Console.WriteLine("5. Импорт/Экспорт данных");
                    Console.WriteLine("6. Просмотр всех данных");
                    Console.WriteLine("0. Выход");
                    break;

                case "accounts":
                    Console.WriteLine("=== Меню счетов ===");
                    Console.WriteLine("1. Добавить счет");
                    Console.WriteLine("2. Редактировать счет");
                    Console.WriteLine("3. Удалить счет");
                    Console.WriteLine("4. Выполнить операцию по счету");
                    Console.WriteLine("0. Назад");
                    break;

                case "categories":
                    Console.WriteLine("=== Меню категорий ===");
                    Console.WriteLine("1. Добавить категорию");
                    Console.WriteLine("2. Редактировать категорию");
                    Console.WriteLine("3. Удалить категорию");
                    Console.WriteLine("0. Назад");
                    break;

                case "operations":
                    Console.WriteLine("=== Меню операций ===");
                    Console.WriteLine("1. Добавить операцию");
                    Console.WriteLine("2. Редактировать операцию");
                    Console.WriteLine("3. Удалить операцию");
                    Console.WriteLine("0. Назад");
                    break;
            }
        }

        /// <summary>
        /// Обрабатывает выбор пользователя и возвращает false для выхода.
        /// </summary>
        /// <param name="choice">Выбор пользователя.</param>
        /// <returns>True, если продолжить выполнение, False для выхода.</returns>
        private static bool HandleChoice(string choice)
        {
            switch (_currentMenu)
            {
                case "main":
                    switch (choice)
                    {
                        case "1":
                            _currentMenu = "accounts";
                            break;
                        case "2":
                            _currentMenu = "categories";
                            break;
                        case "3":
                            _currentMenu = "operations";
                            break;
                        case "4":
                            ExecuteCommand(_serviceProvider.GetRequiredService<AnalyticsCommand>());
                            Pause();
                            break;
                        case "5":
                            ExecuteCommand(_serviceProvider.GetRequiredService<ImportExportCommand>());
                            Pause();
                            break;
                        case "6":
                            ExecuteCommand(_serviceProvider.GetRequiredService<ListDataCommand>());
                            Pause();
                            break;
                        case "0":
                            Console.WriteLine("До свидания!");
                            return false;
                        default:
                            Console.WriteLine("Ошибка: Неверный выбор.");
                            Pause();
                            break;
                    }

                    break;

                case "accounts":
                    switch (choice)
                    {
                        case "1":
                            ExecuteCommand(_serviceProvider.GetRequiredService<CreateBankAccountCommand>());
                            Pause();
                            break;
                        case "2":
                            ExecuteCommand(_serviceProvider.GetRequiredService<EditBankAccountCommand>());
                            Pause();
                            break;
                        case "3":
                            ExecuteCommand(_serviceProvider.GetRequiredService<RemoveBankAccountCommand>());
                            Pause();
                            break;
                        case "4":
                            ExecuteCommand(_serviceProvider.GetRequiredService<PerformAccountOperationCommand>());
                            Pause();
                            break;
                        case "0":
                            _currentMenu = "main";
                            break;
                        default:
                            Console.WriteLine("Ошибка: Неверный выбор.");
                            Pause();
                            break;
                    }

                    break;

                case "categories":
                    switch (choice)
                    {
                        case "1":
                            ExecuteCommand(_serviceProvider.GetRequiredService<CreateCategoryCommand>());
                            Pause();
                            break;
                        case "2":
                            ExecuteCommand(_serviceProvider.GetRequiredService<EditCategoryCommand>());
                            Pause();
                            break;
                        case "3":
                            ExecuteCommand(_serviceProvider.GetRequiredService<RemoveCategoryCommand>());
                            Pause();
                            break;
                        case "0":
                            _currentMenu = "main";
                            break;
                        default:
                            Console.WriteLine("Ошибка: Неверный выбор.");
                            Pause();
                            break;
                    }

                    break;

                case "operations":
                    switch (choice)
                    {
                        case "1":
                            ExecuteCommand(_serviceProvider.GetRequiredService<CreateOperationCommand>());
                            Pause();
                            break;
                        case "2":
                            ExecuteCommand(_serviceProvider.GetRequiredService<EditOperationCommand>());
                            Pause();
                            break;
                        case "3":
                            ExecuteCommand(_serviceProvider.GetRequiredService<RemoveOperationCommand>());
                            Pause();
                            break;
                        case "0":
                            _currentMenu = "main";
                            break;
                        default:
                            Console.WriteLine("Ошибка: Неверный выбор.");
                            Pause();
                            break;
                    }

                    break;
            }

            return true;
        }

        /// <summary>
        /// Выполняет команду с обработкой исключений.
        /// </summary>
        /// <param name="command">Команда для выполнения.</param>
        private static void ExecuteCommand(ICommand command)
        {
            try
            {
                command.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка выполнения команды: {ex.Message}");
            }
        }

        /// <summary>
        /// Приостанавливает выполнение, ожидая нажатия клавиши пользователем.
        /// </summary>
        private static void Pause()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey(true);
        }
    }
}