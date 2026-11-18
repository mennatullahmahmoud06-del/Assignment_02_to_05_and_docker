namespace Assignment.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }

        #region Audit
        public bool IsDeleted { get; set; } = false;
        public DateTime? CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
        #endregion
    }
}
