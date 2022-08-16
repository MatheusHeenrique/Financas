using System;
using System.IO;
using CsvHelper.Configuration.Attributes;

namespace Finanças
{
    class Program
    {

        static void Main(string[] args)
        {
            string caminhoPasta = @"C:\Users\mathe\OneDrive\Documentos\Finanças";
            var listaDeCartões = new List<Cartão>();
            var menuDoPrograma = new Menu();

            // Verificando se a pasta Finanças existe
            if (!Directory.Exists(caminhoPasta)) Directory.CreateDirectory(caminhoPasta);

            // Verificando se tem existe arquivos na pasta Finanças
            var caminhoDosArquivos = Directory.GetFiles(caminhoPasta, "*.csv").ToList();

            if (caminhoDosArquivos.Count() == 0)
            {
                var resposta = menuDoPrograma.NãoTemCartão();

                if (resposta == 1)
                {
                    // Criado primeiro cartão
                    var primeirCartão = menuDoPrograma.CriandoCartão();

                    // Adicionado cartão a lista de cartões
                    listaDeCartões.Add(primeirCartão);
                }
                else if (resposta == 2)
                {
                    System.Environment.Exit(0);
                }
            } // Se não tiver nem um cartão cadastrado
            else
            {
                // Variavel usada para carregar gastos do mês atual
                var mes = DateTime.Now.ToString(" MM ");

                // Verificando se o arquivo no novo mes foi criado
                bool arquivoExiste = false;
                foreach (var arquivo in caminhoDosArquivos)
                {
                    if (arquivo.Contains(mes))
                    {
                        arquivoExiste = true;
                    }
                }

                foreach (var arquivo in caminhoDosArquivos)
                {
                    Salvar leitor = new Salvar(new Cartão("", "", 0, 0));

                    if (!arquivo.Contains("Gasto"))
                    {
                        // Verificando se esta no mês atual
                        if (arquivoExiste)
                        {
                            if (!arquivo.Contains(mes))
                            {
                                continue;
                            }
                        }


                        // Adicionando cartão a lista de cartões
                        var cartão = leitor.LerArquivoCsv(arquivo);
                        listaDeCartões.Add(cartão);
                    }
                    else
                    {
                        continue;
                    }
                } // Pegando cartões salvo e add em listaDeCartões

                foreach (var arquivo in caminhoDosArquivos)
                {
                    foreach (var cartão in listaDeCartões)
                    {
                        // Verificando se esta no mês atual
                        if (arquivoExiste)
                        {
                            if (!arquivo.Contains(mes))
                            {
                                continue;
                            }
                        }

                        if (arquivo.Contains(cartão.NomeCartão) && arquivo.Contains("Gasto"))
                        {
                            // adicionando gasto fixo ao cartão existente
                            Salvar leitor = new Salvar(cartão);
                            var cartãoNovo = leitor.LerArquivoCsv(arquivo, 'g');

                            // Atualizando cartão na listaDeCartões
                            listaDeCartões.Remove(cartão);
                            listaDeCartões.Add(cartãoNovo);

                            // saindo do loop
                            break;
                        }
                    }
                } // Adicionado gasto fixo as cartões existente em listaDeCartões

            } // Se existir cartão cadastrado vai adicionar em listaDeCartões


            while (true)
            {
                // Pegando nome do banco
                var nomeDosCartões = new List<string>();
                foreach (var cartão in listaDeCartões)
                {
                    string criandoNome = $"{cartão.NomeCartão}-{cartão.NomeProprietario}";
                    nomeDosCartões.Add(criandoNome);
                }

                int resposta = menuDoPrograma.MenuPrincipal();


                // Verificando se alguma divida foi quitada
                foreach (var cartão in listaDeCartões)
                {
                    cartão.AbaterParcelas();
                }

                if (resposta == 1)
                {
                    if (listaDeCartões.Count > 1)
                    {
                        // Pegando nome do cartão que o usuario deseja Adicionar o gasto
                        var cartãoEscolhido = menuDoPrograma.ListagemDosCartões(nomeDosCartões);

                        // Pegando o valor e outras informações que o usuario deseja adicionar no cartão
                        var informaçõesDeGastos = menuDoPrograma.AdicionarGasto(); // informaçõesDeGastos[0] = Quantia, informaçõesDeGastos[1] = Modaliade, informaçõesDeGastos[2] = Parcelas, informaçõesDeGastos[3] = Descrição

                        // for criado para ir passando por todos os cartões existentes em listaDeCartões
                        for (int i = 0; i < listaDeCartões.Count(); i++)
                        {
                            // Verficando se é o cartão escolhido
                            if (listaDeCartões[i].NomeCartão == cartãoEscolhido[0] && listaDeCartões[i].NomeProprietario == cartãoEscolhido[1])
                            {
                                // Adicionando gasto
                                listaDeCartões[i].AdicionarGasto(Convert.ToDouble(informaçõesDeGastos[0]), Convert.ToChar(informaçõesDeGastos[1]), Convert.ToInt16(informaçõesDeGastos[2]), DateTime.Now, informaçõesDeGastos[3]);

                                // saindo
                                break;

                            }
                        }

                    }
                    else
                    {
                        // Pegando o valor e outras informações que o usuario deseja adicionar no cartão
                        var informaçõesDeGastos = menuDoPrograma.AdicionarGasto(); // informaçõesDeGastos[0] = Quantia, informaçõesDeGastos[1] = Modaliade, informaçõesDeGastos[2] = Parcelas, informaçõesDeGastos[3] = Descrição
                        
                        // Adicionando gasto ao cartão
                        listaDeCartões[0].AdicionarGasto(Convert.ToDouble(informaçõesDeGastos[0]), Convert.ToChar(informaçõesDeGastos[1]), Convert.ToInt16(informaçõesDeGastos[2]), DateTime.Now, informaçõesDeGastos[3]);
                    }
                } // Adicionando Gasto
                else if (resposta == 2)
                {
                    if (listaDeCartões.Count > 1)
                    {
                        var cartãoEscolhido = menuDoPrograma.ListagemDosCartões(nomeDosCartões);

                        // Pegando resposta
                        var infomaçõesRemover = menuDoPrograma.RemoverGasto();

                        for (int i = 0; i < listaDeCartões.Count(); i++)
                        {
                            if (listaDeCartões[i].NomeCartão == cartãoEscolhido[0] && listaDeCartões[i].NomeProprietario == cartãoEscolhido[1])
                            {
                                // removendo arquivo
                                listaDeCartões[i].RemoverGasto(Convert.ToDouble(infomaçõesRemover[0]), Convert.ToChar(infomaçõesRemover[1]), Convert.ToInt16(infomaçõesRemover[2]));

                                // saindo
                                break;
                            }
                        }
                    }
                    else
                    {
                        // Pegando resposta
                        var infomaçõesRemover = menuDoPrograma.RemoverGasto();

                        // Removendo
                        listaDeCartões[0].RemoverGasto(quantia: Convert.ToDouble(infomaçõesRemover[0]), modalidade: Convert.ToChar(infomaçõesRemover[1]));
                    }
                } // Removendo Gasto
                else if (resposta == 3)
                {
                    var novoCartão = menuDoPrograma.CriandoCartão();
                    listaDeCartões.Add(novoCartão);
                } // Adicionando Cartão
                else if (resposta == 4)
                {
                    var cartãoEscolhido = menuDoPrograma.ListagemDosCartões(nomeDosCartões);

                    foreach (var cartão in listaDeCartões)
                    {
                        if (cartão.NomeCartão == cartãoEscolhido[0] && cartão.NomeProprietario == cartãoEscolhido[1])
                        {
                            // Deletando cartão da lista de cartões
                            listaDeCartões.Remove(cartão);
                            
                            // Deletando arquivos salvos
                            foreach (string arquivo in caminhoDosArquivos)
                            {
                                if (arquivo.Contains(cartão.NomeProprietario) && arquivo.Contains(cartão.NomeCartão))
                                {
                                    File.Delete(arquivo);
                                }
                            }

                            break;
                        }
                    }
                } // Deletar cartões
                else if (resposta == 5)
                {
                    // Pegando o nome do cartão
                    List<string> cartãoEscolhido = new List<string>();
                    if (listaDeCartões.Count > 1)
                    {
                         cartãoEscolhido = menuDoPrograma.ListagemDosCartões(nomeDosCartões);
                    }
                    else
                    {
                        cartãoEscolhido.Add(listaDeCartões[0].NomeCartão);
                        cartãoEscolhido.Add(listaDeCartões[0].NomeProprietario);
                    }

                    foreach (var cartão in listaDeCartões)
                    {
                        if (cartão.NomeCartão == cartãoEscolhido[0] && cartão.NomeProprietario == cartãoEscolhido[1])
                        {
                            menuDoPrograma.MostrarSaldo(cartão);
                        }
                    }
                } // Saldo
                else if (resposta == 6)
                {
                    // Salvando arquivos
                    foreach (var cartão in listaDeCartões)
                    {
                        Salvar gravador = new Salvar(cartão);
                        gravador.SalvarEmCsv('t');
                        gravador.SalvarEmCsv('g');
                    }

                    // Saindo
                    break;
                } // Salvar e sair
            }
           
        }

    }
}
