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

        var openLoop = new OpenLoop
        {
            Note = note,
            CreatedDate = DateTimeOffset.UtcNow
        };

        OpenLoopsRepository.Add(openLoop);
    }
}
