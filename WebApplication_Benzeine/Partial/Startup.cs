using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication_Benzeine
{
    
    public partial  class Startup
    {
        /// <summary>
        /// Returns connectinon string depending on application mode (debug/release)
        /// Debug mode connection string pertain to Local DB when Release mode is for remote DB
        /// </summary>
        /// <returns></returns>
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
