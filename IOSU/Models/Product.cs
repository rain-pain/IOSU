using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IOSU.Models
{
    public class Product
    {
        [Key]
        [DisplayName("Код ассортимента")]
        public int Id { get; set; }
        [DisplayName("Наименование")]
        public string Name { get; set; }
        [DisplayName("Количество")]
        [Range(0, 3000, ErrorMessage = "Max value 3000")]
        public int Amount { get; set; }
        [DisplayName("Эксплуатационный срок (дней)")]
        public float Exploitation { get; set; }
        [DisplayName("Дата поступления на склад")]
        public DateTime EmDate { get; set; }

        [DisplayName("Производитель")]
        public int ManufacturerId { get; set; }
        [DisplayName("Производитель")]
        public Manufacturer Manufacturer { get; set; }
        public ICollection<Contract> Contracts { get; set; }
    }   
}
