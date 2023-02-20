﻿using AceleraPleno.API.Data;
using AceleraPleno.API.Interface;
using AceleraPleno.API.Models;
using AceleraPleno.API.Models.Enuns;
using AceleraPleno.API.Models.PartialModels;
using Microsoft.EntityFrameworkCore;

namespace AceleraPleno.API.Repository
{
    public class MesaRepository : IRepositoryMesa<Mesa>
    {
        private readonly DataContext _dataContext;
        public MesaRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<Mesa> Adicionar(Mesa mesa)
        {
            if (mesa == null)
            {
                throw new ArgumentNullException("Erro mesa nulla");
            }

            mesa.DataInclusao = DateTime.Now;
            mesa.Ocupada = false;
            mesa.StatusMesa = StatusMesa.Disponivel;
            mesa.ClienteId = null;

            _dataContext.Mesas.Add(mesa);
            await _dataContext.SaveChangesAsync();

            return mesa;
        }

        public async Task<Mesa> Atualizar(Mesa cartao, Guid id)
        {
            if (cartao.Id != id)
            {
                throw new Exception("Cliente e Id não conferem");
            }
            Mesa m = await FiltrarId(id);

            m.Ambiente = cartao.Ambiente;
            m.Descricao = cartao.Descricao;
            m.DataAlteracao = DateTime.Now;
            m.Lugares = cartao.Lugares;

            bool t = await AlterarMesa(m);
            if (t)
                return m;
            else
                throw new Exception("Não foi possivel altera a mesa");
        }

        public Task<bool> Excluir(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Mesa> FiltrarId(Guid id)
        {
            return await _dataContext.Mesas.
                Include(m => m.Clientes).
                FirstOrDefaultAsync(m => m.Id == id);

        }

        public async Task<IEnumerable<Mesa>> Listar()
        {
            return await _dataContext.Mesas.ToListAsync();
        }

        public async Task<string> OcuparMesa(OcuparMesa mesaOcupada)
        {
            if (mesaOcupada == null)
                return "Favor informar a mesa";

            Mesa c = new Mesa();                

            c = await FiltrarId(mesaOcupada.MesaId);

            if (c.Ocupada)
                return "Mesa ja esta ocupada!";

            if (c.StatusMesa == StatusMesa.Reservado)
                return "Mesa esta Reservada";

            c.Ocupada = true;
            c.ClienteId = mesaOcupada.ClienteId;
            c.DataAlteracao = DateTime.Now;

            await AlterarMesa(c);

            return $"Mesa ocupada com sucesso!";
        }

        public Task<string> DesocuparMesa(Guid id)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> AlterarMesa(Mesa alt)
        {
            if (alt == null)
            {
                return false;
            }
            try
            {
                _dataContext.Entry<Mesa>(alt).State = EntityState.Modified;
                await  _dataContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
    }
}
