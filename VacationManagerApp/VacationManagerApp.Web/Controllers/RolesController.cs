﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using VacationManagerApp.Data;
using VacationManagerApp.Data.Models;
using VacationManagerApp.ViewModels;
using VacationManagerApp.Data;
using VacationManagerApp.ViewModels.Roles;
using VacationManagerApp.Common;

namespace VacationManager.Controllers
{
    public class RolesController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesController(ApplicationDbContext db, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _db = db;
            _roleManager = roleManager;
        }
        [Authorize(Roles = GlobalConstants.AdminRole)]
        public async Task<IActionResult> Index()
        {
            List<IdentityRole> roles = _roleManager.Roles.ToList();
            Dictionary<string, int> rolesMembersCount = new Dictionary<string, int>();
            foreach (var role in roles)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
                rolesMembersCount.Add(role.Name, usersInRole.Count);
            }
            return View(rolesMembersCount);
        }
        public async Task<IActionResult> Members(string id)
        {
            if (await _roleManager.RoleExistsAsync(id))
            {
                RolesMembersViewModel data = new RolesMembersViewModel();
                data.Members = new List<User>(await _userManager.GetUsersInRoleAsync(id));
                data.RoleName = id;
                return View(data);
            }
            return NotFound();
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (!await _roleManager.RoleExistsAsync(id))
            {
                return NotFound();
            }

            RolesEditViewModel data = new RolesEditViewModel();
            IdentityRole role = await _roleManager.FindByNameAsync(id);
            data.RoleName = role.Name;
            data.RoleId = role.Id;

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRoleName(RolesEditViewModel data)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(data.RoleId);

            if (role == null)
            {
                return NotFound();
            }

            role.Name = data.RoleName;
            role.NormalizedName = data.RoleName.ToUpper();

            await _db.SaveChangesAsync();

            return RedirectToAction("Members", new { id = data.RoleName });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                return NotFound();
            }

            IdentityRole roleToDelete = await _roleManager.FindByNameAsync(roleName);

            await _roleManager.DeleteAsync(roleToDelete);

            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel data)
        {
            await _roleManager.CreateAsync(new IdentityRole(data.NewRoleName));
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
