using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.ViewModel;
using ApiCatalogoJogos.Services;
using System.ComponentModel.DataAnnotations;
using ApiCatalogoJogos.Exceptions;

namespace ApiCatalogoJogos.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        private readonly IJogoService _jogoService;

        public JogosController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }

        /// <summary>
        /// Buscar a lista de jogos de forma paginada
        /// </summary>
        /// <remarks>
        /// Não é possível retornar a lista sem paginação
        /// </remarks>
        /// <param name="pagina">Indica qual página está sendo consultada, iniciando pela primeira.</param>
        /// <param name="quantidade">Indica a quantidade de registros por páginas, retornando o mínimo de 1 e o máximo de 50</param>
        /// <response code="200">Retorna a lista de jogos</response>
        /// <response code="204">Caso não existam jogos cadastrados</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            var jogos = await _jogoService.Obter(pagina, quantidade);

            if (jogos.Count() == 0)
                return NoContent();

            return Ok(jogos);
        }

        /// <summary>
        /// Buscar um jogo pela chave.
        /// </summary>
        /// <param name="idJogo">Identificação do jogo consultado</param>
        /// <response code="200">Retorna os dados do jogo filtrado</response>
        /// <response code="204">Caso não exista jogo cadastrado com a chava pesquisada</response>
        [HttpGet("{idJogo:guid}")]
        public async Task<ActionResult<List<JogoViewModel>>> Obter([FromRoute] Guid idJogo)
        {
            var jogo = await _jogoService.Obter(idJogo);

            if (jogo == null)
                return NoContent();

            return Ok(jogo);
        }
        
        [HttpPost]
        public async Task<ActionResult<List<JogoViewModel>>> InserirJogo([FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                var jogo = await _jogoService.Inserir(jogoInputModel);

                return Ok(jogo);
            }
            catch (JogoJaCadastradoException ex)
            {
                return UnprocessableEntity("Já existe um jogo com este nome nesta produtora");
            }
        }

        [HttpPut("{idJogo:guid}")]
        public async Task<ActionResult<List<object>>> AtualizarJogo([FromRoute] Guid idJogo, [FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, jogoInputModel);
                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return UnprocessableEntity("Jogo inexistente");
            }
        }


        [HttpPatch("{idJogo:guid}/preco/{preco:double}")]
        public async Task<ActionResult<List<object>>> AtualizarJogo([FromRoute] Guid idJogo, [FromRoute] double preco)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, preco);
                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return UnprocessableEntity("Jogo inexistente");
            }
        }
        [HttpDelete("{idJogo:guid}")]
        public async Task<ActionResult<List<object>>> ApagarJogo([FromRoute] Guid idJogo)
        {
            try
            {

                await _jogoService.Remover(idJogo);
                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return UnprocessableEntity("Jogo inexistente");
            }
        }
    }
}