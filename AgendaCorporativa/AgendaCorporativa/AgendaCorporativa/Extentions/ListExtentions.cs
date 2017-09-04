using AgendaCorporativa.Modelos;
using Plugin.Contacts.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaCorporativa.Extentions
{
    public static class ListExtentions
    {
        public static IEnumerable<Contato> ToListaDeContato(this IEnumerable<Contact> contatos)
        {
            List<Contato> contatosLista = new List<Contato>();

            foreach (Contact contato in contatos)
            {
                List<Telefone> telefones = new List<Telefone>();
                List<string> emails = new List<string>();

                foreach (Phone phone in contato.Phones)
                {
                    Telefone telefone = new Telefone();
                    telefone.Numero = phone.Number;
                    telefones.Add(telefone);
                }

                foreach (Email email in contato.Emails)
                {
                    emails.Add(email.Address);
                }

                Contato cont = new Contato();
                cont.NomeFuncionario = contato.DisplayName;
                cont.Telefones = telefones;
                cont.Emails = emails;

                contatosLista.Add(cont);
            }

            return contatosLista;
        }
    }
}
