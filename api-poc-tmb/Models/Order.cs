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
        public string Cliente { get; set; }
        public string Produto { get; set; }
        public float Valor { get; set; }
        public EOrderStatus Status { get; set; }
        public DateTimeOffset Sata_criacao { get; set; }
    }
}
