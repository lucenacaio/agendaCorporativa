using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AgendaCorporativa.Gerenciadores
{
    class GerenciadorDeArquivo
    {
        //URL do Arquivo
        public Uri uri = new Uri("http://localhost:9090/img/teste.txt");

        public async Task<string> BaixarConteudoDoArquivo()
        {
            var httpClient = new HttpClient();

            //Baixa o arquivo
            byte[] arquivo = await httpClient.GetByteArrayAsync(uri);
            
            //Converte de Bytes para String
            string conteudoDoArquivo = Encoding.UTF8.GetString(arquivo, 0, arquivo.Length);

            return conteudoDoArquivo;
        }
    }
}
