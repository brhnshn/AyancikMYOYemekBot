using System.ComponentModel.DataAnnotations;

namespace AyancikYemekWeb.Data
{
    public class Abone
    {
        [Key]
        public int Id { get; set; }
        public long ChatId { get; set; } 
        public DateTime KayitTarihi { get; set; } = DateTime.Now;
        public bool AktifMi { get; set; } = true; 
    }
}