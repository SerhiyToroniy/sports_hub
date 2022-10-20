using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sports_hub.Models.Entities;
using sports_hub.Models.Entities.Navigation;
using sports_hub.Models.Entities.Comments;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace sports_hub.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        
        public DbSet<FooterPage> FooterPages { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Conference> Conferences { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleLifestyleDealbook> ArticlesLifestyleDealbook { get; set; }
        public DbSet<ArticleLifestyle> ArticlesLifestyle { get; set; }
        public DbSet<ArticleDealbook> ArticlesDealbook { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<BreakdownSection> BreakdownSections { get; set; }
        public DbSet<MainComment> MainComments { get; set; }
        public DbSet<SubComment> SubComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //var converter = new ValueConverter<int[], string>(
            //    v => string.Join(";", v),
            //    v => v.Split(";", StringSplitOptions.RemoveEmptyEntries).Select(val => int.Parse(val)).ToArray());

            //var valueComparer = new ValueComparer<int[]>(
            //    (c1, c2) => c1.SequenceEqual(c2),
            //    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            //    c => c.ToArray());

            //modelBuilder.Entity<BaseComment>()
            //    .Property(e => e.LikesCount)
            //    .HasConversion(converter);

            //modelBuilder.Entity<BaseComment>()
            //    .Property(e => e.DislikesCount)
            //    .HasConversion(converter);

            //modelBuilder.Entity<BaseComment>()
            //    .Property(e => e.LikesCount)
            //    .Metadata
            //    .SetValueComparer(valueComparer);

            //modelBuilder.Entity<BaseComment>()
            //    .Property(e => e.DislikesCount)
            //    .Metadata
            //    .SetValueComparer(valueComparer);

            //modelBuilder.Entity<BaseComment>()
            //    .HasNoKey();

            modelBuilder
                .Entity<BreakdownSection>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            modelBuilder
                .Entity<BreakdownSection>()
                .HasOne(p => p.Conference)
                .WithMany()
                .HasForeignKey(p => p.ConferenceId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            modelBuilder
                .Entity<BreakdownSection>()
                .HasOne(p => p.Team)
                .WithMany()
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            modelBuilder.Entity<UserTeam>()
            .HasKey(t => new { t.UserId, t.TeamId });

            modelBuilder.Entity<UserTeam>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.UserTeams)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<UserTeam>()
                .HasOne(sc => sc.Team)
                .WithMany(c => c.UserTeams)
                .HasForeignKey(sc => sc.TeamId);

            modelBuilder.Entity<Entities.Navigation.Location>().HasData(
                new Entities.Navigation.Location[]
                {
                    new Entities.Navigation.Location
                    {
                        Id = 1,
                        Name = "location1"
                    },
                    new Entities.Navigation.Location
                    {
                        Id = 2,
                        Name = "location2"
                    }
            });
            modelBuilder.Entity<FooterPage>().HasData(
                new FooterPage[]
                {
                    new FooterPage
                    {
                        Id = 1,
                        Name = "Privacy policy",
                        Content = "none",
                        Section = "CompanyInfo",
                        Visible = true
                    },
                    new FooterPage
                    {
                        Id = 2,
                        Name = "Terms and conditions",
                        Content = "none",
                        Section = "CompanyInfo",
                        Visible = true
                    },

                    new FooterPage
                    {
                        Id = 3,
                        Name = "About Sports Hub",
                        Content = "none",
                        Section = "CompanyInfo",
                        Visible = true
                    },
                    new FooterPage
                    {
                        Id = 4,
                        Name = "News / In the Press",
                        Content = "none",
                        Section = "CompanyInfo",
                        Visible = true
                    },
                    new FooterPage
                    {
                        Id = 5,
                        Name = "Advertising / Sports Blogger Ad Network",
                        Content = "none",
                        Section = "CompanyInfo",
                        Visible = true
                    },
                    new FooterPage
                    {
                        Id = 6,
                        Name = "Events",
                        Content = "none",
                        Section = "CompanyInfo",
                        Visible = true
                    },
                    new FooterPage
                    {
                        Id = 7,
                        Name = "Contact Us",
                        Content = "none",
                        Section = "CompanyInfo",
                        Visible = true
                    },
                    new FooterPage
                    {
                        Id = 8,
                        Name = "Featured Writers Program",
                        Content = "none",
                        Section = "Contributors",
                        Visible = true
                    },
                    new FooterPage
                    {
                        Id = 9,
                        Name = "Featured Team Writers Program",
                        Content = "none",
                        Section = "Contributors",
                        Visible = true
                    },
                    new FooterPage
                    {
                        Id = 10,
                        Name = "Internship Program",
                        Content = "none",
                        Section = "Contributors",
                        Visible = true
                    },
                    new FooterPage
                    {
                        Id = 11,
                        Name = "Sign up to receive the latest sports news",
                        Content = "none",
                        Section = "Newsletter",
                        Visible = true
                    }
                });
            modelBuilder.Entity<ContactInfo>().HasData(
                new ContactInfo[]
                {
                    new ContactInfo
                    {
                        Id = 7,
                        Adress = "2D Sadova, Lviv",
                        Telephone = "+380994329602",
                        Email = "admin@gmail.com"
                    }
                });
            
            modelBuilder.Entity<Section>().HasData(
                new Section[]
                {
                    new Section
                    {
                        Id = 1,
                        SectionName = "CompanyInfo",
                        Visible = true
                    },
                    new Section
                    {
                        Id = 2,
                        SectionName = "Contributors",
                        Visible = true
                    },
                    new Section
                    {
                        Id = 3,
                        SectionName = "Newsletter",
                        Visible = true
                    }
                });

            modelBuilder.Entity<Language>().HasData(
                new Language[]
                {
                    new Language
                    {
                        Id = 1,
                        Name = "English",
                        Visible = true
                    },
                    new Language
                    {
                        Id = 2,
                        Name = "Ukrainian",
                        Visible = true
                    }
                });

            //Adding test data which is present on mockup
            modelBuilder.Entity<Category>().HasData(
                new Category[]
                {
                    new Category
                    {
                        Id = 1,
                        Name = "NBA",
                        Order = 1,
                        Conferences = new List<Conference>()
                    },
                    new Category
                    {
                        Id = 2,
                        Name = "NFL",
                        Order = 2,
                        Conferences = new List<Conference>()
                    },
                    new Category
                    {
                        Id = 3,
                        Name = "MLB",
                        Order = 3,
                        Conferences = new List<Conference>()
                    }
                });
            modelBuilder.Entity<Conference>().HasData(
                new Conference[]
                {
                    new Conference
                    {
                        Id = 1,
                        Name = "AFC East",
                        Order = 1,
                        CategoryId = 1
                    },
                    new Conference
                    {
                        Id = 2,
                        Name = "AFC West",
                        Order = 2,
                        CategoryId = 1
                    },
                    new Conference
                    {
                        Id = 3,
                        Name = "AFC North",
                        Order = 3,
                        CategoryId = 1
                    },
                    new Conference
                    {
                        Id = 4,
                        Name = "AFC South",
                        Order = 4,
                        CategoryId = 1
                    },
                });
            modelBuilder.Entity<Team>().HasData(
                new Team[]
                {
                    new Team
                    {
                        Id = 1,
                        Name = "Houston",
                        Order = 1,
                        ConferenceId = 2
                    },
                    new Team
                    {
                        Id = 2,
                        Name = "Los Angeles L",
                        Order = 2,
                        ConferenceId = 2
                    },
                    new Team
                    {
                        Id = 3,
                        Name = "Memphis",
                        Order = 3,
                        ConferenceId = 2
                    },
                    new Team
                    {
                        Id = 4,
                        Name = "Utah Jazz",
                        Order = 4,
                        ConferenceId = 2
                    }
                });
            base.OnModelCreating(modelBuilder);
        }
    }
}
