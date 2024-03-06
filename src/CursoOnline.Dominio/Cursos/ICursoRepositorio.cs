using CursoOnline.Dominio.Base;

namespace CursoOnline.Dominio.Cursos
{
    public interface ICursoRepositorio : IRepositorio<Curso>
    {
        Curso? ObterPeloNome(string nome);
    }
}