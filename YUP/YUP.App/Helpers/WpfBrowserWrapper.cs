using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace YUP.App.Helpers
{
    public class WpfWebBrowserWrapper : ContentControl
    {
        public WpfWebBrowserWrapper()
        {
            m_innerBrowser = new WebBrowser()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                VerticalAlignment   = System.Windows.VerticalAlignment.Stretch,
                
            };

            m_innerBrowser.Source = new Uri(@"C:\temp\testme.html");

            this.Content = m_innerBrowser;

            m_innerBrowser.Navigated        += m_innerBrowser_Navigated_;
            m_innerBrowser.Navigating       += m_innerBrowser_Navigating_;
            m_innerBrowser.LoadCompleted    += m_innerBrowser_LoadCompleted_;
            m_innerBrowser.Loaded           += m_innerBrowser_Loaded_;
            m_innerBrowser.SizeChanged      += m_innerBrowser_SizeChanged_;

            this.CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseBack, BrowseBackExecuted_, CanBrowseBack_));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseForward, BrowseForwardExecuted_, CanBrowseForward_));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.Refresh, BrowseRefreshExecuted_));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseStop, BrowseStopExecuted_));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.GoToPage, BrowseGoToPageExecuted_));

            
        }

        public object InvokeScript(string functionName)
        {
           //return  this.m_innerBrowser.InvokeScript(functionName);
            return null;
        }

        public object InvokeScript(string functionName, params object[] args)
        {
            return this.m_innerBrowser.InvokeScript(functionName,args);
        }

        void m_innerBrowser_SizeChanged_(object sender, SizeChangedEventArgs e)
        {
            ApplyZoom(this);
        }

        private void m_innerBrowser_Loaded_(object sender, EventArgs e)
        {
            // make browser control not silent: allow HTTP-Auth-dialogs. Requery command availability
            var ie = ActiveXControl;
            ie.Silent = false;
            CommandManager.InvalidateRequerySuggested();
        }

        // called when the loading of a web page is done
        private void m_innerBrowser_LoadCompleted_(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            ApplyZoom(this);  // apply later and not only at changed event, since only works if browser is rendered.
            SetCurrentValue(IsNavigatingProperty, false);
            CommandManager.InvalidateRequerySuggested();

            this.m_innerBrowser.InvokeScript("stopVideo");
        }

        // called when the browser started to load and retrieve data.
        private void m_innerBrowser_Navigating_(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            SetCurrentValue(IsNavigatingProperty, true);
        }

        // re query the commands once done navigating.
        private void m_innerBrowser_Navigated_(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            RegisterWindowErrorHanlder_();

            CommandManager.InvalidateRequerySuggested();
            // publish the just navigated (maybe redirected) url
            if (e.Uri != null)
            {
                SetCurrentValue(CurrentUrlProperty, e.Uri.ToString());
            }
        }

        // bindable source property to make the browser navigate to the given url. Assign this from your url bar.
        public string BindableSource
        {
            get
            {
                return (string)GetValue(BindableSourceProperty);
            }
            set
            {
                SetValue(BindableSourceProperty, value);
            }
        }

        // bindable property depicting the current url. Use this to read out and present in your url bar.
        public string CurrentUrl
        {
            get
            {
                return (string)GetValue(CurrentUrlProperty);
            }
            set
            {
                SetValue(CurrentUrlProperty, value);
            }
        }

        // percentage value : 20..800 change to let control zoom in out
        public int Zoom
        {
            get
            {
                return (int)GetValue(ZoomProperty);
            }
            set
            {
                SetValue(ZoomProperty, value);
            }
        }

        // percentage value : 20..800 change to let control zoom in out
        public string LastError
        {
            get
            {
                return (string)GetValue(LastErrorProperty);
            }
        }

        // Indicates to the outside that the browser is currently working on a page aka loading. Read only
        public int IsNavigating
        {
            get
            {
                return (int)GetValue(IsNavigatingProperty);
            }
            set
            {
                SetValue(IsNavigatingProperty, value);
            }
        }

        // gets the browser control's underlying activeXcontrol. Ready only from within Loaded-event but before loaded Document!
        // do not use prior loaded event.
        public SHDocVw.InternetExplorer ActiveXControl
        {
            get
            {
                // this is a brilliant way to access the WebBrowserObject prior to displaying the actual document (eg. Document property)
                FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
                if (fiComWebBrowser == null) return null;
                object objComWebBrowser = fiComWebBrowser.GetValue(m_innerBrowser);
                if (objComWebBrowser == null) return null;
                return objComWebBrowser as SHDocVw.InternetExplorer;
            }
        }

        private void BrowseForwardExecuted_(object sender, ExecutedRoutedEventArgs e)
        {
            m_innerBrowser.GoForward();
        }

        private void CanBrowseForward_(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = m_innerBrowser.IsLoaded && m_innerBrowser.CanGoForward;
        }

        private void BrowseBackExecuted_(object sender, ExecutedRoutedEventArgs e)
        {
            m_innerBrowser.GoBack();
        }

        private void CanBrowseBack_(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = m_innerBrowser.IsLoaded && m_innerBrowser.CanGoBack;
        }

        private void BrowseRefreshExecuted_(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                //ActiveXControl.Refresh2(false);
                m_innerBrowser.Refresh(noCache: true);
                //RegisterWindowErrorHanlder_();
            }
            catch (COMException comException)
            {

            }
            // No setting of navigating=true, since the reset event never triggers on Refresh!
            //SetCurrentValue(IsNavigatingProperty, true);
        }

        private void BrowseStopExecuted_(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            var control = this.ActiveXControl;
            if (control != null)
                control.Stop();
        }

        /// <summary>
        /// Navigates to the page specified in the parameter.
        /// </summary>
        private void BrowseGoToPageExecuted_(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            string urlstr = executedRoutedEventArgs.Parameter as string;
            if (urlstr != null)
                m_innerBrowser.Navigate(urlstr);
        }


        private static void BindableSourcePropertyChanged_(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var wrapper = (WpfWebBrowserWrapper)o;
            var browser = wrapper.m_innerBrowser as System.Windows.Controls.WebBrowser;
            if (browser != null)
            {

                string uri = e.NewValue as string;
                if (!string.IsNullOrWhiteSpace(uri) && Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                {
                    Uri uriObj;
                    try
                    {
                        uriObj = new Uri(uri);
                        browser.Source = uriObj;
                    }
                    catch (System.UriFormatException)
                    {
                        // just don't crash because of a malformed url
                    }

                }
                else
                {
                    browser.Source = null;
                }
            }
        }

        // register script errors handler on DOM - document.window
        private void RegisterWindowErrorHanlder_()
        {
            object parwin = ((dynamic)m_innerBrowser.Document).parentWindow;
            var cookie = new System.Windows.Forms.AxHost.ConnectionPointCookie(parwin, new HtmlWindowEvents2Impl(this), typeof(IIntHTMLWindowEvents2));
            // MemoryLEAK? No: cookie has a Finalize() to Disconnect istelf. We'll rely on that. If disconnected too early, 
            // though (eg. in LoadCompleted-event) scripts continue to run and can cause error messages to appear again.
            // --> forget cookie and be happy.
        }

        // needed to implement the Event for script errors
        [Guid("3050f625-98b5-11cf-bb82-00aa00bdce0b"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch), TypeLibType(TypeLibTypeFlags.FHidden)]
        [ComImport]
        private interface IIntHTMLWindowEvents2
        {
            [DispId(1002)]
            bool onerror(string description, string url, int line);
        }

        // needed to implement the Event for script errors
        private class HtmlWindowEvents2Impl : IIntHTMLWindowEvents2
        {
            private WpfWebBrowserWrapper m_control;

            public HtmlWindowEvents2Impl(WpfWebBrowserWrapper control)
            { m_control = control; }

            // implementation of the onerror Javascript error. Return true to indicate a "Handled" state.
            public bool onerror(string description, string urlString, int line)
            {
                this.m_control.SetCurrentValue(LastErrorProperty, description + "@" + urlString + ": " + line);
                // Handled:
                return true;
            }
        }

        // Needed to expose the WebBrowser's underlying ActiveX control for zoom functionality
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("6d5140c1-7436-11ce-8034-00aa006009fa")]
        internal interface IServiceProvider
        {
            [return: MarshalAs(UnmanagedType.IUnknown)]
            object QueryService(ref Guid guidService, ref Guid riid);
        }
        static readonly Guid SID_SWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");

        public static void ZoomPropertyChanged_(DependencyObject src, DependencyPropertyChangedEventArgs e)
        {
            ApplyZoom(src);
        }

        private static void ApplyZoom(DependencyObject src)
        {
            const int k_minZoom = 10;
            const int k_maxZoom = 1000;
            const float k_zoomInReference = 800.0f;


            var browser = src as WpfWebBrowserWrapper;
            if (browser != null && browser.IsLoaded)
            {
                WebBrowser webbr = browser.m_innerBrowser;
                int zoomPercent = browser.Zoom;

                // Determine auto-zoom
                if (browser.Zoom == -1)
                {
                    if (browser.ActualWidth < k_zoomInReference)
                        zoomPercent = (int)(browser.ActualWidth / k_zoomInReference * 100);
                    else
                        zoomPercent = 100;
                }

                // rescue too high or too low values
                zoomPercent = Math.Min(zoomPercent, k_maxZoom);
                zoomPercent = Math.Max(zoomPercent, k_minZoom);

                // grab a handle to the underlying ActiveX object
                IServiceProvider serviceProvider = null;
                if (webbr.Document != null)
                {
                    serviceProvider = (IServiceProvider)webbr.Document;
                }
                if (serviceProvider == null)
                    return;

                Guid serviceGuid = SID_SWebBrowserApp;
                Guid iid = typeof(SHDocVw.IWebBrowser2).GUID;
                SHDocVw.IWebBrowser2 browserInst = (SHDocVw.IWebBrowser2)serviceProvider.QueryService(ref serviceGuid, ref iid);

                try
                {
                    object zoomPercObj = zoomPercent;
                    // send the zoom command to the ActiveX object
                    browserInst.ExecWB(SHDocVw.OLECMDID.OLECMDID_OPTICAL_ZOOM,
                                       SHDocVw.OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER,
                                       ref zoomPercObj,
                                       IntPtr.Zero);
                }
                catch (Exception)
                {
                    // ignore this dynamic call if it fails.
                }
            }
        }


        public static readonly DependencyProperty BindableSourceProperty =
            DependencyProperty.Register("BindableSource", typeof(string), typeof(WpfWebBrowserWrapper), new UIPropertyMetadata("about:blank", BindableSourcePropertyChanged_));

        public static readonly DependencyProperty CurrentUrlProperty =
            DependencyProperty.Register("CurrentUrl", typeof(string), typeof(WpfWebBrowserWrapper), new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty ZoomProperty =
            DependencyProperty.Register("Zoom", typeof(int), typeof(WpfWebBrowserWrapper), new UIPropertyMetadata(100, ZoomPropertyChanged_));

        public static readonly DependencyProperty IsNavigatingProperty =
            DependencyProperty.Register("IsNavigating", typeof(bool), typeof(WpfWebBrowserWrapper), new UIPropertyMetadata(false));

        public static readonly DependencyProperty LastErrorProperty =
            DependencyProperty.Register("LastError", typeof(string), typeof(WpfWebBrowserWrapper), new UIPropertyMetadata(string.Empty));

        private readonly WebBrowser m_innerBrowser;

    }
}

