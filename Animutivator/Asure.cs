using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace JetBrains.Annotations
{
	/// <summary>
	/// Custom & lightweight contracts.
	/// </summary>
	internal static class @Asure
	{
		private const string LogName = "@errors.log";

		[Conditional("DEBUG")]
		public static void Null([CanBeNull] object arg, string comment)
		{
			if(arg != null)
			{
				throw new Exception($"Argument is not null. {comment}");
			}
		}

		/// <summary>
		/// Assures arguments aren't null.
		/// </summary>
		/// <param name="args"></param>
		[Conditional("DEBUG")]
		internal static void NotNull(params object[] args)
		{
			if (args == null || args.Length == 0)
				throw new Exception("Argument list is empty");
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i] == null)
				{
					throw new Exception($"Argument no.{i} is null.");
				}
			}
		}

		/// <summary>
		/// Assures argument isn't null.
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="comment"></param>
		[Conditional("DEBUG")]
		internal static void NotNull([NotNull] object arg, string comment = "")
		{
			if (arg == null)
			{
				throw new Exception($"Argument is null. {comment}");
			}
		}

		/// <summary>
		/// Assures that argument do not present invalid state.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="comment"></param>
		[Conditional("DEBUG")]
		internal static void CantBe(bool expression, string comment = "")
		{
			if (expression)
			{
				throw new Exception($"Expression violates set boundary rules. "+comment);
			}
		}

		/// <summary>
		/// Assures that argument do not present invalid state.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="asserted"></param>
		/// <param name="comment"></param>
		[Conditional("DEBUG")]
		internal static void CantBe(bool expression, object asserted, string comment = "")
		{
			if(expression)
			{
				var values = WriteObjectFields(asserted);

				throw new Exception($"Expression violates set boundary rules. " + comment + values);
			}
		}

		private static string WriteObjectFields(object asserted)
		{
			var fields = asserted.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			return fields.Aggregate($" {asserted.GetType().Name}:\n ", (current, field) => 
				current + $"{field.Name}: {field.GetValue(asserted)}\n");
		}

		/// <summary>
		/// Assures that argument do not present invalid state.
		/// Uses lambda expression to present failing code.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="comment"></param>
		[Conditional("DEBUG")]
		internal static void CantBe([NotNull] Expression<Func<bool>> expression,
									string comment = "")
		{
			var expr = expression.Compile();
			if(expr.Invoke())
			{
				var msg = expression.Body.ToString();
				throw new Exception(
					$@"Expression violates set boundary rules: {msg}. {comment}");
			}
		}

		/// <summary>
		/// Assures that state of argument is valid.
		/// Uses expression lambda to present failing code.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="comment"></param>
		[Conditional("DEBUG")]
		internal static void Must([NotNull] Expression<Func<bool>> expression, string comment = "")
		{
			var expr = expression.Compile();
			if(! expr.Invoke())
			{
				var msg = expression.Body.ToString();
				throw new Exception(
					$@"Expression do not satisfy the rules: {msg}. {comment}");
			}
		}

		/// <summary>
		/// Assures that state of argument is valid.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="asserted"></param>
		/// <param name="comment"></param>
		[Conditional("DEBUG")]
		internal static void Must(bool expression, object asserted, string comment = "")
		{
			if(! expression)
			{
				var values = WriteObjectFields(asserted);
				throw new Exception(
					$@"Expression do not satisfy the rules. {comment} {values}");
			}
		}

		/// <summary>
		/// Assures that state of argument is valid.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="comment"></param>
		[Conditional("DEBUG")]
		internal static void Must(bool expression, string comment = "")
		{
			if(!expression)
			{
				throw new Exception($@"Expression do not satisfy the rules. {comment}");
			}
		}

		internal static void PrintFields(object obj)
		{
			Debug.Print(WriteObjectFields(obj));
		}

		/// <summary>
		/// Assures that arguments do reference same object.
		/// </summary>
		/// <param name="arg2"></param>
		/// <param name="comment"></param>
		/// <param name="arg1"></param>
		[Conditional("DEBUG")]
		internal static void RefEquals(object arg1, object arg2, string comment = "")
		{
			if (ReferenceEquals(arg1, arg2) == false)
			{
				throw new Exception("Arguments do not refer to same object. "+comment);
			}
		}
	}
}
