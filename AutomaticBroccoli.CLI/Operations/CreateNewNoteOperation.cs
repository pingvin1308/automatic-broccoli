using AutomaticBroccoli.DataAccess;

namespace AutomaticBroccoli.CLI.Operations;

public class CreateNewNoteOperation : IOperation
{
    public void Invoke()
    {
        Console.WriteLine("Что вас беспокоит сейчас?");

        string? note;

        do
        {
            note = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(note));

        var openLoop = new OpenLoop(
            id: Guid.NewGuid(),
            note: note,
            createdDate: DateTimeOffset.UtcNow);

        OpenLoopsRepository.Add(openLoop);
    }
}
