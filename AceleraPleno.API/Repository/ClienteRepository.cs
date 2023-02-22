using AceleraPleno.API.Data;
using AceleraPleno.API.Interface;
using AceleraPleno.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AceleraPleno.API.Repository
{
    public class ClienteRepository : IRepository<Cliente>
    {
        private readonly DataContext _dataContext;
        public ClienteRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IEnumerable<Cliente>> Listar()
        {
            return await _dataContext.Clientes.ToListAsync();
        }
        public async Task<Cliente> FiltrarId(Guid id)
        {
            return await _dataContext.Clientes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Cliente> Adicionar(Cliente cliente)
        {
            cliente.DataInclusao = DateTime.Now;
            await _dataContext.Clientes.AddAsync(cliente);
            _dataContext.SaveChanges();
            return cliente;
        }
        public async Task<Cliente> Atualizar(Cliente cliente, Guid id)
        {
            if (cliente == null) throw new System.Exception("Erro ao atualizar");

            //var cartaoAtual = await _dataContext.CARTAO.FirstOrDefaultAsync(x => x.Id == id);
            Cliente clienteDb = await FiltrarId(id);
            if (clienteDb == null) throw new System.Exception(string.Format("Cliente não encontrado"));

            try
            {
                clienteDb.Nome = cliente.Nome == null ? clienteDb.Nome : cliente.Nome;
                clienteDb.CPF = cliente.CPF == null ? clienteDb.CPF : cliente.CPF;
                clienteDb.DataAlteracao = DateTime.Now;

                _dataContext.Clientes.Update(clienteDb);

                await _dataContext.SaveChangesAsync();
                return clienteDb;

            }
            catch (Exception ex)
            {
                throw new Exception( ex.Message.ToString());
            }
        }
        public async Task<bool> Excluir(Guid id)
        {
            Cliente cliente = await FiltrarId(id);

            if (cliente == null) throw new System.Exception("Erro ao Excluir");

            _dataContext.Clientes.Remove(cliente);
            await _dataContext.SaveChangesAsync();
            return true;
        }
    }
}
