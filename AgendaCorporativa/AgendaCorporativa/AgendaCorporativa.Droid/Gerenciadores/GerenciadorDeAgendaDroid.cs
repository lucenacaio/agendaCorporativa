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
using System.Threading.Tasks;
using Android.Database;
using Java.Lang;
using Android.Text;

[assembly: Dependency(typeof(GerenciadorDeAgendaDroid))]
namespace AgendaCorporativa.Droid.Gerenciadores
{
    class GerenciadorDeAgendaDroid : IGerenciadorDeAgenda
    {
        public const int maxDigitos = 8;
        private List<Contact> agenda;
        private Context context = Forms.Context;
        private Android.Net.Uri QUERY_URI = ContactsContract.Contacts.ContentUri;
        private string CONTACT_ID = ContactsContract.Contacts.InterfaceConsts.Id;
        private string DISPLAY_NAME = ContactsContract.Contacts.InterfaceConsts.DisplayName;
        private Android.Net.Uri EMAIL_CONTENT_URI = ContactsContract.CommonDataKinds.Email.ContentUri;
        private string EMAIL_CONTACT_ID = ContactsContract.CommonDataKinds.Email.InterfaceConsts.ContactId;
        private string EMAIL_DATA = ContactsContract.CommonDataKinds.Email.InterfaceConsts.Data;
        private string HAS_PHONE_NUMBER = ContactsContract.Contacts.InterfaceConsts.HasPhoneNumber;
        private string PHONE_NUMBER = ContactsContract.CommonDataKinds.Phone.Number;
        private Android.Net.Uri PHONE_CONTENT_URI = ContactsContract.CommonDataKinds.Phone.ContentUri;
        private string PHONE_CONTACT_ID = ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId;
        private string STARRED_CONTACT = ContactsContract.Contacts.InterfaceConsts.Starred;
        private ContentResolver contentResolver;


        public GerenciadorDeAgendaDroid()
        {

        }
        public async void AtualizarAgendaDoAparelho(List<Contato> contatos)
        {
            contentResolver = ((Activity)context).ContentResolver;
            //agenda = await GerenciadorDeAgenda.CarregaAgendaDoAparelho();
            try
            {
                agenda = await CarregarContatosAgendaAparelho();
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Erro ao tentar carregar a lista da agenda do aparelho: " + e.Message);
            }


            if (agenda.Count == 0)
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
                foreach (string email in contatoArquivo.Emails)
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
                break;
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


            //Remove Contatos Existentes
            foreach (var contato in contatosExistente)
            {
                Android.Net.Uri uri = ContactsContract.Contacts.ContentUri;

                string[] colunas = {
                    ContactsContract.Contacts.InterfaceConsts.Id,
                    ContactsContract.Contacts.InterfaceConsts.DisplayName,
                    ContactsContract.Contacts.InterfaceConsts.LookupKey
                };
                string paramsBusca = string.Format("{0} = '{1}'", ContactsContract.ContactsColumns.DisplayName, contato.FirstName);

                var cursor = ((Activity)context).ContentResolver.Query(uri, colunas, paramsBusca, null, null);

                while (cursor.MoveToNext())
                {
                    try
                    {
                        int indice = cursor.GetColumnIndex(ContactsContract.ContactsColumns.LookupKey);
                        if (indice != -1)
                        {
                            string lookupKey = cursor.GetString(indice);
                            Android.Net.Uri uriBusca = Android.Net.Uri.WithAppendedPath(ContactsContract.Contacts.ContentLookupUri, lookupKey);

                            ((Activity)context).ContentResolver.Delete(uriBusca, null, null);
                        }

                    }
                    catch (System.Exception e)
                    {
                        Console.WriteLine("Error", e.Message);
                    }
                }
                cursor.Close();
            }
        }

        /// <summary>
        /// Método responsável por retornar apenas os últimos 8 dígitos do número
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private string OitoDitigos(string num)
        {
            string numero = num.ApenasDigitos();
            if (numero.Length < 8)
            {
                return "";
            }

            return numero.Substring(numero.Length - maxDigitos);
        }

        /// <summary>
        /// Responsável por carregar os contatos da agente do aparelho
        /// </summary>
        /// <returns></returns>
        public async Task<List<Contact>> CarregarContatosAgendaAparelho()
        {
            List<Contact> dadosAgenda = new List<Contact>();
            Android.Net.Uri uri = ContactsContract.Contacts.ContentUri;

            string[] colunas = {
                    ContactsContract.Contacts.InterfaceConsts.Id,
                    ContactsContract.Contacts.InterfaceConsts.DisplayName,
                    ContactsContract.Contacts.InterfaceConsts.LookupKey

                };
            var cursor = ((Activity)context).ContentResolver.Query(uri, colunas, null, null, null);
            if (cursor != null)
                while (cursor.MoveToNext())
                {
                    try
                    {
                        Contact contato = new Contact();


                        int indice = cursor.GetColumnIndex(ContactsContract.ContactsColumns.LookupKey);
                        if (indice != -1)
                        {
                            contato.DisplayName = cursor.GetString(cursor.GetColumnIndex(DISPLAY_NAME));
                            contato.FirstName = cursor.GetString(cursor.GetColumnIndex(DISPLAY_NAME));
                            string lookupKey = cursor.GetString(indice);
                            Android.Net.Uri uriBusca = Android.Net.Uri.WithAppendedPath(ContactsContract.Contacts.ContentLookupUri, lookupKey);
                            string idContato = cursor.GetString(cursor.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.Id));

                            var telefones = ((Activity)context).ContentResolver.Query(ContactsContract.CommonDataKinds.Phone.ContentUri,
                            null, ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId + " = " + idContato, null, null);

                            if (telefones != null)
                                while (telefones.MoveToNext())
                                {
                                    string telefone = telefones.GetString(telefones.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number));
                                    Phone numero = new Phone();
                                    numero.Number = telefone;
                                    contato.Phones.Add(numero);
                                }
                            telefones.Close();

                            var emails = ((Activity)context).ContentResolver.Query(ContactsContract.CommonDataKinds.Email.ContentUri,
                            null, ContactsContract.CommonDataKinds.Email.InterfaceConsts.ContactId + " = " + idContato, null, null);

                            if (emails != null)
                                while (emails.MoveToNext())
                                {
                                    string email = emails.GetString(emails.GetColumnIndex(ContactsContract.CommonDataKinds.Email.Address));
                                    Email endereco = new Email();
                                    endereco.Address = email;
                                    contato.Emails.Add(endereco);
                                }
                            dadosAgenda.Add(contato);
                            emails.Close();
                        }

                    }
                    catch (System.Exception e)
                    {
                        Console.WriteLine("Error", e.Message);
                    }
                }
            cursor.Close();
            return dadosAgenda;
        }
    }
}