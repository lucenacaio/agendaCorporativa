using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Net;
using AgendaCorporativa.Contratos;
using Plugin.Contacts.Abstractions;
using AgendaCorporativa.Droid.Gerenciadores;
using Xamarin.Forms;
using AgendaCorporativa.Modelos;
using Android.Provider;
using Stefanini.Xamarin.Gerenciadores;
using Stefanini.Framework.Extensoes;

[assembly: Dependency(typeof(GerenciadorDeAgendaDroid))]
namespace AgendaCorporativa.Droid.Gerenciadores
{
    class GerenciadorDeAgendaDroid : IGerenciadorDeAgenda
    {
        public const int maxDigitos = 8;
        private List<Contact> agenda;
        public GerenciadorDeAgendaDroid() {
        }
        public async void AtualizarAgendaDoAparelho(List<Contato> contatos)
        {
            agenda = await GerenciadorDeAgenda.CarregaAgendaDoAparelho();

            if (agenda.Count == 0)
            {
                Console.WriteLine("O Aparelho não possui contatos.");
            }
            else
            {
                //Remove os contatos existentes
                RemoveContatosExistentes(contatos);

                //Cadastra os contatos do arquivo
                //CadastraContatos(contatos);
            }
           

           
        }

        private void CadastraContatos(List<Contato> contatos)
        {
            foreach (Contato contatoArquivo in contatos)
            {
                List<ContentProviderOperation> ops = new List<ContentProviderOperation>();
                int rawContactInsertIndex = ops.Count;

                ContentProviderOperation.Builder builder =
                    ContentProviderOperation.NewInsert(ContactsContract.RawContacts.ContentUri);
                builder.WithValue(ContactsContract.RawContacts.InterfaceConsts.AccountType, null);
                builder.WithValue(ContactsContract.RawContacts.InterfaceConsts.AccountName, null);
                ops.Add(builder.Build());

                //Nome
                builder = ContentProviderOperation.NewInsert(ContactsContract.Data.ContentUri);
                builder.WithValueBackReference(ContactsContract.Data.InterfaceConsts.RawContactId, rawContactInsertIndex);
                builder.WithValue(ContactsContract.Data.InterfaceConsts.Mimetype,
                    ContactsContract.CommonDataKinds.StructuredName.ContentItemType);
                builder.WithValue(ContactsContract.CommonDataKinds.StructuredName.FamilyName, contatoArquivo.SobrenomeFuncionario);
                builder.WithValue(ContactsContract.CommonDataKinds.StructuredName.GivenName, contatoArquivo.NomeFuncionario);
                ops.Add(builder.Build());

                //Adicionando telefones
                int nItem = 1;
                foreach (Telefone telefone in contatoArquivo.Telefones)
                {
                    builder = ContentProviderOperation.NewInsert(ContactsContract.Data.ContentUri);
                    builder.WithValueBackReference(ContactsContract.Data.InterfaceConsts.RawContactId, rawContactInsertIndex);
                    builder.WithValue(ContactsContract.Data.InterfaceConsts.Mimetype,
                        ContactsContract.CommonDataKinds.Phone.ContentItemType);
                    builder.WithValue(ContactsContract.CommonDataKinds.Phone.Number, telefone.DDD + telefone.Numero);
                    builder.WithValue(ContactsContract.CommonDataKinds.StructuredPostal.InterfaceConsts.Type,
                            ContactsContract.CommonDataKinds.StructuredPostal.InterfaceConsts.TypeCustom);
                    builder.WithValue(ContactsContract.CommonDataKinds.Phone.InterfaceConsts.Label, "Telefone " + nItem);
                    ops.Add(builder.Build());
                    nItem++;
                }

                nItem = 1;
                foreach(String email in contatoArquivo.Emails)
                {
                    //Email
                    builder = ContentProviderOperation.NewInsert(ContactsContract.Data.ContentUri);
                    builder.WithValueBackReference(ContactsContract.Data.InterfaceConsts.RawContactId, rawContactInsertIndex);
                    builder.WithValue(ContactsContract.Data.InterfaceConsts.Mimetype,
                        ContactsContract.CommonDataKinds.Email.ContentItemType);
                    builder.WithValue(ContactsContract.CommonDataKinds.Email.InterfaceConsts.Data, email);
                    builder.WithValue(ContactsContract.CommonDataKinds.Email.InterfaceConsts.Type,
                        ContactsContract.CommonDataKinds.Email.InterfaceConsts.TypeCustom);
                    builder.WithValue(ContactsContract.CommonDataKinds.Email.InterfaceConsts.Label, "Email " + nItem);
                    ops.Add(builder.Build());
                    nItem++;
                }
                

                try
                {
                    var res = Forms.Context.ContentResolver.ApplyBatch(ContactsContract.Authority, ops);
                    Console.WriteLine("Contato:  " + contatoArquivo.NomeFuncionario + " adicionado");
                }
                catch
                {
                    Console.WriteLine("ERRO AO TENTAR ADICIONAR:  " + contatoArquivo.NomeFuncionario);
                }

            }
        }

        private void RemoveContatosExistentes(List<Contato> contatos)
        {
            var telefonesDoArquivo = contatos.SelectMany(l => l.Telefones).ToList();

            List<Contact> contatosExistente =
                (from person in agenda
                 where ((from phone in person.Phones
                         where (telefonesDoArquivo.Exists(t => OitoDitigos(t.Numero) == OitoDitigos(phone.Number)))
                         select phone).Count() > 0)

                 select person).ToList();
            var context = Forms.Context;

            //Remove Contatos Existentes
            foreach (var contato in contatosExistente)
            {
                Android.Net.Uri uri = ContactsContract.Contacts.ContentUri;

                string[] projection = {
                    ContactsContract.Contacts.InterfaceConsts.Id,
                    ContactsContract.Contacts.InterfaceConsts.DisplayName,
                    ContactsContract.Contacts.InterfaceConsts.LookupKey
                };
                String selection = string.Format("{0} = '{1}'", ContactsContract.ContactsColumns.DisplayName, contato.FirstName);

                var cursor = ((Activity)context).ManagedQuery(uri, projection, selection, null, null);
      
                cursor.MoveToFirst();
                int idx = cursor.GetColumnIndex(ContactsContract.ContactsColumns.LookupKey);
                if (idx != -1)
                {
                    try
                    {
                        string lookupkey = cursor.GetString(idx);
                        var lookupuri = Android.Net.Uri.WithAppendedPath(ContactsContract.Contacts.ContentLookupUri, lookupkey);
                        //((Activity)context).ContentResolver.Delete(lookupuri, null, null);
                        Console.WriteLine("Contato Deletado: {0}", contato.FirstName);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error", e.Message);
                    }
                }

            }
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