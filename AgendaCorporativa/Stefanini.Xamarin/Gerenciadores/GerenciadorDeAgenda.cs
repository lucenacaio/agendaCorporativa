using Plugin.Contacts;
using Plugin.Contacts.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stefanini.Xamarin.Gerenciadores
{
    /// <summary>
    /// Classe responsavel em acessar, alterar e deletar contatos na agenda do aparelho.
    /// </summary>
    public class GerenciadorDeAgenda
    {
        /// <summary>
        /// Obtem uma lista de <see cref="Contact"/> da agenda do celular
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Contact>> CarregaAgendaDoAparelho()
        {
            List<Contact> contatos = null;
            if (await CrossContacts.Current.RequestPermission())
            {
                CrossContacts.Current.PreferContactAggregation = false;

                await Task.Run(() =>
                {
                    if (CrossContacts.Current.Contacts == null)
                        return;

                    var contacts = CrossContacts.Current.Contacts
                                            .Where(c => !string.IsNullOrWhiteSpace(c.FirstName) && c.Phones.Count > 0);

                    contatos = contacts.OrderBy(c => c.FirstName).ToList();
                });
            }

            return contatos;
        }
    }
}
