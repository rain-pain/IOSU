using System.ComponentModel;

namespace IOSU.Models
{
    public class Manufacturer
    {
        [DisplayName("Код производителя")]
        public int Id { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("Сертификация товара")]
        public bool Certification { get; set; }
        [DisplayName("Год создания")]
        public DateTime CreationData { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}
