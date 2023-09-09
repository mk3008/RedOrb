﻿using Microsoft.Extensions.Logging;
using RedOrb;
using Xunit.Abstractions;

namespace PostgresSample;

public class LoadTest : IClassFixture<PostgresDB>
{
	public LoadTest(PostgresDB postgresDB, ITestOutputHelper output)
	{
		PostgresDB = postgresDB;
		Logger = new UnitTestLogger() { Output = output };

		using var cn = PostgresDB.ConnectionOpenAsNew(Logger);

		cn.CreateTableOrDefault<Blog>();
	}

	private readonly PostgresDB PostgresDB;

	private readonly UnitTestLogger Logger;

	[Fact]
	public void SelectByPrimaryKey()
	{
		using var cn = PostgresDB.ConnectionOpenAsNew(Logger);
		using var trn = cn.BeginTransaction();

		// Create
		Logger.LogInformation("Inserting a new blog");
		var newBlog = new Blog { Url = "http://blogs.msdn.com/adonet/FetchTest/SelectByPrimaryKey" };
		cn.Save(newBlog);

		// Read
		Logger.LogInformation("Querying for a blog");
		var loadedBlog = cn.Load(new Blog() { BlogId = newBlog.BlogId });

		Assert.Equal(newBlog.BlogId, loadedBlog.BlogId);
		Assert.Equal(newBlog.Url, loadedBlog.Url);

		trn.Commit();
	}

	[Fact]
	public void InstanceIsNotEqual()
	{
		using var cn = PostgresDB.ConnectionOpenAsNew(Logger);
		using var trn = cn.BeginTransaction();

		// Create
		Logger.LogInformation("Inserting a new blog");
		var newBlog = new Blog { Url = "http://blogs.msdn.com/adonet/FetchTest/InstanceIsNotEqual" };
		cn.Save(newBlog);

		// Read
		Logger.LogInformation("Querying for a blog");
		var loadedBlog = cn.Load(newBlog);

		Assert.Equal(newBlog.BlogId, loadedBlog.BlogId);
		Assert.Equal(newBlog.Url, loadedBlog.Url);
		Assert.NotEqual(newBlog, loadedBlog);

		trn.Commit();
	}

	[Fact]
	public void SelectByUniqueKey()
	{
		using var cn = PostgresDB.ConnectionOpenAsNew(Logger);
		using var trn = cn.BeginTransaction();

		var url = "http://blogs.msdn.com/adonet/FetchTest/SelectByUniqueKey";

		// Create
		Logger.LogInformation("Inserting a new blog");
		var newBlog = new Blog { Url = url };
		cn.Save(newBlog);

		// Read
		Logger.LogInformation("Querying for a blog");
		var loadedBlog = cn.Load(new Blog() { Url = url });

		Assert.Equal(newBlog.BlogId, loadedBlog.BlogId);
		Assert.Equal(newBlog.Url, loadedBlog.Url);

		trn.Commit();
	}

	[Fact]
	public void NotFoundExceptionTest()
	{
		using var cn = PostgresDB.ConnectionOpenAsNew(Logger);
		using var trn = cn.BeginTransaction();

		var ex = Assert.Throws<ArgumentException>(() =>
		{
			// Read
			Logger.LogInformation("Querying for a blog");
			var loadedBlog = cn.Load(new Blog() { BlogId = 0 });
		});
		Assert.Equal("No records found.(BlogId=0)", ex.Message);
	}
}