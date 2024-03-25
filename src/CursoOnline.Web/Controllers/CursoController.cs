using CursoOnline.Dominio.Base;
using CursoOnline.Dominio.Cursos;
using CursoOnline.Web.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Caching;
using Polly.Caching.Memory;
using Polly.Registry;
using System.Net.Http;

namespace CursoOnline.Web.Controllers
{
    public class CursoController : Controller
    {
        private readonly ArmazenadorDeCurso _armazenadorDeCurso;
        private readonly IRepositorio<Curso> _cursoRepositorio;
        private readonly IAsyncPolicy<List<Curso>> _cachePolicy;

        public CursoController(ArmazenadorDeCurso armazenadorDeCurso, IRepositorio<Curso> cursoRepositorio
            , IReadOnlyPolicyRegistry<string> policyRegistry) 
        { 
            _armazenadorDeCurso = armazenadorDeCurso;
            _cursoRepositorio = cursoRepositorio;

            _cachePolicy = policyRegistry.Get<IAsyncPolicy<List<Curso>>>("CachingPolicy2");
        }

        public IActionResult Index()
        {
            var result = _cachePolicy.ExecuteAndCaptureAsync(context => 
                _cursoRepositorio.ConsultarAsync()
            , new Context("KeyForSomething"));

            var cursos = result.Result.Result;                                

            if (cursos.Any())
            {
                var dtos = cursos.Select(c => new CursoParaListagemDto
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Descricao = c.Descricao,
                    CargaHoraria = c.CargaHoraria,
                    PublicoAlvo = c.PublicoAlvo.ToString(),
                    Valor = c.Valor,
                });
                return View("Index", PagedList<CursoParaListagemDto>.Create(dtos, Request));
            }

            return View("Index", PagedList<CursoParaListagemDto>.Create(null, Request));
        }

        public IActionResult Editar(int id)
        {
            var curso = _cursoRepositorio.ObterPorId(id);

            var dto = new CursoDTO
            {
                Id = curso.Id,
                Nome = curso.Nome,
                Descricao = curso.Descricao,
                CargaHoraria = curso.CargaHoraria,
                PublicoAlvo = curso.PublicoAlvo.ToString(),
                Valor = curso.Valor,
            };

            return View("NovoOuEditar", dto);
        }

        public IActionResult Novo()
        {
            return View("NovoOuEditar", new CursoDTO());
        }

        [HttpPost]
        public IActionResult Salvar(CursoDTO model)
        {
            _armazenadorDeCurso.Armazenar(model);

            return Ok();
        }
    }
}
