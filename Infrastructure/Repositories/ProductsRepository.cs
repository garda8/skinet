using Core.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Infastructure.Repositories
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>> SelectAllProducts(CancellationToken cancellationToken);
        Task<Product> SelectProduct(int Id, CancellationToken cancellationToken);

    }



    public class ProductsRepository : IProductsRepository
    {

        //private readonly IConfiguration _configuration;
        private string connectionString = ""; 
        

        public ProductsRepository(IConfiguration configuration)
        {
            //_configuration = configuration;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Product>> SelectAllProducts(CancellationToken cancellationToken)
        {
            
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var cmd = new CommandDefinition(
                commandText: $"SELECT Id, Name FROM dbo.Products",
            //,
            //parameters: new
            //{
            //    FarmId = farmId,
            //    AnimalSpecie = specie
            //    //endDate,
            //    //startDate
            //},
            cancellationToken: cancellationToken);

                return await connection.QueryAsync<Product>(cmd);
            }
        }

        public async Task<Product> SelectProduct(int Id, CancellationToken cancellationToken)
        {
            try 
            { 
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var cmd = new CommandDefinition(
                    commandText: $"SELECT Id, Name" +
                    $" FROM dbo.Products" + 
                    $" WHERE Id = @GivenID"
                    ,
                    parameters: new
                    {
                        GivenId = Id
                    },
                    cancellationToken: cancellationToken);

                    return connection.QueryFirst<Product>(cmd);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
