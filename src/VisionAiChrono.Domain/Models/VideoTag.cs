namespace VisionAiChrono.Domain.Models
{
    public class VideoTag
    {
        public Guid VideoId { get; set; }
        public virtual Video Video { get; set; }
        public Guid TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
