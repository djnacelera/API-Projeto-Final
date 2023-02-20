namespace AceleraPleno.API.Interface
{
    public interface IRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> Listar();
        Task<TEntity> FiltrarId(Guid id);
        Task<TEntity> Adicionar(TEntity cartao);
        Task<TEntity> Atualizar(TEntity cartao, Guid id);
        Task<bool> Excluir(Guid id);
    }
}
