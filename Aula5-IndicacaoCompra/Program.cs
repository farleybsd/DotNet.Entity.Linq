using AluraTunesData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aula5_IndicacaoCompra
{
    class Program
    {
        static void Main(string[] args)
        {
            var nomeDaMusica = "Smells Like Teen Spirit";

            using (var contexto = new AluraTunesEntities())
            {
                // Analise de Afinidade com auto relacionamento
            var faixaIds =  contexto.Faixas.Where(f => f.Nome == nomeDaMusica)
                                            .Select(f=> f.FaixaId);

                var query = from comprouIten in contexto.ItemsNotaFiscal
                            join comprouTambem in contexto.ItemsNotaFiscal
                                  on comprouIten.NotaFiscal equals comprouTambem.NotaFiscal // self join
                            where faixaIds.Contains(comprouIten.FaixaId)
                                 && comprouIten.FaixaId != comprouTambem.FaixaId
                            select comprouTambem;

                foreach (var item in query)
                {
                    Console.WriteLine("{0}\t{1}",item.NotaFiscalId,item.Faixa.Nome);
                }
             
            }

            Console.ReadKey();
        }
    }
}
