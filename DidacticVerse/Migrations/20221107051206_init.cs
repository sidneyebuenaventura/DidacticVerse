using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace DidacticVerse.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyDiscussion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscussionId = table.Column<long>(type: "bigint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyDiscussion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscussionImageModel",
                columns: table => new
                {
                    DiscussionsId = table.Column<int>(type: "int", nullable: false),
                    ImagesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscussionImageModel", x => new { x.DiscussionsId, x.ImagesId });
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<Point>(type: "geography", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Refreshes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Consumed = table.Column<bool>(type: "bit", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    Invalidated = table.Column<bool>(type: "bit", nullable: false),
                    Reconsumption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReconsumptionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refreshes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Signups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Signups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Suspended = table.Column<bool>(type: "bit", nullable: true),
                    SuspendedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TermsConditions = table.Column<bool>(type: "bit", nullable: false),
                    TermsConditionsDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvatarId = table.Column<long>(type: "bigint", nullable: true),
                    DiscussionTopics = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BetaFeedbacks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportingUserId = table.Column<long>(type: "bigint", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BetaFeedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BetaFeedbacks_Accounts_ReportingUserId",
                        column: x => x.ReportingUserId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommentHides",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CommentId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentHides", x => new { x.CommentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CommentHides_Accounts_UserId",
                        column: x => x.UserId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscussionHides",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    DiscussionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscussionHides", x => new { x.DiscussionId, x.UserId });
                    table.ForeignKey(
                        name: "FK_DiscussionHides_Accounts_UserId",
                        column: x => x.UserId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Discussions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationId = table.Column<long>(type: "bigint", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discussions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discussions_Accounts_UserId",
                        column: x => x.UserId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Discussions_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserProfileHides",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    HiddenUserId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfileHides", x => new { x.HiddenUserId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserProfileHides_Accounts_HiddenUserId",
                        column: x => x.HiddenUserId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfileHides_Accounts_UserId",
                        column: x => x.UserId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscussionId = table.Column<long>(type: "bigint", nullable: true),
                    ParentCommentId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Accounts_UserId",
                        column: x => x.UserId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "Comments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Discussions_DiscussionId",
                        column: x => x.DiscussionId,
                        principalTable: "Discussions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DiscussionReports",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscussionId = table.Column<long>(type: "bigint", nullable: false),
                    ReportReason = table.Column<int>(type: "int", nullable: false),
                    ReportingUserId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscussionReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscussionReports_Accounts_ReportingUserId",
                        column: x => x.ReportingUserId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscussionReports_Discussions_DiscussionId",
                        column: x => x.DiscussionId,
                        principalTable: "Discussions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DiscussionTopics",
                columns: table => new
                {
                    DiscussionsId = table.Column<long>(type: "bigint", nullable: false),
                    TopicsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscussionTopics", x => new { x.DiscussionsId, x.TopicsId });
                    table.ForeignKey(
                        name: "FK_DiscussionTopics_Discussions_DiscussionsId",
                        column: x => x.DiscussionsId,
                        principalTable: "Discussions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscussionTopics_Topics_TopicsId",
                        column: x => x.TopicsId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscussionVotes",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    DiscussionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscussionVotes", x => new { x.DiscussionId, x.UserId });
                    table.ForeignKey(
                        name: "FK_DiscussionVotes_Accounts_UserId",
                        column: x => x.UserId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscussionVotes_Discussions_DiscussionId",
                        column: x => x.DiscussionId,
                        principalTable: "Discussions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscussionModelId = table.Column<long>(type: "bigint", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Discussions_DiscussionModelId",
                        column: x => x.DiscussionModelId,
                        principalTable: "Discussions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommentReports",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentId = table.Column<long>(type: "bigint", nullable: false),
                    ReportReason = table.Column<int>(type: "int", nullable: false),
                    ReportingUserId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentReports_Accounts_ReportingUserId",
                        column: x => x.ReportingUserId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentReports_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "Created", "Icon", "Title", "Updated" },
                values: new object[,]
                {
                    { 1L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4220), "fa-solid fa-user-tie-hair-long", "Entrepreneur", null },
                    { 2L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4474), "fa-solid fa-thought-bubble", "Memory", null },
                    { 3L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4509), "fa-solid fa-screwdriver-wrench", "Tool", null },
                    { 4L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4519), "fa-solid fa-rectangle-ad", "Ad", null },
                    { 5L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4527), "fa-solid fa-pot-food", "Food", null },
                    { 6L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4534), "fa-solid fa-pinball", "Entertain", null },
                    { 7L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4541), "fa-solid fa-person-from-portal", "Fitness", null },
                    { 8L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4548), "fa-solid fa-party-horn", "Celebrate", null },
                    { 9L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4554), "fa-solid fa-notes-medical", "Health", null },
                    { 10L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4563), "fa-solid fa-message-quote", "Tip", null },
                    { 11L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4570), "fa-solid fa-message-plus", "Networking", null },
                    { 12L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4577), "fa-solid fa-lightbulb-on", "Inspire", null },
                    { 13L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4585), "fa-solid fa-lightbulb-exclamation-on", "Idea", null },
                    { 14L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4593), "fa-solid fa-house-laptop", "Remote", null },
                    { 15L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4600), "fa-solid fa-handshake-simple", "Job opening", null },
                    { 16L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4608), "fa-solid fa-handshake-angle", "Help", null },
                    { 17L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4616), "fa-solid fa-hands-holding-child", "Parent", null },
                    { 18L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4624), "fa-solid fa-graduation-cap", "Educate", null },
                    { 19L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4632), "fa-solid fa-gift-card", "Give", null },
                    { 20L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4640), "fa-solid fa-gears", "Tech", null },
                    { 21L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4648), "fa-solid fa-game-console-handheld", "Hobby", null },
                    { 22L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4656), "fa-solid fa-face-laugh-beam", "Funny", null },
                    { 23L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4664), "fa-solid fa-car", "Commute", null },
                    { 24L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4672), "fa-solid fa-calendar-lines-pen", "Event", null },
                    { 25L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4680), "fa-solid fa-briefcase", "Business", null },
                    { 26L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4689), "fa-solid fa-briefcase", "Personal", null },
                    { 27L, new DateTime(2022, 11, 7, 0, 12, 6, 771, DateTimeKind.Local).AddTicks(4697), "fa-duotone fa-briefcase", "Career", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AvatarId",
                table: "Accounts",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_EmailAddress",
                table: "Accounts",
                column: "EmailAddress",
                unique: true,
                filter: "Subject IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BetaFeedbacks_ReportingUserId",
                table: "BetaFeedbacks",
                column: "ReportingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentHides_UserId",
                table: "CommentHides",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentReports_CommentId",
                table: "CommentReports",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentReports_ReportingUserId",
                table: "CommentReports",
                column: "ReportingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_DiscussionId",
                table: "Comments",
                column: "DiscussionId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentCommentId",
                table: "Comments",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionHides_UserId",
                table: "DiscussionHides",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionReports_DiscussionId",
                table: "DiscussionReports",
                column: "DiscussionId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionReports_ReportingUserId",
                table: "DiscussionReports",
                column: "ReportingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Discussions_LocationId",
                table: "Discussions",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Discussions_UserId",
                table: "Discussions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionTopics_TopicsId",
                table: "DiscussionTopics",
                column: "TopicsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionVotes_UserId",
                table: "DiscussionVotes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_DiscussionModelId",
                table: "Images",
                column: "DiscussionModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Signups_EmailAddress",
                table: "Signups",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileHides_UserId",
                table: "UserProfileHides",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Images_AvatarId",
                table: "Accounts",
                column: "AvatarId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Images_AvatarId",
                table: "Accounts");

            migrationBuilder.DropTable(
                name: "BetaFeedbacks");

            migrationBuilder.DropTable(
                name: "CommentHides");

            migrationBuilder.DropTable(
                name: "CommentReports");

            migrationBuilder.DropTable(
                name: "DailyDiscussion");

            migrationBuilder.DropTable(
                name: "DiscussionHides");

            migrationBuilder.DropTable(
                name: "DiscussionImageModel");

            migrationBuilder.DropTable(
                name: "DiscussionReports");

            migrationBuilder.DropTable(
                name: "DiscussionTopics");

            migrationBuilder.DropTable(
                name: "DiscussionVotes");

            migrationBuilder.DropTable(
                name: "Refreshes");

            migrationBuilder.DropTable(
                name: "Signups");

            migrationBuilder.DropTable(
                name: "UserProfileHides");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Discussions");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
