﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.ViewModels;

namespace UserManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;

        }
        public async Task<IActionResult> Index()
        {

            var users = await _userManager.Users.Select(users => new UserViewModel
            {

                Id = users.Id,
                FirstName = users.FirstName,
                LastName = users.LastName,
                UserName = users.UserName,
                Email = users.Email,
                Roles = _userManager.GetRolesAsync(users).Result

            }).ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> Add()
        {

            var roles = await _roleManager.Roles.Select(r => new RoleViewModel { RoleId = r.Id, RoleName = r.Name }).ToListAsync();

            var viewModel = new AddUserViewModel
            {

                Roles = roles
            };
            return View(viewModel);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {

            if (!ModelState.IsValid)
                return View(model);

            if (!model.Roles.Any(r => r.IsSelected))
            {
                ModelState.AddModelError("Roles", "Please select at least one role");
                return View(model);
            }

            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("Email", "Email is already exists");
                return View(model);
            }

            if (await _userManager.FindByNameAsync(model.Username) != null)
            {
                ModelState.AddModelError("Username", "Username is already exists");
                return View(model);
            }

            var user = new ApplicationUser
            {
                FirstName = model.FistName,
                LastName = model.LastName,
                UserName = model.Username,
                Email = model.Email


            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Roles", error.Description);
                }
                return View(model);
            }
            await _userManager.AddToRolesAsync(user, model.Roles.Where(r => r.IsSelected).Select(r => r.RoleName));

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string usersId)
        {
            var user = await _userManager.FindByIdAsync(usersId);

            if (user == null)
                return NotFound();

            var viewModel = new ProfileFormViewModel
            {
                Id = usersId,
                FistName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email
            };

            return View(viewModel);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
                return NotFound();

            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);

            if (userWithSameEmail != null && userWithSameEmail.Id != model.Id)
            {
                ModelState.AddModelError("Email", "This email is already assiged to another user");
                return View(model);
            }

            var userWithSameUserName = await _userManager.FindByNameAsync(model.UserName);

            if (userWithSameUserName != null && userWithSameUserName.Id != model.Id)
            {
                ModelState.AddModelError("UserName", "This username is already assiged to another user");
                return View(model);
            }

            user.FirstName = model.FistName;
            user.LastName = model.LastName;
            user.UserName = model.UserName;
            user.Email = model.Email;

            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ManageRoles(string usersId)
        {
            var user = await _userManager.FindByIdAsync(usersId);

            if (user == null)
                return NotFound();

            var roles = await _roleManager.Roles.ToListAsync();

            var viewModel = new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new RoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result
                }).ToList()
            };
            return View(viewModel);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);

                if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.RoleName);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
