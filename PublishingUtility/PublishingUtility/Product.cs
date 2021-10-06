using System.Collections;

namespace PublishingUtility
{
	internal class Product
	{
		public string Label = "";

		public Hashtable Names = new Hashtable();

		public bool Consumable;

		public Product(string label)
		{
			Label = label;
		}
	}
}
