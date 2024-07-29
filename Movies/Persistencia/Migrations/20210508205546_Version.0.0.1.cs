using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistencia.Migrations
{
    public partial class Version001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActorDirector",
                columns: table => new
                {
                    ActorDirectorID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: true),
                    EsActor = table.Column<bool>(nullable: false),
                    EsDirector = table.Column<bool>(nullable: false),
                    URLFoto = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorDirector", x => x.ActorDirectorID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    NombreCompleto = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documento",
                columns: table => new
                {
                    DocumentoId = table.Column<Guid>(nullable: false),
                    ObjetoReferencia = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    Contenido = table.Column<byte[]>(nullable: true),
                    FechaCreacion = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documento", x => x.DocumentoId);
                });

            migrationBuilder.CreateTable(
                name: "EstadoAlquiler",
                columns: table => new
                {
                    EstadoAquilerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoAlquiler", x => x.EstadoAquilerID);
                });

            migrationBuilder.CreateTable(
                name: "Pelicula",
                columns: table => new
                {
                    PeliculaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(nullable: true),
                    Descripcion = table.Column<string>(nullable: true),
                    CostoAlquiler = table.Column<int>(nullable: false),
                    CantidadPeliculas = table.Column<int>(nullable: false),
                    Estrellas = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    URLPelicula = table.Column<string>(nullable: true),
                    Disponible = table.Column<bool>(nullable: false),
                    Lanzamiento = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pelicula", x => x.PeliculaID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "PeliculaActor",
                columns: table => new
                {
                    ActorDirectorID = table.Column<int>(nullable: false),
                    PeliculaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeliculaActor", x => new { x.PeliculaID, x.ActorDirectorID });
                    table.ForeignKey(
                        name: "FK_PeliculaActor_ActorDirector_ActorDirectorID",
                        column: x => x.ActorDirectorID,
                        principalTable: "ActorDirector",
                        principalColumn: "ActorDirectorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeliculaActor_Pelicula_PeliculaID",
                        column: x => x.PeliculaID,
                        principalTable: "Pelicula",
                        principalColumn: "PeliculaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PeliculaAlquiler",
                columns: table => new
                {
                    PeliculaAlquilerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(nullable: true),
                    FechaAlquiler = table.Column<DateTime>(nullable: false),
                    FechaEntrega = table.Column<DateTime>(nullable: false),
                    FechaDebeEntregar = table.Column<DateTime>(nullable: true),
                    FechaCancelacion = table.Column<DateTime>(nullable: true),
                    PeliculaID = table.Column<int>(nullable: false),
                    EstadoAquilerID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeliculaAlquiler", x => x.PeliculaAlquilerID);
                    table.ForeignKey(
                        name: "FK_PeliculaAlquiler_EstadoAlquiler_EstadoAquilerID",
                        column: x => x.EstadoAquilerID,
                        principalTable: "EstadoAlquiler",
                        principalColumn: "EstadoAquilerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeliculaAlquiler_Pelicula_PeliculaID",
                        column: x => x.PeliculaID,
                        principalTable: "Pelicula",
                        principalColumn: "PeliculaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PeliculaDirector",
                columns: table => new
                {
                    ActorDirectorID = table.Column<int>(nullable: false),
                    PeliculaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeliculaDirector", x => new { x.PeliculaID, x.ActorDirectorID });
                    table.ForeignKey(
                        name: "FK_PeliculaDirector_ActorDirector_ActorDirectorID",
                        column: x => x.ActorDirectorID,
                        principalTable: "ActorDirector",
                        principalColumn: "ActorDirectorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeliculaDirector_Pelicula_PeliculaID",
                        column: x => x.PeliculaID,
                        principalTable: "Pelicula",
                        principalColumn: "PeliculaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

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
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PeliculaActor_ActorDirectorID",
                table: "PeliculaActor",
                column: "ActorDirectorID");

            migrationBuilder.CreateIndex(
                name: "IX_PeliculaAlquiler_EstadoAquilerID",
                table: "PeliculaAlquiler",
                column: "EstadoAquilerID");

            migrationBuilder.CreateIndex(
                name: "IX_PeliculaAlquiler_PeliculaID",
                table: "PeliculaAlquiler",
                column: "PeliculaID");

            migrationBuilder.CreateIndex(
                name: "IX_PeliculaDirector_ActorDirectorID",
                table: "PeliculaDirector",
                column: "ActorDirectorID");
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
                name: "Documento");

            migrationBuilder.DropTable(
                name: "PeliculaActor");

            migrationBuilder.DropTable(
                name: "PeliculaAlquiler");

            migrationBuilder.DropTable(
                name: "PeliculaDirector");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "EstadoAlquiler");

            migrationBuilder.DropTable(
                name: "ActorDirector");

            migrationBuilder.DropTable(
                name: "Pelicula");
        }
    }
}
