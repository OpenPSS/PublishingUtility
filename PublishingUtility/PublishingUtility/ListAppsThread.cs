using System;
using System.Threading;
using System.Windows.Forms;

namespace PublishingUtility
{
	public class ListAppsThread : AbstractExecutor
	{
		public delegate void MyDelegate();

		private Guid guid;

		public ListAppsThread(Control mainForm, Guid guid)
			: base(mainForm)
		{
			this.guid = guid;
		}

		public override void Execute()
		{
			((MainForm)control).ListApps(guid);
			try
			{
				MyDelegate method = ((MainForm)control).UpdateAppsInfoInList;
				object[] args = new object[0];
				IAsyncResult asyncResult = control.BeginInvoke(method, args);
				asyncResult.AsyncWaitHandle.WaitOne();
				if (asyncResult.IsCompleted)
				{
					control.EndInvoke(asyncResult);
				}
			}
			catch (ThreadInterruptedException ex)
			{
				Console.WriteLine(ex.ToString());
			}
			catch (Exception)
			{
			}
		}
	}
}
