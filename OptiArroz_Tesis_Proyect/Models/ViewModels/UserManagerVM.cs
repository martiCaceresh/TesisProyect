using OptiArroz_Tesis_Proyect.Models.Dtos;

namespace OptiArroz_Tesis_Proyect.Models.ViewModels
{
    public class UserManagerVM
    {
        public List<RoleDTO> Roles { get;set; } = new List<RoleDTO>();
        public List<UserTableDTO> ActiveList { get; set; } = new List<UserTableDTO>();   
        public List<UserTableDTO> InactiveList { get; set; } = new List<UserTableDTO>();   
    }
}
