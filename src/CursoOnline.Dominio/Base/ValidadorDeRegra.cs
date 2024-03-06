using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CursoOnline.Dominio.Base
{
    public class ValidadorDeRegra
    {
        private List<string> _mensagensDeErros;

        private ValidadorDeRegra()
        {
            _mensagensDeErros = new List<string>();
        }
        public static ValidadorDeRegra Novo()
        {
            return new ValidadorDeRegra();
        }

        public ValidadorDeRegra Quando(bool temErro, string mensagemDeErro)
        {
            if (temErro)
                _mensagensDeErros.Add(mensagemDeErro);

            return this;
        }

        public void DispararExcecaoSeExistir()
        {
            if (_mensagensDeErros.Any())
                throw new ExceptionDeDominio(_mensagensDeErros);
        }
    }

    public class ExceptionDeDominio : ArgumentException
    {
        public List<string> MensagensDeErro { get; set; }

        public ExceptionDeDominio(List<string> mensagensDeErros)
        {
            MensagensDeErro = mensagensDeErros;
        }
    }
}
