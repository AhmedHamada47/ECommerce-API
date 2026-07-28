namespace Shared.Dto
{
    public record PaginatedResponse<TData>(int PageIndex, int PageSize, int Count, IEnumerable<TData> Data);
}
