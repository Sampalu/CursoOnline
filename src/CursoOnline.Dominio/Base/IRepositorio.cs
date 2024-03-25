using CursoOnline.Dominio.Cursos;
using CursoOnline.Dominio.Matriculas;

namespace CursoOnline.Dominio.Base
{
    public interface IRepositorio<TEntidade>
    {
        void Adicionar(TEntidade entity);
        TEntidade ObterPorId(int id);
        List<TEntidade> Consultar();
        Task<List<TEntidade>> ConsultarAsync();
    }
}