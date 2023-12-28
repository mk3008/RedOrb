﻿using PropertyBind;
using RedOrb;
using RedOrb.Attributes;

namespace PostgresSample;

/*
 * https://learn.microsoft.com/ja-jp/ef/core/get-started/overview/first-app?tabs=netcore-cli
 */

[GeneratePropertyBind(nameof(Posts), nameof(Post.Blog))]
[DbTable("blogs")]
[DbIndex(true, nameof(Url))]
public partial class Blog
{
	[DbColumn("serial8", IsAutoNumber = true, IsPrimaryKey = true)]
	public int? BlogId { get; set; }
	[DbColumn("text")]
	public string Url { get; set; } = string.Empty;

	[DbChildren(nameof(DeletedPosts))]
	public IList<Post> Posts { get; }

	public IList<Post> DeletedPosts { get; }
}

[GeneratePropertyBind(nameof(Comments), nameof(Comment.Post))]
[DbTable("posts")]
public partial class Post
{
	[DbColumn("serial8", IsAutoNumber = true, IsPrimaryKey = true)]
	public int? PostId { get; set; }
	[DbParentRelationColumn("bigint", nameof(Post.Blog.BlogId))]
	public Blog Blog { get; set; } = null!;
	[DbColumn("text")]
	public string Title { get; set; } = string.Empty;
	[DbColumn("text")]
	public string Content { get; set; } = string.Empty;

	[DbChildren(nameof(DeletedComments))]
	public IList<Comment> Comments { get; }

	public IList<Comment> DeletedComments { get; }
}

[DbTable("comments")]
public class Comment
{
	[DbColumn("serial8", IsAutoNumber = true, IsPrimaryKey = true)]
	public int? CommentId { get; set; }
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