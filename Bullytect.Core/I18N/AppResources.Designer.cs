﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bullytect.Core.I18N {
    using System;
    using System.Reflection;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class AppResources {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AppResources() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("Bullytect.Core.I18N.AppResources", typeof(AppResources).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static string WelcomePage_MainLabel {
            get {
                return ResourceManager.GetString("WelcomePage.MainLabel", resourceCulture);
            }
        }
        
        internal static string WelcomePage_Login_Btn {
            get {
                return ResourceManager.GetString("WelcomePage.Login.Btn", resourceCulture);
            }
        }
        
        internal static string WelcomePage_Signup_Btn {
            get {
                return ResourceManager.GetString("WelcomePage.Signup.Btn", resourceCulture);
            }
        }
        
        internal static string WelcomePage_Facebook_Btn {
            get {
                return ResourceManager.GetString("WelcomePage.Facebook.Btn", resourceCulture);
            }
        }
        
        internal static string Login_Authenticating {
            get {
                return ResourceManager.GetString("Login.Authenticating", resourceCulture);
            }
        }
        
        internal static string Login_Success {
            get {
                return ResourceManager.GetString("Login.Success", resourceCulture);
            }
        }
        
        internal static string Login_Failed {
            get {
                return ResourceManager.GetString("Login.Failed", resourceCulture);
            }
        }
        
        internal static string Signup_CreatingAccount {
            get {
                return ResourceManager.GetString("Signup.CreatingAccount", resourceCulture);
            }
        }
        
        internal static string Signup_Account_Created {
            get {
                return ResourceManager.GetString("Signup.Account.Created", resourceCulture);
            }
        }
        
        internal static string Global_ErrorOcurred {
            get {
                return ResourceManager.GetString("Global.ErrorOcurred", resourceCulture);
            }
        }
    }
}
