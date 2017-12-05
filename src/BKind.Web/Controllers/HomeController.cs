﻿using System.Threading.Tasks;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Stories.Contracts;
using BKind.Web.Model;
using BKind.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BKind.Web.Controllers
{
    public class HomeController : BkindControllerBase
    {
        private readonly ILogger _logger;

        public HomeController(IMediator mediator, ILogger<HomeController> logger) : base(mediator)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var user = await GetLoggedUserAsync();

            var model = new HomePageViewModel
            {
                Title = "Welcome to Be Kind",
                CanWriteStory = User.Identity.IsAuthenticated,
                Latest = new StoryListModel(
                    await _mediator.Send(new ListStoriesQuery { UserWithRoles = user, Paging = new PagedOptions<Story>(orderBy: s => s.Modified, ascending: false)}),
                    user),
                Best = new StoryListModel(
                    await _mediator.Send(new ListStoriesQuery { UserWithRoles = user, Paging = new PagedOptions<Story>(orderBy: s => s.Views, ascending: false)}),
                    user)
            };

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}