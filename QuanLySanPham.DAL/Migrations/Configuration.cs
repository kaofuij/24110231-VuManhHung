using System.Data.Entity.Migrations;
using QuanLySanPham.DAL;

namespace QuanLySanPham.DAL.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<QuanLySanPhamDbContext>
    {
        public Configuration()
        {
            // Bật tự động Migration để EF tự động đồng bộ CSDL với Entity khi chạy
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "QuanLySanPham.DAL.QuanLySanPhamDbContext";
        }

        protected override void Seed(QuanLySanPhamDbContext context)
        {
            // Nạp dữ liệu mẫu tự động vào CSDL
            QuanLySanPhamDatabaseInitializer.SeedData(context);
        }
    }
}
