using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Pohgee.SIF.DesktopAgent.Abstractions;

namespace Pohgee.SIF.Desktop.DesktopAgent.Shell {
	public class ApplicationLauncher {

		/// <summary>
		/// The singleton
		/// </summary>
		private static volatile ApplicationLauncher singleton;

		/// <summary>
		/// The synchronize root
		/// </summary>
		private static readonly object syncRoot = new object();


		/// <summary>
		/// Prevents a default instance of the <see cref="ApplicationLauncher"/> class from being created.
		/// </summary>
		private ApplicationLauncher() {

		}

		/// <summary>
		/// Gets the current.
		/// </summary>
		/// <value>
		/// The current.
		/// </value>
		public static ApplicationLauncher Current {
			get {
				if (singleton == null) {
					lock (syncRoot) {
						if (singleton == null)
							singleton = new ApplicationLauncher();
					}
				}
				return singleton;
			}
		}


		#region "Enums"

		/// <summary>
		/// 
		/// </summary>
		public enum TokenType {
			TokenPrimary = 1,
			TokenImpersonation = 2
		}

		/// <summary>
		/// 
		/// </summary>
		public enum SecurityImpersonationLevel {
			SecurityAnonymous = 0,
			SecurityIdentification = 1,
			SecurityImpersonation = 2,
			SecurityDelegation = 3,
		}

		/// <summary>
		/// 
		/// </summary>
		public enum Wts_ConnectState_Class {
			WTSActive,
			WTSConnected,
			WTSConnectQuery,
			WTSShadow,
			WTSDisconnected,
			WTSIdle,
			WTSListen,
			WTSReset,
			WTSDown,
			WTSInit
		}



		/// <summary>
		/// 
		/// </summary>
		public enum TokenInformationClass {
			/// <summary>
			/// The buffer receives a TOKEN_USER structure that contains the user account of the token.
			/// </summary>
			TokenUser = 1,

			/// <summary>
			/// The buffer receives a TOKEN_GROUPS structure that contains the group accounts associated with the token.
			/// </summary>
			TokenGroups,

			/// <summary>
			/// The buffer receives a TOKEN_PRIVILEGES structure that contains the privileges of the token.
			/// </summary>
			TokenPrivileges,

			/// <summary>
			/// The buffer receives a TOKEN_OWNER structure that contains the default owner security identifier (SID) for newly created objects.
			/// </summary>
			TokenOwner,

			/// <summary>
			/// The buffer receives a TOKEN_PRIMARY_GROUP structure that contains the default primary group SID for newly created objects.
			/// </summary>
			TokenPrimaryGroup,

			/// <summary>
			/// The buffer receives a TOKEN_DEFAULT_DACL structure that contains the default DACL for newly created objects.
			/// </summary>
			TokenDefaultDacl,

			/// <summary>
			/// The buffer receives a TOKEN_SOURCE structure that contains the source of the token. TOKEN_QUERY_SOURCE access is needed to retrieve this information.
			/// </summary>
			TokenSource,

			/// <summary>
			/// The buffer receives a TOKEN_TYPE value that indicates whether the token is a primary or impersonation token.
			/// </summary>
			TokenType,

			/// <summary>
			/// The buffer receives a SECURITY_IMPERSONATION_LEVEL value that indicates the impersonation level of the token. If the access token is not an impersonation token, the function fails.
			/// </summary>
			TokenImpersonationLevel,

			/// <summary>
			/// The buffer receives a TOKEN_STATISTICS structure that contains various token statistics.
			/// </summary>
			TokenStatistics,

			/// <summary>
			/// The buffer receives a TOKEN_GROUPS structure that contains the list of restricting SIDs in a restricted token.
			/// </summary>
			TokenRestrictedSids,

			/// <summary>
			/// The buffer receives a DWORD value that indicates the Terminal Services session identifier that is associated with the token. 
			/// </summary>
			TokenSessionId,

