namespace TemplateApi.Helpers;

public class FileUploader
{
    public string DestinationPath { get; }
    public string[] AllowedExtensions { get; }
    public long MaxSize { get; }

    public FileUploader(string? destinationPath, string[] allowedExtensions, long maxSize)
    {
        if (string.IsNullOrEmpty(destinationPath))
        {
            throw new ArgumentException("DestinationPath cannot be null or empty.");
        }
        DestinationPath = destinationPath;
        AllowedExtensions = allowedExtensions;
        MaxSize = maxSize;
    }

    public async Task<UploadInfo> UploadSingleFile(IFormFile file)
    {
        if (file is null)
        {
            throw new ArgumentException("No file provided.");
        }

        ValidateFile(file);

        var fileName = GenerateUniqueFileName(file.FileName);
        await SaveUploadedFile(file, fileName);

        return new UploadInfo
        {
            OriginalFileName = file.FileName,
            FinalName = fileName,
            Size = file.Length,
            ContentType = file.ContentType,
            Extension = Path.GetExtension(file.FileName),
            UploadTime = DateTime.Now
        };
    }

    public async Task<List<UploadInfo>> UploadMultipleFiles(IFormFileCollection files)
    {
        if (files is null || files.Count == 0)
        {
            throw new ArgumentException("No files provided.");
        }

        var uploadInfos = new List<UploadInfo>();

        foreach (var file in files)
        {
            if (file.Length == 0)
            {
                continue;
            }

            ValidateFile(file);

            var fileName = GenerateUniqueFileName(file.FileName);
            await SaveUploadedFile(file, fileName);

            uploadInfos.Add(new UploadInfo
            {
                OriginalFileName = file.FileName,
                FinalName = fileName,
                Size = file.Length,
                ContentType = file.ContentType,
                Extension = Path.GetExtension(file.FileName),
                UploadTime = DateTime.Now
            });
        }

        return uploadInfos;
    }

    private void ValidateFile(IFormFile file)
    {
        // Validate file size
        if (file.Length > MaxSize)
        {
            throw new ArgumentException($"File '{file.FileName}' is too large. Maximum allowed size: {MaxSize / 1024 / 1024}MB.");
        }

        // Validate file extension
        var fileExt = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!IsValidExtension(fileExt))
        {
            throw new ArgumentException($"File '{file.FileName}' has an invalid extension. Allowed extensions: {string.Join(", ", AllowedExtensions)}.");
        }
    }

    private bool IsValidExtension(string extension)
    {
        return AllowedExtensions.Contains(extension.ToLowerInvariant());
    }

    private string GenerateUniqueFileName(string fileName)
    {
        return Guid.NewGuid().ToString() + Path.GetExtension(fileName);
    }

    private async Task SaveUploadedFile(IFormFile file, string fileName)
    {
        var filePath = Path.Combine(DestinationPath, fileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
    }
}

public class UploadInfo
{
    public string? OriginalFileName { get; set; }
    public string? FinalName { get; set; }
    public long Size { get; set; }
    public string? ContentType { get; set; }
    public string? Extension { get; set; }
    public DateTime UploadTime { get; set; }
}
