using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TonyM.Models
{
    public class GraphicCard
    {
        public string Name { get; set; }

        public string Skuname { get; set; }

        public string LightName
        {
            get
            {
                return Name.Replace("NVIDIA RTX ", "").Replace(" ", "%20");
            }
        }
        public string Link
        {
            get
            {
                return "https://api.store.nvidia.com/partner/v1/feinventory?skus=" + Skuname + " &locale=FR";
            }
        }
        public bool UserWanted = true;

        public GraphicCard(string name, string skuname)
        {
            this.Name = name;
            this.Skuname = skuname;
        }
    }
}
