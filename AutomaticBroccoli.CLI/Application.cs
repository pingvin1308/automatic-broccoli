using AutomaticBroccoli.CLI.Operations;

namespace AutomaticBroccoli.CLI;

public class Application
{
    // menu with operations
    private readonly Dictionary<string, IOperation> _menu;

    public Application()
    {
        _menu = new Dictionary<string, IOperation>
        {
            { "create", new CreateNewNoteOperation() },
            { "get", new GetNotesOperation() },
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="token"></param>
    public void Run(CancellationToken token)
    {
        Console.Clear();

        while (!token.IsCancellationRequested)
        {
            PrintMenu();
            var operationName = Console.ReadLine();

            if (!_menu.TryGetValue(operationName, out var operation) || operation == null)
            {
                Console.WriteLine($"Команды '{operationName}' не существует");
                Console.WriteLine("Нажмите любую клавишу, чтобы продолжить");
                Console.ReadKey(true);
                Console.Clear();
                continue;
            }

            operation.Invoke();
        }
    }

    private void PrintMenu()
    {
        Console.WriteLine("Список доступных операций над заметками:");
        foreach (var item in _menu)
        {
            Console.WriteLine($"- {item.Key}");
        }
        Console.WriteLine("Введите Ctrl + C чтобы выйти из программы");
    }
}