			/// <summary>
			/// The buffer receives a TOKEN_GROUPS_AND_PRIVILEGES structure that contains the user SID, the group accounts, the restricted SIDs, and the authentication ID associated with the token.
			/// </summary>
			TokenGroupsAndPrivileges,

			/// <summary>
			/// Reserved.
			/// </summary>
			TokenSessionReference,

			/// <summary>
			/// The buffer receives a DWORD value that is nonzero if the token includes the SANDBOX_INERT flag.
			/// </summary>
			TokenSandBoxInert,

			/// <summary>
			/// Reserved.
			/// </summary>
			TokenAuditPolicy,

			/// <summary>
			/// The buffer receives a TOKEN_ORIGIN value. 
			/// </summary>
			TokenOrigin,

			/// <summary>
			/// The buffer receives a TOKEN_ELEVATION_TYPE value that specifies the elevation level of the token.
			/// </summary>
			TokenElevationType,

			/// <summary>
			/// The buffer receives a TOKEN_LINKED_TOKEN structure that contains a handle to another token that is linked to this token.
			/// </summary>
			TokenLinkedToken,

			/// <summary>
			/// The buffer receives a TOKEN_ELEVATION structure that specifies whether the token is elevated.
			/// </summary>
			TokenElevation,

			/// <summary>
			/// The buffer receives a DWORD value that is nonzero if the token has ever been filtered.
			/// </summary>
			TokenHasRestrictions,

			/// <summary>
			/// The buffer receives a TOKEN_ACCESS_INFORMATION structure that specifies security information contained in the token.
			/// </summary>
			TokenAccessInformation,

			/// <summary>
			/// The buffer receives a DWORD value that is nonzero if virtualization is allowed for the token.
			/// </summary>
			TokenVirtualizationAllowed,

			/// <summary>
			/// The buffer receives a DWORD value that is nonzero if virtualization is enabled for the token.
			/// </summary>
			TokenVirtualizationEnabled,

			/// <summary>
			/// The buffer receives a TOKEN_MANDATORY_LABEL structure that specifies the token's integrity level. 
			/// </summary>
			TokenIntegrityLevel,

			/// <summary>
			/// The buffer receives a DWORD value that is nonzero if the token has the UIAccess flag set.
			/// </summary>
			TokenUIAccess,

			/// <summary>
			/// The buffer receives a TOKEN_MANDATORY_POLICY structure that specifies the token's mandatory integrity policy.
			/// </summary>
			TokenMandatoryPolicy,

			/// <summary>
			/// The buffer receives the token's logon security identifier (SID).
			/// </summary>
			TokenLogonSid,

			/// <summary>
			/// The maximum value for this enumeration
			/// </summary>
			MaxTokenInfoClass
		}


		#endregion

		#region "Structures"

		/// <summary>
		/// 
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct SecurityAttributes {
			public int Length;
			public IntPtr lpSecurityDescriptor;
			public bool bInheritHandle;
		}

		/// <summary>
		/// 
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct WtsSessionInfo {
			public int SessionId;
			[MarshalAs(UnmanagedType.LPStr)]
			public string pWinStationName;
			public Wts_ConnectState_Class State;
		}

		/// <summary>
		/// 
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct StartUpInfo {
			public int cb;
			public string lpReserved;
			public string lpDesktop;
			public string lpTitle;
			public uint dwX;
			public uint dwY;
			public uint dwXSize;
			public uint dwYSize;
			public uint dwXCountChars;
			public uint dwYCountChars;
			public uint dwFillAttribute;
			public uint dwFlags;
			public short wShowWindow;
			public short cbReserved2;
			public IntPtr lpReserved2;
			public IntPtr hStdInput;
			public IntPtr hStdOutput;
			public IntPtr hStdError;
		}

		/// <summary>
		/// 
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct ProcessInformation {
			public IntPtr hProcess;
			public IntPtr hThread;
			public uint dwProcessId;
			public uint dwThreadId;
		}

		/// <summary>
		/// 
		/// </summary>
		public struct LogOnDetails {
			public string Domain;
			public string UserName;
			public string Password;
		}

