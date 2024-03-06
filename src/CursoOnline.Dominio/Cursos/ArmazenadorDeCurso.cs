using CursoOnline.Dominio.Base;
using System;

namespace CursoOnline.Dominio.Cursos
{
    public class ArmazenadorDeCurso
    {
        private readonly ICursoRepositorio _cursoRepositorio;

        public ArmazenadorDeCurso(ICursoRepositorio cursoRepositorio)
        {
            _cursoRepositorio = cursoRepositorio;
        }

        public void Armazenar(CursoDTO cursoDTO)
        {
            var cursoJaSalvo = _cursoRepositorio.ObterPeloNome(cursoDTO.Nome);

            ValidadorDeRegra.Novo()
                .Quando(cursoJaSalvo != null, "Nome do curso já consta no banco de dados")
                .Quando(!Enum.TryParse<PublicoAlvo>(cursoDTO.PublicoAlvo, out var publicoAlvo), "Público Alvo inválido")
                .DispararExcecaoSeExistir();

            var curso = new Curso(cursoDTO.Nome, cursoDTO.Descricao, cursoDTO.CargaHoraria, publicoAlvo, cursoDTO.Valor);

            _cursoRepositorio.Adicionar(curso);
        }
    }
}
