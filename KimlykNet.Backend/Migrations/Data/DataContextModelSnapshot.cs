﻿// <auto-generated />
using System;
using KimlykNet.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KimlykNet.Backend.Migrations.Data
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("KimlykNet.Data.Abstractions.Entities.UserNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTimeOffset(new DateTime(2024, 7, 2, 7, 1, 40, 736, DateTimeKind.Unspecified).AddTicks(420), new TimeSpan(0, 0, 0, 0, 0)))
                        .HasColumnName("created_at");

                    b.Property<DateTimeOffset?>("DateModified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTimeOffset(new DateTime(2024, 7, 2, 7, 1, 40, 736, DateTimeKind.Unspecified).AddTicks(570), new TimeSpan(0, 0, 0, 0, 0)))
                        .HasColumnName("updated_at");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("boolean");

                    b.Property<string>("Note")
                        .HasMaxLength(4096)
                        .HasColumnType("character varying(4096)")
                        .HasColumnName("note");

                    b.Property<Guid>("NoteId")
                        .HasColumnType("uuid")
                        .HasColumnName("note_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserNotes", "app");
                });
#pragma warning restore 612, 618
        }
    }
}
