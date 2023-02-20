﻿using AceleraPleno.API.Interface;
using AceleraPleno.API.Models;
using AceleraPleno.API.Models.PartialModels;
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

        [HttpPut, Route("AlterarMesa")]
        public async Task<Mesa> AlterarMesa(Mesa m)
        {
            return await _iRepository.Atualizar(m, m.Id);
        }

        [HttpPut, Route("OcuparMesa")]
        public async Task<string> OcuparMesa(OcuparMesa mesaOcupada)
        {
            return await _iRepository.OcuparMesa(mesaOcupada);
        }

    }
}
