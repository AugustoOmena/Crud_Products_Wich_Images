using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Loja_.Data;
using Loja_.Models;

namespace Loja_.Controllers
{
    public class ProdutoesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly string _caminhoServidor;

        public ProdutoesController(ApplicationDbContext context, IWebHostEnvironment sistema)
        {
            _context = context;
            _caminhoServidor = Path.Combine(sistema.WebRootPath, "imagem");

        }

        // GET: Produtoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Produto.ToListAsync());
        }

        // GET: Produtoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .FirstOrDefaultAsync(m => m.ID == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // GET: Produtoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produtoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,OwnerID,Name,Resume,Category,Description,Phone,UserImage,Active")] Produto produto, IFormFile foto)
        {
            if (!(foto != null && foto.Length > 0))
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                string caminhoParaSalvarImagem = _caminhoServidor + "\\Userimagem\\";
                string novoNomeParaImagem = Guid.NewGuid().ToString() + foto.FileName;
                string CaminhoDaImagem = caminhoParaSalvarImagem + novoNomeParaImagem;

                if (!Directory.Exists(caminhoParaSalvarImagem))
                {
                    Directory.CreateDirectory(caminhoParaSalvarImagem);
                }

                string[] tiposDeImagemPermitidos = { ".jpg", ".jpeg", ".png", ".gif" };
                string extensao = Path.GetExtension(foto.FileName).ToLower();

                if (tiposDeImagemPermitidos.Contains(extensao))
                {
                    using (var stream = System.IO.File.Create(CaminhoDaImagem))
                    {
                        foto.CopyTo(stream);
                    }
                    await _context.SaveChangesAsync();
                }

                produto.UserImage = Path.Combine("/imagem/Userimagem", novoNomeParaImagem); // Caminho absoluto relativo a wwwroot


                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        // GET: Produtoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        // POST: Produtoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,OwnerID,Name,Resume,Category,Description,Phone,UserImage,Active")] Produto produto)
        {
            if (id != produto.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        // GET: Produtoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .FirstOrDefaultAsync(m => m.ID == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produtoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produto = await _context.Produto.FindAsync(id);
            if (produto != null)
            {
                _context.Produto.Remove(produto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produto.Any(e => e.ID == id);
        }
    }
}
