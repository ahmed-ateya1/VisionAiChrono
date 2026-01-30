using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisionAiChrono.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addpipelineRunVideo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PipelineRunVideos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PipelineRunId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VideoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PipelineRunVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PipelineRunVideos_PipelineRuns_PipelineRunId",
                        column: x => x.PipelineRunId,
                        principalTable: "PipelineRuns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PipelineRunVideos_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PipelineRunVideos_PipelineRunId",
                table: "PipelineRunVideos",
                column: "PipelineRunId");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineRunVideos_VideoId",
                table: "PipelineRunVideos",
                column: "VideoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PipelineRunVideos");
        }
    }
}
