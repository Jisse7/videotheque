using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using videotheque.Models;

namespace videotheque.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {
        //constructeur
        public AppDbContext(DbContextOptions options) : base(options) { }

        // Chaque DbSet représente une table dans la base de données
        public DbSet<Film> Films { get; set; }

      
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationDetail> LocationDetails { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Paiement> Paiements { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<ExemplaireDVD> ExemplairesDVD { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Appelle la configuration de base pour Identity
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExemplaireDVD>()
          .HasOne(e => e.Film)
          .WithMany()
          .HasForeignKey(e => e.FilmId)
          .OnDelete(DeleteBehavior.Restrict);

            // pour que le code barre soi unique
            modelBuilder.Entity<ExemplaireDVD>()
                .HasIndex(e => e.CodeBarre)
                .IsUnique();

            // Modifie la relation LocationDetail pour inclure l'exemplaire spécifique
            modelBuilder.Entity<LocationDetail>()
                .HasOne(ld => ld.ExemplaireDVD)
                .WithOne(e => e.LocationDetail)
                .HasForeignKey<LocationDetail>("ExemplaireDVDId")
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration spécifique pour Users
            modelBuilder.Entity<Users>()
                .Property(u => u.Adresse)
                .IsRequired(false);  // Rend explicitement la colonne nullable

            modelBuilder.Entity<Users>()
                .Property(u => u.DateInscription)
                .HasDefaultValueSql("GETUTCDATE()");  // Valeur par défaut côté base de données

            // Ajoute des données initiales pour les catégories
            modelBuilder.Entity<Categorie>().HasData(
                new Categorie
                {
                    Id = 1,
                    Nom = "Action",
                    Description = "Films d'action et d'aventure"
                }
            ); 

            // Configure la relation entre Film et Categorie
            modelBuilder.Entity<Film>()
                .HasOne(f => f.Categorie)
                .WithMany(c => c.Films)
                .HasForeignKey(f => f.CategorieId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure la relation entre Evaluation et Film
            modelBuilder.Entity<Evaluation>()
                .HasOne(e => e.Film)
                .WithMany(f => f.Evaluations)
                .HasForeignKey(e => e.FilmId)
                .OnDelete(DeleteBehavior.Restrict);

            // Définit la plage de valeurs valides pour le score
            modelBuilder.Entity<Evaluation>()
                .Property(e => e.Score)
                .HasAnnotation("Range", new[] { 1, 5 });

            // Configure la relation entre Location et User
            modelBuilder.Entity<Location>()
                .HasOne(l => l.User)
                .WithMany(u => u.Locations)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure les relations pour LocationDetail
            modelBuilder.Entity<LocationDetail>()
                .HasOne(ld => ld.Location)
                .WithMany(l => l.LocationDetails)
                .HasForeignKey(ld => ld.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LocationDetail>()
                .HasOne(ld => ld.Film)
                .WithMany()
                .HasForeignKey(ld => ld.FilmId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure la relation un-à-un entre Paiement et Location
            modelBuilder.Entity<Paiement>()
                .HasOne(p => p.Location)
                .WithOne(l => l.Paiement)
                .HasForeignKey<Paiement>(p => p.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure  relations pour CartItem
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Film)
                .WithMany(f => f.CartItems)
                .HasForeignKey(ci => ci.FilmId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(ci => ci.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // c pour créer un index unique pour éviter les doublons dans le panier
            modelBuilder.Entity<CartItem>()
                .HasIndex(ci => new { ci.CartId, ci.FilmId })
                .IsUnique();
        }
    }
}
