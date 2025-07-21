namespace Bug.Chatter.Domain.Common
{
	public abstract class Entity<TId> where TId : class
	{
		public TId Id { get; protected set; }

		protected Entity(TId id) => Id = id;

		public override bool Equals(object? obj)
		{
			Entity<TId>? other = obj as Entity<TId>;

			if (other == null || other.Id == null)
				return false;

			return Id == other.Id;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
