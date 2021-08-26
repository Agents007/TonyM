using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TonyM.Models
{
    public class SecondCall
    {
        public bool success { get; set; }
        public List<ListMap> listMap { get; set; }

        public bool GetGpu()
        {
            bool inStock = listMap.First().SearchStock();
            return inStock;
        }
    }
}
