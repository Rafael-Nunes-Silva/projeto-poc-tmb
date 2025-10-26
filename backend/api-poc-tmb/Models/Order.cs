namespace api_poc_tmb.Models
{
    public enum EOrderStatus
    {
        Pendente,
        Processando,
        Finalizado
    }

    public class Order
    {
        public int Id { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Produto { get; set; } = string.Empty;
        public float Valor { get; set; }
        public EOrderStatus Status { get; set; } = EOrderStatus.Pendente;
        public DateTimeOffset Data_criacao { get; set; } = DateTime.UtcNow;
        
        public List<OrderStatusHistory> HistoricoStatus { get; set; } = new();
    }
}
