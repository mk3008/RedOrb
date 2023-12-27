﻿using System.Reflection;

namespace RedOrb.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public class DbColumnAttribute : Attribute
{
	public DbColumnAttribute(string columnType)
	{
		ColumnType = columnType;
	}

	public string ColumnName { get; set; } = string.Empty;

	public string ColumnType { get; set; }

	public string RelationColumnType { get; set; } = string.Empty;

	public bool IsPrimaryKey { get; set; } = false;

	public bool IsUniqueKey { get; set; } = false;

	public bool IsAutoNumber { get; set; } = false;

	public string DefaultValue { get; set; } = string.Empty;

	public string Comment { get; set; } = string.Empty;

	public SpecialColumn SpecialColumn { get; set; } = SpecialColumn.None;

	public DbColumnDefinition ToDefinition(PropertyInfo prop)
	{
		var c = new DbColumnDefinition()
		{
			Identifer = prop.Name,
			IsNullable = (IsPrimaryKey) ? false : prop.IsNullable(),
			ColumnName = (!string.IsNullOrEmpty(ColumnName)) ? ColumnName : prop.Name.ToSnakeCase(),
			ColumnType = ColumnType,
			RelationColumnType = RelationColumnType,
			Comment = Comment,
			DefaultValue = DefaultValue,
			IsAutoNumber = IsAutoNumber,
			IsPrimaryKey = IsPrimaryKey,
			IsUniqueKey = IsUniqueKey,
			SpecialColumn = SpecialColumn
		};
		return c;
	}
}