using Microsoft.EntityFrameworkCore;

namespace CINEMA_MANAGEMENT_SYSTEM.Models
{
    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options)
        {
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(g => g.Name).IsUnique();
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Title).IsRequired().HasMaxLength(200);
                entity.Property(m => m.Director).IsRequired().HasMaxLength(100);
                entity.Property(m => m.TicketPrice).HasPrecision(10, 2);
                entity.HasIndex(m => m.Title).IsUnique();

                entity.HasOne(m => m.Genre)
                      .WithMany(g => g.Movies)
                      .HasForeignKey(m => m.GenreId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.FullName).IsRequired().HasMaxLength(150);
                entity.Property(c => c.Email).IsRequired().HasMaxLength(256);
                entity.HasIndex(c => c.Email).IsUnique();
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.BookingDate).IsRequired().HasDefaultValueSql("GETDATE()");
                entity.Property(b => b.TotalPrice).HasPrecision(10, 2);

                entity.HasOne(b => b.Customer)
                      .WithMany(c => c.Bookings)
                      .HasForeignKey(b => b.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Movie)
                      .WithMany(m => m.Bookings)
                      .HasForeignKey(b => b.MovieId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "Comedy" },
                new Genre { Id = 3, Name = "Drama" },
                new Genre { Id = 4, Name = "Sci-Fi" },
                new Genre { Id = 5, Name = "Horror" }
            );

            modelBuilder.Entity<Movie>().HasData(
                new Movie { Id = 1, Title = "The Dark Knight", Director = "Christopher Nolan", Duration = 152, ReleaseYear = 2008, Rating = 9.0, TicketPrice = 120.00m, AvailableSeats = 50, GenreId = 1 },
                new Movie { Id = 2, Title = "Inception", Director = "Christopher Nolan", Duration = 148, ReleaseYear = 2010, Rating = 8.8, TicketPrice = 110.00m, AvailableSeats = 45, GenreId = 4 },
                new Movie { Id = 3, Title = "Interstellar", Director = "Christopher Nolan", Duration = 169, ReleaseYear = 2014, Rating = 8.7, TicketPrice = 130.00m, AvailableSeats = 60, GenreId = 4 },
                new Movie { Id = 4, Title = "The Hangover", Director = "Todd Phillips", Duration = 100, ReleaseYear = 2009, Rating = 7.7, TicketPrice = 75.00m, AvailableSeats = 40, GenreId = 2 },
                new Movie { Id = 5, Title = "Superbad", Director = "Greg Mottola", Duration = 113, ReleaseYear = 2007, Rating = 7.6, TicketPrice = 70.00m, AvailableSeats = 35, GenreId = 2 },
                new Movie { Id = 6, Title = "The Shawshank Redemption", Director = "Frank Darabont", Duration = 142, ReleaseYear = 1994, Rating = 9.3, TicketPrice = 95.00m, AvailableSeats = 30, GenreId = 3 },
                new Movie { Id = 7, Title = "Forrest Gump", Director = "Robert Zemeckis", Duration = 142, ReleaseYear = 1994, Rating = 8.8, TicketPrice = 85.00m, AvailableSeats = 25, GenreId = 3 },
                new Movie { Id = 8, Title = "The Conjuring", Director = "James Wan", Duration = 112, ReleaseYear = 2013, Rating = 7.5, TicketPrice = 90.00m, AvailableSeats = 20, GenreId = 5 }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, FullName = "John Doe", Email = "john.doe@example.com", PhoneNumber = "+1234567890" },
                new Customer { Id = 2, FullName = "Jane Smith", Email = "jane.smith@example.com", PhoneNumber = "+1987654321" },
                new Customer { Id = 3, FullName = "Alice Johnson", Email = "alice.johnson@example.com", PhoneNumber = null },
                new Customer { Id = 4, FullName = "Bob Brown", Email = "bob.brown@example.com", PhoneNumber = "+1122334455" },
                new Customer { Id = 5, FullName = "Charlie Davis", Email = "charlie.davis@example.com", PhoneNumber = null }
            );

            modelBuilder.Entity<Booking>().HasData(
                new Booking { Id = 1, BookingDate = new DateTime(2026, 9, 1, 14, 30, 0), NumberOfTickets = 2, TotalPrice = 240.00m, CustomerId = 1, MovieId = 1 },
                new Booking { Id = 2, BookingDate = new DateTime(2026, 9, 5, 18, 0, 0), NumberOfTickets = 3, TotalPrice = 330.00m, CustomerId = 2, MovieId = 2 },
                new Booking { Id = 3, BookingDate = new DateTime(2026, 9, 10, 20, 15, 0), NumberOfTickets = 1, TotalPrice = 130.00m, CustomerId = 1, MovieId = 3 },
                new Booking { Id = 4, BookingDate = new DateTime(2026, 9, 15, 16, 45, 0), NumberOfTickets = 4, TotalPrice = 300.00m, CustomerId = 4, MovieId = 4 },
                new Booking { Id = 5, BookingDate = new DateTime(2026, 9, 20, 21, 0, 0), NumberOfTickets = 2, TotalPrice = 190.00m, CustomerId = 2, MovieId = 6 }
            );
        }
    }
}

