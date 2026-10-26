using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TreeYield.Core.Models;

namespace TreeYield.Core.Data
{
    /// <summary>Reads the Categories table.</summary>
    public class CategoryRepository : RepositoryBase
    {
        /// <summary>Creates the repository.</summary>
        public CategoryRepository(SqlConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }

        /// <summary>Returns all categories in ID order.</summary>
        public Task<List<Category>> GetAllAsync()
        {
            return WithConnectionAsync("load the categories", async connection =>
            {
                var categories = new List<Category>();
                using (SqlCommand command = CreateCommand(connection,
                    "SELECT CategoryId, Name, Description FROM dbo.Categories ORDER BY CategoryId;"))
                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        categories.Add(new Category
                        {
                            CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                            Name = ReadString(reader, "Name"),
                            Description = ReadString(reader, "Description")
                        });
                    }
                }

                return categories;
            });
        }
    }
}
