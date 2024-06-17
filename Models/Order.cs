using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace applicationmvc.Models
{
    [Table(Name = "Order")]
    public class Order
    {
        [PrimaryKey, Identity]
        public int OrderId { get; set; }

        [Column]
        [Display(Name = "Номер")]
        public string OrderNumber { get; set; }

        [Column]
        [Display(Name = "Дата")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime OrderDate { get; set; }

        [Column]
        [Display(Name = "Сумма")]
        public float OrderSum { get; set; }

        [Column]
        [Display(Name = "Магазин для доставки")]
        public int StoreId { get; set; }

        [LinqToDB.Mapping.Association(ThisKey = nameof(StoreId), OtherKey = nameof(Models.Store.StoreId), CanBeNull = true)]
        [Display(Name = "Магазин для доставки")]
        public Store Store { get; set; }

        [LinqToDB.Mapping.Association(ThisKey = nameof(OrderId), OtherKey = nameof(Models.ProductOrder.OrderId))]
        public List<ProductOrder> ProductOrders { get; set; }

        [Column]
        [Display(Name = "Пользователь")]
        public int UserId { get; set; }

        [LinqToDB.Mapping.Association(ThisKey = nameof(UserId), OtherKey = nameof(Models.User.UserId))]
        public User User { get; set; }
    }
}
