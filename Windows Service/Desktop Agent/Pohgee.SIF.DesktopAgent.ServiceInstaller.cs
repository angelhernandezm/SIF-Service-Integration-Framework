using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace  Pohgee.SIF.DesktopAgent {
	[RunInstaller(true)]
	public partial class SifAgentServiceInstaller : System.Configuration.Install.Installer {
		public SifAgentServiceInstaller() {
			InitializeComponent();
		}
	}
}
