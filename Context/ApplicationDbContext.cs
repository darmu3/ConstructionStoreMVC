using LinqToDB;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using applicationmvc.Models;

namespace applicationmvc.Context
{
    public class ApplicationDbContext : DataConnection
    {
        public ApplicationDbContext(DataOptions options) : base(options)
        {
        }

        public ITable<Supplier> Supplier => this.GetTable<Supplier>();
        public ITable<Store> Store => this.GetTable<Store>();
        public ITable<Product> Product => this.GetTable<Product>();
        public ITable<ProductCategory> ProductCategory => this.GetTable<ProductCategory>();
        public ITable<Order> Order => this.GetTable<Order>();
        public ITable<ProductOrder> ProductOrder => this.GetTable<ProductOrder>();
        public ITable<User> Users => this.GetTable<User>();
        public ITable<Role> Roles => this.GetTable<Role>();
    }
}
