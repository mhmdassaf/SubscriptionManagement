namespace SubscriptionManagement.DAL.Entities.Base;

public class BaseEntity<TKey>
{
    public TKey? Id { get; set; }


    #region Create
    [Required]
    public DateTime CreateDate { get; set; }
    [Required]
    public string? CreatedByUserId { get; set; }
    [ForeignKey(nameof(CreatedByUserId))]
    public User? CreatedByUser { get; set; }
    #endregion

    #region Update
    public DateTime? UpdateDate { get; set; }
    public string? UpdatedByUserId { get; set; }
    [ForeignKey(nameof(UpdatedByUserId))]
    public User? UpdatedByUser { get; set; }
    #endregion

    #region Delete
    [Required]
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }
    public string? DeletedByUserId { get; set; }
    [ForeignKey(nameof(DeletedByUserId))]
    public User? DeletedByUser { get; set; }
    #endregion
}
