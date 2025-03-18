using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemplateEventDriven.Migrations
{
    /// <inheritdoc />
    public partial class CreateViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText("Database/views/create_views.sql"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText("Database/views/drop_views.sql"));
        }
    }
}
