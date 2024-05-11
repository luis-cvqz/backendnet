using backendnet.Data;
using backendnet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendnet.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PeliculasController(DataContext context) : Controller
{
    // GET: api/peliculas?s=titulo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pelicula>>> GetPeliculas(string? str)
    {
        if (string.IsNullOrEmpty(str))
            return await context.Pelicula.Include(i => i.Categorias).AsNoTracking().ToListAsync();
        
        return await context.Pelicula.Include(i => i.Categorias).Where(c => c.Titulo.Contains(str))
            .AsNoTracking().ToListAsync();
    }
        
    // GET: api/peliculas/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Pelicula>> GetPelicula(int id)
    {
        var pelicula = await context.Pelicula.Include(i => i.Categorias).AsNoTracking()
            .FirstOrDefaultAsync(s => s.PeliculaId == id);
        if (pelicula == null) return NotFound();

        return pelicula;
    }
        
    // POST: api/peliculas
    [HttpPost]
    public async Task<ActionResult<Pelicula>> PostPelicula(PeliculaDTO peliculaDto)
    {
        Pelicula pelicula = new()
        {
            Titulo = peliculaDto.Titulo,
            Sinopsis = peliculaDto.Sinopsis,
            Anio = peliculaDto.Anio,
            Poster = peliculaDto.Poster,
            Categorias = []
        };

        context.Pelicula.Add(pelicula);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPelicula), new { id = pelicula.PeliculaId }, pelicula);
    }
        
    // PUT: api/peliculas/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPelicula(int id, PeliculaDTO peliculaDto)
    {
        if (id != peliculaDto.PeliculaId) return BadRequest();

        var pelicula = await context.Pelicula.FirstOrDefaultAsync(s => s.PeliculaId == id);
        if (pelicula == null) return NotFound();

        pelicula.Titulo = peliculaDto.Titulo;
        pelicula.Sinopsis = peliculaDto.Sinopsis;
        pelicula.Anio = peliculaDto.Anio;
        pelicula.Poster = peliculaDto.Poster;
        await context.SaveChangesAsync();

        return NoContent();
    }
        
    // DELETE: api/peliculas/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePelicula(int id)
    {
        var pelicula = await context.Pelicula.FindAsync(id);
        if (pelicula == null) return NotFound();

        context.Pelicula.Remove(pelicula);
        await context.SaveChangesAsync();

        return NoContent();
    }
        
    // POST: api/peliculas/5/categoria
    [HttpPost("{id}/categoria")]
    public async Task<IActionResult> PostCategoriaPelicula(int id, AsignaCategoriaDTO itemToAdd)
    {
        Categoria? categoria = await context.Categoria.FindAsync(itemToAdd.CategoriaId);
        if (categoria == null) return NotFound();

        var pelicula = await context.Pelicula.Include(i => i.Categorias)
            .FirstOrDefaultAsync(s => s.PeliculaId == id);
        if (pelicula == null) return NotFound();

        if (pelicula?.Categorias?.FirstOrDefault(categoria) != null)
        {
            pelicula.Categorias.Add(categoria);
            await context.SaveChangesAsync();
        }

        return NoContent();
    }
        
    // DELETE: api/peliculas/5/categoria/1
    [HttpDelete]
    public async Task<IActionResult> DeleteCategoriaPelicula(int id, int categoriaid)
    {
        Categoria? categoria = await context.Categoria.FindAsync(categoriaid);
        if (categoria == null) return NotFound();

        var pelicula = await context.Pelicula.Include(i => i.Categorias)
            .FirstOrDefaultAsync(s => s.PeliculaId == id);
        if (pelicula == null) return NotFound();

        if (pelicula?.Categorias?.FirstOrDefault(categoria) != null)
        {
            pelicula.Categorias.Remove(categoria);
            await context.SaveChangesAsync();
        }

        return NoContent();
    }
}