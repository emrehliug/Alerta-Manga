using MangaBotApi.Domain.Entities;
using MangaBotApi.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaBotApi.Infra.Data.Context
{
    public class MySqlContext : IdentityDbContext<User, Role, int,
                                                    IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
                                                    IdentityRoleClaim<int>, IdentityUserToken<int>>
    {

        public DbSet<Manga> tbManga { get; set; }
        public DbSet<Usuario> tbUsuario { get; set; }
        public DbSet<MangaUsuario> tbMangaUsuario { get; set; }
        public DbSet<LogMangaBotApi> tbLogMangaBotApi { get; set; }
        public DbSet<LogMangaBot> tbLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MangaUsuario>(builder =>
            {
                builder.ToTable("tbMangaUsuario");
                builder.HasKey(sc => new { sc.UsuarioId, sc.MangaId });
            });

            modelBuilder.Entity<UserRole>(userRole =>
            {

                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

        }

        public MySqlContext(DbContextOptions<MySqlContext> options)
            : base(options)
        {

        }
    }
}
