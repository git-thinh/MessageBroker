using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuesPechkin;

namespace MessageBroker
{
    public class PdfCreator
    {
        private static IConverter converter =
            new StandardConverter(
                new PdfToolset(
                    new Win64EmbeddedDeployment(
                        new TempFolderDeployment())));
        public static bool export(string file, string url)
        {
            //const string url = "http://localhost:9096/pdf/giay-yeu-cau-bao-hiem.html";
            HtmlToPdfDocument Document = new HtmlToPdfDocument() { Objects = { new ObjectSettings { PageUrl = url } } };

            byte[] buf = null;

            try
            {
                buf = converter.Convert(Document);
                Console.WriteLine("All conversions done");

                if (buf == null)
                {
                    Console.WriteLine("No exceptions were raised but wkhtmltopdf failed to convert. => Error Converting");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occurred1: " + ex.Message);
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
                //Console.WriteLine("OK >>> " + fn);
                return true;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Exception Occurred2: " + ex.Message);
                return false;
            }
        }
    }
}
