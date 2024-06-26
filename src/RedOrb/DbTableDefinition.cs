﻿namespace RedOrb;

public class DbTableDefinition : IDbTableDefinition
{
	public string SchemaName { get; set; } = string.Empty;

	public required string TableName { get; init; }

	public string Comment { get; set; } = string.Empty;

	public List<IDbColumnContainer> ColumnContainers { get; init; } = new();

	public IEnumerable<string> ColumnNames => GetDbColumnDefinitions().Select(x => x.ColumnName);

	public IEnumerable<DbColumnDefinition> ColumnDefinitions => GetDbColumnDefinitions();

	private IEnumerable<DbColumnDefinition> GetDbColumnDefinitions()
	{
		foreach (var column in ColumnContainers)
		{
			foreach (var def in column.GetDbColumnDefinitions())
			{
				yield return def;
			}
		}
	}

	public List<string> PKeyIdentifiers { get; init; } = new();

	public List<DbIndexDefinition> Indexes { get; init; } = new();

	public IEnumerable<DbParentRelationDefinition> ParentRelationDefinitions => GetParentRelations();

	private IEnumerable<DbParentRelationDefinition> GetParentRelations()
	{
		foreach (DbParentRelationDefinition relation in ColumnContainers.Where(x => x is DbParentRelationDefinition))
		{
			yield return relation;
		}
	}

	public List<string> ChildIdentifiers { get; init; } = new();

	public virtual Type Type { get; } = typeof(object);

	public string TableFullName => GetTableFullName();

	private string GetTableFullName()
	{
		return string.IsNullOrEmpty(SchemaName) ? TableName : SchemaName + "." + TableName;
	}
}

public class DbTableDefinition<T> : DbTableDefinition
{
	public override Type Type { get; } = typeof(T);
}