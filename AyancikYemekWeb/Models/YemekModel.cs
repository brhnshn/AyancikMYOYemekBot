using System.ComponentModel.DataAnnotations;

namespace AyancikYemekWeb.Models
{
    public class YemekModel
    {
        [Key]
        public int Id { get; set; }

        public string Tarih { get; set; } = string.Empty;
        public string Corba { get; set; } = string.Empty;
        public string AnaYemek { get; set; } = string.Empty;
        public string YardimciYemek { get; set; } = string.Empty;
        public string Tatli { get; set; } = string.Empty;

        public bool MenüVarMi { get; set; } = false;

        public override string ToString()
        {
            if (!MenüVarMi)
                return "📅 *Bugün yemekhanede menü bulunamadı veya tatil.*";

            return $"📅 *{Tarih} - Yemek Menüsü*\n\n" +
                   $"🍜 *Çorba:* {Corba}\n" +
                   $"🍗 *Ana Yemek:* {AnaYemek}\n" +
                   $"🍚 *Yan:* {YardimciYemek}\n" +
                   $"🍰 *Tatlı:* {Tatli}\n\n" +
                   "Afiyet olsun! 😋";
        }
    }
}