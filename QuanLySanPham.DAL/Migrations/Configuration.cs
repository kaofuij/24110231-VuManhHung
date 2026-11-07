using System.Data.Entity.Migrations;
using QuanLySanPham.DAL;

namespace QuanLySanPham.DAL.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<QuanLySanPhamDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(QuanLySanPhamDbContext db)
        {
            QuanLySanPhamDatabaseInitializer.SeedData(db);
        }
    }
}
