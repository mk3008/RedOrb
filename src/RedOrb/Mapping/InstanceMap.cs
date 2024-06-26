﻿using System.Reflection;

namespace RedOrb.Mapping;

public class InstanceMap
{
	public required TypeMap TypeMap { get; set; }

	public object? Item { get; set; }

	public string TableAlias => TypeMap.TableAlias;

	public Type GetInstanceType() => TypeMap.Type;

	public PropertyInfo GetSequenceProperty()
	{
		var tp = GetInstanceType();
		var name = ObjectRelationMapper.FindFirst(tp).GetSequence().Identifier;
		return tp.GetProperty(name)!;
	}

	public List<PropertyInfo> GetPrimaryKeyProperties()
	{
		var tp = GetInstanceType();
		return ObjectRelationMapper.FindFirst(tp).GetPrimaryKeys().Select(x => tp.GetProperty(x.Identifier)!).ToList();
	}

	public List<ColumnMap> GetPrimaryKeyColumnMap()
	{
		var tp = GetInstanceType();
		var props = ObjectRelationMapper.FindFirst(tp).GetPrimaryKeys().Select(x => x.Identifier).ToList();
		return TypeMap.ColumnMaps.Where(x => props.Contains(x.PropertyName)).ToList();
	}

	public List<ColumnMap> GetSubordinationColumnMap()
	{
		var tp = GetInstanceType();
		var props = ObjectRelationMapper.FindFirst(tp).GetPrimaryKeys().Select(x => x.Identifier).ToList();
		return TypeMap.ColumnMaps.Where(x => !props.Contains(x.PropertyName)).ToList();
	}
}
