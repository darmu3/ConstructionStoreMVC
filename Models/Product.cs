using LinqToDB.Mapping;
using System.ComponentModel.DataAnnotations;

namespace applicationmvc.Models
{
    [Table(Name = "Product")]
    public class Product
    {
        [PrimaryKey, Identity]
        public int ProductId { get; set; }

        [Column]
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Поле Название обязательно для заполнения.")]
        [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов.")]
        public string Name { get; set; }

        [Column]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Column]
        [Display(Name = "Цена")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше нуля.")]
        public float Price { get; set; }

        [Column]
        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Поле Категория обязательно для заполнения.")]
        public int ProductCategoryId { get; set; }

        [LinqToDB.Mapping.Association(ThisKey = nameof(ProductCategoryId), OtherKey = nameof(ProductCategory.ProductCategoryId), CanBeNull = true)]
        [Display(Name = "Категория")]
        public ProductCategory ProductCategory { get; set; }

        [Column]
        [Display(Name = "Поставщик")]
        [Required(ErrorMessage = "Поле Поставщик обязательно для заполнения.")]
        public int SupplierId { get; set; }

        [LinqToDB.Mapping.Association(ThisKey = nameof(SupplierId), OtherKey = nameof(Supplier.SupplierId), CanBeNull = true)]
        [Display(Name = "Поставщик")]
        public Supplier Supplier { get; set; }

        [LinqToDB.Mapping.Association(ThisKey = nameof(ProductId), OtherKey = nameof(ProductOrder.ProductId))]
        public List<ProductOrder> ProductOrders { get; set; }
    }
}
