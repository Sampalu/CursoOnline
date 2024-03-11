using CursoOnline.Dominio.Base;

namespace CursoOnline.Dominio.Alunos
{
    public interface IAlunoRepositorio : IRepositorio<Aluno>
    {
        Aluno ObterPeloCpf(string cpf);
    }
}