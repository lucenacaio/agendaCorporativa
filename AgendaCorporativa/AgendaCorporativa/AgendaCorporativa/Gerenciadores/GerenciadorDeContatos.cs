using AgendaCorporativa.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaCorporativa.Gerenciadores
{
    /// <summary>
    /// Classe responsavel em gerenciar os Contatos
    /// </summary>
    public class GerenciadorDeContatos
    {
        /// <summary>
        /// Busca os contatos
        /// </summary>
        /// <param name="termo">Termo da pesquisa(se for vazio, retorna todos)</param>
        /// <returns>Lista de contatos corporativos</returns>
        public async Task<List<Contato>> PesquisaContatos(string termo)
        {
            
            //TODO - Deve obter os contatos na lista de contatos.
            return new List<Contato>();
        }

        /// <summary>
        /// Sincroniza os contatos(baixa o arquivo CSV e atualizar os contatos na agenda do usuário)
        /// </summary>
        public void SincronizarContatos()
        {
            //TODO - Implementar lógica para baixar o arquivo
            //TODO - Implementar lógica para pegar a lista de Contatos do Usuario
            //TODO - Implementar lógica de atualização dos contatos do usuário
        }
    }
}
