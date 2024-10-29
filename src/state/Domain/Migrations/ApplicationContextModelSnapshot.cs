﻿// <auto-generated />
using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Domain.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.23");

            modelBuilder.Entity("Objects.Dto.AttemptHistoryDto", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("CorrectAttempts")
                        .HasColumnName("correct_answers")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnName("created_utc")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("TotalAttempts")
                        .HasColumnName("total_attempts")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnName("updated_utc")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("WordsErrors")
                        .HasColumnName("words_errors")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AttemptHistories");
                });

            modelBuilder.Entity("Objects.Src.Dto.WordDto", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Attributes")
                        .HasColumnName("attributes")
                        .HasColumnType("TEXT");

                    b.Property<string>("Conjugation")
                        .HasColumnName("conjugation")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnName("created_utc")
                        .HasColumnType("TEXT");

                    b.Property<string>("Translation")
                        .IsRequired()
                        .HasColumnName("translation")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnName("updated_utc")
                        .HasColumnType("TEXT");

                    b.Property<string>("Word")
                        .HasColumnName("word")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Words");
                });
#pragma warning restore 612, 618
        }
    }
}
