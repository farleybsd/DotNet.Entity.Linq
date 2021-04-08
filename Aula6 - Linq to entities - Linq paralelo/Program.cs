using AluraTunesData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;

namespace Aula6___Linq_to_entities___Linq_paralelo
{
    class Program
    {
        private const string Imagens = "Imagens";

        static void Main(string[] args)
        {
            // pacotes gerar QrCode
            //Install-Package ZXing.Net
            // Adcionar nas referencias system.Drawing
            var barcodWriter = new BarcodeWriter();
            barcodWriter.Format = BarcodeFormat.QR_CODE;
            barcodWriter.Options = new ZXing.Common.EncodingOptions
            {
                Width=100,
                Height=100
            };

            if (!Directory.Exists(Imagens))
                    Directory.CreateDirectory(Imagens);


            using (var contexto = new AluraTunesEntities())
            {
                var queryfaixas=
                            from f in contexto.Faixas
                            select f;

                var listaFaixas = queryfaixas.ToList();

                //Cronometro
                Stopwatch stopwatch= Stopwatch.StartNew();

                var queryCodigos =
                                listaFaixas
                                .AsParallel()//usar todos os nucleos do pc
                                .Select(f => new
                                {
                                    Arquivo = string.Format("{0}\\{1}.jpg",Imagens,f.FaixaId),
                                    Imagem =barcodWriter.Write(string.Format("aluratunes.com/faixa/{0}",f.FaixaId))
                                });

              int contagem =  queryCodigos.Count();

                stopwatch.Stop();

                Console.WriteLine("Codigos Gerados: {0} em {1} segundos.",contagem,stopwatch.ElapsedMilliseconds / 1000.0);

                stopwatch = Stopwatch.StartNew();

                //foreach (var item in queryCodigos)
                //{
                //    item.Imagem.Save(item.Arquivo, ImageFormat.Jpeg);
                //}

                queryCodigos.ForAll(item =>item.Imagem.Save(item.Arquivo, ImageFormat.Jpeg)); // distribui em varias trades

                stopwatch.Stop();
                Console.WriteLine("Codigos Salvos em arquivos: {0} em {1} segundos.", contagem, stopwatch.ElapsedMilliseconds / 1000.0);
            }

            Console.ReadKey();
        }
    }
}
