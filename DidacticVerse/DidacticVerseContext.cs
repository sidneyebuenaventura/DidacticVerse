using Bogus;
using Microsoft.EntityFrameworkCore;
using DidacticVerse.Models;
using EfVueMantle;
using EfVueMantle.Helpers;
using DidacticVerse.Accounts;
using Bogus.DataSets;
using Microsoft.AspNetCore.Identity;
using EfVueMantle.Extensions;
using DidacticVerse.Discussions;
using Newtonsoft.Json;
//using EfVueMantle.Helpers;

namespace DidacticVerse;

public class DidacticVerseContext : DbContext
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly AccountProvider _accountProvider;
    public DidacticVerseContext(
        DbContextOptions<DidacticVerseContext> options,
        IWebHostEnvironment hostingEnvironment,
        IHttpContextAccessor httpContextAccessor,
        AccountProvider accountProvider

        ) : base(options)
    {
        _hostingEnvironment = hostingEnvironment;
        _contextAccessor = httpContextAccessor;
        _accountProvider = accountProvider;
    }

    public DbSet<AccountModel> Accounts { get; set; } = null!;
    public DbSet<SignupModel> Signups { get; set; } = null!;
    public DbSet<RefreshTokens> Refreshes { get; set; } = null!;

    public DbSet<BetaFeedbackModel> BetaFeedbacks { get; set; } = null!;

    public DbSet<CommentModel> Comments { get; set; } = null!;
    public DbSet<CommentHiddenModel> CommentHides { get; set; } = null!;
    public DbSet<CommentReportModel> CommentReports { get; set; } = null!;

    public DbSet<DailyDiscussionModel> DailyDiscussion { get; set; } = null!;
    public DbSet<DiscussionModel> Discussions { get; set; } = null!;
    public DbSet<DiscussionReportModel> DiscussionReports { get; set; } = null!;
    public DbSet<DiscussionHiddenModel> DiscussionHides { get; set; } = null!;
    public DbSet<DiscussionVoteModel> DiscussionVotes { get; set; } = null!;
    public DbSet<DiscussionTopicModel> DiscussionTopics { get; set; } = null!;

    public DbSet<ImageModel> Images { get; set; } = null!;
    
    public DbSet<LocationModel> Locations { get; set; } = null!;

    public DbSet<UserProfileHiddenModel> UserProfileHides { get; set; } = null!;
    
    public DbSet<TopicModel> Topics { get; set; } = null!;

    public string hasher(int id, string password)
    {

        var hasher = new PasswordHasher<IdentityUser>();
        IdentityUser identityUser = new IdentityUser(id.ToString());

        return hasher.HashPassword(identityUser, password);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //Setup all the special primary keys
        builder.Entity<CommentHiddenModel>()
            .HasKey(nameof(CommentHiddenModel.CommentId), nameof(CommentHiddenModel.UserId));

        builder.Entity<DiscussionHiddenModel>()
            .HasKey(nameof(DiscussionHiddenModel.DiscussionId), nameof(DiscussionHiddenModel.UserId));
        builder.Entity<DiscussionVoteModel>()
            .HasKey(nameof(DiscussionVoteModel.DiscussionId), nameof(DiscussionVoteModel.UserId));
        builder.Entity<DiscussionTopicModel>()
            .HasKey(nameof(DiscussionTopicModel.DiscussionsId), nameof(DiscussionTopicModel.TopicsId));
        builder.Entity<DiscussionImageModel>()
            .HasKey(nameof(DiscussionImageModel.DiscussionsId), nameof(DiscussionImageModel.ImagesId));

        builder.Entity<UserProfileHiddenModel>()
            .HasKey(nameof(UserProfileHiddenModel.HiddenUserId), nameof(UserProfileHiddenModel.UserId));


        builder.Entity<AccountModel>()
            .HasIndex(x => x.EmailAddress)
            .HasFilter("Subject IS NULL")
            .IsUnique();
        builder.Entity<AccountModel>()
            .HasOne(x => x.Avatar);
        builder.Entity<AccountModel>()
            .WithProjection(
                x => x.DiscussionTopicsIds,
                x => JsonConvert.DeserializeObject<List<long>>(x.DiscussionTopics) ?? new List<long>()
            );

        builder.Entity<SignupModel>()
            .HasIndex(x => x.EmailAddress)
            .IsUnique();

        builder.Entity<CommentModel>()
            .HasQueryFilter(x => 
                !CommentHides.Any(y => y.CommentId == x.Id && y.UserId == _accountProvider.GetAccountId())
                && !UserProfileHides.Any(y => 
                    y.HiddenUserId == x.UserId && y.UserId == _accountProvider.GetAccountId()
                    || y.UserId == x.UserId && y.HiddenUserId == _accountProvider.GetAccountId()
                )
            );

        builder.Entity<CommentModel>()
            .HasOne(x => x.ParentComment)
            .WithMany(x => x.ChildComments);
        builder.Entity<CommentModel>()
            .WithProjection(
                x => x.ChildCommentsIds,
                x => x.ChildComments.Select(y => y.Id).ToList()
            );

        builder.Entity<CommentReportModel>()
            .HasOne(x => x.Comment)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<CommentReportModel>()
            .HasOne(x => x.ReportingUser)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<CommentReportModel>()
            .HasQueryFilter(x =>
                !CommentHides.Any(y => y.CommentId == x.Id && y.UserId == _accountProvider.GetAccountId())
                && !UserProfileHides.Any(y =>
                    y.HiddenUserId == x.Comment.UserId && y.UserId == _accountProvider.GetAccountId()
                    || y.UserId == x.Comment.UserId && y.HiddenUserId == _accountProvider.GetAccountId()
                )
            );


        builder.Entity<DiscussionModel>()
            .HasMany(x => x.Comments)
            .WithOne(x => x.Discussion)
            .HasForeignKey(x => x.DiscussionId);
        builder.Entity<DiscussionModel>()
            .HasMany(x => x.DiscussionVotes)
            .WithOne()
            .HasForeignKey(x => x.DiscussionId);
        builder.Entity<DiscussionModel>()
            .HasMany(x => x.Images);

        builder.Entity<DiscussionModel>()
            .HasMany(x => x.Topics)
            .WithMany(x => x.Discussions)
            .UsingEntity<DiscussionTopicModel>();

        builder.Entity<DiscussionModel>()
            .HasQueryFilter(x => 
                !DiscussionHides.Any(y => y.DiscussionId.Equals(x.Id) && y.UserId == 1)
                && !UserProfileHides.Any(y =>
                    y.HiddenUserId == x.UserId && y.UserId == _accountProvider.GetAccountId()
                    || y.UserId == x.UserId && y.HiddenUserId == _accountProvider.GetAccountId()
                )
            );
        builder.Entity<DiscussionModel>()
            .WithProjection(
                x => x.CommentsIds,
                x => x.Comments.Where(y => y.ParentCommentId == null).Select(y => y.Id).ToList()
            );
        builder.Entity<DiscussionModel>()
            .WithProjection(
                x => x.CommentsCount,
                x => x.Comments.Count
            );
        builder.Entity<DiscussionModel>()
            .WithProjection(
                x => x.DiscussionVoteCount,
                x => x.DiscussionVotes.Count
            );
        builder.Entity<DiscussionModel>()
            .WithProjection(
                x => x.TopicsIds,
                x => x.Topics.Select(y => y.Id).ToList()
            );
        builder.Entity<DiscussionModel>()
            .WithProjection(
                x => x.ImagesIds,
                x => x.Images.Select(y => y.Id).ToList()
            );
        builder.Entity<DiscussionModel>()
            .WithProjection(
                x => x.CurrentDiscussionVote,
                x => x.DiscussionVotes.Any(y => y.UserId == _accountProvider.GetAccountId())
            );
        builder.Entity<DiscussionModel>()
            .WithProjection(
                x => x.Weight,
                x => (float)((10 + x.DiscussionVotes.Count * 2 + x.Comments.Count * 10) / (DateTime.Now - x.Created).TotalHours)
            );

        builder.Entity<DiscussionReportModel>()
            .HasOne(x => x.Discussion)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<DiscussionReportModel>()
            .HasOne(x => x.ReportingUser)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<DiscussionReportModel>()
            .HasQueryFilter(x =>
                !DiscussionHides.Any(y => y.DiscussionId == x.Id && y.UserId == 1)
                && !UserProfileHides.Any(y =>
                    y.HiddenUserId == x.Discussion.UserId && y.UserId == _accountProvider.GetAccountId()
                    || y.UserId == x.Discussion.UserId && y.HiddenUserId == _accountProvider.GetAccountId()
                )
            );

        builder.Entity<DiscussionVoteModel>()
            .HasOne(x => x.User)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<UserProfileHiddenModel>()
            .HasOne(x => x.User)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<UserProfileHiddenModel>()
            .HasOne(x => x.HiddenUser)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);




        /// SEED DATA///

        var topicList = new Dictionary<string, string>()
        {
            {"Career", "fa-duotone fa-briefcase"},
            {"Job opening", "fa-solid fa-handshake-simple"},
            {"Parent", "fa-solid fa-hands-holding-child"},
            {"Tech", "fa-solid fa-gears"},
            {"Tool", "fa-solid fa-screwdriver-wrench"},
            {"Business", "fa-solid fa-briefcase"},
            {"Entrepreneur", "fa-solid fa-user-tie-hair-long"},
            {"Remote", "fa-solid fa-house-laptop"},
            {"Commute", "fa-solid fa-car"},
            {"Entertain", "fa-solid fa-pinball"},
            {"Hobby", "fa-solid fa-game-console-handheld"},
            {"Event", "fa-solid fa-calendar-lines-pen"},
            {"Networking", "fa-solid fa-message-plus"},
            {"Educate", "fa-solid fa-graduation-cap"},
            {"Inspire", "fa-solid fa-lightbulb-on"},
            {"Help", "fa-solid fa-handshake-angle"},
            {"Give", "fa-solid fa-gift-card"},
            {"Memory", "fa-solid fa-thought-bubble"},
            {"Celebrate", "fa-solid fa-party-horn"},
            {"Idea", "fa-solid fa-lightbulb-exclamation-on"},
            {"Tip", "fa-solid fa-message-quote"},
            {"Personal", "fa-solid fa-briefcase"},
            {"Health", "fa-solid fa-notes-medical"},
            {"Food", "fa-solid fa-pot-food"},
            {"Fitness", "fa-solid fa-person-from-portal"},
            {"Funny", "fa-solid fa-face-laugh-beam"},
            {"Ad", "fa-solid fa-rectangle-ad"},
        };
        topicList = topicList.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        long topicId = 1;
        var topicData = new Faker<TopicModel>()
            .RuleFor(m => m.Id, f => topicId++)
            .RuleFor(m => m.Title, f => topicList.ElementAt((int)(topicId - 2)).Key)
            .RuleFor(m => m.Icon, f => topicList.ElementAt((int)(topicId - 2)).Value)
            .Generate(topicList.Count);

        builder.Entity<TopicModel>().HasData(topicData);

        /*

        Randomizer.Seed = new Random(7777777);
        var faker = new Faker("en");


        int imageId = 1;
        var images = new Faker<ImageModel>()
            .RuleFor(m => m.Id, f => imageId++)
            .RuleFor(m => m.Url, f => f.Person.Avatar)
            .RuleFor(m => m.Key, f => f.Person.Avatar);
        var imageData = images.Generate(200);


        int accountId = 1;
        var accountData = new Faker<AccountModel>()
            .RuleFor(m => m.Id, f => accountId++)
            .RuleFor(m => m.Birthdate, f => f.Date.Past(50))
            .RuleFor(m => m.FirstName, f => f.Person.FirstName)
            .RuleFor(m => m.LastName, f => f.Person.LastName)
            .RuleFor(m => m.EmailAddress, f => f.Person.Email)
            .RuleFor(m => m.HashedPassword, f => hasher(accountId - 1, f.Person.UserName))
            .Generate(100);


        int userProfileId = 1;
        var userProfileData = new Faker<UserProfileModel>()
            .RuleFor(m => m.Id, f => userProfileId++)
            .RuleFor(m => m.DisplayName, f => f.Person.FullName)
            .RuleFor(m => m.AvatarId, f => userProfileId - 1)
            .RuleFor(m => m.AccountId, f => userProfileId - 1)
            .Generate(100);

        var icons = new string[]
        {
            "fa-solid fa-tower-cell",
            "fa-solid fa-stairs",
            "fa-solid fa-trowel",
            "fa-solid fa-shield-cat",
            "fa-solid fa-shield-dog",
            "fa-solid fa-person-walking-luggage",
            "fa-solid fa-person-pregnant",
            "fa-solid fa-jet-fighter-up",
        };






        if (_hostingEnvironment.IsDevelopment())
        {


            int commentId = 1;
            var comments = new Faker<CommentModel>()
                .RuleFor(m => m.Id, f => commentId++)
                .RuleFor(m => m.UserId, f => f.PickRandom(userProfileData).Id)
                .RuleFor(m => m.Content, f => f.Commerce.ProductDescription());
            var commentData = new List<CommentModel>();

            int discussionId = 1;
            var discussionData = new Faker<DiscussionModel>()
                .RuleFor(m => m.Id, f => discussionId++)
                .RuleFor(m => m.UserId, f => f.PickRandom(userProfileData).Id)
                .RuleFor(m => m.Title, f => f.Company.CatchPhrase())
                .RuleFor(m => m.Content, f => $"{f.Commerce.ProductDescription()}\r\n{f.Commerce.ProductDescription()}")
                .Generate(30);

            var discussionVotes = new Faker<DiscussionVoteModel>()
                .RuleFor(m => m.UserId, f => f.PickRandom(userProfileData).Id);
            var discussionVoteData = new List<DiscussionVoteModel>();
            var discussionImageData = new List<DiscussionImageModel>();

            foreach (var discussion in discussionData)
            {
                commentData.AddRange(comments.RuleFor(m => m.DiscussionId, f => discussion.Id).Generate(faker.Random.Int(0, 25)));
                discussionVoteData.AddRange(discussionVotes.RuleFor(m => m.DiscussionId, f => discussion.Id).Generate(faker.Random.Int(0, 50)));
            }

            var moreComments = new List<CommentModel>();
            foreach (var comment in commentData)
            {
                moreComments.AddRange(comments
                    .RuleFor(m => m.DiscussionId, f => comment.DiscussionId)
                    .RuleFor(m => m.ParentCommentId, f => comment.Id)
                    .Generate(faker.Random.Int(0, 3)));
            }
            commentData.AddRange(moreComments);

            foreach (DiscussionModel discussion in discussionData)
            {
                var topicIds = new List<int>();
                for (var i = 0; i < faker.Random.Int(1, 3); i++)
                {
                    topicIds.Add(faker.PickRandom(topicData).Id);
                }
                topicIds = topicIds.Distinct().ToList();
                foreach (var id in topicIds)
                {
                    discussionTopicData.Add(new
                    {
                        DiscussionsId = discussion.Id,
                        TopicsId = id
                    });
                }

                if (faker.Random.Int(0, 100) < 20) {

                    for (int i = 0, l = faker.Random.Int(0,5); i < l; i++)
                    {
                        imageData.AddRange(images.Generate(1));
                        var discussionImages = new Faker<DiscussionImageModel>()
                            .RuleFor(m => m.DiscussionId, discussion.Id)
                            .RuleFor(m => m.ImageId, m => imageData.Last().Id);
                        discussionImageData.AddRange(discussionImages.Generate(1));
                    }

                }
            }
            discussionVoteData = discussionVoteData.DistinctBy(x => new { x.DiscussionId, x.UserId }).ToList();


            //builder.Entity<CommentModel>().HasData(commentData);
            builder.Entity<AccountModel>().HasData(accountData);
            builder.Entity<ImageModel>().HasData(imageData);
            builder.Entity<DiscussionModel>().HasData(discussionData);
            builder.Entity<DiscussionVoteModel>().HasData(discussionVoteData);
            builder.Entity<UserProfileModel>().HasData(userProfileData);
            builder.Entity<CommentModel>().HasData(commentData);

        } 


        */
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ChangeTracker.SetAuditProperties<long>();
        return await base.SaveChangesAsync(cancellationToken);
    }
    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ChangeTracker.SetAuditProperties<long>();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    public override int SaveChanges()
    {
        ChangeTracker.SetAuditProperties<long>();
        return base.SaveChanges();
    }
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ChangeTracker.SetAuditProperties<long>();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }



}