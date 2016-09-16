using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaCorporativa.Excecoes
{
    public class ExcecaoDeAutenticacao : Exception
    {
        public ExcecaoDeAutenticacao() :
            base("Erro de autenticação")
        { }

        public ExcecaoDeAutenticacao(string msg) : 
            base(msg)
        { }
    }
}
