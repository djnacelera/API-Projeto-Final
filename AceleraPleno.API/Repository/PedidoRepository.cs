using AceleraPleno.API.Data;
using AceleraPleno.API.Interface;
using AceleraPleno.API.Models;
using AceleraPleno.API.Models.PartialModels;
using Microsoft.EntityFrameworkCore;

namespace AceleraPleno.API.Repository
{
    public class PedidoRepository : IRepository<Pedido>
    {
        private readonly DataContext _dataContext;
        private readonly IRepositoryMesa<Mesa> _mesa;
        public PedidoRepository(DataContext dataContext, IRepositoryMesa<Mesa> mesa)
        {
            _dataContext = dataContext;
            _mesa = mesa;
        }

        public async Task<IEnumerable<Pedido>> Listar()
        {
            return await _dataContext.Pedidos.ToListAsync();
        }

        public async Task<Pedido> FiltrarId(Guid id)
        {
            return await _dataContext.Pedidos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Pedido> Adicionar(Pedido pedido)
        {
            pedido.DataInclusao = DateTime.Now;
            pedido.DtRecebimento = DateTime.Now;
            pedido.StatusPedido = Models.Enuns.StatusPedido.Recebido;
            await _dataContext.Pedidos.AddAsync(pedido);
            _dataContext.SaveChanges();

            OcuparMesa mesa = new OcuparMesa() {
                MesaId = pedido.MesaId
            };

            _mesa.OcuparMesa(mesa);
            return pedido;
        }

        public async Task<Pedido> Atualizar(Pedido pedido, Guid id)
        {
            if (pedido == null) throw new System.Exception("Erro ao atualizar");

            Pedido pedidoDb = await FiltrarId(id);
            if (pedidoDb == null) throw new System.Exception(string.Format("Pedido não encontrado"));

            try
            {
                pedidoDb.MesaId = pedido.MesaId == null ? pedidoDb.MesaId : pedido.MesaId;
                pedidoDb.PratoId = pedido.PratoId == null ? pedidoDb.PratoId : pedido.PratoId;
                pedidoDb.Quantidade = pedido.Quantidade == 0 ? pedidoDb.Quantidade : pedido.Quantidade;
                pedidoDb.StatusPedido = pedido.StatusPedido == null ? pedidoDb.StatusPedido : pedido.StatusPedido;
                pedidoDb.Valor = pedido.Valor == 0 ? pedidoDb.Valor : pedido.Valor;
                pedidoDb.DataAlteracao = DateTime.Now;

                _dataContext.Pedidos.Update(pedidoDb);

                await _dataContext.SaveChangesAsync();
                return pedidoDb;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message.ToString());
            }
        }

        public async Task<Pedido> AlterarPedidoParaPreparando(Pedido pedido, Guid id)
        {
            if (pedido == null) throw new System.Exception("Erro ao alterar status");

            Pedido pedidoDb = await FiltrarId(id);
            if (pedidoDb == null) throw new System.Exception(string.Format("Pedido não encontrado"));

            pedidoDb.StatusPedido = Models.Enuns.StatusPedido.Preparando;
            pedidoDb.DataAlteracao = DateTime.Now;

            _dataContext.Pedidos.Update(pedidoDb);

            await _dataContext.SaveChangesAsync();
            return pedidoDb;
        }

        public async Task<Pedido> AlterarPedidoParaEntregue(Pedido pedido, Guid id)
        {
            if (pedido == null) throw new System.Exception("Erro ao alterar status");

            Pedido pedidoDb = await FiltrarId(id);
            if (pedidoDb == null) throw new System.Exception(string.Format("Pedido não encontrado"));

            pedidoDb.StatusPedido = Models.Enuns.StatusPedido.Entregue;
            pedidoDb.DataAlteracao = DateTime.Now;

            _dataContext.Pedidos.Update(pedidoDb);

            await _dataContext.SaveChangesAsync();
            return pedidoDb;
        }

        public async Task<Pedido> AlterarPedidoParaCancelado(Pedido pedido, Guid id)
        {
            if (pedido == null) throw new System.Exception("Erro ao alterar status");

            Pedido pedidoDb = await FiltrarId(id);
            if (pedidoDb == null) throw new System.Exception(string.Format("Pedido não encontrado"));

            pedidoDb.StatusPedido = Models.Enuns.StatusPedido.Cancelado;
            pedidoDb.DataAlteracao = DateTime.Now;

            _dataContext.Pedidos.Update(pedidoDb);

            await _dataContext.SaveChangesAsync();
            return pedidoDb;
        }

        public async Task<bool> Excluir(Guid id)
        {
            Pedido pedido = await FiltrarId(id);

            if (pedido == null) throw new System.Exception("Erro ao Excluir");

            _dataContext.Pedidos.Remove(pedido);
            await _dataContext.SaveChangesAsync();
            return true;
        }
    }
}