		/// <summary>
		/// 
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct TokenPrivileges {
			public Int32 PrivilegeCount;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public Int32[] Privileges;
		}


		/// <summary>
		/// 
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct Luid {
			public int LowPart;
			public int HighPart;
		}


		#endregion

		#region "Consts"

		private const int TokenDuplicate = 0x0002;
		private const int HighPriorityClass = 0x80;
		private const int IdlePriorityClass = 0x40;
		private const int NormalPriorityClass = 0x20;
		private const int Logon32ProviderDefault = 0;
		private const int Logon32LogonInteractive = 2;
		private const uint MaximumAllowed = 0x2000000;
		private const int RealtimePriorityClass = 0x100;
		private const Int32 SePrivilegeEnabled = 0x0002;
		private const int CreateNewConsole = 0x00000010;
		private const string WinLogonProcessName = "winlogon";
		private const string SeDebugName = "SeDebugPrivilege";
		private const string InteractiveWindowStation = @"winsta0\default";
		private const int CreationFlags = NormalPriorityClass | CreateNewConsole;

		#endregion

		#region "Delegates"

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hWnd">The h WND.</param>
		/// <param name="lParam">The l parameter.</param>
		/// <returns></returns>
		public delegate bool EnumChildProc(IntPtr hWnd, int lParam);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lpszWindowStation">The LPSZ window station.</param>
		/// <param name="lParam">The l parameter.</param>
		/// <returns></returns>
		public delegate bool EnumWindowStationProc(string lpszWindowStation, IntPtr lParam);

		#endregion

		#region "Win32 Imports"

		/// <summary>
		/// Closes the handle.
		/// </summary>
		/// <param name="hSnapshot">The authentication snapshot.</param>
		/// <returns></returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool CloseHandle(IntPtr hSnapshot);

		/// <summary>
		/// WTSs the get active console session unique identifier.
		/// </summary>
		/// <returns></returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern uint WTSGetActiveConsoleSessionId();

