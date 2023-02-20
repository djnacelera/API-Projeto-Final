using AceleraPleno.API.Models;

namespace AceleraPleno.API.Interface
{
    public interface IRepositoryPrato<TEntity> : IRepository<TEntity>
    {
        Task<string> ConverteImg(string path);
    }
}
