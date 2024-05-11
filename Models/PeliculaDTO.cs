namespace backendnet.Models;

public class PeliculaDTO
{
    public int? PeliculaId { get; set; }
    public string Titulo { get; set; } = "Sin titulo";
    public string Sinopsis { get; set; } = "Sin sinopsis";
    public int Anio { get; set; }
    public string Poster { get; set; } = "N/A";
    public int[]? Categorias { get; set; }
}