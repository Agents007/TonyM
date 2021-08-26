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
        public List<string> Links
        {
            get
            {
                return GenerateLink();
            }
        }
        public bool UsrWanted = true;

        public GraphicCard(string name, string skuname)
        {
            this.Name = name;
            this.Skuname = skuname;
        }

        public List<string> GenerateLink()
        {
            List<string> links = new();
            string fUrl = "https://api.nvidia.partners/edge/product/search?page=1&limit=9&locale=fr-fr&category=GPU&gpu=";
            string sUrl = "https://api.store.nvidia.com/partner/v1/feinventory?skus=";
            links.Add(fUrl + LightName);
            links.Add(sUrl + Skuname + "&locale=FR");
            return links;
        }


    }
}
