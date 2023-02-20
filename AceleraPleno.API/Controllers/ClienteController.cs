using AceleraPleno.API.Interface;
using AceleraPleno.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AceleraPleno.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IRepository<Cliente> _iRepository;

        public ClienteController(IRepository<Cliente> iRepository)
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


            var clientes = await _iRepository.Listar();
            return Ok(clientes);
        }

        [HttpPost]
        public async Task<IActionResult> Incluir(Cliente cliente)
        {
           await  _iRepository.Adicionar(cliente);
            return Ok(cliente);
        }
    }
}
