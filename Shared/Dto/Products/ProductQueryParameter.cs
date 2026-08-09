namespace Shared.Dto.Products;

public class ProductQueryParameter
{
    private const int DefaultPageSize = 5;
    private const int MaxPageSize = 10;

    public int? BrandId { get; set; }
    public int? TypeId { get; set; }
    public ProductSortingOptions? SortBy { get; set; }
    public string? Search { get; set; }

    private int _pageIndex = 1;
    public int PageIndex
    {
        get => _pageIndex;
        set => _pageIndex = value > 0 ? value : 1;
    }
    private int _pageSize = DefaultPageSize;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > 0 && value <= MaxPageSize) ? value : DefaultPageSize;
    }
}
