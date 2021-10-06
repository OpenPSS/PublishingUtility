using System.Windows.Forms;

namespace PublishingUtility
{
	public abstract class AbstractExecutor
	{
		protected Control control;

		public AbstractExecutor(Control control)
		{
			this.control = control;
		}

		public abstract void Execute();
	}
}
