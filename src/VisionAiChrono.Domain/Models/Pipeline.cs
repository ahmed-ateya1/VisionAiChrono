using VisionAiChrono.Domain.Models.Identity;

namespace VisionAiChrono.Domain.Models
{
    public class Pipeline
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public string ContentJson { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public Guid? BasePipelineId { get; set; }
        public virtual Pipeline? BasePipeline { get; set; }
        public virtual ICollection<Pipeline> DerivedPipelines { get; set; } = [];
        public virtual ICollection<Favourite> Favourites { get; set; } = [];
        public virtual ICollection<PipelineModels> PipelineModels { get; set; } = [];
        public virtual ICollection<PipelineRun> PipelineRuns { get; set; } = [];

    }
}
