using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication_Benzeine
{
    public partial  class Startup
    {
        private string GetConnectionString()
        {
            string conString = default;
#if DEBUG
            conString = Configuration.GetSection("ConnectionStrings:Debug").Value;
#elif RELEASE
            conString = Configuration.GetSection("ConnectionStrings:Production").Value;
#endif
            return conString;
        }
    }
}
