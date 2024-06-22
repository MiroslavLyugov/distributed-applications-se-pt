using TextBoard.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace TextBoard.DataMock.Contexts;

public class TextBoardDbContextMock : TextBoardDbContext
{
    private SqliteConnection? dbConn;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        this.dbConn = new SqliteConnection("Data Source=:memory:");
        this.dbConn.Open(); // Preserve in-memory database until end of xUnit test suite.
        optionsBuilder.UseSqlite(this.dbConn);
    }

    public override void Dispose()
    {
        base.Dispose();
        if (this.dbConn != null)
        {
            this.dbConn.Dispose();
            this.dbConn = null;
        }
    }
}
