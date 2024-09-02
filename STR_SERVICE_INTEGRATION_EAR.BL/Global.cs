using STR_SERVICE_INTEGRATION_EAR.EL.Commons;
using STR_SERVICE_INTEGRATION_EAR.SQ;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.BL
{
    public class Global
    {
        public static ConsultationResponse<T> ReturnError<T>(Exception ex)
        {
            return new ConsultationResponse<T>
            {
                CodRespuesta = "99",
                DescRespuesta = ex.Message,

            };
        }

        public static ConsultationResponse<T> ReturnOk<T>(List<T> list, string respIncorrect)
        {
            string respOk = "OK";
            return new ConsultationResponse<T>
            {
                CodRespuesta = list.Count() > 0 ? "00" : "22",
                DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                Result = list
            };
        }
        public static void WriteToFile(string Message)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filepath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Logs\\Service_Creation_Log_{DateTime.Now.Date.ToShortDateString().Replace('/', '_')}.txt";
                if (!File.Exists(filepath))
                {
                    using (StreamWriter sw = File.CreateText(filepath))
                    {
                        sw.WriteLine(DateTime.Now.ToString() + " - " + Message);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(filepath))
                    {
                        sw.WriteLine(DateTime.Now.ToString() + " - " + Message);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
