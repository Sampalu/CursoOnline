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
                .Quando(cursoJaSalvo != null, Resource.NomeCursoJaExiste)
                .Quando(!Enum.TryParse<PublicoAlvo>(cursoDTO.PublicoAlvo, out var publicoAlvo), Resource.PublicoAlvoInvalido)
                .DispararExcecaoSeExistir();

            var curso = new Curso(cursoDTO.Nome, cursoDTO.Descricao, cursoDTO.CargaHoraria, publicoAlvo, cursoDTO.Valor);

            if(cursoDTO.Id > 0)
            {
                curso = _cursoRepositorio.ObterPorId(cursoDTO.Id);
                curso.AlterarNome(cursoDTO.Nome);
                curso.AlterarValor(cursoDTO.Valor);
                curso.AlterarCargaHoraria(cursoDTO.CargaHoraria);
            }

            if (cursoDTO.Id == 0)
                _cursoRepositorio.Adicionar(curso);
        }
    }
}
