namespace AutomaticBroccoli.API.Controllers;

public static class Attachments
{
    public static string Path = System.IO.Path.Combine(AppContext.BaseDirectory, "files");

    public static async Task<(string FileName, Guid AttachmentId)> Create(IFormFile file, string name)
    {
        var fileExtension = System.IO.Path.GetExtension(name);
        var attachmentId = Guid.NewGuid();
        var fileName = attachmentId + fileExtension;
        if (!Directory.Exists(Path))
        {
            Directory.CreateDirectory(Path);
        }
        using (var fileStream = new FileStream(
            path: System.IO.Path.Combine(Path, fileName),
            mode: FileMode.Create,
            access: FileAccess.Write,
            share: FileShare.None))
        {
            await file.CopyToAsync(fileStream);
        }

        return (fileName, attachmentId);
    }
}
