using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IOSU.Models
{
    public class Manager
    {
        [Key]
        [DisplayName("Номер паспорта")]
        public string PassportNumber { get; set; }
        [DisplayName("Имя")]
        public string FullName  { get; set; }
        [DisplayName("Заработная плата (руб)")]
        [Range(0, 5000.00, ErrorMessage = "Max value 5000")]
        public decimal Wage { get; set; }
        [DisplayName("Дата рождения")]
        public DateTime BirthDate { get; set; }
        [DisplayName("Номер телефона")]
        public string PhoneNumber { get; set; }

        public ICollection<Contract> Contracts { get; set; }

    }
}
