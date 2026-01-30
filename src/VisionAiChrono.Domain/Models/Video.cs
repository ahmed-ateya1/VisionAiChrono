using VisionAiChrono.Domain.Models.Identity;

namespace VisionAiChrono.Domain.Models
{
    public class Video
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Url { get; set; }
        public long SizeInBytes { get; set; }
        public TimeSpan Duration { get; set; }
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTime UploadedAt { get; set; }
        public virtual ICollection<VideoTag> VideoTags { get; set; } = [];
        public virtual ICollection<PipelineResult> PipelineResults { get; set; } = [];
        public virtual ICollection<PipelineRunVideo> PipelineRunVideos { get; set; } = [];
    }
}
