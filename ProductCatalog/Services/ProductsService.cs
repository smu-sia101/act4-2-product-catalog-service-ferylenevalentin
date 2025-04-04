using MongoDB.Driver;
using ProductCatalog.Models;
using Microsoft.Extensions.Options;
namespace ProductCatalog.Services;

public class ProductsService
{
    private readonly IMongoCollection<ProductModel> _productsCollection;

    public ProductsService(
   IOptions<ProductsDatabaseSettings> productDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            productDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
        productDatabaseSettings.Value.DatabaseName);

        _productsCollection = mongoDatabase.GetCollection<ProductModel>(
        productDatabaseSettings.Value.ProductsCollectionName);
    }
    public async Task<List<ProductModel>> GetAsync() =>
        await _productsCollection.Find(_ => true).ToListAsync();

    public async Task<ProductModel?> GetAsync(string id) =>
        await _productsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(ProductModel newProduct) =>
        await _productsCollection.InsertOneAsync(newProduct);

    public async Task UpdateAsync(string id, ProductModel updatedProduct) =>
        await _productsCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);

    public async Task RemoveAsync(string id) =>
        await _productsCollection.DeleteOneAsync(x => x.Id == id);
}
