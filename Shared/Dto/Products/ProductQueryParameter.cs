namespace Shared.Dto.Products;

public class ProductQueryParameter
{
    private const int DefaultPageSize = 5;
    private const int MaxPageSize = 10;

    public int? BrandId { get; set; }
    public int? Typeid { get; set; }
    public ProductSortinOptions options { get; set; }
    public string? Search { get; set; }

    public int PageIndex { get; set; } = 1; // Fixed: renamed to follow C# conventions (Pascal case)
    private int _pageSize = DefaultPageSize;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > 0 && value <= MaxPageSize) ? value : DefaultPageSize; // Fixed: spacing and added equal sign for MaxPageSize
    }
}