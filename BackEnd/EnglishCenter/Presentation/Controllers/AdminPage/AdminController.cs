using EnglishCenter.DataAccess.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.AdminPage
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepo;

        public AdminController(IAdminRepository adminRepo)
        {
            _adminRepo = adminRepo;
        }
    }
}
