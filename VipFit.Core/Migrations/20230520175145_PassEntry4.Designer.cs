﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VipFit.Core.DataAccessLayer;

#nullable disable

namespace VipFit.Core.Migrations
{
    [DbContext(typeof(VipFitContext))]
    [Migration("20230520175145_PassEntry4")]
    partial class PassEntry4
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("VipFit.Core.Models.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("AgreementMarketing")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AgreementPromoImage")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AgreementSocialsImage")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AgreementWebsiteImage")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Trash")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("VipFit.Core.Models.Entry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PassId")
                        .HasColumnType("TEXT");

                    b.Property<byte>("PositionInPass")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PassId");

                    b.ToTable("Entry", (string)null);
                });

            modelBuilder.Entity("VipFit.Core.Models.Pass", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PassTemplateId")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("PassTemplateId");

                    b.ToTable("Passes");
                });

            modelBuilder.Entity("VipFit.Core.Models.PassTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Duration")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("PassTemplate", (string)null);
                });

            modelBuilder.Entity("VipFit.Core.Models.Entry", b =>
                {
                    b.HasOne("VipFit.Core.Models.Pass", "Pass")
                        .WithMany()
                        .HasForeignKey("PassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pass");
                });

            modelBuilder.Entity("VipFit.Core.Models.Pass", b =>
                {
                    b.HasOne("VipFit.Core.Models.Client", "Client")
                        .WithMany("Passes")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VipFit.Core.Models.PassTemplate", "PassTemplate")
                        .WithMany()
                        .HasForeignKey("PassTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("PassTemplate");
                });

            modelBuilder.Entity("VipFit.Core.Models.Client", b =>
                {
                    b.Navigation("Passes");
                });
#pragma warning restore 612, 618
        }
    }
}
