namespace TonyM.Models
{
    public class GraphicCard
    {
        public string Name { get; set; }
        public string Skuname { get; set; }
        public string ShortName
        {
            get
            {
                return Name.Replace("NVIDIA RTX ", "");
            }
        }
        public bool Wanted = true;
        public string Link
        {
            get
            {
                return "https://api.store.nvidia.com/partner/v1/feinventory?skus=" + Skuname + "&locale=FR";
            }
        }

        public GraphicCard(string name, string skuname)
        {
            this.Name = name;
            this.Skuname = skuname;
        }
    }
}
