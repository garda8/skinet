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
        Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken);
        Task<Product> GetProductByIdAsync(int Id, CancellationToken cancellationToken);

        Task<IEnumerable<ProductType>> GetProductTypesAsync(CancellationToken cancellationToken);

        Task<IEnumerable<ProductBrand>> GetProductBrandsAsync(CancellationToken cancellationToken);

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

        public async Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken)
        {
            
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var cmd = new CommandDefinition(
                commandText: 
                        $"SELECT P.[Id], P.[Name],[Description],[Price],[PictureUrl],[ProductTypeId], PT.Name AS ProductType"+
                        $" ,[ProductBrandId], PB.Name AS ProductBrand" +
                        $" FROM [dbo].[Products] P" +
                        $" LEFT JOIN dbo.ProductBrands PB ON p.ProductBrandId = PB.Id" +
                        $"  LEFT JOIN dbo.ProductTypes PT ON P.ProductTypeId = PT.Id",
            //,
            //parameters: new
            //{
            //    FarmId = farmId,
            //    AnimalSpecie = specie
            //    //endDate,
            //    //startDate
            //},
            cancellationToken: cancellationToken);

                var res = await connection.QueryAsync<ProductViewModel>(cmd);
                List<Product> products = new List<Product>();
                foreach (var p in res)
                {
                    products.Add(new Product
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        PictureUrl = p.PictureUrl,
                        ProductBrand = new ProductBrand { Id = p.ProductBrandId, Name = p.ProductBrand },
                        ProductType = new ProductType { Id = p.ProductTypeId, Name = p.ProductType },
                        ProductTypeId = p.ProductTypeId,
                        ProductBrandId = p.ProductBrandId
                    });
                }

                return products;
            }
        }

        public async Task<Product> GetProductByIdAsync(int Id, CancellationToken cancellationToken)
        {
            try 
            { 
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var cmd = new CommandDefinition(
                    commandText:
                    $"SELECT P.[Id], P.[Name],[Description],[Price],[PictureUrl],[ProductTypeId], PT.Name AS ProductType" +
                        $" ,[ProductBrandId], PB.Name AS ProductBrand" +
                        $" FROM [dbo].[Products] P" +
                        $" LEFT JOIN dbo.ProductBrands PB ON p.ProductBrandId = PB.Id" +
                        $"  LEFT JOIN dbo.ProductTypes PT ON P.ProductTypeId = PT.Id" + 
                        $" WHERE P.Id = @GivenID"
                    ,
                    parameters: new
                    {
                        GivenId = Id
                    },
                    cancellationToken: cancellationToken);

                    var p = await connection.QueryFirstAsync<ProductViewModel>(cmd);
                    var product = new Product
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Description = p.Description,
                            Price = p.Price,
                            PictureUrl = p.PictureUrl,
                            ProductBrand = new ProductBrand { Id = p.ProductBrandId, Name = p.ProductBrand },
                            ProductType = new ProductType { Id = p.ProductTypeId, Name = p.ProductType },
                            ProductTypeId = p.ProductTypeId,
                            ProductBrandId = p.ProductBrandId
                        };
                    return product;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ProductType>> GetProductTypesAsync(CancellationToken cancellationToken)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var cmd = new CommandDefinition(
                    commandText:
                    $"SELECT P.[Id], P.[Name]FROM [dbo].[ProductTypes] P" ,
                    cancellationToken: cancellationToken);
                    return await connection.QueryAsync<ProductType>(cmd);
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<IEnumerable<ProductBrand>> GetProductBrandsAsync(CancellationToken cancellationToken)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var cmd = new CommandDefinition(
                    commandText:
                    $"SELECT P.[Id], P.[Name]FROM [dbo].[ProductBrands] P",
                    cancellationToken: cancellationToken);
                    return await connection.QueryAsync<ProductBrand>(cmd);
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}
