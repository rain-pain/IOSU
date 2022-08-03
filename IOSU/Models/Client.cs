using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IOSU.Models
{
    public class Client
    {
        [Key]
        [DisplayName("Код заказчика")]
        public int Id { get; set; }
        [DisplayName("Название организации")]
        public string Name { get; set; }
        [DisplayName("Скидка 10%")]
        public bool Discount { get; set; }

        public ICollection<Contract> Contracts { get; set; }
    }
}
