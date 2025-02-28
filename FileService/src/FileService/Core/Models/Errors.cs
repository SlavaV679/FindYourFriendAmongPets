namespace FileService.Core.Models;

public static class Errors
{
    public static class Files
    {
        public static Error FailUpload()
        {
            return Error.Failure("file.upload.failed", "Fail to upload file");
        }

        public static Error FailRemove()
        {
            return Error.Failure("file.remove.failed", "Fail to remove file");
        }

    }

   
}