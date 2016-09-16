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
using Contacts;
using System.Linq;

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
        public const int maxDigitos = 8;

        /// <summary>
        /// Construtor de nova instância da classe <see cref="GerenciadorDeAgendaIOS"/>.
        /// </summary>
        public GerenciadorDeAgendaIOS()
        {
            NSError error;
            this.agenda = ABAddressBook.Create(out error);
            if (error != null)
            {
                Console.WriteLine("Erro ao iniciar a agenda Local");
            }
        }
        public void AtualizarAgendaDoAparelho(List<Contato> contatos)
        {
            // if the app was not authorized then we need to ask permission
            if (ABAddressBook.GetAuthorizationStatus() != ABAuthorizationStatus.Authorized)
            {
                agenda.RequestAccess(delegate (bool granted, NSError error)
                {
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
            }
            else
            {
                //Remove os contatos existentes
                RemoveContatosExistentes(contatos);

                //Cadastra os contatos do arquivo
                CadastraContatos(contatos);
            }
        }

        private void CadastraContatos(List<Contato> contatos)
        {
            foreach (Contato contatoArquivo in contatos)
            {
                var novoContato = new ABPerson();
                novoContato.FirstName = contatoArquivo.NomeFuncionario;
                novoContato.LastName = contatoArquivo.SobrenomeFuncionario;

                var fones = new ABMutableStringMultiValue();

                foreach (Telefone telefone in contatoArquivo.Telefones)
                {
                    fones.Add(telefone.DDD + telefone.Numero, ABPersonPhoneLabel.Main);
                }

                novoContato.SetPhones(fones);

                agenda.Add(novoContato);
                agenda.Save();
            }
        }

        private void RemoveContatosExistentes(List<Contato> contatos)
        {
            var telefonesDoArquivo = contatos.SelectMany(l => l.Telefones).ToList();

            List<ABPerson> contatosExistente =
                (from person in agenda.GetPeople()
                 where ((from phone in person.GetPhones()
                         where (telefonesDoArquivo.Exists(t => OitoDitigos(t.Numero) == OitoDitigos(phone.Value)))
                         select phone).Count() > 0)

                 select person).ToList();

            //Remove Contatos Existentes
            foreach (var contato in contatosExistente)
            {
                agenda.Remove(contato);
                agenda.Save();
            }
        }
        

        private bool DeletarContato()
        {
            var predicate = CNContact.GetPredicateForContacts("Appleseed");

            return false;
        }
        /// <summary>
        /// Método responsável por retornar apenas os últimos 8 dígitos do número
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private String OitoDitigos(String num)
        {
            String numero = num.ApenasDigitos();
            if (numero.Length < 8)
            {
                return "";
            }

            return numero.Substring(numero.Length - maxDigitos);
        }
    }
}