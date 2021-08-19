using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TonyM.Models;

namespace TonyM.Modules
{
    class UserSelection
    {
        public static List<GraphicsCard> getUserSelection(List<GraphicsCard> gpusAvailable)
        {
            gpusAvailable = gpusAvailable.OrderBy(g => g.DisplayName).ToList();

            for (int i = 0; i < gpusAvailable.Count; i++)
            {
                Console.WriteLine("Choix " + (i + 1) + " : " + gpusAvailable[i].DisplayName);
            }
            Console.WriteLine("Choix 10 : Sélection terminée\n");

            while (true)
            {
                Console.Write("Votre choix : ");
                string choice = Console.ReadLine();

                try
                {
                    int choiceInt = int.Parse(choice);

                    if ((choiceInt > 0) && (choiceInt < (gpusAvailable.Count + 1)))
                    {
                        var gpu = gpusAvailable[choiceInt - 1];
                        if (gpu.Wanted == true)
                        {
                            Console.WriteLine("Ce GPU fait déjà parti de votre sélection\n");
                        }
                        else
                        {
                            gpu.Wanted = true;
                            Console.WriteLine(gpu.DisplayName + " ajoutée à la liste. Ajouter une autre carte ou terminer la sélection.\n");
                        }
                    }
                    else if ((choiceInt == 10) && (gpusAvailable.Any(g => g.Wanted == true)))
                    {
                        List<GraphicsCard> gpusUser = gpusAvailable.Where(g => g.Wanted).ToList();
                        return gpusUser;   
                    }
                    else
                    {
                        Console.WriteLine("Votre choix n'est pas valide\n");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Erreur : Vous devez entrer un nombre\n");
                }
            }
        }
    }
}
