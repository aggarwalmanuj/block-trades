﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Spider.Invest.Sys.BlockTrades.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2014-10-01")]
        public global::System.DateTime FromDate {
            get {
                return ((global::System.DateTime)(this["FromDate"]));
            }
            set {
                this["FromDate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2014-10-17")]
        public global::System.DateTime ToDate {
            get {
                return ((global::System.DateTime)(this["ToDate"]));
            }
            set {
                this["ToDate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("SPY")]
        public string FavSymbol {
            get {
                return ((string)(this["FavSymbol"]));
            }
            set {
                this["FavSymbol"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string Symbol {
            get {
                return ((string)(this["Symbol"]));
            }
            set {
                this["Symbol"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("358333297-G3JbJs6MhtVzklY8TOJrriQGaFTGtL5USBr2lv1f")]
        public string TwitterAccessToken {
            get {
                return ((string)(this["TwitterAccessToken"]));
            }
            set {
                this["TwitterAccessToken"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("iJHR0If6gxgCsB18FClG72z9P3F9pYOPHZSh5O8jVu6tt")]
        public string TwitterAccessTokenSecret {
            get {
                return ((string)(this["TwitterAccessTokenSecret"]));
            }
            set {
                this["TwitterAccessTokenSecret"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("tTvS4oGgZPzj1QqsMVSR6BVVZ")]
        public string TwitterConsumerKey {
            get {
                return ((string)(this["TwitterConsumerKey"]));
            }
            set {
                this["TwitterConsumerKey"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("eIhPqpgDnjlUKk6pDRQAMkMaga8TOfcKk7h2Dk42gCFMDTZzc7")]
        public string TiwtterConsumerSecret {
            get {
                return ((string)(this["TiwtterConsumerSecret"]));
            }
            set {
                this["TiwtterConsumerSecret"] = value;
            }
        }
    }
}
