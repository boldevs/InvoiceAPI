namespace Application.Features.Items.Get
{
    public sealed class ItemsRespone
    {
        public Guid ItemId { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Descriptions { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
    }
}
