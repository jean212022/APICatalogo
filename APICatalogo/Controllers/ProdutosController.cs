using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        //Injecao de dependencias
        private readonly AppDbContext _context;
        public ProdutosController(AppDbContext context)
        {
            this._context = context;
        }
        //Verbo GET
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                var produtos = this._context.Produtos.AsNoTracking().ToList();
                if (produtos is null)
                {
                    return NotFound("Produtos nao encontrados!");
                }
                return produtos;
            } catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitacao!");
            }
        }
        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            try
            {
                var produto = this._context.Produtos.AsNoTracking().FirstOrDefault(x => x.ProdutoId == id); //AsNoTracking melhorar o desempenho
                if (produto is null)
                {
                    return NotFound("Produto nao encontrado!");
                }
                return produto;
            } catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitacao!");
            }
        }
        //Verbo POST
        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            try
            {
                if (produto is null)
                {
                    return BadRequest("Produto nao encontrado!");
                }
                this._context.Produtos.Add(produto);
                this._context.SaveChanges();

                return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
            } catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitacao!");
            }
        }
        //Verbo PUT
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            try
            {
                if (produto.ProdutoId != id)
                {
                    return BadRequest($"O id = {id} não foi encontrado!");
                }

                this._context.Entry(produto).State = EntityState.Modified;
                this._context.SaveChanges();
                return Ok(produto);
            } catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitacao!");
            }
        }
        //Verbo DELETE
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var produto = this._context.Produtos.FirstOrDefault(x => x.ProdutoId == id);
                if (produto is null)
                {
                    return NotFound($"Produto de id = {id} nao foi localizado!");
                }
                this._context.Produtos.Remove(produto);
                this._context.SaveChanges();
                return Ok(produto);
            } catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitacao!");
            }
        }
    }
}
