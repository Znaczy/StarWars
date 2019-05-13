using Microsoft.EntityFrameworkCore;


namespace StarWars.DAL.Entities
{
    public class StarWarsDbContext : DbContext
    {
        public StarWarsDbContext()
        {
        }

        public StarWarsDbContext(DbContextOptions<StarWarsDbContext> options)
            : base(options)
        {
        }

        public DbSet<CharacterEntity> Characters { get; set; }
        public DbSet<PlanetEntity> Planets { get; set; }
        public DbSet<EpisodeEntity> Episodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterEntity>(entity =>
            {
                entity.Property(e => e.Id)
                .HasColumnName("Id");

                entity.Property(e => e.Name)
                .HasColumnName("Name")
                .HasMaxLength(255)
                .IsRequired(true);

                entity.HasOne(c => c.Planet)
                .WithMany(p => p.Characters)
                .HasForeignKey(c => c.PlanetId);
            });

            modelBuilder.Entity<PlanetEntity>(entity =>
            {
                entity.Property(e => e.Id)
                .HasColumnName("Id")
                .IsRequired(true);

                entity.Property(e => e.Name)
                .HasColumnName("Name")
                .IsRequired(true)
                .HasMaxLength(255);

            });

            modelBuilder.Entity<EpisodeEntity>(entity =>
            {
                entity.Property(e => e.Id)
                .HasColumnName("Id")
                .IsRequired(true);

                entity.Property(e => e.Name)
                .HasColumnName("Name")
                .IsRequired(true)
                .HasMaxLength(255);
            });

            modelBuilder.Entity<CharacterEpisodeEntity>()
                .HasKey(k => new { k.CharacterId, k.EpisodeId });

            modelBuilder.Entity<CharacterEpisodeEntity>()
                .HasOne(ce => ce.Character)
                .WithMany(e => e.CharacterEpisodes)
                .HasForeignKey(ce => ce.CharacterId);

            modelBuilder.Entity<CharacterEpisodeEntity>()
                .HasOne(ce => ce.Episode)
                .WithMany(e => e.CharacterEpisodes)
                .HasForeignKey(ce => ce.EpisodeId);

            modelBuilder.Entity<CharacterFriendEntity>()
                .HasKey(f => new { f.CharacterId, f.FriendId });

            modelBuilder.Entity<CharacterFriendEntity>()
                .HasOne(f => f.Character)
                .WithMany(c => c.CharacterFriends)
                .HasForeignKey(f => f.CharacterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CharacterFriendEntity>()
                .HasOne(f => f.Friend)
                .WithMany(u => u.FriendCharacters)
                .HasForeignKey(f => f.FriendId);

        }
    }
}
