using System;
using System.Collections.Generic;
using Plugin.Contacts.Abstractions;
using System.Threading.Tasks;
using AgendaCorporativa.Modelos;

namespace AgendaCorporativa.Contratos
{
    public interface IGerenciadorDeAgenda
    {
        void AtualizarAgendaDoAparelho(List<Contato> contatos);
    }
}