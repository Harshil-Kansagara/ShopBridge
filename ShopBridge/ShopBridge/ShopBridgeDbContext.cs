using Microsoft.EntityFrameworkCore;
using ShopBridge.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge
{
    public class ShopBridgeDbContext : DbContext
    {
        #region Constructor
        public ShopBridgeDbContext(DbContextOptions<ShopBridgeDbContext> options) : base(options)
        {

        }
        #endregion

        public DbSet<Product> Product { get; set; }
    }
}
