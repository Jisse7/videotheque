using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using videotheque.Models;
using videotheque.ViewModels;

namespace videotheque.Controllers
{
    public class AccountController : Controller
    {

        private readonly SignInManager<Users> signInManager;
        // pour gérer la connexion et la déconnexion de l'utilisateur
        private readonly UserManager<Users> userManager;
        // gère les opérations utilisateur (comme la création d’utilisateur).
        // viennent d'asp.NetCore.Identity

        public AccountController(SignInManager<Users> signInManager, UserManager<Users> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Recherche l'utilisateur par email car c'est ce qu'on utilise pour la connexion
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // Utilise l'email pour la connexion puisque c'est aussi le username
                    var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Email ou mot de passe incorrect.");
            }
            return View(model);
        }

        /* [HttpPost]
         public async Task<IActionResult> Login(LoginViewModel model)
         {
             if (ModelState.IsValid)
             {
                 var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                 if (result.Succeeded)
                 {
                     return RedirectToAction("Index", "Home");
                 }
                 else
                 {
                     ModelState.AddModelError("", "Email ou mot de passe incorrecte.");
                     return View(model);
                 }
             }
             return View(model);
         }*/

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Users users = new Users
                {
                    FullName = model.Name,  
                    Email = model.Email,
                    UserName = model.Email, 
                    DateInscription = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(users, model.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(users, "client");
                    // Connecte directement l'utilisateur après l'inscription
                    await signInManager.SignInAsync(users, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        /*  [HttpPost]
          public async Task<IActionResult> Register(RegisterViewModel model)
          //Task<...> est un type qui représente une tâche asynchrone. Ici, la méthode retourne une Task (tâche) qui, une fois terminée, produira un résultat de type IActionResult.
          {
              if (ModelState.IsValid)
              //vérifie si les données de model respectent les contraintes de validation définies dans RegisterViewModel. Si elles sont valides, le code continue ; sinon, il retourne la vue avec les erreurs.
              {
                  Users users = new Users
                  {
                      FullName = model.Name,
                      Email = model.Email,
                      UserName = model.Name,
                      DateInscription = DateTime.UtcNow
                  };

                  var result = await userManager.CreateAsync(users, model.Password);
                  //userManager (une instance de UserManager<Users>) pour créer l'utilisateur dans la base de données.

                  if (result.Succeeded)

                  {
                      // Ajoute le rôle "client" au nouvel utilisateur
                      await userManager.AddToRoleAsync(users, "client");
                      return RedirectToAction("Login", "Account");
                      //RedirectToAction est utilisé ici pour finaliser l'inscription proprement, éviter les resoumissions, et rediriger l'utilisateur vers une étape logique suivante
                  }
                  else
                  {
                      foreach (var error in result.Errors)
                      {
                          ModelState.AddModelError("", error.Description);
                      }

                      return View(model);
                  }
              }
              return View(model);
              //Si ModelState.IsValid est false ou que la création de l'utilisateur a échoué, la méthode retourne la vue Register avec le modèle model, permettant de réafficher le formulaire et les erreurs.
          }*/

        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Erreur!");
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
                }
            }
            return View(model);
        }

        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }
            return View(new ChangePasswordViewModel { Email = username });
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    var result = await userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await userManager.AddPasswordAsync(user, model.NewPassword);
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email not found!");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong. try again.");
                return View(model);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
