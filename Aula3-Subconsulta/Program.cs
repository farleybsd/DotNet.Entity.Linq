using AluraTunesData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aula3_Subconsulta
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var contexto = new AluraTunesEntities())
            {
                var querymedia = contexto.NotasFiscais.Average(n => n.Total);

                var query = from nf in contexto.NotasFiscais
                            where nf.Total > querymedia //subconsulta
                            orderby nf.Total descending
                            select new 
                            { 
                                Numero = nf.NotaFiscalId,
                                Data = nf.DataNotaFiscal,
                                Cliente = nf.Cliente.PrimeiroNome + "" + nf.Cliente.Sobrenome,
                                Valor = nf.Total
                            };

                foreach (var notafiscal in query)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}",notafiscal.Numero,notafiscal.Data,notafiscal.Cliente,notafiscal.Valor);
                }

                Console.WriteLine("A média é {0}",querymedia);
            }

            Console.ReadKey();
        }
    }
}
