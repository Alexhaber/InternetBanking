using InternetBanking.Core.Domain.Enums;

namespace InternetBanking.Core.Domain.Entities
{
	public class Transaction
	{
		public int Id { get; set; }
		public string SourceProductId { get; set; }
        public string DestinationProductId { get; set; }
        public decimal Monto { get; set; }
        public PaymentTypes Tipo { get; set; } = PaymentTypes.PaymentTransaction;
        public DateTime Made { get; set; } = DateTime.Today;
    }

}
                                