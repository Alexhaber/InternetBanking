namespace InternetBanking.Core.Domain.Entities
{
	public class Transaction
	{
		public int Id { get; set; }
        public int SourceProductId { get; set; }
        public int DestinationProductId { get; set; }
        public decimal Monto { get; set; }
        public DateTime Made { get; set; } = DateTime.Now;
    }
}
