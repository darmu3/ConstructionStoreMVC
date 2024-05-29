using LinqToDB.Mapping;
using System.ComponentModel.DataAnnotations;

namespace applicationmvc.Models
{
    [Table(Name = "Supplier")]
    public class Supplier
    {
        [PrimaryKey, Identity]
        public int SupplierId { get; set; }

        [Column]
        [Display(Name = "Название")]
        public string SupplierName { get; set; }

        [Column]
        [Display(Name = "Адрес")]
        public string SupplierAddress { get; set; }

        [LinqToDB.Mapping.Association(ThisKey = nameof(SupplierId), OtherKey = nameof(Models.Product.SupplierId))]
        public List<Product> Products { get; set; }
    }
}
