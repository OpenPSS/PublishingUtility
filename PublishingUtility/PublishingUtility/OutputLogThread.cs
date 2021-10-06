using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace PublishingUtility
{
	public class OutputLogThread : AbstractExecutor
	{
		public delegate void MyDelegate(string msg);

		private Process p;

		private Guid guid;

		public OutputLogThread(Control mainForm, Guid guid)
			: base(mainForm)
		{
			this.guid = guid;
		}

		public override void Execute()
		{
			p = PsmDeviceFuncBinding.GetLog(guid);
			try
			{
				while (true)
				{
					string text;
					try
					{
						text = p.StandardOutput.ReadLine();
						if (text == null)
						{
							return;
						}
					}
					catch (ObjectDisposedException)
					{
						return;
					}
					catch (NullReferenceException)
					{
						return;
					}
					if (string.IsNullOrEmpty(text))
					{
						Thread.Sleep(10);
						continue;
					}
					MyDelegate method = ((MainForm)control).ConsoleCallback;
					object[] args = new object[1] { text };
					IAsyncResult asyncResult = control.BeginInvoke(method, args);
					asyncResult.AsyncWaitHandle.WaitOne();
					if (asyncResult.IsCompleted)
					{
						control.EndInvoke(asyncResult);
					}
				}
			}
			catch (ThreadInterruptedException ex3)
			{
				Console.WriteLine(ex3.ToString());
			}
			catch (Exception)
			{
			}
			finally
			{
				p.Close();
				p.Dispose();
			}
		}

		public void Terminate()
		{
			try
			{
				p.StandardInput.WriteLine("get_log_end");
				p.WaitForExit();
			}
			catch (Exception)
			{
			}
		}
	}
}
