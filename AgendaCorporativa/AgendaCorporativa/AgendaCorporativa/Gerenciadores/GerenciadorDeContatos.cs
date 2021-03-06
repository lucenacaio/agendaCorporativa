﻿using AgendaCorporativa.Contratos;
using AgendaCorporativa.Modelos;
using Stefanini.Framework.Extensoes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private readonly string UrlDoArquivo = Properties.Resources.UrlDoArquivo;

        private readonly string NomeArquivoLocal = "stf_agenda.txt";

        public GerenciadorDeContatos(IGerenciadorDeDownload gerenciadorDeDownload)
        {
            this.gerenciadorDeDownload = gerenciadorDeDownload;
        }

        /// <summary>
        /// Obtem a lista de Imeis do arquivo
        /// </summary>
        /// <returns></returns>
        public List<string> ObtemImeis()
        {
            string conteudo = CarregaConteudoDoArquivo();

            return MontaListaImeis(conteudo);
        }


        /// <summary>
        /// Obtem contatos que estão no arquivo.
        /// </summary>
        /// <returns></returns>
        public List<Contato> ObtemContatosDoArquivo()
        {
            string conteudo = CarregaConteudoDoArquivo();

            return ConverteParaLista(conteudo).OrderBy(x => x.NomeFuncionario).ToList();
        }

        /// <summary>
        /// Busca os contatos
        /// </summary>
        /// <param name="termo">Termo da pesquisa(se for vazio, retorna todos)</param>
        /// <returns>Lista de contatos corporativos</returns>
        //public List<Contato> PesquisaContatos(string termo = "")
        //{
        //    List<Contato> resultadoDePesquisa = new List<Contato>();
        //    List<Contato> contatos = ConverteParaLista(conteudo);

        //    //Busca pelo termo nos contatos e ordenado por nome.
        //    resultadoDePesquisa = (from contato in contatos
        //                           where contato.NomeCompleto.ToUpper().Contains(termo.ToUpper())
        //                           orderby contato.NomeCompleto
        //                           select contato)?.ToList();

        //    return resultadoDePesquisa;
        //}

        private string CarregaConteudoDoArquivo()
        {
            string conteudo = "";
            try
            {
                conteudo = DependencyService.Get<IGerenciadorDeArquivo>().CarregarTexto(NomeArquivoLocal);
            }
            catch (Exception)
            {
                //TODO - Tratar melhor o erro ao tentar carregar o arquivo.(Conexao web)
            }

            return conteudo;
        }

        /// <summary>
        /// Sincroniza os contatos(baixa o arquivo CSV e atualizar os contatos na agenda do usuário)
        /// </summary>
        public async Task BaixarArquivoDeContatosAsync()
        {
            var conteudo = await gerenciadorDeDownload.BaixaConteudoArquivoAsync(UrlDoArquivo);

            DependencyService.Get<IGerenciadorDeArquivo>().SalvarTexto(NomeArquivoLocal, conteudo);
        }

        /// <summary>
        /// Sincroniza os contatos(baixa o arquivo CSV e atualizar os contatos na agenda do usuário)
        /// </summary>
        public void BaixarArquivoDeContatos()
        {
            string conteudo = gerenciadorDeDownload.BaixaConteudoArquivo(UrlDoArquivo);

            DependencyService.Get<IGerenciadorDeArquivo>().SalvarTexto(NomeArquivoLocal, conteudo);
        }

        #region Metodos que acessam o arquivo

        /// <summary>
        /// Converte
        /// </summary>
        /// <param name="conteudoDoArquivo"></param>
        /// <returns></returns>
        protected List<Contato> ConverteParaLista(string conteudoDoArquivo)
        {
            List<Contato> contatos = new List<Contato>();

            var linhasDoArquivo = conteudoDoArquivo.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            //A primeira linha do arquivo é um cabeçalho
            //Começa a ler o arquivo a partir da segunda linha
            for (int i = 1; i < linhasDoArquivo.Count(); i++)
            {
                //O arquivo pode ser divido por ';' ou ',' 
                //Dependendo da versão do excel ele gera o arquivo csv separado com ',' ou ';'.
                string[] valores = linhasDoArquivo[i].Split(new[] { ';', ',' });

                //Pulando linhas irregulares.
                if (valores.Count() != 5)
                    continue;

                //IMEI
                string imei = FormataEmei(valores[0]);

                //Nome Completo(Apenas para separar)
                string nomeCompleto = valores[1];
                //Nome
                string nome = nomeCompleto.Split(' ')[0];
                //Sobrenome
                string sobrenome = "";
                if (nomeCompleto.Split(' ').Count() > 1)
                {
                    sobrenome = nomeCompleto.Remove(0, nome.Length + 1);
                }
                //Email
                string email = valores[2];
                //DDD
                string ddd = valores[3];
                //NUmero Telefone
                string numeroTelefone = valores[4];

                //Se já existe o contato cadastrado(Verifica pelo nome)
                var contatoExistente = (from c in contatos
                                        where string.Equals(c.NomeCompleto, nomeCompleto)
                                        select c).FirstOrDefault();

                //Caso exista deve, se necessário, adicionar o email e o telefone  
                if (contatoExistente != null)
                {
                    //Verifica se o telefone está cadastrado
                    if (!contatoExistente.Telefones.Exists(tel => string.Equals(tel.DDD, ddd) && string.Equals(tel.Numero, numeroTelefone)))
                    {
                        Telefone novoTelefone = new Telefone();
                        novoTelefone.DDD = ddd;
                        novoTelefone.Numero = numeroTelefone;
                        contatoExistente.Telefones.Add(novoTelefone);
                    };

                    //Verifica se email já está cadastrado
                    if (!contatoExistente.Emails.Exists(e => string.Equals(e, email)))
                    {
                        contatoExistente.Emails.Add(email);
                    }
                }
                else
                {
                    Contato contato = new Contato();
                    contato.IMEIs.Add(imei);

                    //Nome
                    contato.NomeFuncionario = nome;

                    //Sobrenome
                    contato.SobrenomeFuncionario = sobrenome;

                    //Email
                    contato.Emails.Add(email);

                    Telefone telefone = new Telefone();
                    //DDD
                    telefone.DDD = ddd;
                    //Telefone
                    telefone.Numero = numeroTelefone;
                    contato.Telefones.Add(telefone);

                    contatos.Add(contato);
                }
            }

            return contatos;
        }

        private List<string> MontaListaImeis(string conteudoDoArquivo)
        {
            List<string> imeis = new List<string>();

            //TODO - Deve pular a primeira linha
            //TODO - Deve validar se o contato já não existe na lista
            foreach (string linhaDoArquivo in conteudoDoArquivo.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                Contato contato = new Contato();
                string[] valores = linhaDoArquivo.Split(new[] { ';', ',' });

                string dadosImeis = valores[0];

                //IMEI
                 //IMEI
                if(dadosImeis.Length > 1)
				{
					string imei = FormataEmei(dadosImeis);
					if (imei.Length > 1)
						imeis.Add(imei);
				}
			}

            return imeis;
        }

        #endregion

        /// <summary>
        /// Obtem um único IMEI
        /// </summary>
        /// <param name="dadosImeis"></param>
        /// <returns></returns>
        private string FormataEmei(string dadosImeis)
        {
            //Deixa apenas os digitos
            //string digitosImei = dadosImeis.ApenasDigitos();
			 string digitosImei = dadosImeis;
            //Se tiver 15 digitos é um imei válido.
            return (digitosImei.Length == 12 || digitosImei.Length == 13 || digitosImei.Length == 15) ? digitosImei : "";

        }
    }
}
