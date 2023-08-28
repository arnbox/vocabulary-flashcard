using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace VocabularyFlashCard.DataService.Repository;

public class AppDbContext : IdentityDbContext<AppUser>
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
	public DbSet<Vocabulary> Vocabularys => Set<Vocabulary>();
	public DbSet<VocabularyMedia> VocabularyMedia => Set<VocabularyMedia>();
	// public DbSet<AppUser> AppUser { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Indices
		modelBuilder.Entity<Vocabulary>().HasIndex(v => v.Word).IsUnique();

		// Relations
		modelBuilder.Entity<VocabularyMedia>()
			.HasOne(vm => vm.Vocabulary)
			.WithMany(v => v.VocabularyMedias)
			.HasPrincipalKey(v => v.VocabularyId)
			.HasForeignKey(vm => vm.VocabularyId);
	}
}