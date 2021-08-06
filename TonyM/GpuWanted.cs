using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TonyM
{
    class GpuWanted
    {
        public string Name { get; init; }
        public string ApiLink { get; init; }
        public bool Status { get; set; }


        public GpuWanted(string name, string apiLink, bool status)
        {
            this.Name = name;
            this.ApiLink = apiLink;
            this.Status = status;
        }

    }
}
