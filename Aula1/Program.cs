using AluraTunesData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aula1
{
    class Program
    {
        private const int TAMANHO_PAGINA = 10;
        private const  int numeroPagina= 3;

        static void Main(string[] args)
        {
            using (var contexto = new AluraTunesEntities())
            {
              var numeroNotasFiscais= contexto.NotasFiscais.Count();
              var numeroPaginas= Math.Ceiling((decimal)numeroNotasFiscais / TAMANHO_PAGINA);

                for (var pagina = 1; pagina <= numeroPaginas; pagina++)
                {
                    ImprimirPagina(TAMANHO_PAGINA,contexto,pagina);
                }

                Console.ReadKey();

            }
        }

        private static void ImprimirPagina(int tAMANHO_PAGINA, AluraTunesEntities contexto, int pagina)
        {
            var query = from nf in contexto.NotasFiscais
                        orderby nf.NotaFiscalId
                        select new
                        {
                            Numero = nf.NotaFiscalId,
                            Data = nf.DataNotaFiscal,
                            Cliente = nf.Cliente.PrimeiroNome + " " + nf.Cliente.Sobrenome,
                            Total = nf.Total
                        };

            var tamanhoDoPulo = (pagina - 1) * TAMANHO_PAGINA;

            query = query.Skip(tamanhoDoPulo);

            query = query.Take(TAMANHO_PAGINA);

            Console.WriteLine();
            Console.WriteLine("Página: {0}", pagina);
            Console.WriteLine();

            foreach (var nf in query)
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}", nf.Numero, nf.Data, nf.Cliente, nf.Total);
            }
        }

    }

}
