namespace AutomaticBroccoli.CLI.Operations;

public class GetNotesOperation : IOperation
{
    public void Invoke()
    {
        var openLoops = OpenLoopsRepository.Get();
        var groups = openLoops
            .GroupBy(x => new DateTime(
                x.CreatedDate.Value.Year,
                x.CreatedDate.Value.Month,
                x.CreatedDate.Value.Day));

        foreach (var groupOfOpenLoops in groups)
        {
            Console.WriteLine($"Ваши заботы за: {groupOfOpenLoops.Key:dd.MM.yyyy}");

            foreach (var openLoop in groupOfOpenLoops.ToArray())
            {
                Console.WriteLine(openLoop.Note);
            }
        }
    }
}
