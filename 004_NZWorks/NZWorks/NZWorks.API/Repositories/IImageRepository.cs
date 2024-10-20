using NZWorks.API.Models.Domain;

namespace NZWorks.API.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
