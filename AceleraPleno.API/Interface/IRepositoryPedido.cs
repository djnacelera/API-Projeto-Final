using AceleraPleno.API.Models;

namespace AceleraPleno.API.Interface
{
    public interface IRepositoryPedido<TEntity> : IRepository<TEntity>
    {
        Task<Pedido> Adicionar2(Guid mesaId, Guid pratoId, int qtd, decimal valor);
        Task<string> AlterarPedidoParaPreparando(Guid id);
        Task<string> AlterarPedidoParaDisponivel(Guid id);
        Task<string> AlterarPedidoParaEntregue(Guid id);
        Task<string> AlterarPedidoParaCancelado(Guid id);
    }
}
