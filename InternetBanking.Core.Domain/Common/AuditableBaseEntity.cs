namespace InternetBanking.Core.Domain.Common
{
	public abstract class AuditableBaseEntity
	{
        public virtual string Id { get; set; }
        public string UserId { get; set; }
    }
}
