using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class UserModel : BaseEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserModelId { get; set; }
    public required string PhoneNumber { get; set; }
    public string Role { get; set; }
}
