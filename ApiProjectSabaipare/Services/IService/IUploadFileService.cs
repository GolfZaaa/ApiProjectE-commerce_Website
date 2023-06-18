namespace ApiProjectSabaipare.Services.IService
{
    public interface IUploadFileService
    {
        bool IsUpload(IFormFileCollection formFiles);


        string Validation(IFormFileCollection formFiles);


        Task<List<string>> UploadImages(IFormFileCollection formFiles);

        Task DeleteFileImages(List<string> files);
    }
}
