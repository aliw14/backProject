using System.IO;

namespace BackProject.Areas.AdminPanel.Data
{
    public static class FileExtensions
    {
        public async static Task<string> GenerateFile(this IFormFile file, string path)
        {
            var fileName = $"{Guid.NewGuid()}-{file.FileName}";

            path = Path.Combine(path, fileName);

            var fs = new FileStream(path, FileMode.CreateNew);

            await file.CopyToAsync(fs);

            fs.Close();

            return fileName;
        }
    }
}

