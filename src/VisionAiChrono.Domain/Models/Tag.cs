namespace VisionAiChrono.Domain.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<VideoTag> VideoTags { get; set; } = [];
    }
}
