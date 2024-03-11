using CursoOnline.Dominio.Alunos;
using CursoOnline.Dominio.Cursos;
using CursoOnline.Dominio.Matriculas;
using CursoOnline.Dominio.PublicosAlvo;
using CursoOnline.Dominio.Test.Builders;

namespace CursoOnline.Dominio.Test.Builders
{
    public class MatriculaBuilder
    {
        protected Aluno Aluno;
        protected Curso Curso;
        protected double ValorPago;
        protected bool Cancelada;
        protected bool Concluido;

        public static MatriculaBuilder Novo()
        {
            var curso = Dominio.Test.Builders.CursoBuilder.Novo().ComPublicoAlvo(PublicoAlvo.Empreendedor).Build();

            return new MatriculaBuilder
            {
                Aluno = AlunoBuilder.Novo().ComPublicoAlvo(PublicoAlvo.Empreendedor).Build(),
                Curso = curso,
                ValorPago = curso.Valor
            };
        }

        public MatriculaBuilder ComAluno(Aluno aluno)
        {
            Aluno = aluno;
            return this;
        }

        public MatriculaBuilder ComCurso(Curso curso)
        {
            Curso = curso;
            return this;
        }

        public MatriculaBuilder ComValorPago(double valorPago)
        {
            ValorPago = valorPago;
            return this;
        }

        public MatriculaBuilder ComCancelada(bool cancelada)
        {
            Cancelada = cancelada;
            return this;
        }

        public MatriculaBuilder ComConcluido(bool concluido)
        {
            Concluido = concluido;
            return this;
        }

        public Matricula Build()
        {
            var matricula = new Matricula(Aluno, Curso, ValorPago);

            if (Cancelada)
                matricula.Cancelar();

            if (Concluido)
            {
                const double notaDoAluno = 7;
                matricula.InformarNota(notaDoAluno);
            }

            return matricula;
        }

        
    }
}
