namespace VisionAiChrono.Infrastructure.Data.Configuration
{
    public class VideoConfiguration : IEntityTypeConfiguration<Video>
    {
        public void Configure(EntityTypeBuilder<Video> builder)
        {
            builder.HasKey(v => v.Id);

            builder.Property(v => v.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(v => v.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(v => v.SizeInBytes)
                .IsRequired();

            builder.Property(v => v.Duration)
                .IsRequired();

            builder.Property(v => v.UploadedAt)
                .IsRequired();

            builder.HasOne(v => v.User)
                .WithMany(u => u.Videos)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(v => v.VideoTags)
                .WithOne(vt => vt.Video)
                .HasForeignKey(vt => vt.VideoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(v => v.PipelineResults)
                .WithOne(pr => pr.Video)
                .HasForeignKey(pr => pr.VideoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}