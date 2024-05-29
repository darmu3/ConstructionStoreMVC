using LinqToDB.Mapping;

namespace applicationmvc.Models
{
    [Table(Name = "ProductOrder")]
    public class ProductOrder
    {
        [PrimaryKey, Identity]
        public int ProductOrderId { get; set; }

        [Column]
        public int? ProductId { get; set; }

        [Association(ThisKey = nameof(ProductId), OtherKey = nameof(Models.Product.ProductId))]
        public Product Product { get; set; }

        [Column]
        public int? OrderId { get; set; }

        [Association(ThisKey = nameof(OrderId), OtherKey = nameof(Models.Order.OrderId))]
        public Order Order { get; set; }
    }
}
