﻿using AceleraPleno.API.Interface;
using AceleraPleno.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AceleraPleno.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : Controller
    {
        private readonly IRepositoryPedido<Pedido> _iRepositoryPedido;

        public PedidoController(IRepositoryPedido<Pedido> iRepositoryPedido)
        {
            _iRepositoryPedido = iRepositoryPedido;
        }

        [AllowAnonymous]
        [HttpGet, Route("Listar")]
        public async Task<IActionResult> Listar()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            var pedidos = await _iRepositoryPedido.Listar();
            return Ok(pedidos);
        }

        /*[HttpPost, Route("Incluir")]
        public async Task<IActionResult> Incluir(Pedido pedido)
        {
            await _iRepositoryPedido.Adicionar(pedido);
            return Ok(pedido);
        }*/
        [Authorize]
        [HttpPost, Route("Incluir")]
        public async Task<IActionResult> Incluir2(Guid mesaId, Guid pratoId, int qtd, decimal valor)
        {
            //await _iRepositoryPedido.Adicionar2(mesaId, pratoId, qtd, valor);
            return Ok(await _iRepositoryPedido.Adicionar2(mesaId, pratoId, qtd, valor));
        }

        [AllowAnonymous]
        [HttpGet, Route("FiltrarPorId/{id}")]
        public async Task<IActionResult> FiltrarPorId(Guid id)
        {
            var pedido = await _iRepositoryPedido.FiltrarId(id);
            return Ok(pedido);
        }

        [Authorize]
        [HttpPut, Route("Atualizar/{id}")]
        public async Task<IActionResult> Atualizar(Guid id, Pedido pedido)
        {
            if (pedido == null)
                return BadRequest();
            var pedidoAtualizado = await _iRepositoryPedido.Atualizar(pedido, pedido.Id);
            return Ok(pedidoAtualizado);
        }

        [Authorize]
        [HttpPut, Route("Preparando/{id}")]
        public async Task<IActionResult> AtualizarParaPreparando(Guid id)
        {
            if (id == null)
                return BadRequest();
            await _iRepositoryPedido.AlterarPedidoParaPreparando(id);
            var pedidoAtualizado = await _iRepositoryPedido.FiltrarId(id);
            return Ok(pedidoAtualizado);
        }

        [AllowAnonymous]
        [HttpPut, Route("Entregue/{id}")]
        public async Task<IActionResult> AtualizarParaEntregue(Guid id)
        {
            if (id == null)
                return BadRequest();
            await _iRepositoryPedido.AlterarPedidoParaEntregue(id);
            var pedidoAtualizado = await _iRepositoryPedido.FiltrarId(id);
            return Ok(pedidoAtualizado);
        }

        [Authorize]
        [HttpPut, Route("Cancelado/{id}")]
        public async Task<IActionResult> AtualizarParaCancelado(Guid id)
        {
            if (id == null)
                return BadRequest();
            await _iRepositoryPedido.AlterarPedidoParaCancelado(id);
            var pedidoAtualizado = await _iRepositoryPedido.FiltrarId(id);
            return Ok(pedidoAtualizado);
        }

        [Authorize]
        [HttpDelete, Route("Deletar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _iRepositoryPedido.Excluir(id))
                return BadRequest();

            var pedidos = await _iRepositoryPedido.Listar();
            return Ok(pedidos);
        }
    }
}
