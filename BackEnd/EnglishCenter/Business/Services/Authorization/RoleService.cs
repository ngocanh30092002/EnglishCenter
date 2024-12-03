using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Authorization
{
    public class RoleService : IRoleService
    {
        private RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unit;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IUnitOfWork unit)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _unit = unit;
        }

        public async Task<Response> AddUserRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any users",
                    Success = false
                };
            }

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any roles",
                    Success = false
                };
            }

            var isInRole = await _userManager.IsInRoleAsync(user, roleName);
            if (isInRole)
            {
                return new Response() { StatusCode = System.Net.HttpStatusCode.OK, Message = "", Success = true };
            }

            var studentModel = _unit.Students.GetById(userId);

            if (roleName == AppRole.ADMIN || roleName == AppRole.TEACHER)
            {
                var isExistTeacher = _unit.Teachers.IsExist(t => t.UserId == userId);
                if (!isExistTeacher)
                {
                    var teacherModel = new Teacher()
                    {
                        FirstName = studentModel.FirstName,
                        LastName = studentModel.LastName,
                        Gender = studentModel.Gender,
                        Address = studentModel?.Address,
                        DateOfBirth = studentModel?.DateOfBirth,
                        PhoneNumber = studentModel?.PhoneNumber,
                        UserId = studentModel!.UserId,
                        UserName = studentModel?.FirstName + " " + studentModel?.LastName,
                        Image = studentModel?.Image,
                        BackgroundImage = studentModel?.BackgroundImage,
                        Description = studentModel?.Description,
                    };

                    _unit.Teachers.Add(teacherModel);
                    await _unit.CompleteAsync();
                }
            }

            await _userManager.AddToRoleAsync(user, roleName);
            return new Response() { StatusCode = System.Net.HttpStatusCode.OK, Message = "", Success = true };
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            var isExisted = await IsExistRoleAsync(roleName);

            if (isExisted)
            {
                return false;
            }

            await _roleManager.CreateAsync(new IdentityRole(roleName));
            return true;
        }

        public async Task<bool> DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                return false;
            }

            var isAnyUserInRole = await _userManager.GetUsersInRoleAsync(roleName);
            if (isAnyUserInRole.Any())
            {
                return false;
            }

            await _roleManager.DeleteAsync(role);
            return true;
        }

        public async Task<Response> DeleteUserRolesAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any users",
                    Success = false
                };
            }

            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Role isn't exist",
                    Success = false
                };
            }

            if (roleName == AppRole.STUDENT)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Student is default role so can't delete",
                    Success = false
                };
            }

            if (roleName == AppRole.TEACHER || roleName == AppRole.ADMIN)
            {
                var teacherModel = await _unit.Teachers
                                              .Include(t => t.Classes)
                                              .FirstOrDefaultAsync(t => t.UserId == userId);

                if (teacherModel != null)
                {
                    if (teacherModel.Classes.Count != 0)
                    {
                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "This user is in charge of a class so cannot delete",
                            Success = false
                        };
                    }

                    _unit.Teachers.Remove(teacherModel);
                    await _unit.CompleteAsync();
                }
            }

            await _userManager.RemoveFromRoleAsync(user, roleName);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> GetRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return new Response()
            {
                Message = roles.Select(r => r.Name).ToList(),
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<Response> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Response()
                {
                    Message = "Can't find any users",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                }; ;
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            return new Response()
            {
                Message = userRoles,
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<bool> IsExistRoleAsync(string roleName)
        {
            var isExistRole = await _roleManager.Roles.FirstOrDefaultAsync(r => r.NormalizedName == roleName.ToUpper());

            return isExistRole != null;
        }
    }
}
