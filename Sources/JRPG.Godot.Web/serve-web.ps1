param(
	[int]$Port = 8080,
	[string]$Folder = 'AppBundle'
)

Add-Type -TypeDefinition @"
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

public static class KillOnCloseJob
{
	[StructLayout(LayoutKind.Sequential)]
	struct IO_COUNTERS
	{
		public ulong ReadOperationCount, WriteOperationCount, OtherOperationCount;
		public ulong ReadTransferCount, WriteTransferCount, OtherTransferCount;
	}

	[StructLayout(LayoutKind.Sequential)]
	struct JOBOBJECT_BASIC_LIMIT_INFORMATION
	{
		public long PerProcessUserTimeLimit;
		public long PerJobUserTimeLimit;
		public uint LimitFlags;
		public UIntPtr MinimumWorkingSetSize;
		public UIntPtr MaximumWorkingSetSize;
		public uint ActiveProcessLimit;
		public UIntPtr Affinity;
		public uint PriorityClass;
		public uint SchedulingClass;
	}

	[StructLayout(LayoutKind.Sequential)]
	struct JOBOBJECT_EXTENDED_LIMIT_INFORMATION
	{
		public JOBOBJECT_BASIC_LIMIT_INFORMATION BasicLimitInformation;
		public IO_COUNTERS IoInfo;
		public UIntPtr ProcessMemoryLimit;
		public UIntPtr JobMemoryLimit;
		public UIntPtr PeakProcessMemoryUsed;
		public UIntPtr PeakJobMemoryUsed;
	}

	const uint JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 0x2000;
	const int JobObjectExtendedLimitInformation = 9;

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
	static extern IntPtr CreateJobObject(IntPtr attributes, string name);

	[DllImport("kernel32.dll")]
	static extern bool SetInformationJobObject(IntPtr job, int infoClass, ref JOBOBJECT_EXTENDED_LIMIT_INFORMATION info, int length);

	[DllImport("kernel32.dll")]
	static extern bool AssignProcessToJobObject(IntPtr job, IntPtr process);

	public static IntPtr Create(IntPtr process)
	{
		IntPtr job = CreateJobObject(IntPtr.Zero, null);
		if (job == IntPtr.Zero)
		{
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}

		var info = new JOBOBJECT_EXTENDED_LIMIT_INFORMATION();
		info.BasicLimitInformation.LimitFlags = JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE;

		if (!SetInformationJobObject(job, JobObjectExtendedLimitInformation, ref info, Marshal.SizeOf(typeof(JOBOBJECT_EXTENDED_LIMIT_INFORMATION))))
		{
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}

		if (!AssignProcessToJobObject(job, process))
		{
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}

		return job;
	}
}
"@

$server = Start-Process python -ArgumentList '-m', 'http.server', $Port, '-d', $Folder -WindowStyle Hidden -PassThru
[void][KillOnCloseJob]::Create($server.Handle)
$server.WaitForExit()
