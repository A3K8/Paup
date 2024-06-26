using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pp.Models
{
    public class ProizvodiDB
    {
        private static List<Proizvodi> lista;
        private static bool listaInicijalizirana;

        public ProizvodiDB()
        {
            if (!listaInicijalizirana)
            {
                lista = new List<Proizvodi>
            {
                new Proizvodi { Id = 1, Vrsta = "Prozor-jednokrilni", Materijal = "PVC", Cijena = 332 },
                new Proizvodi { Id = 2, Vrsta = "Prozor-dvokrilni", Materijal = "PVC", Cijena = 450 },
                new Proizvodi { Id = 3, Vrsta = "Prozor-trokrilni", Materijal = "PVC", Cijena = 510 },
                new Proizvodi { Id = 4, Vrsta = "Vrata-bijela", Materijal = "PVC", Cijena = 1725 },
                new Proizvodi { Id = 5, Vrsta = "Vrata-crna", Materijal = "PVC", Cijena = 1810 },
                new Proizvodi { Id = 6, Vrsta = "Prozor-jednokrilni", Materijal = "ALU", Cijena = 325 },
                new Proizvodi { Id = 7, Vrsta = "Prozor-dvokrilni", Materijal = "ALU", Cijena = 432 },
                new Proizvodi { Id = 8, Vrsta = "Prozor-trokrilni", Materijal = "ALU", Cijena = 505 },
                new Proizvodi { Id = 9, Vrsta = "Vrata-bijela", Materijal = "ALU", Cijena = 3640 },
                new Proizvodi { Id = 10, Vrsta = "Vrata-crna", Materijal = "ALU", Cijena = 3730 }

                };
                listaInicijalizirana = true;
            
            }

        }

        public List<Proizvodi> VratiListu()
        {
            return lista;
        }

        public void AzurirajProizvod(Proizvodi proizvodi)
        {
            int proizvodiIndex = lista.FindIndex(x => x.Id == proizvodi.Id);
            if (proizvodiIndex >= 0)
            {
                lista[proizvodiIndex] = proizvodi;
            }
        }
    }
}
