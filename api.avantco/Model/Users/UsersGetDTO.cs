
namespace api.avantco.Model.Users;
public class UsersGetDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public int? RecoveryPassword { get; set; }
    public int? Active { get; set; }
}
