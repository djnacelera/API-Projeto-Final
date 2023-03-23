using AceleraPleno.API.Interface;
using AceleraPleno.API.Models;
using AceleraPleno.API.Models.PartialModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace AceleraPleno.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MesaController : ControllerBase
    {
        private readonly IRepositoryMesa<Mesa> _iRepository;

        public MesaController(IRepositoryMesa<Mesa> iRepository)
        {
            _iRepository = iRepository;
        }

        [HttpPost, Route("AdicionarMesa")]
        public async Task<Mesa> AdicionarMesa([FromBody]Mesa mesa)
        {
            if (mesa == null)
            {
                throw new Exception("Mesa não pode ser Nulla");
            }
            try { 
               return await _iRepository.Adicionar(mesa); 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }

        [Authorize]
        [HttpGet, Route("ListarMesas")]
        public async Task<IEnumerable<Mesa>> ListarMesas()
        {
            return await _iRepository.Listar();
        }

        [HttpGet, Route("ListarMesaPorId/{id}")]
        public async Task<Mesa> ListarMesaPorId(Guid id)
        {
            return await _iRepository.FiltrarId(id);
        }

        [HttpPut, Route("AlterarMesa/{id}")]
        public async Task<Mesa> AlterarMesa(Guid id, Mesa m)
        {
            return await _iRepository.Atualizar(m, id);
        }

        [HttpPut, Route("OcuparMesa")]
        public async Task<string> OcuparMesa(OcuparMesa mesaOcupada)
        {
            return await _iRepository.OcuparMesa(mesaOcupada);
        }

        [HttpPut, Route("DesocuparMesa/{id}")]
        public async Task<string> DesocuparMesa(Guid id)
        {
            return await _iRepository.DesocuparMesa(id);
        }

    }
}
