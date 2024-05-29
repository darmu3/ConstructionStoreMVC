using LinqToDB.Mapping;
using System.ComponentModel.DataAnnotations;

namespace applicationmvc.Models
{
    [Table(Name = "Store")]
    public class Store
    {
        [PrimaryKey, Identity]
        public int StoreId { get; set; }

        [Column]
        [Display(Name = "Название")]
        public string StoreName { get; set; }

        [Column]
        [Display(Name = "Адрес")]
        public string StoreAddress { get; set; }

        [LinqToDB.Mapping.Association(ThisKey = nameof(StoreId), OtherKey = nameof(Models.Order.StoreId))]
        public List<Order> Orders { get; set; }
    }
}
