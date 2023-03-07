﻿using AceleraPleno.API.Interface;
using AceleraPleno.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace AceleraPleno.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : Controller
    {
        private readonly IRepositoryLog<Log> _iRepositoryLog;

        public LogController(IRepositoryLog<Log> iRepositoryLog)
        {
            _iRepositoryLog = iRepositoryLog;
        }

        [HttpGet, Route("Listar")]
        public async Task<IActionResult> Listar()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            var logs = await _iRepositoryLog.Listar();
            return Ok(logs);
        }

        [HttpGet, Route("FiltrarPorData")]
        public async Task<IActionResult> FiltrarPorData(DateTime data)
        {
            var log = await _iRepositoryLog.FiltrarData(data);
            return Ok(log);
        }
    }
}
