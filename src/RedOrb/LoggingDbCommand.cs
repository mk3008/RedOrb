﻿using Microsoft.Extensions.Logging;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;

namespace RedOrb;

public class LoggingDbCommand : IDbCommand
{
	public LoggingDbCommand(IDbCommand command, ILogger logger)
	{
		DbCommand = command;
		Logger = logger;
	}

	private ILogger Logger { get; init; }

	private IDbCommand DbCommand { get; init; }

	public LogLevel LogLevel { get; set; } = LogLevel.Information;

	private string GetParameterText()
	{
		if (DbCommand.Parameters.Count == 0) return string.Empty;

		var sb = new StringBuilder();
		sb.AppendLine("/*");
		foreach (IDbDataParameter item in DbCommand.Parameters)
		{
			if (item.Value == null)
			{
				sb.AppendLine("  " + item.ParameterName + " is NULL");
			}
			else if (item.Value.GetType() == typeof(string))
			{
				sb.AppendLine($"  {item.ParameterName} = '{item.Value}'");
			}
			else
			{
				sb.AppendLine($"  {item.ParameterName} = {item.Value}");
			}
		}
		sb.AppendLine("*/");

		return sb.ToString();
	}

	private void WriteLog([CallerMemberName] string callerMethodName = "unknown")
	{
		Logger.Log(LogLevel, callerMethodName + ";\n" + GetParameterText() + CommandText + ";");
	}

	#region "implements interface"

	public string CommandText { get => DbCommand.CommandText; set => DbCommand.CommandText = value; }

	public int CommandTimeout { get => DbCommand.CommandTimeout; set => DbCommand.CommandTimeout = value; }

	public CommandType CommandType { get => DbCommand.CommandType; set => DbCommand.CommandType = value; }

	public IDbConnection? Connection { get => DbCommand.Connection; set => DbCommand.Connection = value; }

	public IDataParameterCollection Parameters => DbCommand.Parameters;

	public IDbTransaction? Transaction { get => DbCommand.Transaction; set => DbCommand.Transaction = value; }

	public UpdateRowSource UpdatedRowSource { get => DbCommand.UpdatedRowSource; set => DbCommand.UpdatedRowSource = value; }

	public void Cancel()
	{
		DbCommand.Cancel();
	}

	public IDbDataParameter CreateParameter()
	{
		return DbCommand.CreateParameter();
	}

	public void Dispose()
	{
		DbCommand.Dispose();
	}

	public int ExecuteNonQuery()
	{
		WriteLog();
		return DbCommand.ExecuteNonQuery();
	}

	public IDataReader ExecuteReader()
	{
		WriteLog();
		return DbCommand.ExecuteReader();
	}

	public IDataReader ExecuteReader(CommandBehavior behavior)
	{
		WriteLog();
		return DbCommand.ExecuteReader(behavior);
	}

	public object? ExecuteScalar()
	{
		WriteLog();
		return DbCommand.ExecuteScalar();
	}

	public void Prepare()
	{
		DbCommand.Prepare();
	}
	#endregion
}