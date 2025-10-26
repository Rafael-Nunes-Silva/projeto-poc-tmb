using System.Text.Json.Serialization;

namespace api_poc_tmb.Models
{
    public class OrderStatusHistory
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public EOrderStatus StatusAntigo { get; set; }
        public EOrderStatus StatusNovo { get; set; }
        public DateTimeOffset DataAlteracao { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public Order? Order { get; set; }
    }
}
