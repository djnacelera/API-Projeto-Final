using AceleraPleno.API.Interface;
using AceleraPleno.API.Models;
using AceleraPleno.API.Models.PartialModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AceleraPleno.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PratoController : ControllerBase
    {
        private readonly IRepositoryPrato<Prato> _iRepository;

        public PratoController(IRepositoryPrato<Prato> iRepository)
        {
            _iRepository = iRepository;
        }

        [HttpGet, Route("Listar")]
        public async Task<IActionResult> Listar()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            var pratos = await _iRepository.Listar();
            return Ok(pratos);
        }

        [HttpPost, Route("Incluir")]
        public async Task<IActionResult> Incluir(Prato prato)
        {
            try
            {
                await _iRepository.Adicionar(prato);

                return Ok(prato);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut, Route("AlterarPrato")]
        public async Task<Prato> AlterarPrato(Prato prato)
        {
            return await _iRepository.Atualizar(prato, prato.Id);
        }

        [HttpGet, Route("ListarPratoPorId/{id}")]
        public async Task<Prato> ListarPratoPorId(Guid id)
        {
            return await _iRepository.FiltrarId(id);
        }


        [HttpPut, Route("AtivarPrato")]
        public async Task<string> OcuparMesa(Guid id)
        {
            return await _iRepository.AtivarPrato(id);
        }

        [HttpPut, Route("InativarPrato")]
        public async Task<string> DesocuparMesa(Guid id)
        {
            return await _iRepository.InativarPrato(id);
        }
    }
}
