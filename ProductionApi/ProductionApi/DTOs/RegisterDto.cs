namespace ProductionApi.DTOs
{
    public class RegisterDto
    {
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
        public string FullName { get; set; } = "";
        public int RoleId { get; set; }
    }
}