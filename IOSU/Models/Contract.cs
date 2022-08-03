using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IOSU.Models
{
    public class Contract
    {
        public int Id { get; set; }

        [DisplayName("ФИО менеджера")]
        public string ManagerPassportNumber { get; set; }
        [DisplayName("ФИО менеджера")]
        public Manager Manager { get; set; }

        [DisplayName("Название организации")]
        public int ClientId { get; set; }
        [DisplayName("Название организации")]
        public Client Client { get; set; }

        [DisplayName("Наименование продукта")]
        public int ProductId { get; set; }
        [DisplayName("Наименование продукта")]
        public Product Product { get; set; }
        

        [DisplayName("Количество товара")]
        public int AmountOfProduct { get; set; }
        [DisplayName("Дата контракта")]
        public DateTime Date { get; set; }
        
    }
}
