using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace News_Site.Models.Entity;

public partial class NewsSiteContext : DbContext
{
    public NewsSiteContext()
    {
    }

    public NewsSiteContext(DbContextOptions<NewsSiteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Action> Actions { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Emote> Emotes { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public DbSet<NewsEmote> NewsEmote { get; set; }

    public DbSet<UserNewsEmote> UserNewsEmotes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Action>(entity =>
        {
            entity.HasKey(e => e.ActionId).HasName("PK__Actions__74EFC217E75F6A34");

            entity.Property(e => e.ActionId).HasColumnName("action_id");
            entity.Property(e => e.ActionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("action_date");
            entity.Property(e => e.EmoteId).HasColumnName("emote_id");
            entity.Property(e => e.NewsId).HasColumnName("news_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Emote).WithMany(p => p.Actions)
                .HasForeignKey(d => d.EmoteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Actions_Emotes");

            entity.HasOne(d => d.News).WithMany(p => p.Actions)
                .HasForeignKey(d => d.NewsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Actions_News");

            entity.HasOne(d => d.User).WithMany(p => p.Actions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Actions_Users");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__D54EE9B45BD4DBF3");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryDescription).HasColumnName("category_description");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comments__E79576878C04A76F");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.CommentContent)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("comment_content");
            entity.Property(e => e.LoadingDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("loading_date");
            entity.Property(e => e.NewsId).HasColumnName("news_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.News).WithMany(p => p.Comments)
                .HasForeignKey(d => d.NewsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_News");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Comment_User");
        });

        modelBuilder.Entity<Emote>(entity =>
        {
            entity.HasKey(e => e.EmoteId).HasName("PK__Emotes__0BFC959FDC400658");

            entity.Property(e => e.EmoteId).HasColumnName("emote_id");
            entity.Property(e => e.EmoteName)
                .HasMaxLength(100)
                .HasColumnName("emote_name");
            entity.Property(e => e.EmotePath)
                .HasMaxLength(255)
                .HasColumnName("emote_path");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__Image__DC9AC955C77F8C03");

            entity.ToTable("Image");

            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.AddDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("add_date");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(255)
                .HasColumnName("image_path");
            entity.Property(e => e.NewsId).HasColumnName("news_id");

            entity.HasOne(d => d.News).WithMany(p => p.Images)
                .HasForeignKey(d => d.NewsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Image_News");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__4C27CCD8F8E33F78");

            entity.Property(e => e.NewsId).HasColumnName("news_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.Link)
                .HasMaxLength(255)
                .HasColumnName("link");
            entity.Property(e => e.LoadingDate)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("loading_date");
            entity.Property(e => e.MetaDescription)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("meta_description");
            entity.Property(e => e.NewsDetail).HasColumnName("news_detail");
            entity.Property(e => e.NewsTitle)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("news_title");

            entity.HasOne(d => d.Category).WithMany(p => p.News)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_News_Categories");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370F0A231FF7");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_email");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_name");
            entity.Property(e => e.UserSurname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_surname");
        });

        modelBuilder.Entity<NewsEmote>(entity =>
        {
            entity.HasKey(ne => ne.NewsEmoteId);
            entity.ToTable("NewsEmote");

            entity.Property(ne => ne.NewsEmoteId).HasColumnName("news_emote_id");
            entity.Property(ne => ne.NewsId).HasColumnName("news_id");
            entity.Property(ne => ne.EmoteId).HasColumnName("emote_id");
            entity.Property(ne => ne.ClickCount).HasColumnName("click_count");

            entity.HasOne(ne => ne.News)
                .WithMany(n => n.NewsEmote)
                .HasForeignKey(ne => ne.NewsId)
                .HasConstraintName("FK_NewsEmotes_News");

            entity.HasOne(ne => ne.Emote)
                .WithMany(e => e.NewsEmote)
                .HasForeignKey(ne => ne.EmoteId)
                .HasConstraintName("FK_NewsEmotes_Emotes");
        });

        modelBuilder.Entity<UserNewsEmote>(entity =>
        {
            entity.ToTable("UserNewsEmote");

            entity.HasKey(e => e.UserNewsEmoteId);

            entity.Property(e => e.UserNewsEmoteId).HasColumnName("user_news_emote_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.NewsId).HasColumnName("news_id");
            entity.Property(e => e.EmoteId).HasColumnName("emote_id");
            entity.Property(e => e.ClickDate).HasColumnName("click_date");

            
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("FK_UserNewsEmote_User");

            entity.HasOne(e => e.News)
                .WithMany()
                .HasForeignKey(e => e.NewsId)
                .HasConstraintName("FK_UserNewsEmote_News");

            entity.HasOne(e => e.Emote)
                .WithMany()
                .HasForeignKey(e => e.EmoteId)
                .HasConstraintName("FK_UserNewsEmote_Emote");

           
            entity.HasIndex(e => new { e.UserId, e.NewsId })
                .IsUnique()
                .HasDatabaseName("UQ_User_News");
        });


        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
