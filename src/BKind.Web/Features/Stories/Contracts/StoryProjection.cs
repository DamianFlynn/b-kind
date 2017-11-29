using System;
using System.Collections.Generic;
using BKind.Web.Model;

namespace BKind.Web.Features.Stories.Contracts
{
    public class StoryProjection
    {
        public StoryProjection()
        {
            Tags = new List<string>();    
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int ThumbsUp { get; set; }
        public string AuthorName { get; set; }
        public int AuthorId { get; set; }
        public DateTime Created { get; set; }
        public Status Status { get; set; }
        public List<string> Tags { get; set; }
        public int Views { get; set; }

        public string FormattedStatus
        {
            get { return Status != Status.Published ? $"[{Status.ToString()}]" : ""; }
        }
    }
}