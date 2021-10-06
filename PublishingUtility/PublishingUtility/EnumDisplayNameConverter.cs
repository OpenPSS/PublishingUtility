using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace PublishingUtility
{
	public class EnumDisplayNameConverter : EnumConverter
	{
		private Dictionary<object, string> table = new Dictionary<object, string>();

		public EnumDisplayNameConverter(Type type)
			: base(type)
		{
			BuildTable(table);
		}

		protected virtual void BuildTable(Dictionary<object, string> table)
		{
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text == null)
			{
				return base.ConvertFrom(context, culture, value);
			}
			Dictionary<object, string>.Enumerator enumerator = table.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Value == text)
				{
					return enumerator.Current.Key;
				}
			}
			FieldInfo[] fields = base.EnumType.GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				string displayName = GetDisplayName(fieldInfo, culture);
				if (displayName == text)
				{
					return fieldInfo.GetValue(null);
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType != typeof(string))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			if (table.TryGetValue(value, out var value2))
			{
				return value2;
			}
			string name = Enum.GetName(base.EnumType, value);
			if (name != null)
			{
				FieldInfo field = base.EnumType.GetField(name);
				string displayName = GetDisplayName(field, culture);
				if (displayName != null)
				{
					return displayName;
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		private string GetDisplayName(FieldInfo field, CultureInfo culture)
		{
			if (field == null)
			{
				return null;
			}
			Type typeFromHandle = typeof(EnumDisplayNameAttribute);
			Attribute customAttribute = Attribute.GetCustomAttribute(field, typeFromHandle);
			return (customAttribute as EnumDisplayNameAttribute)?.GetName(culture);
		}

		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			string text = value as string;
			Dictionary<object, string>.Enumerator enumerator = table.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Value == text)
				{
					return true;
				}
			}
			return false;
		}
	}
}
