using System;
using System.Collections.Generic;
using System.Linq;
using TonyM.Models;

namespace TonyM.Modules
{
    public class UserSelection
    {
        public static List<GraphicCard> GetSelection(List<ProductDetail> gpusAvailable)
        {
            gpusAvailable = gpusAvailable.OrderBy(g => g.DisplayName).ToList();

            for (int i = 0; i < gpusAvailable.Count; i++)
            {
                Console.WriteLine("Choix " + (i + 1) + " : " + gpusAvailable[i].DisplayName);
            }
            Console.WriteLine("Choix 10 : Sélection terminée\n");

            List<GraphicCard> gpusUser = new();

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
                        if (gpusUser.Any(g => g.Name == gpu.DisplayName))
                        {
                            Console.WriteLine("Ce GPU fait déjà parti de votre sélection\n");
                        }
                        else
                        {
                            GraphicCard gpuW = new(gpu.DisplayName, gpu.ProductSKU);
                            gpusUser.Add(gpuW);
                            Console.WriteLine(gpu.DisplayName + " ajoutée à la liste. Ajouter une autre carte ou terminer la sélection.\n");
                        }
                    }
                    else if ((choiceInt == 10) && (gpusUser.Count > 0))
                    {
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
