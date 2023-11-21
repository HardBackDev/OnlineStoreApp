using Shared.Dtos.ProductDtos;

namespace Shared.RequestFeatures;

public abstract class RequestParameters
{
    const int maxPageSize = 50;
    public int PageNumber { get; set; } = 1;

    private int _pageSize = 9;
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }

    public string? OrderBy { get; set; } = nameof(ProductDto.Price) + " desc";
}