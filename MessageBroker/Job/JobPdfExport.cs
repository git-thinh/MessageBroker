using MessageShared;
using System;
using System.IO;
using System.Reflection;
using TuesPechkin;

namespace MessageBroker
{
    public class JobPdfExport : JobBase
    {
        public override void freeResource() { pdfStop(); }

        ////////////////////////////////////////////////////////////////////
        /// 

        private static bool _inited = false;
        private readonly mDbUpdateRequest _requestUpdateDb;
        public JobPdfExport(mDbUpdateRequest requestUpdateDb = null)
        {
            _requestUpdateDb = requestUpdateDb;
        }

        ////////////////////////////////////////////////////////////////////
        /// 

        static void pdfStart()
        {
            test_01();
        }

        static void pdfStop()
        {
        }

        ////////////////////////////////////////////////////////////////////
        /// 

        public override void execute()
        {
            if (!_inited)
            {
                _inited = true;
                pdfStart();
                return;
            }

            if (_requestUpdateDb == null) return;
        }

        ////////////////////////////////////////////////////////////////////
        //

        private static IConverter converter =
            new StandardConverter(
                new PdfToolset(
                    new Win64EmbeddedDeployment(
                        new TempFolderDeployment())));

        static void test_01()
        {
            const string url = "http://localhost:9096/pdf/giay-yeu-cau-bao-hiem.html";
            HtmlToPdfDocument Document = new HtmlToPdfDocument() { Objects = { new ObjectSettings { PageUrl = url } } };

            byte[] buf = null;

            try
            {
                buf = converter.Convert(Document);
                Console.WriteLine("All conversions done");

                if (buf == null)
                {
                    Console.WriteLine( "No exceptions were raised but wkhtmltopdf failed to convert. => Error Converting");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occurred1: " + ex.Message);
            }

            try
            {
                string fn = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "test.pdf");

                FileStream fs = new FileStream(fn, FileMode.Create);
                fs.Write(buf, 0, buf.Length);
                fs.Close();

                //Process myProcess = new Process();
                //myProcess.StartInfo.FileName = fn;
                //myProcess.Start();
                Console.WriteLine("OK >>> " + fn);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occurred2: " + ex.Message);
            }
        }
    }
}
