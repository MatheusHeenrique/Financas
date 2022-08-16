using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Finanças
{
    internal class Salvar
    { 
        private string CaminhoPasta = @"C:\Users\mathe\OneDrive\Documentos\Finanças";

        private Cartão CartãoDeCredito;

        public Salvar(Cartão cartãoDeCredito)
        {
            if (!Directory.Exists(CaminhoPasta))
            {
                Directory.CreateDirectory(CaminhoPasta);
            } // Verificando se o diretorio existe

            this.CartãoDeCredito = cartãoDeCredito;
        }

        public void SalvarEmCsv(char tipo = 't')
        {
            string caminhoDoArquivo = NomeArquivo(tipo);

            // Salvando o arquivo
            using (var writer = new StreamWriter(caminhoDoArquivo))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                // Se for Transações, ira salvar nome do bando, nome do proprietario, valor na conta, e limite do cartão
                if (tipo == 't')
                {
                    var infomaçãoBasica = new List<string> { CartãoDeCredito.NomeCartão, CartãoDeCredito.NomeProprietario,
                    CartãoDeCredito.LimiteCredito.ToString(), CartãoDeCredito.ValorNaConta.ToString() };

                    csv.WriteField(infomaçãoBasica);
                    csv.NextRecord();
                }

                // Verificando se é Transações ou GastoFixo
                var listaDeString = new List<List<string>>();
                if (tipo == 't')
                {
                    listaDeString = this.CartãoDeCredito.Transações;
                }
                else if (tipo == 'g')
                {
                    listaDeString = this.CartãoDeCredito.GastosFixos;
                }

                // Salvado
                foreach (var item in listaDeString)
                {
                    csv.WriteField(item);
                    csv.NextRecord();
                }

                // permite que salvem o cartão mesmo que não tenha nem um valor nele
                if (listaDeString.Count == 0)
                {
                    csv.WriteField(0);
                }

            }
        }

        public Cartão LerArquivoCsv(string path, char tipo = 't')
        {
            string caminhoDoArquivo = path;
            var lista = new List<List<string>>();

            using (StreamReader sr = File.OpenText(caminhoDoArquivo))
            {
                string Leitor;
                while ((Leitor = sr.ReadLine()) != null)
                {
                    var linha = new List<string>();
                    if (Leitor.Substring(0, 1) == "\"")
                    {
                        // Quantia
                        string quantiaString = Leitor.Substring(1, Leitor.LastIndexOf("\"") - 1);
                        linha.Add(quantiaString);

                        // Pegando os resto das informações
                        var strDividida = Leitor.Split(',').ToList();
                        linha.Add(strDividida[2]); // Descrição
                        linha.Add(strDividida[3]); // Data
                    }
                    else
                    {
                        linha = Leitor.Split(',').ToList();
                    }
                    lista.Add(linha);
                }
            } // Lendo o arquivo e transformando em uma lista de lista de string

            // converter para list para classe Cartão. lista[0][0] = nomeCartão,  lista[0][1] = nomeProprietario,  Convert.ToDouble(lista[0][2]) = LimiteCredito,  Convert.ToDouble(lista[0][3]) = ValorNaConta
            if (tipo == 't')
            {
                var novoCartão = new Cartão(lista[0][0], lista[0][1], Convert.ToDouble(lista[0][2]), Convert.ToDouble(lista[0][3]));
                lista.RemoveAt(0);

                // permite que salvem o cartão mesmo que não tenha nem um valor nele
                if (Convert.ToInt32(lista[0][0]) == 0)
                {
                    lista.RemoveAt(0);
                }

                novoCartão.Transações = lista;
                return novoCartão;
            }
            else
            {
                this.CartãoDeCredito.GastosFixos = lista;
                return this.CartãoDeCredito;
            }  
        }

        public string NomeArquivo(char tipo = 't', bool somenteNome = false)
        {
            string nomeDoArquivo = ""; // Variavel que vai conter nome no arquivo ou path
            if (tipo == 't')
            {
                nomeDoArquivo = $"{this.CartãoDeCredito.NomeCartão} - {this.CartãoDeCredito.NomeProprietario} {DateTime.Now.ToString("MM yyyy")}.csv";
            } // se for Transação crie no nome desse jeito | C6 Bank - Matheus 08 2022.csv
            else if (tipo == 'g')
            {
                nomeDoArquivo = $"{this.CartãoDeCredito.NomeCartão} [Gasto fixo] - {this.CartãoDeCredito.NomeProprietario} {DateTime.Now.ToString("MM yyyy")}.csv";
            } // se for Gasto Fixo crie no nome desse jeito | C6 Bank [Gasto fixo] - Matheus 08 2022.csv

            // Vai retorna somente o nome se o usuario querer
            if (somenteNome)
            {
                return nomeDoArquivo;
            }

            // retorna local do arquivo + nome
            string caminhoDoArquivo = $@"{this.CaminhoPasta}\{nomeDoArquivo}";
            return caminhoDoArquivo;
        }
    }
}
