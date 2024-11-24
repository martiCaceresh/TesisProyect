using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.DataAccess
{
    public class UserDA : IUserDA
    {
        public readonly ApplicationDbContext DbContext;
        public UserDA(ApplicationDbContext DbContext, UserManager<ApplicationUser> UserManager)
        {
            this.DbContext = DbContext;
        }

        public async Task ActivateUser(string UserId)
        {
            try
            {
                var findUser = await DbContext.Users.FindAsync(UserId) ?? throw new Exception("No se pudo encontrar el usuario");
                findUser.State = 1;
                DbContext.Entry(findUser).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }


        }

        public async Task ChangeRoleUser(string UserId, string RolId)
        {
            try
            {
                // Encuentra y elimina el rol existente
                var currentUserRole = await DbContext.UserRoles
                    .Where(x => x.UserId == UserId)
                    .FirstOrDefaultAsync() ?? throw new Exception("No se pudo encontrar el usuario");

                DbContext.UserRoles.Remove(currentUserRole);
                await DbContext.SaveChangesAsync();

                // Crea el nuevo rol
                var newUserRole = new IdentityUserRole<string>
                {
                    UserId = UserId,
                    RoleId = RolId
                };

                // Agrega el nuevo rol
                await DbContext.UserRoles.AddAsync(newUserRole);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Es mejor loggear el error específico o manejarlo apropiadamente
                throw new Exception("Error al cambiar el rol del usuario: " + ex.Message);
            }
        }

        public async Task DeactivateUser(string UserId)
        {
            try
            {
                var findUser = await DbContext.Users.FindAsync(UserId) ?? throw new Exception("No se pudo encontrar el usuario");
                findUser.State = 0;
                DbContext.Entry(findUser).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<UserTableDTO>> GetUsers(int state)
        {
            var Users = await (from user in DbContext.Users
                             join userRole in DbContext.UserRoles on user.Id equals userRole.UserId
                             join role in DbContext.Roles on userRole.RoleId equals role.Id
                             where user.State == state && role.Name != "SUPERUSUARIO"
                              select new UserTableDTO
                             {
                                 Name = user.Name,
                                 LastName = user.LastName,
                                 PhoneNumber = user.PhoneNumber,
                                 Username = user.UserName,
                                 UserId = user.Id,
                                 RegisterDate = user.RegisterDate,
                                 RolId = int.Parse(role.Id),
                                 Rol = role.Name
                             }).ToListAsync();

            return Users;
        }
    }
}
