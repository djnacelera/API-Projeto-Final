using AceleraPleno.API.Interface;
using AceleraPleno.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace AceleraPleno.API.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IRepository<Pedido> _iRepository;

        public PedidoController(IRepository<Pedido> iRepository)
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


            var pedidos = await _iRepository.Listar();
            return Ok(pedidos);
        }
    }
}
