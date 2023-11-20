﻿// <auto-generated />
using System;
using LocalIdentity.SimpleInfra.Persistence.DataContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LocalIdentity.SimpleInfra.Persistence.Migrations
{
    [DbContext(typeof(IdentityDbContext))]
    [Migration("20231120110420_AddRoleAndUserRelation")]
    partial class AddRoleAndUserRelation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LocalIdentity.SimpleInfra.Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("ModifiedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("d0b0d6c0-2b7a-4b1a-9f1a-0b9b6a9a5b1a"),
                            CreatedTime = new DateTimeOffset(new DateTime(2023, 11, 20, 16, 4, 19, 843, DateTimeKind.Unspecified).AddTicks(5091), new TimeSpan(0, 5, 0, 0, 0)),
                            IsActive = true,
                            IsDeleted = false,
                            Type = 0
                        },
                        new
                        {
                            Id = new Guid("d0b0d6c0-2b7a-4b1a-9f1a-0b9b6a9a5b1b"),
                            CreatedTime = new DateTimeOffset(new DateTime(2023, 11, 20, 16, 4, 19, 843, DateTimeKind.Unspecified).AddTicks(5124), new TimeSpan(0, 5, 0, 0, 0)),
                            IsActive = true,
                            IsDeleted = false,
                            Type = 1
                        });
                });

            modelBuilder.Entity("LocalIdentity.SimpleInfra.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Age")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsEmailAddressVerified")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("ModifiedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LocalIdentity.SimpleInfra.Domain.Entities.User", b =>
                {
                    b.HasOne("LocalIdentity.SimpleInfra.Domain.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });
#pragma warning restore 612, 618
        }
    }
}
