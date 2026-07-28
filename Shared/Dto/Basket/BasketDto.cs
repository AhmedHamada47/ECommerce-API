namespace Shared.Dto.Basket
{
    public record BasketDto
    {
        public string Id { get; init; }
        public ICollection<BasketItemDto> BasketItems { get; init; }
    }
}
