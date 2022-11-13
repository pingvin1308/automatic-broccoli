using System.Text.Json;
using AutomaticBroccoli.DataAccess;

namespace AutomaticBroccoli.CLI;

public static class OpenLoopsRepository
{
    public static string DirectoryName = "./openLoops/";
    public static string DataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DirectoryName);

    public static Guid Add(OpenLoop newOpenLoop)
    {
        Directory.CreateDirectory(DirectoryName);

        var json = JsonSerializer.Serialize(
            newOpenLoop,
            new JsonSerializerOptions { WriteIndented = true });

        var fileName = $"{newOpenLoop.Id}.json";
        var filePath = Path.Combine(DataDirectory, fileName);
        File.WriteAllText(filePath, json);

        return newOpenLoop.Id;
    }

    public static OpenLoop[] Get()
    {
        var files = Directory.GetFiles(DataDirectory);

        var openLoops = new List<OpenLoop>();

        foreach (var file in files)
        {
            var json = File.ReadAllText(file);
            var openLoop = JsonSerializer.Deserialize<OpenLoop>(json);
            if (openLoop == null)
            {
                throw new Exception("OpenLoop cannot be deserialized.");
            }

            openLoops.Add(openLoop);
        }

        return openLoops.ToArray();
    }
}