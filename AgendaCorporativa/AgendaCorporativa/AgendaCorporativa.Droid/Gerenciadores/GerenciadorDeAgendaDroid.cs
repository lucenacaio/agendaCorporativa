using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Content;
using AgendaCorporativa.Contratos;
using Plugin.Contacts.Abstractions;
using AgendaCorporativa.Droid.Gerenciadores;
using Xamarin.Forms;
using AgendaCorporativa.Modelos;
using Android.Provider;
using Stefanini.Framework.Extensoes;
using System.Threading.Tasks;

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
            //Gerenciador de acesso a conteudos
            contentResolver = ((Activity)context).ContentResolver;

            agenda = await CarregarContatosAgendaAparelho();

            if (agenda.Count > 0)
            {
                //Monta uma lista de telefones do arquivo.
                List<Telefone> telefonesDoArquivo = contatos.SelectMany(l => l.Telefones).ToList();
                //Remove os contatos existentes
                RemoveContatosExistentes(telefonesDoArquivo);

                //Cadastra os contatos do arquivo
                CadastraContatos(contatos);
            }



        }

        private void CadastraContatos(List<Contato> contatos)
        {
            foreach (Contato contatoArquivo in contatos)
            {
                List<ContentProviderOperation> ops = new List<ContentProviderOperation>();
                //contador de contatos que serve como indice para adicionar novos contatos
                int rawContactInsertIndex = ops.Count;

                //Agrupador dos dados do contato - nele serao adicionados todos os dados do contato
                ContentProviderOperation.Builder builder =
                    ContentProviderOperation.NewInsert(ContactsContract.RawContacts.ContentUri);
                builder.WithValue(ContactsContract.RawContacts.InterfaceConsts.AccountType, null);
                builder.WithValue(ContactsContract.RawContacts.InterfaceConsts.AccountName, null);
                ops.Add(builder.Build());

                //Nome
                //instancia um novo objeto onde serao inseridos os dados do contato, buscando pela constante do sistema que possui o endereco
                builder = ContentProviderOperation.NewInsert(ContactsContract.Data.ContentUri);
                //criar uma referencia com um identificador unico no agrupador de dados
                builder.WithValueBackReference(ContactsContract.Data.InterfaceConsts.RawContactId, rawContactInsertIndex);
                //define o tipo de objeto que sera inserido no agrupador e modelo de contexto
                builder.WithValue(ContactsContract.Data.InterfaceConsts.Mimetype,
                    ContactsContract.CommonDataKinds.StructuredName.ContentItemType);
                //define valores do contato, podem ser utilizados quaisquer atributos do contato e um respectivo valor nas 2 chamadas abaixo
                builder.WithValue(ContactsContract.CommonDataKinds.StructuredName.FamilyName, contatoArquivo.SobrenomeFuncionario);
                builder.WithValue(ContactsContract.CommonDataKinds.StructuredName.GivenName, contatoArquivo.NomeFuncionario);
                //adiciona a nova entrada ao agrupador de dados.
                ops.Add(builder.Build());

                //Empresa
                builder = ContentProviderOperation.NewInsert(ContactsContract.Data.ContentUri);
                builder.WithValueBackReference(ContactsContract.Data.InterfaceConsts.RawContactId, rawContactInsertIndex);
                builder.WithValue(ContactsContract.Data.InterfaceConsts.Mimetype,
                    ContactsContract.CommonDataKinds.Organization.ContentItemType);
                builder.WithValue(ContactsContract.CommonDataKinds.Organization.Company, "STEFANINI - Agenda Corporativa");
                ops.Add(builder.Build());

                //Adicionando telefones
                int nItem = 1;
                //percore a lista de todos os telefones existentes no contato
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
                //percorre a lista de todos os emails existentes no contato.
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
                    //Utiliza o gerenciador de conteudo verificando se o usuario possui permissao de escrita e adiciona o mesmo na lista de contatos
                    var res = Forms.Context.ContentResolver.ApplyBatch(ContactsContract.Authority, ops);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Busca e remove os contatos da lista da agenda do aparelho. 
        /// Comparando com a lista da base de servico.
        /// </summary>
        /// <param name="contatos">Lista de contatos do arquivo</param>
        private void RemoveContatosExistentes(List<Telefone> telefonesDoArquivo)
        {
            //percre a agenda do aparelho e compara com a lista e contatos do parametro e verifica se o contato existe
            //em ambas as listas, se baseando nos ultimos 8 digitos do numero de telefone do contato
            List<Contact> contatosExistente =
                (from person in agenda
                 where ((from phone in person.Phones
                         where (telefonesDoArquivo.Exists(t => OitoDitigos(t.Numero) == OitoDitigos(phone.Number)))
                         select phone).Count() > 0)

                 select person).ToList();

            //Remove Contatos da agenda do aparelho, que estao duplicados em ambas as listas
            foreach (var contato in contatosExistente)
            {
                Android.Net.Uri uri = ContactsContract.Contacts.ContentUri;

                //define um grupo de colunas que sera acessada do Cursor da busca
                string[] colunas = {
                    ContactsContract.Contacts.InterfaceConsts.Id,
                    ContactsContract.Contacts.InterfaceConsts.DisplayName,
                    ContactsContract.Contacts.InterfaceConsts.LookupKey
                };

                //define qual(is) parametro(s) sera(ao) utilizados na pesquisa
                string paramsBusca = string.Format("{0} = '{1}'", ContactsContract.ContactsColumns.DisplayName, contato.FirstName);

                //Define um objeto do tipo ICursor que acessa o conteudo atraves de uma Query chamada pelo Gerenciador de Conteudo
                //utilizando URI (local do servico), COLUNAS (quais dados primario retornarao no cursor da pesquisa),
                //PARAMSBUSCA (Quais parametros serao comparados para que seja realizada a pesquisa
                var cursor = ((Activity)context).ContentResolver.Query(uri, colunas, paramsBusca, null, null);
                //o ICursor pode retornar nulo caso aconteca algum problema na query
                if (cursor != null)
                    while (cursor.MoveToNext())//percore todos os itens retornados na pesquisa
                    {
                        //Obtem o indice da coluna LookupKey do cursor
                        int indice = cursor.GetColumnIndex(ContactsContract.ContactsColumns.LookupKey);

                        //Caso não encontre o valor será -1, onde significa que nao foi possivel encontrar o mesmo
                        if (indice != -1)
                        {
                            //Obtem o conteudo que está posicionado no indice da coluna LookupKey
                            string lookupKey = cursor.GetString(indice);

                            //Caso o conteudo seja NULL, deve passar pro próximo.
                            if (lookupKey == null)
                                continue;

                            //uriBsca retorna o endereco real do item em tempo de execucao dentro do ICursor a partir da chave de posicao
                            Android.Net.Uri uriBusca = Android.Net.Uri.WithAppendedPath(ContactsContract.Contacts.ContentLookupUri, lookupKey);

                            //Caso a uriBusca seja NULL, deve passar pro próximo
                            if (uriBusca == null)
                                continue;

                            //O gerenciador de conteudo executa a acao de Delete se baseando no endereco real, acima citado
                            ((Activity)context).ContentResolver.Delete(uriBusca, null, null);
                        }
                    }
                //sempre que um ICursor for aberto, o mesmo precisa ser fechado, afim de evitar estouro de memoria
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
        /// Carrega os contatos da agenda do aparelho
        /// </summary>
        /// <returns></returns>
        public async Task<List<Contact>> CarregarContatosAgendaAparelho()
        {
            List<Contact> dadosAgenda = new List<Contact>();

            Android.Net.Uri uri = ContactsContract.Contacts.ContentUri;

            //Mapeia as colunas que vão ser retornadas. (Id, Nome, Chave da posição do cursor em tempo de execução)
            string[] colunas = {
                    ContactsContract.Contacts.InterfaceConsts.Id,
                    ContactsContract.Contacts.InterfaceConsts.DisplayName,
                    ContactsContract.Contacts.InterfaceConsts.LookupKey
                };
            //Pesquisa nos contatos(uri) as informações(colunas)
            var cursor = ((Activity)context).ContentResolver.Query(uri, colunas, null, null, null);

            if (cursor != null)
                while (cursor.MoveToNext())
                {
                    Contact contato = new Contact();

                    int indice = cursor.GetColumnIndex(ContactsContract.ContactsColumns.LookupKey);
                    if (indice != -1)
                    {
                        contato.DisplayName = cursor.GetString(cursor.GetColumnIndex(DISPLAY_NAME));
                        contato.FirstName = cursor.GetString(cursor.GetColumnIndex(DISPLAY_NAME));

                        //Obtem o conteudo que está posicionado no indice da coluna LookupKey
                        string lookupKey = cursor.GetString(indice);
                        //Caso o conteudo seja NULL, deve passar pro próximo.
                        if (lookupKey == null) continue;


                        Android.Net.Uri uriBusca = Android.Net.Uri.WithAppendedPath(ContactsContract.Contacts.ContentLookupUri, lookupKey);
                        //Caso a uriBusca seja NULL, deve passar pro próximo
                        if (uriBusca == null) continue;

                        string idContato = cursor.GetString(cursor.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.Id));
                        if (idContato == null) continue;

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
            cursor.Close();
            return dadosAgenda;
        }
    }
}