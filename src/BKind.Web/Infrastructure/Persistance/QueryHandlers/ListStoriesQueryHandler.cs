using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BKind.Web.Features.Stories;
using BKind.Web.Features.Stories.Contracts;
using BKind.Web.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BKind.Web.Infrastructure.Persistance.QueryHandlers
{
    public class ListStoriesQueryHandler : IAsyncRequestHandler<ListStoriesQuery, List<StoryProjection>>
    {
        private readonly DbContext _db;

        public ListStoriesQueryHandler(DbContext db)
        {
            _db = db;
        }

        public async Task<List<StoryProjection>> Handle(ListStoriesQuery message)
        {
            IQueryable<Story> query = _db.Set<Story>().AsNoTracking();

            // admin can see everything
            // reviewer can see everything
            // author can see all his stories and others published stories 
            // visitor/anonymous can see just published stories

            if (message.UserWithRoles != null)
            {
                if (message.UserWithRoles.Is<Reviewer>() || message.UserWithRoles.Is<Administrator>())
                {
                    query = query.Where(story => story.Status == Status.Published || story.Status == Status.Draft);
                }
                else if (message.UserWithRoles.Is<Author>())
                {
                    var author = message.UserWithRoles.GetRole<Author>();
                    query = query.Where(story => story.AuthorId == author.Id || story.Status == Status.Published);
                }
                else // visitor
                {
                    query = query.Where(story => story.Status == Status.Published);
                }
            }
            else // anonymous
                query = query.Where(story => story.Status == Status.Published);

            if (message.FromDate.HasValue)
                query = query.Where(story => story.Created > message.FromDate.Value);

            if (message.ToDate.HasValue)
                query = query.Where(story => story.Created < message.ToDate.Value);

            if (message.StoryId > 0)
                query = query.Where(x => x.Id == message.StoryId);


            var result = await query.OrderByDescending(story => story.Created)
                .Skip(0).Take(message.MaxStories)
                .Select(story => new StoryProjection
                {
                    Id = story.Id,
                    Title = story.Title,
                    Content = story.Content,
                    Created = story.Created,
                    AuthorId = story.AuthorId,
                    AuthorName = story.Author.User.Nickname,
                    ThumbsUp = story.ThumbsUp,
                    Views = story.Views,
                    Status = story.Status
                }).ToListAsync();

            if(message.IncludeTags)
            {
                var storyIds = result.Select(x => x.Id).ToArray();

                var tags = _db.Set<StoryTags>()
                    .AsNoTracking()
                    .Where(x => storyIds.Contains(x.StoryId))
                    .Select(x => new { x.Tag.Title, x.StoryId })
                    .ToList();

                foreach (var projection in result)
                {
                    projection.Tags = tags
                        .Where(x => x.StoryId == projection.Id)
                        .Select(x => x.Title)
                        .ToList(); 
                }
            }

            return result;
        }
    }
}