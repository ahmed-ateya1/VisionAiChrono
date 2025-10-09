namespace VisionAiChrono.Infrastructure.Data.Configuration
{
    public class VideoTagConfiguration : IEntityTypeConfiguration<VideoTag>
    {
        public void Configure(EntityTypeBuilder<VideoTag> builder)
        {
            builder.HasKey(vt => new { vt.VideoId, vt.TagId });

            builder.HasOne(vt => vt.Video)
                .WithMany(v => v.VideoTags)
                .HasForeignKey(vt => vt.VideoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(vt => vt.Tag)
                .WithMany(t => t.VideoTags)
                .HasForeignKey(vt => vt.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}