namespace VisionAiChrono.Infrastructure.Data.Configuration
{
    public class AiModelConfiguration : IEntityTypeConfiguration<AiModel>
    {
        public void Configure(EntityTypeBuilder<AiModel> builder)
        {
            builder.HasKey(am => am.Id);

            builder.Property(am => am.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(am => am.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(am => am.Version)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(am => am.Endpoint)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(am => am.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(am => am.CreatedAt)
                .IsRequired();

            builder.HasOne(am => am.User)
                .WithMany(u => u.AiModels)
                .HasForeignKey(am => am.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(am => am.PipelineModels)
                .WithOne(pm => pm.AiModel)
                .HasForeignKey(pm => pm.AiModelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(am => am.PipelineResults)
                .WithOne(pr => pr.AiModel)
                .HasForeignKey(pr => pr.AiModelId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}