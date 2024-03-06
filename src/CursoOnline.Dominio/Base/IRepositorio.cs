namespace CursoOnline.Dominio.Base
{
    public interface IRepositorio<TEntidade>
    {
        void Adicionar(TEntidade entity);
        TEntidade ObterPorId(int id);
        List<TEntidade> Consultar();
    }
}