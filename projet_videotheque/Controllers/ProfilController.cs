using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using videotheque.Models;
using videotheque.ViewModels; 

namespace videotheque.Controllers
{
    public class ProfilController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;

        public ProfilController(UserManager<Users> userManager, SignInManager<Users> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> EditProfil()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new EditProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                Address = user.Adresse,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfil(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Met à jour les informations de l'utilisateur
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.UserName = model.Email; // Important : garde email comme username
            user.Adresse = model.Address;
            user.PhoneNumber = model.PhoneNumber;

            // Mise à jour du profil
            var result = await _userManager.UpdateAsync(user);

            // Si un nouveau mot de passe est fourni
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (model.NewPassword == model.ConfirmPassword)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                    if (!passwordResult.Succeeded)
                    {
                        foreach (var error in passwordResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Les mots de passe ne correspondent pas");
                    return View(model);
                }
            }

            if (result.Succeeded)
            {
                // Rafraîchit le cookie d'authentification
                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = "Profil mis à jour avec succès";
                return RedirectToAction("EditProfil");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
    }
}