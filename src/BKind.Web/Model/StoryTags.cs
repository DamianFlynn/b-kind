namespace BKind.Web.Model
{
    public class StoryTags
    {
        public int StoryId { get; set; }
        public int TagId { get; set; }

        public Story Story { get; set; }
        public Tag Tag { get; set; }
    }
}