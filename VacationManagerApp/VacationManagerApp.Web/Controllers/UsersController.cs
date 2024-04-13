using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VacationManagerApp.Common;
using VacationManagerApp.Services.Contracts;
using VacationManagerApp.Services;
using System.Security.Claims;
using VacationManagerApp.ViewModels.Users;

namespace VacationManager.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService service;

        public UsersController(IUserService service)
        {
            this.service = service;
        }
        [Authorize(Roles = GlobalConstants.AdminRole)]
        public async Task<IActionResult> Index(IndexUsersViewModel? model)
        {

            model = await service.GetUsersAsync(model);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                await service.CreateUserAsync(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await service.GetUserToEditAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                await service.UpdateUserAsync(model);
                return this.RedirectToAction(nameof(Index));
            }
            return View(model);
        }
		[HttpGet]
		public async Task<IActionResult> AddToTeam(string id)
		{
			var model = await service.GetUserToAddAsync(id);
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> AddToTeam(AddToTeamViewModel model)
		{
			if (ModelState.IsValid)
			{
				await service.AddUserToTeamAsync(model);
				return this.RedirectToAction(nameof(Index));
			}
			return View(model);
		}

		[HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var userViewModel = await service.GetUserDetailsAsync(id);
            if (userViewModel == null)
            {
                return NotFound();
            }

            return View(userViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Seed()
        {
            const string Password = "123456";
            for (int i = 0; i < 20; i++)
            {
                string result = await service.CreateUserAsync(

                      new CreateUserViewModel()
                      {
                          FirstName = $"Name {i}",
                          LastName = $"LastName {i}",
                          Password = Password,
                          ConfirmPassword = Password,
                          Email = $"user{i}@app.bg",
                          Role = GlobalConstants.Unassigned
                      }
                      );
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (id != userId)
            {
                await service.DeleteUserAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            string returnUrl = Url.Content("~/");


            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await service.Login(model);
                if (result.Succeeded)
                {
                    // _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    // _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
