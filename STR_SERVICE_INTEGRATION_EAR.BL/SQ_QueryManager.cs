using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace STR_SERVICE_INTEGRATION_EAR.BL
{
    public class SQ_QueryManager
    {
        public static string Generar(string sql)
        {
            var xdoc = new XDocument();
            using (StringReader s = new StringReader(Properties.Resources.Queries))
                xdoc = XDocument.Load(s);

            XElement xQRY = xdoc.
                Descendants("query").
                FirstOrDefault(s => s.Attribute("nameid").Value.Equals(sql));

            if (xQRY != null)
            {
                switch (xQRY.Attribute("definition").Value)
                {
                    case "I":
                        return xQRY.Element("hana").Value;

                    case "P":
                        string query = xQRY.Element("sql").Value;

                        string parms = "";
                        if (xQRY.Element("params") != null)
                            parms = xQRY.Element("params").Value;

                        string llamada = ("CALL " + query + " (" + parms + ")").Trim();
                        return ("CALL " + query + " (" + parms + ")").Trim();

                }

                return string.Empty;
            }
            return string.Empty;
        }
    }
}

