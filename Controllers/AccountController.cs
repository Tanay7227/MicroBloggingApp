using Blog_MSP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog_MSP.Controllers
{
    public class AccountController : Controller
    {
        //Phase 1
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //Phase 2
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            //Checking Model is Valid or not
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Finding the username from db using FindByNameAsync Method.
            var existingUser = await _userManager.FindByNameAsync(model.UserName);

            //Checking Username is taken or not.
            // If username is present and not equal to null
            if (existingUser != null)
            {
                ModelState.AddModelError(
                    nameof(model.UserName),
                    "This username is already taken.");

                return View(model);
            }

            //Finding the Email from db using FindByEmailAsync Method.
            var existingEmail = await _userManager.FindByEmailAsync(model.Email);

            //Checking Email is taken or not.
            // If Email is present and not equal to null
            if (existingEmail != null)
            {
                ModelState.AddModelError(
                    nameof(model.Email),
                    "This email address is already registered.");

                return View(model);
            }

            //Converting cityname into the array list.
            var allowedCities = new[]
            {
                "Ahmedabad", "Surat", "Vadodara", "Rajkot", "Other"
            };


            //Verify the form values 
            if (!allowedCities.Contains(model.City))
            {
                ModelState.AddModelError(
                    nameof(model.City),
                    "Please select a valid city.");

                return View(model);
            }

            //Verify the form values 
            var genders = new[] { "Male", "Female", "Other" };

            if (!genders.Contains(model.Gender))
            {
                ModelState.AddModelError(nameof(model.Gender), "Select Valid Gender");
                return View(model);
            }

            //Verify the form values 
            var validHobbies = new[] { "Reading", "Music", "Sports", "Travel" };

            if (model.Hobbies == null || model.Hobbies.Count == 0 ||
                model.Hobbies.Any(h => !validHobbies.Contains(h)))
            {
                ModelState.AddModelError(nameof(model.Hobbies), "Select Valid Hobbies");
                return View(model);
            }

            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                City = model.City,
                Address = model.Address,
                Pincode = model.Pincode,
                Gender = model.Gender,
                Hobbies = string.Join(", ", model.Hobbies)
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }   

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // LOGIN - GET
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // LOGIN - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Profile", "Account");
            }

            ModelState.AddModelError(
                string.Empty,
                "Invalid username or password.");

            return View(model);
        }

        // LOGOUT - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }

        //Profile
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            // Check whether the user is logged in
            //if (!User.Identity!.IsAuthenticated)
            //{
            //    return RedirectToAction("Login");
            //}

            // Get the currently logged-in user from Identity
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // Copy database details into the ViewModel
            var model = new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                City = user.City,
                Address = user.Address,
                Pincode = user.Pincode,
                Gender = user.Gender,
                Hobbies = user.Hobbies
            };

            return View(model);
        }

        //Edit
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            // Check whether the user is logged in
            //if (!User.Identity!.IsAuthenticated)
            //{
            //    return RedirectToAction("Login");
            //}

            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // Fill the edit form with existing user details
            var model = new EditProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                City = user.City,
                Address = user.Address,
                Pincode = user.Pincode,
                Gender = user.Gender,
                Hobbies = string.IsNullOrWhiteSpace(user.Hobbies)
                    ? new List<string>()
                    : user.Hobbies.Split(", ").ToList()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            //if (!User.Identity!.IsAuthenticated)
            //{
            //    return RedirectToAction("Login");
            //}

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // Check whether another user already uses this email
            var existingemail = await _userManager.FindByEmailAsync(model.Email);

            if (existingemail != null && existingemail.Id != user.Id)
            {
                ModelState.AddModelError(
                    nameof(model.Email),
                    "This email address is already registered.");

                return View(model);
            }

            // Update profile fields
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.City = model.City;
            user.Address = model.Address;
            user.Pincode = model.Pincode;
            user.Gender = model.Gender;
            user.Hobbies = string.Join(", ", model.Hobbies);

            // Update email, but do not change username
            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Profile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

    }
}
