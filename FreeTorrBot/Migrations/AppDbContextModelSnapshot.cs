﻿// <auto-generated />
using AdTorrBot.BotTelegram.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AdTorrBot.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("AdTorrBot.BotTelegram.Db.Model.SettingsBot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("IdChat")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("TimeZoneOffset")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("SettingsBot");
                });

            modelBuilder.Entity("AdTorrBot.BotTelegram.Db.Model.SettingsTorrserver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("SettingsTorrserver");
                });

            modelBuilder.Entity("AdTorrBot.BotTelegram.Db.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("IdChat")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("User");
                });
#pragma warning restore 612, 618
        }
    }
}