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
        Task<IEnumerable<ProductViewModel>> GetProductsAsync(CancellationToken cancellationToken, string sort, int? brandId, int? typeId);
        Task<ProductViewModel> GetProductByIdAsync(int Id, CancellationToken cancellationToken);

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

        public async Task<IEnumerable<ProductViewModel>>
            GetProductsAsync(CancellationToken cancellationToken, string sort, int? brandId, int? typeId)
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
                        $" LEFT JOIN dbo.ProductTypes PT ON P.ProductTypeId = PT.Id " + Filters(sort, brandId, typeId),
                        
            
            cancellationToken: cancellationToken);

                var res = await connection.QueryAsync<ProductViewModel>(cmd);
                return res; 
            }
        }

        private string Filters(string sort, int? brandId, int? typeId)
        {
            string filter = " WHERE 1 = 1 ";
            string retVal = " ORDER BY ";

            if (brandId != null)
            {
                filter += " AND P.ProductBrandId = " + brandId;
            }
            if (typeId != null)
            {
                filter += " AND P.ProductTypeId = " + typeId;
            }
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "priceAsc":
                        retVal += "P.Price";
                        break;
                    case "priceDesc":
                        retVal += "P.Price DESC";
                        break;
                    case "nameDesc":
                        retVal += "P.Name DESC";
                        break;
                    default:
                        retVal += "P.Name";
                        break;
                }
                return (filter + retVal); 
            }
            return (filter + (retVal += "P.Name"));
        }

        public async Task<ProductViewModel> GetProductByIdAsync(int Id, CancellationToken cancellationToken)
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

                    var p = await connection.QueryFirstOrDefaultAsync<ProductViewModel>(cmd);
                    //var product = new Product
                    //    {
                    //        Id = p.Id,
                    //        Name = p.Name,
                    //        Description = p.Description,
                    //        Price = p.Price,
                    //        PictureUrl = p.PictureUrl,
                    //        ProductBrand = new ProductBrand { Id = p.ProductBrandId, Name = p.ProductBrand },
                    //        ProductType = new ProductType { Id = p.ProductTypeId, Name = p.ProductType },
                    //        ProductTypeId = p.ProductTypeId,
                    //        ProductBrandId = p.ProductBrandId
                    //    };
                    //return product;
                    return p;
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
