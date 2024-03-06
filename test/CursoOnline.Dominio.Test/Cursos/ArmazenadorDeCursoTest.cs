using Bogus;
using CursoOnline.Dominio.Cursos;
using CursoOnline.Dominio.Test.Builders;
using CursoOnline.Dominio.Test.Util;
using Moq;
using Xunit;

namespace CursoOnline.Dominio.Test.Cursos
{
    public class ArmazenadorDeCursoTest
    {
        private readonly CursoDTO _cursoDTO;
        private readonly ArmazenadorDeCurso _armazenadorDeCurso;
        private readonly Mock<ICursoRepositorio> _cursoRepositorioMock;

        public ArmazenadorDeCursoTest()
        {
            var fake = new Faker();

            _cursoDTO = new CursoDTO
            {
                Nome = fake.Random.Words(),
                Descricao = fake.Lorem.Paragraph(),
                CargaHoraria = fake.Random.Double(50, 1000),
                PublicoAlvo = fake.Random.Enum<PublicoAlvo>().ToString(),
                Valor = fake.Random.Double(1000, 2000)
            };

            _cursoRepositorioMock = new Mock<ICursoRepositorio>();

            _armazenadorDeCurso = new ArmazenadorDeCurso(_cursoRepositorioMock.Object);
        }

        [Fact]
        public void DeveAdicionarCurso()
        {
            _cursoRepositorioMock.Setup(r => r.ObterPeloNome(It.IsAny<string>())).Returns(value: null);

            _armazenadorDeCurso.Armazenar(_cursoDTO);

            _cursoRepositorioMock.Verify(r => r.Adicionar(It.Is<Curso>(c =>
                c.Nome == _cursoDTO.Nome &&
                c.Descricao == _cursoDTO.Descricao &&
                c.CargaHoraria == _cursoDTO.CargaHoraria &&
                c.PublicoAlvo.ToString() == _cursoDTO.PublicoAlvo &&
                c.Valor == _cursoDTO.Valor
                )
            ));
        }

        [Fact]
        public void NaoDeveInformarPublicoAlvoInvalido()
        {
            _cursoRepositorioMock.Setup(r => r.ObterPeloNome(It.IsAny<string>())).Returns(value: null);

            var publicoAlvoInvalido = "Médico";
            _cursoDTO.PublicoAlvo = publicoAlvoInvalido;

            Assert.Throws<ArgumentException>(() => _armazenadorDeCurso.Armazenar(_cursoDTO))
                .ComMensagem("Público Alvo inválido");
        }

        [Fact]
        public void NaoDeveAdicionarCursoComMesmoNome()
        {
            var cursoJaSalvo = CursoBuilder.Novo().ComNome(_cursoDTO.Nome).Build();
            _cursoRepositorioMock.Setup(r => r.ObterPeloNome(_cursoDTO.Nome)).Returns(cursoJaSalvo);

            Assert.Throws<ArgumentException>(() => _armazenadorDeCurso.Armazenar(_cursoDTO))
                .ComMensagem("Nome do curso já consta no banco de dados");
        }
    }
}