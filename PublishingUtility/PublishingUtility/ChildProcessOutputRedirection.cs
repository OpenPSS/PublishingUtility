using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace PublishingUtility
{
	public class ChildProcessOutputRedirection
	{
		private static StringBuilder childOutput;

		private static int numOutputLines;

		public static int SortInputListText(string command, string arguments, ref string processOutput)
		{
			numOutputLines = 0;
			Process process = new Process();
			process.StartInfo.FileName = command;
			process.StartInfo.Arguments = arguments;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.StartInfo.RedirectStandardOutput = true;
			childOutput = new StringBuilder("");
			process.OutputDataReceived += ChildOutputHandler;
			process.StartInfo.RedirectStandardInput = true;
			process.Start();
			StreamWriter standardInput = process.StandardInput;
			process.BeginOutputReadLine();
			standardInput.Close();
			process.WaitForExit();
			if (numOutputLines > 0)
			{
				Console.WriteLine(childOutput);
				processOutput = childOutput.ToString();
			}
			else
			{
				Console.WriteLine(" No input lines were sorted.");
			}
			int exitCode = process.ExitCode;
			process.Close();
			return exitCode;
		}

		private static void ChildOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
		{
			if (!string.IsNullOrEmpty(outLine.Data))
			{
				numOutputLines++;
				childOutput.Append(Environment.NewLine + outLine.Data);
			}
		}
	}
}
