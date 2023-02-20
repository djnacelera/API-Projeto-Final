using AceleraPleno.API.Interface;
using AceleraPleno.API.Models;
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

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            var pratos = await _iRepository.Listar();
            return Ok(pratos);
        }

        [HttpPost]
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
    }
}
