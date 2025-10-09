public class FavouriteConfiguration : IEntityTypeConfiguration<Favourite>
{
    public void Configure(EntityTypeBuilder<Favourite> builder)
    {
        builder.HasKey(f => f.Id);

        builder.HasOne(f => f.User)
            .WithMany(u => u.Favourites)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne(f => f.Pipeline)
            .WithMany(p => p.Favourites)
            .HasForeignKey(f => f.PipelineId)
            .OnDelete(DeleteBehavior.NoAction); 

        builder.HasIndex(f => new { f.UserId, f.PipelineId })
            .IsUnique();
    }
}
