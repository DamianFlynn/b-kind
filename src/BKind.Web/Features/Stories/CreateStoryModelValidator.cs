﻿using FluentValidation;

namespace BKind.Web.Features.Stories
{
    public class CreateStoryModelValidator : AbstractValidator<SaveStoryInputModel>
    {
        public CreateStoryModelValidator()
        {
            RuleFor(x => x.StoryTitle).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();
            //RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}