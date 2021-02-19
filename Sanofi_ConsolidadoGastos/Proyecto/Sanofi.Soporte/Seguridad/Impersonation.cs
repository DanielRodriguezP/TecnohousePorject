using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;

namespace Sanofi.Soporte.Seguridad
{
    public class Impersonation : IDisposable
    {
        private readonly SafeTokenHandle _handle;
        private const int Logon32LogonInteractive = 2;
        private readonly WindowsImpersonationContext _context;

        public Impersonation(string dominio, string usuario, string contraseña)
        {
            var domain = dominio;
            var username = usuario;
            var password = contraseña;
            var ok = LogonUser(username, domain, password, Logon32LogonInteractive, 0, out _handle);
            if (!ok)
            {
                var errorCode = Marshal.GetLastWin32Error();
                throw new ApplicationException(string.Format("Could not impersonate the elevated user.  LogonUser returned error code {0}.", errorCode));
            }
            _context = WindowsIdentity.Impersonate(_handle.DangerousGetHandle());
        }
        public void Dispose()
        {
            _context.Dispose();
            _handle.Dispose();
        }

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);
        public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            private SafeTokenHandle()
                : base(true) { }

            [DllImport("kernel32.dll")]
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            [SuppressUnmanagedCodeSecurity]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool CloseHandle(IntPtr handle);

            protected override bool ReleaseHandle()
            {
                return CloseHandle(handle);
            }
        }
    }
}
