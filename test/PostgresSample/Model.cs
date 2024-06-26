﻿using PropertyBind;
using RedOrb;
using RedOrb.Attributes;

namespace PostgresSample;

/*
 * https://learn.microsoft.com/ja-jp/ef/core/get-started/overview/first-app?tabs=netcore-cli
 */

public class Tag
{
	public string Name { get; set; } = string.Empty;
}

[GeneratePropertyBind(nameof(Posts), nameof(Post.Blog))]
[DbTable(new[] { nameof(BlogId) }, ConstraintName = "pkey_blog")]
[DbIndex(new[] { nameof(Url) }, IsUnique = true)]
public partial class Blog
{
	[DbColumn("serial8", IsAutoNumber = true)]
	public int BlogId { get; set; }
	[DbColumn("text")]
	public string Url { get; set; } = string.Empty;
	[DbColumn("text")]
	public List<Tag> Tags { get; set; } = new();
	[DbColumn("timestamp", SpecialColumn = SpecialColumn.CreateTimestamp, DefaultValue = "clock_timestamp()")]
	public DateTime CreatedAt { get; set; }
	[DbColumn("timestamp", SpecialColumn = SpecialColumn.UpdateTimestamp, DefaultValue = "clock_timestamp()")]
	public DateTime UpdatedAt { get; set; }
	[DbColumn("numeric", SpecialColumn = SpecialColumn.VersionNumber)]
	public long Version { get; set; }

	[DbChildren]
	public DirtyCheckableCollection<Post> Posts { get; }
}

[GeneratePropertyBind(nameof(Comments), nameof(Comment.Post))]
[DbTable(new[] { nameof(PostId) })]
public partial class Post
{
	[DbColumn("serial8", IsAutoNumber = true)]
	public int PostId { get; set; }
	[DbParentRelationColumn("bigint", nameof(Post.Blog.BlogId))]
	public Blog Blog { get; set; } = null!;
	[DbColumn("text")]
	public string Title { get; set; } = string.Empty;
	[DbColumn("text")]
	public string Content { get; set; } = string.Empty;

	[DbChildren]
	public DirtyCheckableCollection<Comment> Comments { get; }
}

[DbTable(new[] { nameof(CommentId) })]
public class Comment
{
	[DbColumn("serial8", IsAutoNumber = true)]
	public int CommentId { get; set; }
	[DbColumn("text")]
	public string CommentText { get; set; } = string.Empty;
	[DbParentRelationColumn("owner_post_id", "bigint", nameof(Comment.Post.PostId))]
	public Post Post { get; set; } = null!;
}

public static class DbTableDefinitionRepository
{
	public static DbTableDefinition<Blog> GetBlogDefinition()
	{
		return DefinitionBuilder.Create<Blog>();
	}

	public static DbTableDefinition<Post> GetPostDefinition()
	{
		return DefinitionBuilder.Create<Post>();
	}

	public static DbTableDefinition<Comment> GetCommentDefinition()
	{
		return DefinitionBuilder.Create<Comment>();
	}
}