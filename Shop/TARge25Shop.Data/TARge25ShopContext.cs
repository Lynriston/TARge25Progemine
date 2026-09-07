using Microsoft.EntityFrameworkCore;


namespace TARge25Shop.Data
{
    //nimetasime classi TARge25ShopContext, mis pärib DbContext klassi.
    public class TARge25ShopContext : DbContext
    {
        // Tegime konteksti, mis p'rib DbContext klassi.
        public TARge25ShopContext(DbContextOptions<TARge25ShopContext> options) : base(options) { }

    }
}
