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
            var produtos = this._context.Produtos.ToList();
            if (produtos is null)
            {
                return NotFound("Produtos nao encontrados!");
            }
            return produtos;
        }
        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = this._context.Produtos.FirstOrDefault(x => x.ProdutoId == id);
            if (produto is null)
            {
                return NotFound("Produto nao encontrado!");
            }
            return produto;
        }
        //Verbo POST
        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
            {
                return BadRequest();
            }
            this._context.Produtos.Add(produto);
            this._context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }
        //Verbo PUT
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if(produto.ProdutoId != id)
            {
                return BadRequest();
            }

            this._context.Entry(produto).State = EntityState.Modified;
            this._context.SaveChanges();
            return Ok(produto);
        }
        //Verbo DELETE
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = this._context.Produtos.FirstOrDefault(x => x.ProdutoId == id);
            if (produto is null)
            {
                return NotFound("Produto nao localizado!");
            }
            this._context.Produtos.Remove(produto);
            this._context.SaveChanges();
            return Ok(produto);
        }
    }
}
