using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class snakeCase2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_events_event_types_TypeId",
                table: "events");

            migrationBuilder.DropForeignKey(
                name: "FK_events_genres_GenreId",
                table: "events");

            migrationBuilder.DropForeignKey(
                name: "FK_events_locations_LocationId",
                table: "events");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "locations",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "locations",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "locations",
                newName: "location_id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "genres",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "GenreId",
                table: "genres",
                newName: "genre_id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "events",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "events",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Author",
                table: "events",
                newName: "author");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "events",
                newName: "type_id");

            migrationBuilder.RenameColumn(
                name: "ShortDescription",
                table: "events",
                newName: "short_description");

            migrationBuilder.RenameColumn(
                name: "PreviewUrl",
                table: "events",
                newName: "preview_url");

            migrationBuilder.RenameColumn(
                name: "MinAge",
                table: "events",
                newName: "min_age");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "events",
                newName: "location_id");

            migrationBuilder.RenameColumn(
                name: "GenreId",
                table: "events",
                newName: "genre_id");

            migrationBuilder.RenameColumn(
                name: "DateStart",
                table: "events",
                newName: "date_start");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "events",
                newName: "event_id");

            migrationBuilder.RenameIndex(
                name: "IX_events_TypeId",
                table: "events",
                newName: "IX_events_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_events_LocationId",
                table: "events",
                newName: "IX_events_location_id");

            migrationBuilder.RenameIndex(
                name: "IX_events_GenreId",
                table: "events",
                newName: "IX_events_genre_id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "event_types",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "event_types",
                newName: "type_id");

            migrationBuilder.AddForeignKey(
                name: "FK_events_event_types_type_id",
                table: "events",
                column: "type_id",
                principalTable: "event_types",
                principalColumn: "type_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_events_genres_genre_id",
                table: "events",
                column: "genre_id",
                principalTable: "genres",
                principalColumn: "genre_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_events_locations_location_id",
                table: "events",
                column: "location_id",
                principalTable: "locations",
                principalColumn: "location_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_events_event_types_type_id",
                table: "events");

            migrationBuilder.DropForeignKey(
                name: "FK_events_genres_genre_id",
                table: "events");

            migrationBuilder.DropForeignKey(
                name: "FK_events_locations_location_id",
                table: "events");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "locations",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "locations",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "location_id",
                table: "locations",
                newName: "LocationId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "genres",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "genre_id",
                table: "genres",
                newName: "GenreId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "events",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "events",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "author",
                table: "events",
                newName: "Author");

            migrationBuilder.RenameColumn(
                name: "type_id",
                table: "events",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "short_description",
                table: "events",
                newName: "ShortDescription");

            migrationBuilder.RenameColumn(
                name: "preview_url",
                table: "events",
                newName: "PreviewUrl");

            migrationBuilder.RenameColumn(
                name: "min_age",
                table: "events",
                newName: "MinAge");

            migrationBuilder.RenameColumn(
                name: "location_id",
                table: "events",
                newName: "LocationId");

            migrationBuilder.RenameColumn(
                name: "genre_id",
                table: "events",
                newName: "GenreId");

            migrationBuilder.RenameColumn(
                name: "date_start",
                table: "events",
                newName: "DateStart");

            migrationBuilder.RenameColumn(
                name: "event_id",
                table: "events",
                newName: "EventId");

            migrationBuilder.RenameIndex(
                name: "IX_events_type_id",
                table: "events",
                newName: "IX_events_TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_events_location_id",
                table: "events",
                newName: "IX_events_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_events_genre_id",
                table: "events",
                newName: "IX_events_GenreId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "event_types",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "type_id",
                table: "event_types",
                newName: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_events_event_types_TypeId",
                table: "events",
                column: "TypeId",
                principalTable: "event_types",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_events_genres_GenreId",
                table: "events",
                column: "GenreId",
                principalTable: "genres",
                principalColumn: "GenreId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_events_locations_LocationId",
                table: "events",
                column: "LocationId",
                principalTable: "locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
