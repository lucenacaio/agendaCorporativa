using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaCorporativa.Modelos
{
    /// <summary>
    /// Dados do contato do funcionario.
    /// </summary>
    public class Contato
    {
        public Contato()
        {
            Telefones = new List<Telefone>();
			Emails = new List<EmailCorp>();
        }

        /// <summary>
        /// Código do celular do usuário
        /// </summary>
        public string IMEI { get; set; }

        /// <summary>
        /// Nome 
        /// </summary>
        public string NomeFuncionario { get; set; }

        /// <summary>
        /// Sobrenome
        /// </summary>
        public string SobrenomeFuncionario { get; set; }

        /// <summary>
        /// Propriedade para o nome completo(concaternação de NomeUsuario e SobrenomeUsuario)
        /// </summary>
        public string NomeCompleto { get { return string.Format("{0} {1}", NomeFuncionario, SobrenomeFuncionario); } }

        /// <summary>
        /// Nome da Empresa
        /// </summary>
        public string NomeEmpresa { get; set; }

        /// <summary>
        /// Endereço do email coorporativo
        /// </summary>
		public List<EmailCorp> Emails { get; set; }

        /// <summary>
        /// Telefones do funcionario
        /// </summary>
        public List<Telefone> Telefones { get; set; }

    }

    public class Telefone
    {
        /// <summary>
        /// Número de telefone
        /// </summary>
        public string Numero { get; set; }

        /// <summary>
        /// DDD
        /// </summary>
        public string DDD { get; set; }
    }

	public class EmailCorp
	{ 
		public String Endereco { get; set;}
	}
}
