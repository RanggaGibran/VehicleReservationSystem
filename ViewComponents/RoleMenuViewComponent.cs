using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VehicleReservationSystem.Models;

namespace VehicleReservationSystem.ViewComponents
{
    public class RoleMenuViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RoleMenuViewComponent(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            bool isSignedIn = _signInManager.IsSignedIn(UserClaimsPrincipal);
            bool isAdmin = false;
            bool isApprover = false;
            if (isSignedIn)
            {
                var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
                if (user != null)
                {
                    isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                    isApprover = await _userManager.IsInRoleAsync(user, "Approver");
                }
            }
            ViewBag.IsSignedIn = isSignedIn;
            ViewBag.IsAdmin = isAdmin;
            ViewBag.IsApprover = isApprover;
            ViewBag.UserName = isSignedIn ? UserClaimsPrincipal.Identity?.Name : null;
            return View();
        }
    }
}
