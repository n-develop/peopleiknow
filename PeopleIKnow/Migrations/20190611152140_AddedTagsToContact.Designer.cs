﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PeopleIKnow;

namespace PeopleIKnow.Migrations
{
    [DbContext(typeof(ContactContext))]
    [Migration("20190611152140_AddedTagsToContact")]
    partial class AddedTagsToContact
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085");

            modelBuilder.Entity("PeopleIKnow.Models.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<DateTime>("Birthday");

                    b.Property<string>("BusinessTitle");

                    b.Property<string>("Employer");

                    b.Property<string>("Firstname");

                    b.Property<string>("ImagePath");

                    b.Property<string>("Lastname");

                    b.Property<string>("Middlename");

                    b.Property<string>("Tags");

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("PeopleIKnow.Models.EmailAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ContactId");

                    b.Property<string>("Email");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.ToTable("EmailAddresses");
                });

            modelBuilder.Entity("PeopleIKnow.Models.Relationship", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ContactId");

                    b.Property<string>("Person");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.ToTable("Relationships");
                });

            modelBuilder.Entity("PeopleIKnow.Models.StatusUpdate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ContactId");

                    b.Property<DateTime>("Created");

                    b.Property<string>("StatusText");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.ToTable("StatusUpdates");
                });

            modelBuilder.Entity("PeopleIKnow.Models.TelephoneNumber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ContactId");

                    b.Property<string>("Telephone");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.ToTable("TelephoneNumbers");
                });

            modelBuilder.Entity("PeopleIKnow.Models.EmailAddress", b =>
                {
                    b.HasOne("PeopleIKnow.Models.Contact", "Contact")
                        .WithMany("EmailAddresses")
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PeopleIKnow.Models.Relationship", b =>
                {
                    b.HasOne("PeopleIKnow.Models.Contact", "Contact")
                        .WithMany("Relationships")
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PeopleIKnow.Models.StatusUpdate", b =>
                {
                    b.HasOne("PeopleIKnow.Models.Contact", "Contact")
                        .WithMany("StatusUpdates")
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PeopleIKnow.Models.TelephoneNumber", b =>
                {
                    b.HasOne("PeopleIKnow.Models.Contact", "Contact")
                        .WithMany("TelephoneNumbers")
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
