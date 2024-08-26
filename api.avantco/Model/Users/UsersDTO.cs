
namespace api.avantco.Model.Users;
public class UsersDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int? RecoveryPassword { get; set; }
    public int? Active { get; set; }
}
