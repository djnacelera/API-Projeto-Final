using AceleraPleno.API.Models;
using AceleraPleno.API.Models.PartialModels;

namespace AceleraPleno.API.Interface
{
    public interface IRepositoryPrato<TEntity> : IRepository<TEntity>
    {
        Task<string> ConverteImg(string path);

        Task<string> AtivarPrato(Guid id);
        Task<string> InativarPrato(Guid id);
    }
}
