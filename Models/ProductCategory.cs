using LinqToDB.Mapping;
using System.ComponentModel.DataAnnotations;


namespace applicationmvc.Models
{
    [Table(Name = "ProductCategory")]
    public class ProductCategory
    {
        [PrimaryKey, Identity]
        public int ProductCategoryId { get; set; }

        [Column]
        [Display(Name = "Название")]
        public string CategoryName { get; set; }

        [LinqToDB.Mapping.Association(ThisKey = nameof(ProductCategoryId), OtherKey = nameof(Models.Product.ProductCategoryId))]
        public List<Product> Products { get; set; }
    }
}
