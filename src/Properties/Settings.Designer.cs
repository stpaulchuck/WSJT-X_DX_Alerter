﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WSJTX_DX_Alerter.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.7.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string alltxtLocation {
            get {
                return ((string)(this["alltxtLocation"]));
            }
            set {
                this["alltxtLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string xmlLogLocation {
            get {
                return ((string)(this["xmlLogLocation"]));
            }
            set {
                this["xmlLogLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AutoStart {
            get {
                return ((bool)(this["AutoStart"]));
            }
            set {
                this["AutoStart"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool AlertUnconfirmed {
            get {
                return ((bool)(this["AlertUnconfirmed"]));
            }
            set {
                this["AlertUnconfirmed"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SendEmail {
            get {
                return ((bool)(this["SendEmail"]));
            }
            set {
                this["SendEmail"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SendText {
            get {
                return ((bool)(this["SendText"]));
            }
            set {
                this["SendText"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("/nRvkUekipS7ZuZ/B5SjAVoqNNhKbvj1NXB+un421GknkzmZ3pB59qp5hLDpIE44")]
        public string SMTPserver {
            get {
                return ((string)(this["SMTPserver"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("587")]
        public int SMTPport {
            get {
                return ((int)(this["SMTPport"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("fag+M+D3BHlyfgu7BWBy5A==")]
        public string EmailPWD {
            get {
                return ((string)(this["EmailPWD"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("EmMSqih1syzYUgo5ceTqSwLSgsCLuU8DH753ofSCz25gFybqR1418MN89fJicsPwAwk+dSbc7umEVTbJD" +
            "2n2jQ==")]
        public string EmailUID {
            get {
                return ((string)(this["EmailUID"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("EmMSqih1syzYUgo5ceTqSwLSgsCLuU8DH753ofSCz25gFybqR1418MN89fJicsPwAwk+dSbc7umEVTbJD" +
            "2n2jQ==")]
        public string EmailTo {
            get {
                return ((string)(this["EmailTo"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("S8JvM4iWuFkdNSnVJ2aZ9ippfDKjuZjmpEWx2YnVZobrAmqwELfbyRjxdChbQLwTQtH2wqycpSI=")]
        public string EmailFrom {
            get {
                return ((string)(this["EmailFrom"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool LogHits {
            get {
                return ((bool)(this["LogHits"]));
            }
            set {
                this["LogHits"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool QuietMode {
            get {
                return ((bool)(this["QuietMode"]));
            }
            set {
                this["QuietMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("-17")]
        public int ThresholdValue {
            get {
                return ((int)(this["ThresholdValue"]));
            }
            set {
                this["ThresholdValue"] = value;
            }
        }
    }
}
