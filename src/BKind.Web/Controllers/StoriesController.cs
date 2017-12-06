using System;
using System.Linq;
using System.Threading.Tasks;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Stories.Contracts;
using BKind.Web.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Controllers
{
    public class StoriesController : BkindControllerBase
    {
        public StoriesController(IMediator mediator) : base(mediator) {}

        public async Task<IActionResult> Read(string id)
        {
            var user = await GetLoggedUserAsync();
            var stories = await _mediator.Send(new ListStoriesQuery { StorySlug = id, UserWithRoles = user, IncludeTags = true});

            if (stories == null || !stories.Any()) return NotFound();

            var model = new ReadStoryViewModel(stories[0], user);

            await _mediator.Send(new IncreaseStoryViewCountCommand(stories[0].Id));

            if (TempData.ContainsKey(_ErrorKey))
                model.Errors.Add((string)TempData[_ErrorKey]);

            return View(model);
        }

        public async Task<IActionResult> List(bool? recommended, string tag, int? page)
        {
            var user = await GetLoggedUserAsync();
            var stories = await _mediator.Send(new ListStoriesQuery
            {
                UserWithRoles = user,
                Paging = new PagedOptions<Story>(page, orderBy: x => x.Modified, ascending: false),
                Pinned = recommended,
                Tag = tag
            });

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> ThumbsUp(string id)
        {
            var user = await GetLoggedUserAsync();

            var result = await _mediator.Send(new ThumbsUpCommand(user, id));

            return RedirectToAction("Read", new { id });
        }
    }
}
