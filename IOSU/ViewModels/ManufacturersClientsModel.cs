using IOSU.Models;
using System.ComponentModel;

namespace IOSU.ViewModels
{
    public class ManufacturersClientsModel
    {
        [DisplayName("Код производителя")]
        public int? ManufacturerId { get; set; }
        [DisplayName("Название")]
        public string ManufacturerName { get; set; }
        [DisplayName("Сертификация товара")]
        public bool? Certification { get; set; }
        [DisplayName("Год создания")]
        public DateTime? CreationData { get; set; }


        [DisplayName("Код заказчика")]
        public int? ClientId { get; set; }
        [DisplayName("Название организации")]
        public string ClientName { get; set; }
        [DisplayName("Скидка 10%")]
        public bool? Discount { get; set; }
    }
}
