using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Pohgee.SIF.DesktopAgent.Abstractions;
using Pohgee.SIF.DesktopAgent.Abstractions.Enums;
using Pohgee.Framework.LightIoC.Concrete;

namespace Pohgee.SIF.Desktop.DesktopAgent.Shell {
	public class UnifiedShell : IUnifiedShell {
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
		/// The shell window
		/// </summary>
		private object ShellWindow;
		/// <summary>
		/// The singleton
		/// </summary>
		private static volatile UnifiedShell singleton;
		/// <summary>
		/// The synchronize root
		/// </summary>
		private static readonly object syncRoot = new object();

		/// <summary>
		/// Gets or sets the shell.
		/// </summary>
		/// <value>The shell.</value>
		protected ShellType Shell {
			get; set;
		}

		/// <summary>
		/// Gets the h WND desktop.
		/// </summary>
		/// <value>
		/// The h WND desktop.
		/// </value>
		public IntPtr hWndDesktop {
			get; private set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UnifiedShell"/> class.
		/// </summary>
		public UnifiedShell() {
			hWndDesktop = GetDesktopWindow();

		}

		public static IUnifiedShell Current {
			get {
				if (singleton == null) {
					lock (syncRoot) {
						if (singleton == null)
							singleton = new UnifiedShell(ShellType.WindowsForm, GetDesktopWindow());
					}
				}
				return singleton;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UnifiedShell" /> class.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="hwndParent">The HWND parent.</param>
		public UnifiedShell(ShellType type, IntPtr hwndParent) : this() {
			Shell = type;
			hWndDesktop = hwndParent;
		}

		/// <summary>
		/// Configures the specified shell.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="shell">The shell.</param>
		/// <param name="config">The configuration.</param>
		protected virtual void Configure<T>(T shell, IShellConfig config) where T : class {
			Form form;
			Window window;




			if (typeof(T) == typeof(Window)) {
				window = shell as Window;


			} else if (typeof(T) == typeof(Form)) {
				form = shell as Form;

				//TODO: Voy por aqui pero la ladilla no me deja pensar

				var x = SetParent(form.Handle, hWndDesktop);

				form.ShowInTaskbar = true;
				form.Visible = true;
				form.Size = new System.Drawing.Size(800, 800);
				form.Show();


			} /* else
				throw new NotImplementedException("A Window Form or WPF Window were expected. Unable to create unified shell.");	   */

			//			System.Diagnostics.Debugger.Launch();

			ShellWindow = shell;

		}

		/// <summary>
		/// Starts this instance.
		/// </summary>
		/// <param name="config">The configuration.</param>
		public void Start(IShellConfig config) {
			var shellFactory = TypeContainer.Current.Resolve<IShellFactory>();

			switch (Shell) {
				case ShellType.WindowsForm:
					var form =  shellFactory.Build(typeof(Form));
					//var form = (Form)(ShellWindow = new Form());
					Configure(form, config);
					break;
				case ShellType.Wpf:
					// var window = (Window)(ShellWindow = new Window());
					var window = shellFactory.Build(typeof(Form));
					Configure(window, config);
					break;
			}
		}
	}
}
