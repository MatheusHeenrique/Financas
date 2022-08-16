namespace Finanças
{
    internal class Menu
    {
        public int NãoTemCartão()
        {
            int opção = 0;
            while ((opção != 1) && (opção != 2))
            {
                // Apresentação
                Console.Clear();
                Console.WriteLine($"\t\t\t\t\t\t ### Finanças ### \n\n\n");

                // Perguntando se a pessoa que cadastra o cartão
                Console.WriteLine("Você não tem nem um cartão cadastro! Gostaria de cadastra um ?");
                Console.WriteLine("1 - Cadrastra um cartão \n2 - Sair");
                Console.Write("Sua escolha: ");
                opção = Convert.ToInt32(Console.ReadLine());
            }

            return opção;
        }
        public int MenuPrincipal()
        {
            // Cabeçario
            Console.Clear();
            Console.WriteLine($"\t\t\t\t\t\t ### Finanças ### \n\n\n");

            // Opções
            Console.WriteLine("1 - Adicionar gastos \n2 - Remover gastos \n3 - Adicionar cartão");
            Console.WriteLine("4 - Deletar cartões \n5 - Saldo \n6 - Salvar e sair");
            Console.Write("Sua escolha: ");
            int resposta = Convert.ToInt32(Console.ReadLine());

            return resposta;
        }
        public Cartão CriandoCartão()
        {
            var listaDeResposta = new List<string?>();

            Console.Clear();
            Console.Write("Digite o nome do banco do seu cartão: ");
            var resposta = Console.ReadLine();

            listaDeResposta.Add(resposta.Trim());

            Console.Clear();
            Console.Write("Digite o seu nome: ");
            resposta = Console.ReadLine();

            listaDeResposta.Add(resposta.Trim());

            Console.Clear();
            Console.Write("Digite o limite do seu cartão de credito: ");
            resposta = Console.ReadLine();

            listaDeResposta.Add(resposta.Trim());

            Console.Clear();
            Console.Write("Digite quantos você tem na sua conta:");
            resposta = Console.ReadLine();

            listaDeResposta.Add(resposta.Trim());

            Cartão PrimeiroCartão = new Cartão(listaDeResposta[0], listaDeResposta[1], Convert.ToDouble(listaDeResposta[2]), Convert.ToDouble(listaDeResposta[3]));
            return PrimeiroCartão;
        }
        public List<string> AdicionarGasto()
        {
            var listaDeRespota = new List<string>();

            Console.Clear();
            Console.Write("Qual o valor que desse adicionar: ");
            string? resposta = Console.ReadLine();

            listaDeRespota.Add(resposta);

            Console.Clear();
            Console.Write("É credito, debito ou um gasto fixo: ");
            resposta = Console.ReadLine();

            listaDeRespota.Add(resposta);

            Console.Clear();
            Console.Write("Quantidade de parcelas: ");
            resposta = Console.ReadLine();

            listaDeRespota.Add(resposta);

            Console.Clear();
            Console.Write("Adicione um descrição para essse gasto:");
            resposta = Console.ReadLine();

            listaDeRespota.Add(resposta);


            return listaDeRespota;
        }
        public void MostrarSaldo(Cartão cartãoEscolhido)
        {
            while (true)
            {
                // Cabeçario
                Console.Clear();
                Console.WriteLine($"\t\t\t\t ### Saldo ### \n\n\n");

                // Mostrar o valor
                Console.WriteLine("   Data     |      Descrição             |  Valor");
                Console.WriteLine("------------------------------------------------------ ");
                
                // criando a tabela de Transações
                foreach (var linha in cartãoEscolhido.Transações)
                {
                    // Pegando somente a data, e removendo a hora
                    string data = linha[4].Substring(0, 11);
                   
                    string descrição;
                    if (linha[1].Length > 27)
                    {
                        descrição = linha[1].Substring(0, 27);
                    } // Se a descrição for muito grande vai contar ela
                    else
                    {
                        descrição = linha[1].PadRight(27);
                    } // Se a descrição for pequena vai adicionar espaços no final

                    // Pegando a quantia
                    string quantia = linha[0];

                    Console.WriteLine($" {data}| {descrição} | {quantia}");
                }

                // criando a tabela de GastoFixo
                foreach (var linha in cartãoEscolhido.GastosFixos)
                {
                    string data = linha[2].Substring(0, 11);

                    // Pegando somente a data, e removendo a hora
                    string descrição;
                    if (linha[1].Length > 27)
                    {
                        descrição = linha[1].Substring(0, 27);
                    } // Se a descrição for muito grande vai contar ela
                    else
                    {
                        descrição = linha[1].PadRight(27);
                    } // Se a descrição for pequena vai adicionar espaços no final

                    // Pegando a quantia
                    string quantia = linha[0];

                    Console.WriteLine($" {data}| {descrição} | {quantia}");
                }

                Console.WriteLine("------------------------------------------------------ ");

                // Valor total
                double valorTotal = cartãoEscolhido.Saldo();
                Console.WriteLine($"   Total    |                           R$ {valorTotal} \n\n\n");


                Console.Write("Sua Escolha (s  = Sair): ");
                var escolha = Console.ReadLine();


                if (Convert.ToChar(escolha[0]) == 's')
                {
                    break;
                } // Volta para tela Principal do saldo
                else if (Convert.ToChar(escolha[0]) == 'c')
                {
                    while (true)
                    {
                        // Cabeçario
                        Console.Clear();
                        Console.WriteLine($"\t\t\t\t ### Saldo ### \n\n\n");

                        // Mostrando saldo
                        Console.WriteLine($"Saldo: R$ {valorTotal}\n\n\n");

                        // Perguntado o que o usuario que fazer
                        Console.Write("Digite a operação que deja fazer: ");
                        var escolhaDaOperação = Console.ReadLine().Trim();

                        // verificando se o usuario quer sair
                        if (Convert.ToChar(escolhaDaOperação[0]) == 's')
                        {
                            break;
                        }

                        // dividindo a soma
                        var equação = escolhaDaOperação.Split(' ');

                        // calculador
                        if (equação[0] == "/")
                        {
                            valorTotal /= Convert.ToDouble(equação[1]);
                        }
                        else if (equação[0] == "*")
                        {
                            valorTotal *= Convert.ToDouble(equação[1]);
                        }
                        else if (equação[0] == "+")
                        {
                            valorTotal += Convert.ToDouble(equação[1]);
                        }
                        else if (equação[0] == "-")
                        {
                            valorTotal -= Convert.ToDouble(equação[1]);
                        }

                    }
                } // Entra na calculadora
                else if (Convert.ToChar(escolha[0]) == 'a')
                {
                    // Cabeçario
                    Console.Clear();
                    Console.WriteLine($"\t\t\t\t ### Saldo ### \n\n\n");

                    // Abatendo valor da conta
                    valorTotal = cartãoEscolhido.Saldo(true);

                    // Mostrando saldo
                    Console.WriteLine($"Saldo: R$ {valorTotal}\n\n\n");

                    // Pausando para usuario ver
                    Console.Write("< Precione [ENTER] para voltar para saldo >");
                    var pausa = Console.ReadLine();

                } // Abate o valor da conta no saldo

            }

        }
        public List<string> RemoverGasto(char transaçãoOuGasto = 't')
        {

            var listaDeResposta = new List<string>();


            Console.Clear();
            Console.Write("Digite o valor do gasto que deseja remover: ");
            var resposta = Console.ReadLine();

            listaDeResposta.Add(resposta);

            if (transaçãoOuGasto == 'g')
            {
                return listaDeResposta;
            }

            Console.Clear();
            Console.Write("Digite a modalidade desse gasto: ");
            resposta = Console.ReadLine();

            listaDeResposta.Add(resposta);

            Console.Clear();
            Console.Write("Digite quantas parcelas tem esse gasto: ");
            resposta = Console.ReadLine();

            listaDeResposta.Add(resposta);

            return listaDeResposta;
        }
        public List<string> ListagemDosCartões(List<string> nomesDosCartões)
        {
            // Cabeçario
            Console.Clear();
            Console.WriteLine($"\t\t\t\t\t\t ### Finanças ### \n\n\n");

            // Listando os cartões
            for (int i = 0; i < nomesDosCartões.Count(); i++)
            {
                Console.WriteLine($"{i + 1} - {nomesDosCartões[i].Replace("-", " ")}");
            }


            // Pegando resposta do usuario
            Console.Write("Sua escolha: ");
            int resposta = Convert.ToInt32(Console.ReadLine());

            // Convertendo respota do usuario para uma lista com NomeCartão e NomeProprietario
            int contador = 1;
            List<string> nomeDoBanco = new List<string>();
            foreach (var nome in nomesDosCartões)
            {
                if (contador == resposta)
                {
                    nomeDoBanco = nome.Split("-").ToList();
                    break;
                }
                contador++;
            }

            // Retorando o valor
            return nomeDoBanco;
        }
    }
}
