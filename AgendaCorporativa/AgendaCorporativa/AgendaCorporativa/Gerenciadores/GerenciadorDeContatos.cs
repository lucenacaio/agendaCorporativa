using AgendaCorporativa.Contratos;
using AgendaCorporativa.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AgendaCorporativa.Gerenciadores
{
    /// <summary>
    /// Classe responsavel em gerenciar os Contatos
    /// </summary>
    public class GerenciadorDeContatos
    {
        private IGerenciadorDeDownload gerenciadorDeDownload;

        private readonly string UrlDoArquivo = "http://www.codeandlions.com/teste.csv";

        private readonly string NomeArquivoLocal = "temp.txt";

        public GerenciadorDeContatos(IGerenciadorDeDownload gerenciadorDeDownload)
        {
            this.gerenciadorDeDownload = gerenciadorDeDownload;
        }

        /// <summary>
        /// Busca os contatos
        /// </summary>
        /// <param name="termo">Termo da pesquisa(se for vazio, retorna todos)</param>
        /// <returns>Lista de contatos corporativos</returns>
        public async Task<List<Contato>> PesquisaContatos(string termo)
        {
            string conteudo = DependencyService.Get<IGerenciadorDeArquivo>().CarregarTexto(NomeArquivoLocal);

            List<Contato> contatos = ConverteParaLista(conteudo);

            //Busca o termpo ordenado por nome.
            List<Contato> contatosOrdenados = (from contato in contatos
                                               where contato.NomeFuncionario.Contains(termo)
                                               orderby contato.NomeFuncionario
                                               select contato)?.ToList();

            return contatosOrdenados;
        }

        /// <summary>
        /// Sincroniza os contatos(baixa o arquivo CSV e atualizar os contatos na agenda do usuário)
        /// </summary>
        public async void SincronizarContatos()
        {
            string conteudo = await gerenciadorDeDownload.IniciarDownload(UrlDoArquivo);

            DependencyService.Get<IGerenciadorDeArquivo>().SalvarTexto(NomeArquivoLocal, conteudo);

            return;
        }

        /// <summary>
        /// Converte
        /// </summary>
        /// <param name="conteudoDoArquivo"></param>
        /// <returns></returns>
        protected List<Contato> ConverteParaLista(string conteudoDoArquivo)
        {
            List<Contato> contatos = new List<Contato>();
            foreach (string linhaDoArquivo in conteudoDoArquivo.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                Contato contato = new Contato();
                string[] valores = linhaDoArquivo.Split(';');
                //IMEI
                contato.IMEI = valores[0];
                //Telefone
                //TODO Atualizar codigo quando arquivo com DDD estiver pronto
                contato.Telefones.Add(new Telefone { DDD = valores[1].Substring(0,2), Numero = valores[1] });
                //Nome
                contato.NomeFuncionario = valores[2];
                //Sobrenome
                contato.SobrenomeFuncionario = valores[3];

                contatos.Add(contato);
            }

            return contatos;
        }
    }
}
