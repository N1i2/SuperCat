﻿#pragma checksum "..\..\..\..\..\Pages\Log\LogPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "A6BC26207146200D3564AAE19B03A18857C7A37C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SuperCat.Log;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace SuperCat.Log {
    
    
    /// <summary>
    /// LogPage
    /// </summary>
    public partial class LogPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 74 "..\..\..\..\..\Pages\Log\LogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid Located;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\..\..\Pages\Log\LogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label LogHeader;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\..\..\Pages\Log\LogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AlreadyHaveAcccount;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\..\..\..\Pages\Log\LogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CreateNewAccount;
        
        #line default
        #line hidden
        
        
        #line 113 "..\..\..\..\..\Pages\Log\LogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label ErrorLog;
        
        #line default
        #line hidden
        
        
        #line 121 "..\..\..\..\..\Pages\Log\LogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label nikname;
        
        #line default
        #line hidden
        
        
        #line 129 "..\..\..\..\..\Pages\Log\LogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label password;
        
        #line default
        #line hidden
        
        
        #line 142 "..\..\..\..\..\Pages\Log\LogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox niknameBox;
        
        #line default
        #line hidden
        
        
        #line 156 "..\..\..\..\..\Pages\Log\LogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox passwordBox;
        
        #line default
        #line hidden
        
        
        #line 163 "..\..\..\..\..\Pages\Log\LogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox liyPasswordBox;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SuperCat;component/pages/log/logpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Pages\Log\LogPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Located = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.LogHeader = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.AlreadyHaveAcccount = ((System.Windows.Controls.Button)(target));
            
            #line 101 "..\..\..\..\..\Pages\Log\LogPage.xaml"
            this.AlreadyHaveAcccount.Click += new System.Windows.RoutedEventHandler(this.LogInAccount);
            
            #line default
            #line hidden
            return;
            case 4:
            this.CreateNewAccount = ((System.Windows.Controls.Button)(target));
            
            #line 108 "..\..\..\..\..\Pages\Log\LogPage.xaml"
            this.CreateNewAccount.Click += new System.Windows.RoutedEventHandler(this.CreateNewAccounts);
            
            #line default
            #line hidden
            return;
            case 5:
            this.ErrorLog = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.nikname = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.password = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.niknameBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.passwordBox = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 10:
            this.liyPasswordBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            
            #line 173 "..\..\..\..\..\Pages\Log\LogPage.xaml"
            ((System.Windows.Controls.Image)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.ShowPass);
            
            #line default
            #line hidden
            
            #line 175 "..\..\..\..\..\Pages\Log\LogPage.xaml"
            ((System.Windows.Controls.Image)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.UnshowPass);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

