using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemplateApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitalData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText("Database/seeds/seed_data.sql"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
