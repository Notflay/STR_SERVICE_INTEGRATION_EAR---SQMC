using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.SL
{
    public interface ISBODAO
    {
        int add(object obj);
        int update(object obj);
        object readAll();
        object read(string id);

    }
}
