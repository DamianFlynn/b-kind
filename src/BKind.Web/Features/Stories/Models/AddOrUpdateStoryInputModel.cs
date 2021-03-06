﻿using BKind.Web.Core;
using BKind.Web.Model;
using BKind.Web.ViewModels;
using MediatR;

namespace BKind.Web.Features.Stories.Contracts
{
    public class AddOrUpdateStoryInputModel : ViewModelBase, IRequest<Response<Story>>, IUserIdentifier
    {
        public AddOrUpdateStoryInputModel()
        {
            this.Title = "Create or edit story";        
        }

        public int? StoryId { get; set; }
        public string StoryTitle { get; set; }
        public string Content { get; set; }
        public string Tags { get; set; }

        public int UserId { get; set; }

        public static AddOrUpdateStoryInputModel From(Story story)
        {
            return new AddOrUpdateStoryInputModel
            {
                StoryId = story.Id,
                StoryTitle = story.Title,
                Content = story.Content
            };
        }
    }
}