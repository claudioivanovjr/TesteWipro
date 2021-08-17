using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Models;

namespace TesteWipro
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                var caminho = Environment.CurrentDirectory.Split(new String[] { "bin" }, StringSplitOptions.None)[0];

                WebApi.Controllers.ItemApiController api = new WebApi.Controllers.ItemApiController();

                FileInfo file = new FileInfo($@"{caminho}\moedas.json");
                if (!file.Exists)
                {
                    Console.WriteLine("Nenhum arquivo Json encontrado.");
                    Console.ReadKey();
                    return;
                }

                using (StreamReader r = new StreamReader($@"{caminho}\moedas.json"))
                {
                    var json = r.ReadToEnd();

                    //var itens = JsonConvert.DeserializeObject<List<ItemModel>>(json);

                    api.AddItemFila(json);
                }

                var lastRun = DateTime.Now;

                var continuar = true;

                while (continuar)
                {
                    lastRun = DateTime.Now;
                    Console.Clear();
                    Console.WriteLine("Iniciando busca de registro...");
                    var item = api.GetItemFila();

                    if (item == null)
                    {
                        continuar = false;
                        Console.WriteLine("Nenhum registro encontrado na fila.");
                        continue;
                    }

                    //Carregando De-Para de Moeda-Cotacao
                    Console.WriteLine("Verificando registro de-para.");
                    StreamReader sr = new StreamReader($@"{caminho}\de-para.csv");

                    var dePara = new string[2];

                    while (!sr.EndOfStream)
                    {
                        if (sr.ReadLine().Split(';')[0] == item.moeda)
                        {
                            dePara = sr.ReadLine().Split(';');
                            break;
                        }
                    }

                    sr.Close();

                    //Lendo arquivo DadosMoeda.csv
                    Console.WriteLine("Iniciando leitura do arquivo DadosMoeda.csv");
                    sr = new StreamReader($@"{caminho}\DadosMoeda.csv");

                    var listaDados = new List<string[]>();

                    while (!sr.EndOfStream)
                    {
                        var registros = sr.ReadLine().Split(';');
                        if (registros[0] == item.moeda && (DateTime.Parse(registros[1]) >= item.data_inicio && DateTime.Parse(registros[1]) <= item.data_fim))
                        {
                            listaDados.Add(registros);
                        }
                    }

                    sr.Close();

                    //Lendo arquivo de Cotações
                    sr = new StreamReader($@"{caminho}\DadosCotacao.csv");

                    var listaCotacoes = new List<string[]>();

                    while (!sr.EndOfStream)
                    {
                        var registros = sr.ReadLine().Split(';');
                        if (registros[1] == dePara[1] && (DateTime.Parse(registros[2]) >= item.data_inicio && DateTime.Parse(registros[2]) <= item.data_fim))
                        {
                            listaCotacoes.Add(registros);
                        }
                    }

                    sr.Close();

                    //Gerando Arquivo final
                    StreamWriter sw = new StreamWriter($@"{caminho}\Resultado_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv");

                    sw.WriteLine("ID_MOEDA;DATA_COTACAO;VL_COTACAO");

                    foreach (var linha in listaCotacoes)
                    {
                        sw.WriteLine($"{item.moeda};{linha[2]};{linha[0]}");
                    }
                    sw.Close();
                    Console.WriteLine($"Processo finalizado para a moeda {item.moeda}. Arquivo Resultado_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv gerado com sucesso.");
                    Console.WriteLine($"Tempo de execução: {DateTime.Now - lastRun}");
                    Console.WriteLine("Aguardando a próxima execução...");

                    //Timer de 2min
                    Thread.Sleep(120000);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Falha inesperada no processo.");
                Console.WriteLine($"Erro: {ex.Message}");
            }
            Console.ReadKey();
        }
    }
}
