using OptiArroz_Tesis_Proyect.Models.Dtos;
namespace OptiArroz_Tesis_Proyect.Data.Interfaces
{
    public interface IUserDA
    {
        public Task<List<UserTableDTO>> GetUsers(int state);
        public Task ChangeRoleUser(string UserId, string RolId);
        public Task Update(string UserId,string Name, string LastName, string PhoneNumber, string Email);
        public Task ActivateUser(string UserId);
        public Task DeactivateUser(string UserId);

    }
}
