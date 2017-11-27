﻿using System.Threading.Tasks;
using BKind.Web.Features.Account.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Controllers.Account
{
    public class AccountController : BkindControllerBase
    {
        public AccountController(IMediator mediator) : base(mediator) {}
        
        public IActionResult Login() => View(new LoginInputModel());
     
        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel inputModel)
        {
            if (!ModelState.IsValid)
                return View(inputModel);

            var response = await _mediator.Send(inputModel);

            if(response.HasErrors)
            {
                MapToModelState(response);
                return View(inputModel);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register() => View(new ProfileInputModel());
        
        [HttpPost]
        public async Task<IActionResult> Register(ProfileInputModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _mediator.Send(model);

            if(response.HasErrors)
            {
                MapToModelState(response);
                return View(model);
            }

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/home/index");
        }
    }
}
