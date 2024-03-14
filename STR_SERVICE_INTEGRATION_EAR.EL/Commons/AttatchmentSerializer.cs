using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL
{
    public class AttatchmentSerializer
    {

        public int AbsoluteEntry { get; set; }
        public List<PathInfo> Attachments2_Lines { get; set; }
    }

    public class PathInfo
    {
        public string SourcePath { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
    }
}
