using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pp.Models
{
    [Table("proizvodi")]
    public class Proizvodi
    {

        [Column("id")]
        [Key]
        [Display(Name = "ID proizvoda")]
        public int Id { get; set; }

        [Column("vrsta")]
        [Display(Name = "Vrsta")]
        [Required(ErrorMessage = "{0} je obavezno")]
        public string Vrsta { get; set; }

        [Column("materijal")]
        [Display(Name = "Materijal")]
        [Required(ErrorMessage = "{0} je obavezno")]
        public string Materijal { get; set; }

        [Column("cijena")]
        [Display(Name = "Cijena")]
        [Required(ErrorMessage = "{0} je obavezno")]

        public int Cijena { get; set; }

    }
}
    