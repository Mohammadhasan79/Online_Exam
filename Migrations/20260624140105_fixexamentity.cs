using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineExam.Migrations
{
    /// <inheritdoc />
    public partial class fixexamentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "ExamLists");

            migrationBuilder.AddColumn<int>(
                name: "ExamTime",
                table: "Exam",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamTime",
                table: "Exam");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "ExamLists",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
