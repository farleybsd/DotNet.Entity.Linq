using AluraTunesData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aula4__Produto_Mais_Vendido
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var contexto = new AluraTunesEntities())
            {
                var faixasQuery = from f in contexto.Faixas
                                  where f.ItemNotaFiscals.Count() > 0
                                  let TotalVendas = f.ItemNotaFiscals.Sum(inf => inf.Quantidade * inf.PrecoUnitario)
                                  orderby TotalVendas descending
                                  select new 
                                  {
                                    f.FaixaId,
                                    f.Nome,
                                    Total = TotalVendas
                                  };

                var produtoMaisVendido = faixasQuery.First();

                Console.WriteLine("{0}\t{1}\t{2}", produtoMaisVendido.FaixaId, produtoMaisVendido.Nome, produtoMaisVendido.Total);

                var query = from inf in contexto.ItemsNotaFiscal
                            where inf.FaixaId == produtoMaisVendido.FaixaId
                            select new
                            { 
                                NomeCliente = inf.NotaFiscal.Cliente.PrimeiroNome + "-" + inf.NotaFiscal.Cliente.Sobrenome
                            };

                foreach (var cliente in query)
                {
                    Console.WriteLine(cliente.NomeCliente);
                }

                Console.ReadKey();
            }
        }
    }
}
