
namespace Contracts.RepositoryContracts
{
    public interface IRepositoryManager
    {
        IProductRepository ProductRepo { get; }
        ICartRepository CartRepo { get; }
        ICategoryRepository CategoryRepo { get; }
    }
}
