namespace applicationmvc.Models
{
    public class OrdersViewModel
    {
        public List<Order> AllOrders { get; set; }
        public List<Order> MyOrders { get; set; }
        public bool IsEmployeeOrAdmin { get; set; }
    }
}
