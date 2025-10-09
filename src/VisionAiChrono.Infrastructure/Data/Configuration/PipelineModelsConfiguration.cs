namespace VisionAiChrono.Infrastructure.Data.Configuration
{
    public class PipelineModelsConfiguration : IEntityTypeConfiguration<PipelineModels>
    {
        public void Configure(EntityTypeBuilder<PipelineModels> builder)
        {
            builder.HasKey(pm => pm.Id);

            builder.Property(pm => pm.StepOrder)
                .IsRequired();

            builder.Property(pm => pm.CreatedAt)
                .IsRequired();

            builder.HasOne(pm => pm.Pipeline)
                .WithMany(p => p.PipelineModels)
                .HasForeignKey(pm => pm.PipelineId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pm => pm.AiModel)
                .WithMany(am => am.PipelineModels)
                .HasForeignKey(pm => pm.AiModelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(pm => new { pm.PipelineId, pm.StepOrder })
                .IsUnique();
        }
    }
}