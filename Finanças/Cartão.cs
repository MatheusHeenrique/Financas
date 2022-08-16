using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanças
{
    internal class Cartão
    {
        public string NomeCartão { get; }
        public string NomeProprietario { get; }

        public double LimiteCredito;

        public double ValorNaConta;

        public List<List<string>> Transações = new List<List<string>>();

        public List<List<string>> GastosFixos = new List<List<string>>();


        public Cartão(string nomeCartão, string nomeProprietario, double LimiteCredito = 0, double ValorNaConta = 0)
        {
            // Pegando informações basicas
            this.NomeCartão = nomeCartão;
            this.NomeProprietario = nomeProprietario;
            this.LimiteCredito = LimiteCredito;
            this.ValorNaConta = ValorNaConta;
        }

        public void AdicionarGasto(double quantia, char modalidade, short parcelas, DateTime data, string descriçãoDoGasto)
        {
            // modalidade = Credito ou debito
            // parcelas = Quantas vezes você parcelou a sua comprar

            // Regras basicas
            if (modalidade == 'd')
            {
                parcelas = 0;
                if (quantia <= ValorNaConta)
                {
                    this.ValorNaConta -= quantia;
                }
                else
                {
                    Console.WriteLine("Saldo insuficiente!, tente novamente.");
                    return;
                }
            } // Verificando se o valor debitado é maior ou igual o valor da conta

            if (modalidade == 'c' && quantia > this.LimiteCredito)
            {
                Console.WriteLine("Valor maior que o limite do cartão!, Tente novamente.");
                return;
            } // Verificando se o valor passado no credito é maior que o limite do cartão

            if (parcelas < 0)
            {
                Console.WriteLine("O numero de parcelar não pode ser negativo! Tente novamente.");
                return;
            } // Verificando se as parcelas são um número possitivo

            if (this.Gerenciador(quantia))
            {
                Console.WriteLine("A quantia ultrapassa o limite do cartão! Tente novamente");
                return;

            } // varificação para ver se a transão pode ser adcicionada


            // adicionando linha a Transações ou a GastoFixo
            if (modalidade == 'g')
            {
                var linha = new List<string> { quantia.ToString(), descriçãoDoGasto, data.ToString() };
                this.GastosFixos.Add(linha);
            }
            else
            {
                var linha = new List<string> { quantia.ToString(), descriçãoDoGasto, parcelas.ToString(), modalidade.ToString(), data.ToString(), };
                this.Transações.Add(linha);
            }
        } // Adiciona gasto a listaItens de transações

        public void RemoverGasto(double quantia, char modalidade = 'c', short parcelas = 1)
        {
            var listaItens = new List<List<string>>();
            listaItens = this.Transações;

            if (modalidade == 'g')
            {
                listaItens = this.GastosFixos;
            }

            int posição = 0;
            foreach (var item in listaItens)
            {
                if (modalidade == 'g')
                {
                    // item[0] é a quantia, item[1] a modalidade credito ou debito, item[2] quatidade de parcelas
                    if (item[0] == quantia.ToString())
                    {
                        this.GastosFixos.RemoveAt(posição);
                        break;
                    }
                }
                else
                {
                    // item[0] é a quantia, item[1] a modalidade credito ou debito, item[2] quatidade de parcelas
                    if (item[0] == quantia.ToString() && item[3] == modalidade.ToString() && item[2] == parcelas.ToString())
                    {
                        this.Transações.RemoveAt(posição);
                        break;
                    }
                }

                posição++;
            }
        } // Remove gasto da listaItens de trasações

        private bool Gerenciador(double quantia)
        {
            double contador = 0;

            // Somando todos item de Transações
            foreach (var item in this.Transações)
            {
                if (item[1] == 'c'.ToString())
                    contador += Convert.ToDouble(item[0]);
            }

            // Somando os valores do cartão a quantia que o usuario deseja adicionar no cartão
            contador += quantia;
            bool aprovado = false;

            // Retorna True se o limte do cartão foi ultrapassado
            if (contador > this.LimiteCredito)
                aprovado = true;

            return aprovado;
        } // Verifica se pode adicionar o valor ao cartao ou se excede o limite

        public void AbaterParcelas()
        {
            var copiaTransações = new List<List<string>>(this.Transações);
            foreach (var item in copiaTransações)
            {
                // Catando Data da Transação atual e catando a data do dia
                var dataDaTransação = Convert.ToDateTime(item[4]);
                var dataAtual = DateTime.Now;

                // Catando quantidade de parcelas e meses que falta para a conta ser removida
                int meses = dataAtual.Month - dataDaTransação.Month;
                int parcelas = Convert.ToInt32(item[2]);
                
                if (parcelas <= 12)
                {
                    if (meses == 0 || meses == parcelas)
                    {
                        this.Transações.Remove(item);
                    } // Deletando transação
                } // Se o item for parcelado em menos de 12 vezes
                else
                {
                    int parcelaEmAnos = parcelas / 12;
                    int anoAtual = dataAtual.Year - dataDaTransação.Year;
                    if (parcelaEmAnos == anoAtual)
                    {
                        if (meses == 0)
                        {
                            this.Transações.Remove(item);

                        }
                    }
                } // Se o item for parcelado em mais que 12 vezes
            }
        }

        public double Saldo(bool abaterDaConta = false)
        {
            double contador = 0;

            foreach (var item in this.Transações)
            {
                if (item[3] == 'c'.ToString())
                    contador += Convert.ToDouble(item[0]);
            } // Somando Trasações

            foreach (var item in this.GastosFixos)
            {
                contador += Convert.ToDouble(item[0]);
            } // Somando Gastos fixo

            if (abaterDaConta)
            {
                contador -= ValorNaConta;
                if (contador < 0)
                {
                    this.ValorNaConta = contador * -1;
                    contador = 0;
                }
                else
                {
                    this.ValorNaConta = 0;
                }
            } // Subtraindo de contador

            return contador;
        }
    }
}
