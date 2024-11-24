namespace OptiArroz_Tesis_Proyect.Models.Dtos
{
    public class UserTableDTO
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime RegisterDate { get; set; } 
        public string PhoneNumber { get; set; } = string.Empty;
        public int RolId { get; set; }
        public string Rol { get; set; } = string.Empty;
    }
}
