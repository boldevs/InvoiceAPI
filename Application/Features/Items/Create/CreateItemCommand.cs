using Application.Abstractions.Messaging;

namespace Application.Features.Items.Create
{
    public sealed class CreateItemCommand : ICommand<Guid>
    {
        public string Barcode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Descriptions { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
    }
}
