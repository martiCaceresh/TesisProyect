using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptiArroz_Tesis_Proyect.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegisterDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PasswordResetCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CodeExpiration = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    IdNotificationType = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PriorNotificationDays = table.Column<int>(type: "int", nullable: false),
                    FrecuencyAfterFirstNotification = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.IdNotificationType);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderKey = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RiceClasses",
                columns: table => new
                {
                    IdClass = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    IdCreatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdUpdatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiceClasses", x => x.IdClass);
                    table.ForeignKey(
                        name: "FK_RiceClasses_AspNetUsers_IdCreatedBy",
                        column: x => x.IdCreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RiceClasses_AspNetUsers_IdUpdatedBy",
                        column: x => x.IdUpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RiceGrades",
                columns: table => new
                {
                    IdGrade = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    IdCreatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdUpdatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiceGrades", x => x.IdGrade);
                    table.ForeignKey(
                        name: "FK_RiceGrades_AspNetUsers_IdCreatedBy",
                        column: x => x.IdCreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RiceGrades_AspNetUsers_IdUpdatedBy",
                        column: x => x.IdUpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RiceSacks",
                columns: table => new
                {
                    IdSack = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Weight = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    IdCreatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdUpdatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiceSacks", x => x.IdSack);
                    table.ForeignKey(
                        name: "FK_RiceSacks_AspNetUsers_IdCreatedBy",
                        column: x => x.IdCreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RiceSacks_AspNetUsers_IdUpdatedBy",
                        column: x => x.IdUpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RiceSacksDevolutionTypes",
                columns: table => new
                {
                    IdRiceSackDevolutionType = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    IdCreatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdUpdatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiceSacksDevolutionTypes", x => x.IdRiceSackDevolutionType);
                    table.ForeignKey(
                        name: "FK_RiceSacksDevolutionTypes_AspNetUsers_IdCreatedBy",
                        column: x => x.IdCreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RiceSacksDevolutionTypes_AspNetUsers_IdUpdatedBy",
                        column: x => x.IdUpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RiceSacksOutputTypes",
                columns: table => new
                {
                    IdRiceSacksOutputType = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    IdCreatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdUpdatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiceSacksOutputTypes", x => x.IdRiceSacksOutputType);
                    table.ForeignKey(
                        name: "FK_RiceSacksOutputTypes_AspNetUsers_IdCreatedBy",
                        column: x => x.IdCreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RiceSacksOutputTypes_AspNetUsers_IdUpdatedBy",
                        column: x => x.IdUpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    IdZone = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Length = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Width = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    IdCreatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdUpdatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.IdZone);
                    table.ForeignKey(
                        name: "FK_Zones_AspNetUsers_IdCreatedBy",
                        column: x => x.IdCreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Zones_AspNetUsers_IdUpdatedBy",
                        column: x => x.IdUpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    IdNotification = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Message = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WasRead = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IdSendTo = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdNotificationType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.IdNotification);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_IdSendTo",
                        column: x => x.IdSendTo,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_NotificationTypes_IdNotificationType",
                        column: x => x.IdNotificationType,
                        principalTable: "NotificationTypes",
                        principalColumn: "IdNotificationType",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RiceClassifications",
                columns: table => new
                {
                    IdClassification = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MinimunStock = table.Column<int>(type: "int", nullable: false),
                    MaximunStock = table.Column<int>(type: "int", nullable: false),
                    SacksPerLot = table.Column<int>(type: "int", nullable: false),
                    IdRiceClass = table.Column<int>(type: "int", nullable: false),
                    IdRiceGrade = table.Column<int>(type: "int", nullable: false),
                    IdRiceSack = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    IdCreatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdUpdatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiceClassifications", x => x.IdClassification);
                    table.ForeignKey(
                        name: "FK_RiceClassifications_AspNetUsers_IdCreatedBy",
                        column: x => x.IdCreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RiceClassifications_AspNetUsers_IdUpdatedBy",
                        column: x => x.IdUpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RiceClassifications_RiceClasses_IdRiceClass",
                        column: x => x.IdRiceClass,
                        principalTable: "RiceClasses",
                        principalColumn: "IdClass",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiceClassifications_RiceGrades_IdRiceGrade",
                        column: x => x.IdRiceGrade,
                        principalTable: "RiceGrades",
                        principalColumn: "IdGrade",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiceClassifications_RiceSacks_IdRiceSack",
                        column: x => x.IdRiceSack,
                        principalTable: "RiceSacks",
                        principalColumn: "IdSack",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RiceSacksDevolutions",
                columns: table => new
                {
                    IdRiceSacksDevolution = table.Column<int>(type: "int", nullable: false),
                    RiceSacksDevolutionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Observation = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdRiceSacksDevolutionType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    IdCreatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdUpdatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiceSacksDevolutions", x => x.IdRiceSacksDevolution);
                    table.ForeignKey(
                        name: "FK_RiceSacksDevolutions_AspNetUsers_IdCreatedBy",
                        column: x => x.IdCreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RiceSacksDevolutions_AspNetUsers_IdUpdatedBy",
                        column: x => x.IdUpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RiceSacksDevolutions_RiceSacksDevolutionTypes_IdRiceSacksDev~",
                        column: x => x.IdRiceSacksDevolution,
                        principalTable: "RiceSacksDevolutionTypes",
                        principalColumn: "IdRiceSackDevolutionType",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RiceSacksOutputs",
                columns: table => new
                {
                    IdRiceSacksOutput = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RiceSacksOutputDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Observation = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OutputEvidence = table.Column<byte[]>(type: "longblob", nullable: true),
                    IdRiceSacksOutputType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    IdCreatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdUpdatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiceSacksOutputs", x => x.IdRiceSacksOutput);
                    table.ForeignKey(
                        name: "FK_RiceSacksOutputs_AspNetUsers_IdCreatedBy",
                        column: x => x.IdCreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RiceSacksOutputs_AspNetUsers_IdUpdatedBy",
                        column: x => x.IdUpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RiceSacksOutputs_RiceSacksOutputTypes_IdRiceSacksOutputType",
                        column: x => x.IdRiceSacksOutputType,
                        principalTable: "RiceSacksOutputTypes",
                        principalColumn: "IdRiceSacksOutputType",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ubications",
                columns: table => new
                {
                    IdUbication = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Row = table.Column<int>(type: "int", nullable: false),
                    Column = table.Column<int>(type: "int", nullable: false),
                    IdZone = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    IdCurrentRiceLot = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ubications", x => x.IdUbication);
                    table.ForeignKey(
                        name: "FK_Ubications_Zones_IdZone",
                        column: x => x.IdZone,
                        principalTable: "Zones",
                        principalColumn: "IdZone",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RiceLots",
                columns: table => new
                {
                    IdLot = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReceptionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    InitialQuantity = table.Column<int>(type: "int", nullable: false),
                    LeftoverQuantity = table.Column<int>(type: "int", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TechnicalSpecification = table.Column<byte[]>(type: "longblob", nullable: true),
                    IdClassification = table.Column<int>(type: "int", nullable: false),
                    IdLastUbication = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    IdCreatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdUpdatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiceLots", x => x.IdLot);
                    table.ForeignKey(
                        name: "FK_RiceLots_AspNetUsers_IdCreatedBy",
                        column: x => x.IdCreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RiceLots_AspNetUsers_IdUpdatedBy",
                        column: x => x.IdUpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RiceLots_RiceClassifications_IdClassification",
                        column: x => x.IdClassification,
                        principalTable: "RiceClassifications",
                        principalColumn: "IdClassification",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiceLots_Ubications_IdLastUbication",
                        column: x => x.IdLastUbication,
                        principalTable: "Ubications",
                        principalColumn: "IdUbication",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RiceLotMovements",
                columns: table => new
                {
                    IdRiceLotMovement = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RiceLotMovementDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Observation = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdRiceLot = table.Column<int>(type: "int", nullable: false),
                    IdOrigin = table.Column<int>(type: "int", nullable: false),
                    IdDestination = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    IdCreatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdUpdatedBy = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiceLotMovements", x => x.IdRiceLotMovement);
                    table.ForeignKey(
                        name: "FK_RiceLotMovements_AspNetUsers_IdCreatedBy",
                        column: x => x.IdCreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RiceLotMovements_AspNetUsers_IdUpdatedBy",
                        column: x => x.IdUpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RiceLotMovements_RiceLots_IdRiceLot",
                        column: x => x.IdRiceLot,
                        principalTable: "RiceLots",
                        principalColumn: "IdLot",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiceLotMovements_Ubications_IdDestination",
                        column: x => x.IdDestination,
                        principalTable: "Ubications",
                        principalColumn: "IdUbication",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiceLotMovements_Ubications_IdOrigin",
                        column: x => x.IdOrigin,
                        principalTable: "Ubications",
                        principalColumn: "IdUbication",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RiceSacksDevolutionDetails",
                columns: table => new
                {
                    IdRiceSacksDevolutionDetail = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SacksQuantity = table.Column<int>(type: "int", nullable: false),
                    IdRiceSacksDevolution = table.Column<int>(type: "int", nullable: false),
                    IdRiceLot = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiceSacksDevolutionDetails", x => x.IdRiceSacksDevolutionDetail);
                    table.ForeignKey(
                        name: "FK_RiceSacksDevolutionDetails_RiceLots_IdRiceLot",
                        column: x => x.IdRiceLot,
                        principalTable: "RiceLots",
                        principalColumn: "IdLot",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiceSacksDevolutionDetails_RiceSacksDevolutions_IdRiceSacksD~",
                        column: x => x.IdRiceSacksDevolution,
                        principalTable: "RiceSacksDevolutions",
                        principalColumn: "IdRiceSacksDevolution",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RiceSacksOutputDetails",
                columns: table => new
                {
                    IdRiceSacksOutputDetail = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SacksQuantity = table.Column<int>(type: "int", nullable: false),
                    IdRiceSacksOutput = table.Column<int>(type: "int", nullable: false),
                    IdRiceLot = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiceSacksOutputDetails", x => x.IdRiceSacksOutputDetail);
                    table.ForeignKey(
                        name: "FK_RiceSacksOutputDetails_RiceLots_IdRiceLot",
                        column: x => x.IdRiceLot,
                        principalTable: "RiceLots",
                        principalColumn: "IdLot",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiceSacksOutputDetails_RiceSacksOutputs_IdRiceSacksOutput",
                        column: x => x.IdRiceSacksOutput,
                        principalTable: "RiceSacksOutputs",
                        principalColumn: "IdRiceSacksOutput",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_IdNotificationType",
                table: "Notifications",
                column: "IdNotificationType");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_IdSendTo",
                table: "Notifications",
                column: "IdSendTo");

            migrationBuilder.CreateIndex(
                name: "IX_RiceClasses_IdCreatedBy",
                table: "RiceClasses",
                column: "IdCreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiceClasses_IdUpdatedBy",
                table: "RiceClasses",
                column: "IdUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiceClassifications_IdCreatedBy",
                table: "RiceClassifications",
                column: "IdCreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiceClassifications_IdRiceClass",
                table: "RiceClassifications",
                column: "IdRiceClass");

            migrationBuilder.CreateIndex(
                name: "IX_RiceClassifications_IdRiceGrade",
                table: "RiceClassifications",
                column: "IdRiceGrade");

            migrationBuilder.CreateIndex(
                name: "IX_RiceClassifications_IdRiceSack",
                table: "RiceClassifications",
                column: "IdRiceSack");

            migrationBuilder.CreateIndex(
                name: "IX_RiceClassifications_IdUpdatedBy",
                table: "RiceClassifications",
                column: "IdUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiceGrades_IdCreatedBy",
                table: "RiceGrades",
                column: "IdCreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiceGrades_IdUpdatedBy",
                table: "RiceGrades",
                column: "IdUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiceLotMovements_IdCreatedBy",
                table: "RiceLotMovements",
                column: "IdCreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiceLotMovements_IdDestination",
                table: "RiceLotMovements",
                column: "IdDestination");

            migrationBuilder.CreateIndex(
                name: "IX_RiceLotMovements_IdOrigin",
                table: "RiceLotMovements",
                column: "IdOrigin");

            migrationBuilder.CreateIndex(
                name: "IX_RiceLotMovements_IdRiceLot",
                table: "RiceLotMovements",
                column: "IdRiceLot");

            migrationBuilder.CreateIndex(
                name: "IX_RiceLotMovements_IdUpdatedBy",
                table: "RiceLotMovements",
                column: "IdUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiceLots_IdClassification",
                table: "RiceLots",
                column: "IdClassification");

            migrationBuilder.CreateIndex(
                name: "IX_RiceLots_IdCreatedBy",
                table: "RiceLots",
                column: "IdCreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiceLots_IdLastUbication",
                table: "RiceLots",
                column: "IdLastUbication",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RiceLots_IdUpdatedBy",
                table: "RiceLots",
                column: "IdUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiceSacks_IdCreatedBy",
                table: "RiceSacks",
                column: "IdCreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiceSacks_IdUpdatedBy",
                table: "RiceSacks",
                column: "IdUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiceSacksDevolutionDetails_IdRiceLot",
                table: "RiceSacksDevolutionDetails",
                column: "IdRiceLot");

            migrationBuilder.CreateIndex(
                name: "IX_RiceSacksDevolutionDetails_IdRiceSacksDevolution",
                table: "RiceSacksDevolutionDetails",
                column: "IdRiceSacksDevolution");

            migrationBuilder.CreateIndex(
                name: "IX_RiceSacksDevolutions_IdCreatedBy",
                table: "RiceSacksDevolutions",
                column: "IdCreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiceSacksDevolutions_IdUpdatedBy",
                table: "RiceSacksDevolutions",
                column: "IdUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiceSacksDevolutionTypes_IdCreatedBy",
                table: "RiceSacksDevolutionTypes",
                column: "IdCreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiceSacksDevolutionTypes_IdUpdatedBy",
                table: "RiceSacksDevolutionTypes",
                column: "IdUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiceSacksOutputDetails_IdRiceLot",
                table: "RiceSacksOutputDetails",
                column: "IdRiceLot");

            migrationBuilder.CreateIndex(
                name: "IX_RiceSacksOutputDetails_IdRiceSacksOutput",
                table: "RiceSacksOutputDetails",
                column: "IdRiceSacksOutput");

            migrationBuilder.CreateIndex(
                name: "IX_RiceSacksOutputs_IdCreatedBy",
                table: "RiceSacksOutputs",
                column: "IdCreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiceSacksOutputs_IdRiceSacksOutputType",
                table: "RiceSacksOutputs",
                column: "IdRiceSacksOutputType");

            migrationBuilder.CreateIndex(
                name: "IX_RiceSacksOutputs_IdUpdatedBy",
                table: "RiceSacksOutputs",
                column: "IdUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiceSacksOutputTypes_IdCreatedBy",
                table: "RiceSacksOutputTypes",
                column: "IdCreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiceSacksOutputTypes_IdUpdatedBy",
                table: "RiceSacksOutputTypes",
                column: "IdUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Ubications_IdZone",
                table: "Ubications",
                column: "IdZone");

            migrationBuilder.CreateIndex(
                name: "IX_Zones_IdCreatedBy",
                table: "Zones",
                column: "IdCreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Zones_IdUpdatedBy",
                table: "Zones",
                column: "IdUpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "RiceLotMovements");

            migrationBuilder.DropTable(
                name: "RiceSacksDevolutionDetails");

            migrationBuilder.DropTable(
                name: "RiceSacksOutputDetails");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropTable(
                name: "RiceSacksDevolutions");

            migrationBuilder.DropTable(
                name: "RiceLots");

            migrationBuilder.DropTable(
                name: "RiceSacksOutputs");

            migrationBuilder.DropTable(
                name: "RiceSacksDevolutionTypes");

            migrationBuilder.DropTable(
                name: "RiceClassifications");

            migrationBuilder.DropTable(
                name: "Ubications");

            migrationBuilder.DropTable(
                name: "RiceSacksOutputTypes");

            migrationBuilder.DropTable(
                name: "RiceClasses");

            migrationBuilder.DropTable(
                name: "RiceGrades");

            migrationBuilder.DropTable(
                name: "RiceSacks");

            migrationBuilder.DropTable(
                name: "Zones");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