		/// <summary>
		/// Creates the process asynchronous user.
		/// </summary>
		/// <param name="hToken">The authentication token.</param>
		/// <param name="lpApplicationName">Name of the lp application.</param>
		/// <param name="lpCommandLine">The lp command line.</param>
		/// <param name="lpProcessAttributes">The lp process attributes.</param>
		/// <param name="lpThreadAttributes">The lp thread attributes.</param>
		/// <param name="bInheritHandle">if set to <c>true</c> [attribute inherit handle].</param>
		/// <param name="dwCreationFlags">The dw creation flags.</param>
		/// <param name="lpEnvironment">The lp environment.</param>
		/// <param name="lpCurrentDirectory">The lp current directory.</param>
		/// <param name="lpStartupInfo">The lp startup information.</param>
		/// <param name="lpProcessInformation">The lp process information.</param>
		/// <returns></returns>
		[DllImport("advapi32.dll", EntryPoint = "CreateProcessAsUser", SetLastError = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern bool CreateProcessAsUser(IntPtr hToken, string lpApplicationName, string lpCommandLine, ref SecurityAttributes lpProcessAttributes,
			ref SecurityAttributes lpThreadAttributes, bool bInheritHandle, int dwCreationFlags, IntPtr lpEnvironment,
			string lpCurrentDirectory, ref StartUpInfo lpStartupInfo, out ProcessInformation lpProcessInformation);

		/// <summary>
		/// Processes the unique identifier automatic session unique identifier.
		/// </summary>
		/// <param name="dwProcessId">The dw process unique identifier.</param>
		/// <param name="pSessionId">The application session unique identifier.</param>
		/// <returns></returns>
		[DllImport("kernel32.dll")]
		public static extern bool ProcessIdToSessionId(uint dwProcessId, ref uint pSessionId);

		/// <summary>
		/// Duplicates the token executable.
		/// </summary>
		/// <param name="existingTokenHandle">The existing token handle.</param>
		/// <param name="dwDesiredAccess">The dw desired access.</param>
		/// <param name="lpThreadAttributes">The lp thread attributes.</param>
		/// <param name="tokenType">Type of the token.</param>
		/// <param name="impersonationLevel">The impersonation level.</param>
		/// <param name="duplicateTokenHandle">The duplicate token handle.</param>
		/// <returns></returns>
		[DllImport("advapi32.dll", EntryPoint = "DuplicateTokenEx")]
		public static extern bool DuplicateTokenEx(IntPtr existingTokenHandle, uint dwDesiredAccess,
			ref SecurityAttributes lpThreadAttributes, int tokenType,
			int impersonationLevel, ref IntPtr duplicateTokenHandle);

		/// <summary>
		/// Opens the process.
		/// </summary>
		/// <param name="dwDesiredAccess">The dw desired access.</param>
		/// <param name="bInheritHandle">if set to <c>true</c> [attribute inherit handle].</param>
		/// <param name="dwProcessId">The dw process unique identifier.</param>
		/// <returns></returns>
		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

		/// <summary>
		/// Opens the process token.
		/// </summary>
		/// <param name="processHandle">The process handle.</param>
		/// <param name="desiredAccess">The desired access.</param>
		/// <param name="tokenHandle">The token handle.</param>
		/// <returns></returns>
		[DllImport("advapi32", SetLastError = true), SuppressUnmanagedCodeSecurity]
		public static extern bool OpenProcessToken(IntPtr processHandle, int desiredAccess, ref IntPtr tokenHandle);

		/// <summary>
		/// WTSs the query user token.
		/// </summary>
		/// <param name="sessionId">The session unique identifier.</param>
		/// <param name="tokenHandle">The token handle.</param>
		/// <returns></returns>
		[DllImport("Wtsapi32.dll", SetLastError = true)]
		public static extern bool WTSQueryUserToken(uint sessionId, ref IntPtr tokenHandle);

		/// <summary>
		/// Logons the user.
		/// </summary>
		/// <param name="lpszUsername">The LPSZ username.</param>
		/// <param name="lpszDomain">The LPSZ domain.</param>
		/// <param name="lpszPassword">The LPSZ password.</param>
		/// <param name="dwLogonType">Type of the dw logon.</param>
		/// <param name="dwLogonProvider">The dw logon provider.</param>
		/// <param name="phToken">The physical token.</param>
		/// <returns></returns>
		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword,
			int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

		/// <summary>
		/// Sets the token information.
		/// </summary>
		/// <param name="tokenHandle">The token handle.</param>
		/// <param name="tokenInformationCls">The token information CLS.</param>
		/// <param name="tokenInformation">The token information.</param>
		/// <param name="tokenInformationLength">Length of the token information.</param>
		/// <returns></returns>
		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern Boolean SetTokenInformation(IntPtr tokenHandle, TokenInformationClass tokenInformationCls,
			ref UInt32 tokenInformation, UInt32 tokenInformationLength);


		/// <summary>
		/// Adjusts the token privileges.
		/// </summary>
		/// <param name="tokenHandle">The token handle.</param>
		/// <param name="disableAllPrivileges">if set to <c>true</c> [disable all privileges].</param>
		/// <param name="newState">The new state.</param>
		/// <param name="bufferLength">Length of the buffer.</param>
		/// <param name="previousState">State of the previous.</param>
		/// <param name="returnLength">Length of the return.</param>
		/// <returns></returns>
		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool AdjustTokenPrivileges(IntPtr tokenHandle, bool disableAllPrivileges, ref TokenPrivileges newState,
			int bufferLength, IntPtr previousState, IntPtr returnLength);


		/// <summary>
		/// Lookups the privilege value.
		/// </summary>
		/// <param name="lpSystemName">Name of the lp system.</param>
		/// <param name="lpname">The lpname.</param>
		/// <param name="lpLuid">The lp luid.</param>
		/// <returns></returns>
		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool LookupPrivilegeValue(IntPtr lpSystemName, string lpname, [MarshalAs(UnmanagedType.Struct)] ref Luid lpLuid);

		/// <summary>
		/// Gets the desktop window.
		/// </summary>
		/// <returns></returns>
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr GetDesktopWindow();

		/// <summary>
		/// Sets the parent.
		/// </summary>
		/// <param name="hWndChild">The h WND child.</param>
		/// <param name="hWndNewParent">The h WND new parent.</param>
		/// <returns></returns>
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		/// <summary>
		/// Finds the window.
		/// </summary>
		/// <param name="lpClassName">Name of the lp class.</param>
		/// <param name="lpWindowName">Name of the lp window.</param>
		/// <returns></returns>
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindow(StringBuilder lpClassName, StringBuilder lpWindowName);

		/// <summary>
		/// WTSs the open server.
		/// </summary>
		/// <param name="pServerName">Name of the p server.</param>
		/// <returns></returns>
		[DllImport("wtsapi32.dll", SetLastError = true)]
		public static extern IntPtr WTSOpenServer([MarshalAs(UnmanagedType.LPStr)] string pServerName);

		/// <summary>
		/// WTSs the close server.
		/// </summary>
		/// <param name="hServer">The h server.</param>
		[DllImport("wtsapi32.dll")]
		public static extern void WTSCloseServer(IntPtr hServer);

		/// <summary>
		/// Enums the windows.
		/// </summary>
		/// <param name="lpEnumFunc">The lp enum function.</param>
		/// <param name="lParam">The l parameter.</param>
		/// <returns></returns>
		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
		public static extern bool EnumWindows(EnumChildProc lpEnumFunc, int lParam);

		/// <summary>
		/// Gets the window thread process identifier.
		/// </summary>
		/// <param name="hWnd">The h WND.</param>
		/// <param name="lpdwProcessId">The LPDW process identifier.</param>
		/// <returns></returns>
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
		public static extern long GetWindowThreadProcessId(IntPtr hWnd, out long lpdwProcessId);

		/// <summary>
		/// Enums the window stations.
		/// </summary>
		/// <param name="lpEnumFunc">The lp enum function.</param>
		/// <param name="lParam">The l parameter.</param>
		/// <returns></returns>
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool EnumWindowStations(EnumWindowStationProc lpEnumFunc, IntPtr lParam);

		/// <summary>
		/// WTSs the enumerate sessions.
		/// </summary>
		/// <param name="hServer">The h server.</param>
		/// <param name="Reserved">The reserved.</param>
		/// <param name="Version">The version.</param>
		/// <param name="ppSessionInfo">The pp session information.</param>
		/// <param name="pCount">The p count.</param>
		/// <returns></returns>
		[DllImport("wtsapi32.dll", SetLastError = true)]
		public static extern int WTSEnumerateSessions(IntPtr hServer, [MarshalAs(UnmanagedType.U4)] int Reserved,
												      [MarshalAs(UnmanagedType.U4)] int Version, ref IntPtr ppSessionInfo,
													  [MarshalAs(UnmanagedType.U4)] ref int pCount);

		/// <summary>
		/// WTSs the enumerate sessions.
		/// </summary>
		/// <param name="hServer">The h server.</param>
		/// <param name="ppSessionInfo">The pp session information.</param>
		/// <param name="pCount">The p count.</param>
		[DllImport("wtsapi32.dll", SetLastError = true)]
		public static extern void WTSEnumerateSessions(IntPtr hServer, ref IntPtr ppSessionInfo, ref int pCount);

		/// <summary>
		/// WTSs the free memory.
		/// </summary>
		/// <param name="pMemory">The p memory.</param>
		[DllImport("wtsapi32.dll", SetLastError = true)]
		public static extern void WTSFreeMemory(IntPtr pMemory);

		/// <summary>
		/// WTSs the virtual channel open.
		/// </summary>
		/// <param name="hServer">The h server.</param>
		/// <param name="dwSessionID">The dw session identifier.</param>
		/// <param name="pChannelName">Name of the p channel.</param>
		/// <returns></returns>
		[DllImport("wtsapi32.dll", SetLastError = true)]
		public static extern IntPtr WTSVirtualChannelOpen(IntPtr hServer, int dwSessionID, [MarshalAs(UnmanagedType.LPStr)] string pChannelName);

		/// <summary>
		/// WTSs the virtual channel write.
		/// </summary>
		/// <param name="channelHandle">The channel handle.</param>
		/// <param name="buffer">The buffer.</param>
		/// <param name="length">The length.</param>
		/// <param name="bytesWritten">The bytes written.</param>
		/// <returns></returns>
		[DllImport("wtsapi32.dll", SetLastError = true)]
		public static extern bool WTSVirtualChannelWrite(IntPtr channelHandle, byte[] buffer, int length, ref int bytesWritten);

		/// <summary>
		/// WTSs the virtual channel read.
		/// </summary>
		/// <param name="channelHandle">The channel handle.</param>
		/// <param name="buffer">The buffer.</param>
		/// <param name="length">The length.</param>
		/// <param name="bytesRead">The bytes read.</param>
		/// <returns></returns>
		[DllImport("wtsapi32.dll", SetLastError = true)]
		public static extern bool WTSVirtualChannelRead(IntPtr channelHandle, byte[] buffer, int length, ref int bytesRead);

		/// <summary>
		/// WTSs the virtual channel close.
		/// </summary>
		/// <param name="hChannelHandle">The h channel handle.</param>
		/// <returns></returns>
		[DllImport("wtsapi32.dll", SetLastError = true)]
		public static extern int WTSVirtualChannelClose(IntPtr hChannelHandle);


		/// <summary>
		/// WTSs the virtual channel open ex.
		/// </summary>
		/// <param name="SessionId">The session identifier.</param>
		/// <param name="pVirtualName">Name of the p virtual.</param>
		/// <param name="flags">The flags.</param>
		/// <returns></returns>
		[DllImport("wtsapi32.dll", SetLastError = true)]
		public static extern IntPtr WTSVirtualChannelOpenEx(int SessionId, [MarshalAs(UnmanagedType.LPStr)] string pVirtualName, int flags);

		#endregion


		/// <summary>
		/// Starts the process interactively.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="procInfo">The proc information.</param>
		/// <param name="logonInfo">The logon information.</param>
		/// <param name="runAsLocalSystem">if set to <c>true</c> [run asynchronous local system].</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool StartProcessInteractively(IAutomationRequest request, out ProcessInformation procInfo, LogOnDetails? logonInfo = null, bool runAsLocalSystem = false) {
			var retval = false;
			procInfo = new ProcessInformation();
			var winlogonPid = GetSessionId(WTSGetActiveConsoleSessionId());
			IntPtr hUserTokenDup = IntPtr.Zero, hPToken = IntPtr.Zero, hProcess = IntPtr.Zero;
			var sa = new SecurityAttributes();
			sa.Length = Marshal.SizeOf(sa);
			var si = new StartUpInfo() {
				lpDesktop = InteractiveWindowStation
			};
			si.cb = Marshal.SizeOf(si);

			if ((hProcess = OpenProcess(MaximumAllowed, false, winlogonPid)) != IntPtr.Zero &&
				OpenProcessToken(hProcess, TokenDuplicate, ref hPToken)) {
				if (logonInfo == null) {
					if (runAsLocalSystem) {
						if (DuplicateTokenEx(hPToken, MaximumAllowed, ref sa,
							(int)SecurityImpersonationLevel.SecurityIdentification,
							(int)TokenType.TokenPrimary, ref hUserTokenDup)) {
							retval = CreateProcessAsUser(hUserTokenDup, null, request.FileName, ref sa, ref sa,
								false, CreationFlags, IntPtr.Zero, null, ref si, out procInfo);
						}
					} else {
						WTSQueryUserToken(WTSGetActiveConsoleSessionId(), ref hUserTokenDup);

						retval = CreateProcessAsUser(hUserTokenDup, null, request.FileName, ref sa, ref sa,
							false, CreationFlags, IntPtr.Zero, null, ref si, out procInfo);
					}
				} else {
					retval = StartProcessInteractivelyAsDifferentUser(ref hProcess, ref hPToken, ref hUserTokenDup, logonInfo.Value, ref sa,
																	  ref si, ref procInfo, request.FileName);
				}
			}

			CloseHandle(hProcess);
			CloseHandle(hPToken);
			CloseHandle(hUserTokenDup);

			return retval;
		}


		/// <summary>
		/// Starts the process interactively asynchronous different user.
		/// </summary>
		/// <param name="hProcess">The authentication process.</param>
		/// <param name="hPToken">The authentication application token.</param>
		/// <param name="hUserTokenDup">The authentication user token dup.</param>
		/// <param name="logonInfo">The logon information.</param>
		/// <param name="sa">The sa.</param>
		/// <param name="si">The si.</param>
		/// <param name="procInfo">The proc information.</param>
		/// <param name="imageName">Name of the image.</param>
		/// <returns></returns>
		private bool StartProcessInteractivelyAsDifferentUser(ref IntPtr hProcess, ref IntPtr hPToken, ref IntPtr hUserTokenDup,
			LogOnDetails logonInfo, ref SecurityAttributes sa, ref StartUpInfo si, ref ProcessInformation procInfo, string imageName) {
			//TODO: This method needs to be revised because WDM is killing the process
			var retval = false;
			if (LogonUser(logonInfo.UserName, logonInfo.Domain, logonInfo.Password, Logon32LogonInteractive, Logon32ProviderDefault, ref hUserTokenDup)) {
				var luid = new Luid();
				var sessionId = WTSGetActiveConsoleSessionId();
				if (LookupPrivilegeValue(IntPtr.Zero, SeDebugName, ref luid)) {
					if (SetTokenInformation(hUserTokenDup, TokenInformationClass.TokenSessionId, ref sessionId, (UInt32)IntPtr.Size)) {
						var tp = new TokenPrivileges() {
							PrivilegeCount = 1,
							Privileges = new int[] { luid.LowPart, luid.HighPart, SePrivilegeEnabled }
						};

						if (AdjustTokenPrivileges(hUserTokenDup, false, ref tp, Marshal.SizeOf(tp), IntPtr.Zero, IntPtr.Zero)) {
							retval = CreateProcessAsUser(hUserTokenDup, null, imageName, ref sa, ref sa,
							false, CreationFlags, IntPtr.Zero, null, ref si, out procInfo);
						}
					}
				}
			}

			return retval;
		}

		/// <summary>
		/// Gets the session unique identifier.
		/// </summary>
		/// <param name="sessionId">The session unique identifier.</param>
		/// <returns></returns>
		private uint GetSessionId(uint sessionId) {
			uint retval = 0;

			var found = Process.GetProcessesByName(WinLogonProcessName).FirstOrDefault(x => x.SessionId == sessionId);

			if (found != null)
				retval = (uint)found.Id;

			return retval;
		}

		/// <summary>
		/// Lists the sessions.
		/// </summary>
		/// <param name="serverName">Name of the server.</param>
		/// <returns></returns>
		public List<string> ListSessions(string serverName) {
			var count = 0;
			var retval = new List<string>();
			var ppSessionInfo = IntPtr.Zero;
			var server = WTSOpenServer(serverName);

			try {
				var dataSize = Marshal.SizeOf(typeof(WtsSessionInfo));
				var ret = WTSEnumerateSessions(server, 0, 1, ref ppSessionInfo, ref count);
				var current = (int)ppSessionInfo;

				if (ret > 0) {
					for (var i = 0; i < count; i++) {
						var si = (WtsSessionInfo)Marshal.PtrToStructure((IntPtr)current, typeof(WtsSessionInfo));
						current += dataSize;
						retval.Add($"{si.SessionId} - {si.State} - {si.pWinStationName}");
					}

					WTSFreeMemory(ppSessionInfo);
				}
			} finally {
				WTSCloseServer(server);
			}
			return retval;
		}
	}
}
