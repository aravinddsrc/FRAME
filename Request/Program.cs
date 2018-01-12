using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RequestAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //List<ColumnName> list = ExcelValues.column();

                var num=Console.ReadLine();
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                List<Uri> uris = new List<Uri>();
                uris.Add(new Uri("https://stackapps.000webhostapp.com/Send.php?uid=8148681001&&pwd=5323&&phone="+ num + "&&msg=TATA"));
                uris.Add(new Uri("https://stackapps.000webhostapp.com/Send.php?uid=8148681002&&pwd=5323&&phone=8148681001&&msg=JIO"));
                uris.Add(new Uri("https://stackapps.000webhostapp.com/Send.php?uid=8148681002&&pwd=5323&&phone=8148681001&&msg=JIO"));
                uris.Add(new Uri("https://stackapps.000webhostapp.com/Send.php?uid=8148681002&&pwd=5323&&phone=8148681001&&msg=JIO"));
                uris.Add(new Uri("https://stackapps.000webhostapp.com/Send.php?uid=8148681002&&pwd=5323&&phone=8148681001&&msg=JIO"));
                uris.Add(new Uri("https://stackapps.000webhostapp.com/Send.php?uid=8148681002&&pwd=5323&&phone=8148681001&&msg=JIO"));
                uris.Add(new Uri("https://stackapps.000webhostapp.com/Send.php?uid=8148681002&&pwd=5323&&phone=8148681001&&msg=JIO"));
                uris.Add(new Uri("https://stackapps.000webhostapp.com/Send.php?uid=8148681002&&pwd=5323&&phone=8148681001&&msg=JIO"));
                uris.Add(new Uri("https://stackapps.000webhostapp.com/Send.php?uid=8148681002&&pwd=5323&&phone=8148681001&&msg=JIO"));
                uris.Add(new Uri("https://stackapps.000webhostapp.com/Send.php?uid=8148681002&&pwd=5323&&phone=8148681001&&msg=JIO"));
                uris.Add(new Uri("https://stackapps.000webhostapp.com/Send.php?uid=8148681002&&pwd=5323&&phone=8148681001&&msg=JIO"));
                uris.Add(new Uri("https://stackapps.000webhostapp.com/Send.php?uid=8148681002&&pwd=5323&&phone=8148681001&&msg=JIO"));
                uris.Add(new Uri("https://stackapps.000webhostapp.com/Send.php?uid=8148681002&&pwd=5323&&phone=8148681001&&msg=JIO"));




                foreach (var url in uris)
                {

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    try
                    {
                        WebResponse response = request.GetResponse();
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                            var r = reader.ReadToEnd();
                            Console.WriteLine(url);
                        }
                    }
                    catch (WebException ex)
                    {
                        WebResponse errorResponse = ex.Response;
                        using (Stream responseStream = errorResponse.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                            String errorText = reader.ReadToEnd();
                            // log errorText
                            Console.WriteLine(errorText);
                        }
                        throw;
                    }
                }

                stopwatch.Stop();
                Console.WriteLine("Time elapsed: {0:hh\\:mm\\:ss}", stopwatch.Elapsed);


                Console.ReadLine();
            }
            catch (AggregateException exc)
            {
                exc.InnerExceptions.ToList().ForEach(e =>
                {
                    Console.WriteLine(e.Message);
                });
            }
        }
    }
}


