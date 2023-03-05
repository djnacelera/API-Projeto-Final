using AceleraPleno.API.Data;
using AceleraPleno.API.Interface;
using AceleraPleno.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace AceleraPleno.API.Repository
{
    public class PratoRepository : IRepositoryPrato<Prato>
    {
        private readonly DataContext _dataContext;
        private readonly IRepositoryLog<Log> _log;
        public PratoRepository(DataContext dataContext, IRepositoryLog<Log> log)
        {
            _dataContext = dataContext;
            _log = log;
        }
        public async Task<Prato> Adicionar(Prato prato)
        {
            if (prato == null)
                throw new ArgumentNullException("Erro prato");

            string path = @"E:\\AceleraPL\\AceleraPL\\ProjetoFinal\\AceleraPleno\\AceleraPleno.API\\img\\Teste.jpg";
            string img = await ConverteImg(path);

            prato.Foto = img;
            prato.DataInclusao = DateTime.Now;

            _dataContext.Pratos.Add(prato);
            await _dataContext.SaveChangesAsync();

            _log.Adicionar("Prato", prato.Id, "Adicionar", JsonSerializer.Serialize(prato), null);

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

                _log.Adicionar("Prato", pratoAtual.Id, "Atualizar", JsonSerializer.Serialize(pratoAtual), null);
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

            _log.Adicionar("Prato", prato.Id, "Excluir", JsonSerializer.Serialize(prato), null);
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
    }
}
