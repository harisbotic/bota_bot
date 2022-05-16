using Microsoft.EntityFrameworkCore;

class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public string DbPath { get; }

    public DbContext(DbContextOptions<DbContext> options)
        : base(options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "database.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}
