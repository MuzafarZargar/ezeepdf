namespace EzeePdf.Core.Repositories
{
    public class EzeePdfSqlRepository(string connectionString) :
        SqlRepository(connectionString), IEzeePdfSqlRepository
    {
    }
}
