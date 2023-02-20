﻿using AceleraPleno.API.Interface;
using AceleraPleno.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpGet, Route("Listar")]
        public async Task<IActionResult> Listar()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            var clientes = await _iRepository.Listar();
            return Ok(clientes);
        }

        [HttpPost, Route("Incluir")]
        public async Task<IActionResult> Incluir(Cliente cliente)
        {
           await  _iRepository.Adicionar(cliente);
            return Ok(cliente);
        }

        [HttpGet, Route("FiltrarPoId")]
        public async Task<IActionResult> FiltrarPorId(Guid id)
        {
            var cliente = await _iRepository.FiltrarId(id);
            return Ok(cliente);
        }

        [HttpPut, Route("Atualizar")]
        public async Task<IActionResult> Atualizar(Cliente cliente)
        {
            if (cliente == null)
                return BadRequest();
            var clienteAtualizado = await _iRepository.Atualizar(cliente, cliente.Id);
            return Ok(clienteAtualizado);
        }

        [HttpDelete, Route("Deletar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _iRepository.Excluir(id))
                return BadRequest();

            var clientes = await _iRepository.Listar();
            return Ok(clientes);
        }

        /*[HttpPatch]
        public async Task<IActionResult> AtualizarNome(Guid id, [FromBody] JsonPatchDocument<Cliente> clienteParcial)
        {
            if(clienteParcial == null)
                return BadRequest();

            var clienteAtual = await _iRepository.FiltrarId(id);

            if (clienteAtual == null)
                return NotFound();

            clienteParcial.ApplyTo(clienteAtual, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);
            var isValid = TryValidateModel(clienteAtual);

            if (!isValid)
                return BadRequest(ModelState);
        }*/
    }
}
