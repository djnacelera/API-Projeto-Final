using AceleraPleno.API.Data;
using AceleraPleno.API.Interface;
using AceleraPleno.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Text;
using AceleraPleno.API.Models.Enuns;
using AceleraPleno.API.Models.PartialModels;

namespace AceleraPleno.API.Repository
{
    public class PratoRepository : IRepositoryPrato<Prato>
    {
        private readonly DataContext _dataContext;
        public PratoRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<Prato> Adicionar(Prato prato)
        {
            if (prato == null)
                throw new ArgumentNullException("Erro prato");

            string path = "";
            string img = await ConverteImg(path);

            prato.Foto = img;
            prato.DataInclusao = DateTime.Now;

            _dataContext.Pratos.Add(prato);
            await _dataContext.SaveChangesAsync();

            return prato;
        }
        public async Task<Prato> Atualizar(Prato prato, Guid id)
        {
            if (prato == null) throw new System.Exception("Erro ao atualizar");

            Prato pratoAtual = await FiltrarId(id);
            if (pratoAtual == null) throw new System.Exception(string.Format("Cliente não encontrado"));

            try
            {
                pratoAtual.Descricao = prato.Descricao == null ? pratoAtual.Descricao : prato.Descricao;
                pratoAtual.Foto = prato.Foto == null ? pratoAtual.Foto : prato.Foto;
                pratoAtual.DataAlteracao = DateTime.Now;

                _dataContext.Pratos.Update(pratoAtual);

                await _dataContext.SaveChangesAsync();
                return pratoAtual;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> Excluir(Guid id)
        {
            Prato prato = await FiltrarId(id);

            if (prato == null) throw new System.Exception("Erro ao Excluir");

            _dataContext.Pratos.Remove(prato);
            await _dataContext.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Prato>> Listar()
        {
            return await _dataContext.Pratos.ToListAsync();
        }
        public async Task<Prato> FiltrarId(Guid id)
        {
            return await _dataContext.Pratos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<string> ConverteImg(string path)
        {
            byte[] imageArray = await System.IO.File.ReadAllBytesAsync(path);

            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            return base64ImageRepresentation;

        }

        public async Task<string> AtivarPrato(Guid id)
        {
            if (id == null)
                return "Favor informar o prato";

            Prato c = new Prato();

            c = await FiltrarId(id);

            if (c.Status)
                return "Prato já está Ativo";

            c.Status = true;
            

            await AlterarPrato(c);

            return $"Prato ativo com sucesso!";
        }

        public async Task<string> InativarPrato(Guid id)
        {
            Prato c = new Prato();

            c = await FiltrarId(id);

            if (!c.Status)
                return "Prato ja esta Inativo!";

            c.Status = false;

            await AlterarPrato(c);

            return $"Prato Inativo com sucesso!";
        }

        private async Task<bool> AlterarPrato(Prato p)
        {
            if (p == null)
            {
                return false;
            }
            try
            {
                _dataContext.Entry<Prato>(p).State = EntityState.Modified;
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
    }
}
