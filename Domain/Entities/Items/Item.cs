using SharedKernel;

namespace Domain.Entities.Items
{
    public sealed class Item : BaseEntity
    {
        public string Barcode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Descriptions { get; set; } = string.Empty;
        public Decimal UnitPrice { get; set; }
    }
}
