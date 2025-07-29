using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public  DbSet<Answer> Answers { get; set; }

        public  DbSet<AnswerOption> AnswerOptions { get; set; }

        public  DbSet<Option> Options { get; set; }

        public  DbSet<Place> Places { get; set; }

        public  DbSet<Question> Questions { get; set; }

        public  DbSet<QuestionsType> QuestionsTypes { get; set; }

        public DbSet<Role> Roles { get; set; }

        public  DbSet<Section> Sections { get; set; }

        public  DbSet<Survey> Surveys { get; set; }

        public  DbSet<SurveysStatus> SurveysStatuses { get; set; }

        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Survey>()
            .HasOne(s => s.User)
            .WithMany(u => u.Surveys)
            .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<Answer>()
                .HasOne(a => a.User)
                .WithMany(u => u.Answers)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Section>()
            .HasOne(sec => sec.Survey)
            .WithMany(s => s.Sections)
            .HasForeignKey(sec => sec.SurveyId);

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Section)
                .WithMany(sec => sec.Questions)
                .HasForeignKey(q => q.SectionId);

            modelBuilder.Entity<QuestionsType>()
                .Property(q => q.Id)
                .ValueGeneratedNever(); /// Prevent EF from generating values for this property


            modelBuilder.Entity<Option>()
                .HasOne(opt => opt.Question)
                .WithMany(q => q.Options)
                .HasForeignKey(opt => opt.QuestionId);

            modelBuilder.Entity<AnswerOption>()
                .HasOne(ao => ao.Option)
                .WithMany(opt => opt.AnswerOptions)
                .HasForeignKey(ao => ao.OptionId);

            modelBuilder.Entity<Survey>()
                .HasOne(s => s.Status)
                .WithMany()
                .HasForeignKey(s => s.StatusId);

            modelBuilder.Entity<Role>()
                .Property(r=> r.Id)
                .ValueGeneratedNever(); // Prevent EF from generating values for this property

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);



        }

    }
   
}
