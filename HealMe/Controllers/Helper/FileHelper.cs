namespace HealMe.Controllers.Helper
{
    public static class FileHelper
    {
        public static async Task<string> SaveImageAsync(IFormFile file, IWebHostEnvironment env, string folder = "images")
        {
            if (file == null || file.Length == 0)
                return null;

            string fileName = Path.GetFileName(file.FileName);
            string folderPath = Path.Combine(env.WebRootPath, folder);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/{folder}/{fileName}";
        }
    }
}
