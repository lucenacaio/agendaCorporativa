using AgendaCorporativa.Contratos;
using AgendaCorporativa.iOS.Gerenciadores;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Plugin.Contacts.Abstractions;
using System.Threading.Tasks;
using AddressBook;
using Foundation;
using Stefanini.Framework.Extensoes;
using AgendaCorporativa.Modelos;

[assembly: Dependency(typeof(GerenciadorDeAgendaIOS))]
namespace AgendaCorporativa.iOS.Gerenciadores
{
    /// <summary>
    /// Classe responsável por gerenciar a agenda nativa do aparelho
    /// </summary>
    class GerenciadorDeAgendaIOS : IGerenciadorDeAgenda
    {
        /// <summary>
        /// Agenda Local
        /// </summary>
        private ABAddressBook agenda = null;

        /// <summary>
        /// Construtor de nova instância da classe <see cref="GerenciadorDeAgendaIOS"/>.
        /// </summary>
        public GerenciadorDeAgendaIOS()
        {
            NSError error;
            this.agenda = ABAddressBook.Create(out error);
            if(error != null)
            {
                Console.WriteLine("Erro ao iniciar a agenda Local");
            }
        }
        public void AtualizarAgendaDoAparelho(List<Contato> contatos)
        {
            // if the app was not authorized then we need to ask permission
            if (ABAddressBook.GetAuthorizationStatus() != ABAuthorizationStatus.Authorized)
            {
                agenda.RequestAccess(delegate (bool granted, NSError error) {
                    if (error != null)
                    {
                        Console.WriteLine("Erro ao verificar a permissão de acesso a agenda local.");
                        AtualizarAgenda(contatos);
                    }
                    else if (granted)
                    {
                        Console.WriteLine("Permissão a agenda local permitida.");
                    }
                });
            }
            else
            {
                Console.WriteLine("PERMISSÃO JÁ CONCEDIDA, LISTANDO.");
                AtualizarAgenda(contatos);
            }

        }

        private void AtualizarAgenda(List<Contato> contatos)
        {

            if (agenda.GetPeople().Length == 0)
            {
                Console.WriteLine("O Aparelho não possui contatos.");
            }else
            {

                foreach(Contato contatoArquivo in contatos)
                {
                    bool existe = false;
                    foreach (Telefone telefoneArquivo in contatoArquivo.Telefones)
                    {
                        foreach (ABPerson contatoAgenda in agenda.GetPeople())
                        {
                            //Console.WriteLine("Nome: " + contatoAgenda.FirstName);
                            foreach (ABMultiValueEntry<String> telefoneAgenda in contatoAgenda.GetPhones())
                            {
                                if (telefoneArquivo.Numero.ApenasDigitos() == telefoneAgenda.Value.ApenasDigitos())
                                {
                                    existe = true;
                                    break;
                                }
                                
                            }//fim do foreach dos telefones da Agenda
                            if (existe)
                            {
                                break;
                            }

                        }//fim do foreach dos contatos da Agenda
                        if (existe)
                        {
                            break;
                        }
                    }//fim do foreach dos telefones do Arquivo

                    if (existe)
                    {
                        Console.WriteLine("CONTATO: " + contatoArquivo.NomeCompleto + " encontrado, atualizando dados.");
                    }
                    else
                    {
                        Console.WriteLine("CONTATO: " + contatoArquivo.NomeCompleto + " INEXISTENTE, CRIANDO NOVO.");
                    }

                }//fim do foreach dos contatos do Arquivo
                
            }

            
        }
    }
}