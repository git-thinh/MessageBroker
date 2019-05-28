using CacheEngineShared;
using MessageShared;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TuesPechkin;

namespace MessageBroker
{
    public class JobPdfExport : JobBase
    {
        public override void freeResource() { pdf_Stop(); }

        ////////////////////////////////////////////////////////////////////
        /// 

        private static bool _isStop = false;
        private static bool _inited = false;
        private static ConcurrentQueue<string> _urls = new ConcurrentQueue<string>();

        public JobPdfExport(string url = "")
        {
            if (string.IsNullOrWhiteSpace(url)) return;
            _urls.Enqueue(url);
        }

        ////////////////////////////////////////////////////////////////////
        /// 

        static void pdf_Init()
        {
            Task.Factory.StartNew((obj) =>
            {
                IDataflowSubscribers _dataflow = (IDataflowSubscribers)obj;
                while (_isStop == false)
                {
                    if (_urls.Count > 0)
                    {
                        string url;
                        if (_urls.TryDequeue(out url) && !string.IsNullOrEmpty(url))
                        {
                            Console.WriteLine("> EXPORTING -> PDF: " + url);

                            Uri uri = new Uri(url);
                            string Pawn_ID = HttpUtility.ParseQueryString(uri.Query).Get("Pawn_ID"),
                                User_ID = HttpUtility.ParseQueryString(uri.Query).Get("User_ID"),
                                code_temp = uri.Segments[uri.Segments.Length - 1].Substring(7);

                            if (!string.IsNullOrWhiteSpace(Pawn_ID)
                            && !string.IsNullOrWhiteSpace(User_ID)
                            && !string.IsNullOrWhiteSpace(code_temp))
                            {
                                string file = exportPdf(url, code_temp, Pawn_ID), textOutput = "";

                                if (file == null)
                                    textOutput = "#"+ User_ID + ".EXPORT.PDF:FAIL:" + file;
                                else
                                    textOutput = "#"+ User_ID + "EXPORT.PDF:OK:" + file;

                                Console.WriteLine(textOutput);
                                _dataflow.enqueue(new JobClientNotification(textOutput)); 
                            }
                        }
                    }
                    Thread.Sleep(100);
                }
            }, Dataflow);
        }

        static void pdf_Stop()
        {
            _isStop = true;
        }

        ////////////////////////////////////////////////////////////////////
        /// 

        public override void execute()
        {
            if (!_inited)
            {
                _inited = true;
                pdf_Init();
                return;
            }
        }

        ////////////////////////////////////////////////////////////////////
        //

        private static IConverter converter = new StandardConverter(new PdfToolset(new Win64EmbeddedDeployment(new TempFolderDeployment())));

        static string exportPdf(string url, string code_temp, string Pawn_ID)
        {
            if (string.IsNullOrWhiteSpace(Pawn_ID)) return null;

            string path = Path.Combine(Path.GetFullPath("../"), "MessageUI/Exports/");
            if (Directory.Exists(path) == false) Directory.CreateDirectory(path);

            string fileName = code_temp + "." + Pawn_ID + "." + DateTime.Now.ToString("yyyyMMdd-HHmmssfff") + ".pdf";
            string file = Path.Combine(path, fileName);

            //string filePdf = Path.Combine(Path.GetFullPath("../"), "MessageUI/Pdf/Files/" + so_hop_dong.Replace('/', '_') + ".pdf");
            //PdfCreator.export(filePdf, this.Request.RequestUri.ToString());

            //const string url = "http://localhost:9096/pdf/giay-yeu-cau-bao-hiem.html";
            HtmlToPdfDocument Document = new HtmlToPdfDocument() { Objects = { new ObjectSettings { PageUrl = url } } };

            byte[] buf = null;

            try
            {
                buf = converter.Convert(Document);
                //Console.WriteLine("All conversions done");

                if (buf == null)
                {
                    //Console.WriteLine( "No exceptions were raised but wkhtmltopdf failed to convert. => Error Converting");
                    return null;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Exception Occurred1: " + ex.Message);
                return null;
            }

            try
            {
                //string fn = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "test.pdf");


                FileStream fs = new FileStream(file, FileMode.Create);
                fs.Write(buf, 0, buf.Length);
                fs.Close();

                //Process myProcess = new Process();
                //myProcess.StartInfo.FileName = fn;
                //myProcess.Start();
                //Console.WriteLine("OK >>> " + file);
                return fileName;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Exception Occurred2: " + ex.Message);
                return null;
            }
        }
    }
}
