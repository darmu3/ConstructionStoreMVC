﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using applicationmvc.Context;

#nullable disable

namespace applicationmvc.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("applicationmvc.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("OrderId"));

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("OrderNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("OrderSum")
                        .HasColumnType("real");

                    b.Property<int?>("StoreId")
                        .HasColumnType("integer");

                    b.HasKey("OrderId");

                    b.HasIndex("StoreId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("applicationmvc.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProductId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<int?>("ProductCategoryId")
                        .HasColumnType("integer");

                    b.Property<int?>("SupplierId")
                        .HasColumnType("integer");

                    b.HasKey("ProductId");

                    b.HasIndex("ProductCategoryId");

                    b.HasIndex("SupplierId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("applicationmvc.Models.ProductCategory", b =>
                {
                    b.Property<int>("ProductCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProductCategoryId"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ProductCategoryId");

                    b.ToTable("ProductCategory");
                });

            modelBuilder.Entity("applicationmvc.Models.ProductOrder", b =>
                {
                    b.Property<int>("ProductOrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProductOrderId"));

                    b.Property<int?>("OrderId")
                        .HasColumnType("integer");

                    b.Property<int?>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("QuantityProductOrder")
                        .HasColumnType("integer");

                    b.HasKey("ProductOrderId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductOrder");
                });

            modelBuilder.Entity("applicationmvc.Models.ProductWarehouse", b =>
                {
                    b.Property<int>("ProductWarehouseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProductWarehouseId"));

                    b.Property<int?>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("QuantityProduct")
                        .HasColumnType("integer");

                    b.Property<int?>("WarehouseId")
                        .HasColumnType("integer");

                    b.HasKey("ProductWarehouseId");

                    b.HasIndex("ProductId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("ProductWarehouse");
                });

            modelBuilder.Entity("applicationmvc.Models.Store", b =>
                {
                    b.Property<int>("StoreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("StoreId"));

                    b.Property<string>("StoreAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StoreName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("StoreId");

                    b.ToTable("Store");
                });

            modelBuilder.Entity("applicationmvc.Models.Supplier", b =>
                {
                    b.Property<int>("SupplierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SupplierId"));

                    b.Property<string>("SupplierAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SupplierName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("SupplierId");

                    b.ToTable("Supplier");
                });

            modelBuilder.Entity("applicationmvc.Models.Warehouse", b =>
                {
                    b.Property<int>("WarehouseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("WarehouseId"));

                    b.Property<string>("WarehouseAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("WarehouseName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("WarehouseId");

                    b.ToTable("Warehouse");
                });

            modelBuilder.Entity("applicationmvc.Models.Order", b =>
                {
                    b.HasOne("applicationmvc.Models.Store", "Store")
                        .WithMany("Orders")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Order_Store");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("applicationmvc.Models.Product", b =>
                {
                    b.HasOne("applicationmvc.Models.ProductCategory", "ProductCategory")
                        .WithMany("Products")
                        .HasForeignKey("ProductCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Product_ProductCategory");

                    b.HasOne("applicationmvc.Models.Supplier", "Supplier")
                        .WithMany("Products")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_Product_Supplier");

                    b.Navigation("ProductCategory");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("applicationmvc.Models.ProductOrder", b =>
                {
                    b.HasOne("applicationmvc.Models.Order", "Order")
                        .WithMany("ProductOrders")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_ProductOrder_Order");

                    b.HasOne("applicationmvc.Models.Product", "Product")
                        .WithMany("ProductOrders")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_ProductOrder_Product");

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("applicationmvc.Models.ProductWarehouse", b =>
                {
                    b.HasOne("applicationmvc.Models.Product", "Product")
                        .WithMany("ProductWarehouses")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_ProductWarehouse_Product");

                    b.HasOne("applicationmvc.Models.Warehouse", "Warehouse")
                        .WithMany("ProductWarehouses")
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_ProductWarehouse_Warehouse");

                    b.Navigation("Product");

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("applicationmvc.Models.Order", b =>
                {
                    b.Navigation("ProductOrders");
                });

            modelBuilder.Entity("applicationmvc.Models.Product", b =>
                {
                    b.Navigation("ProductOrders");

                    b.Navigation("ProductWarehouses");
                });

            modelBuilder.Entity("applicationmvc.Models.ProductCategory", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("applicationmvc.Models.Store", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("applicationmvc.Models.Supplier", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("applicationmvc.Models.Warehouse", b =>
                {
                    b.Navigation("ProductWarehouses");
                });
#pragma warning restore 612, 618
        }
    }
}
