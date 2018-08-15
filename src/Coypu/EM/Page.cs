using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using Coypu;

namespace Coypu
{
    public class Page : IDisposable
    {
        public string Url = "";
        static protected NullScope dummy;
        protected BrowserSession session;

        static Page()
        {
            dummy = new NullScope();
        }

        public void Init(BrowserSession b)
        {
            session = b;

            var fields = GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var f in fields)
            {
                if (typeof(IHaveScope).IsAssignableFrom(f.FieldType))
                {
                    var e = (IHaveScope)f.GetValue(this);
                    if (e != null)
                        e.SetScope(session);
                }
            }
        }

        public void Dispose()
        {

        }

        public static Options Merge(Options options)
        {
            if (options == null)
                return new Options { Match = Match.First };
            else
                return options;
        }

        public ElementScope FindCss(string locator, Options options = null)
        {
            return session.FindCss(locator, Merge(options));
        }

        public ElementScope FindLink(string locator, Options options = null)
        {
            return session.FindLink(locator, Merge(options));
        }

        public ElementScope FindField(string locator, Options options = null)
        {
            return session.FindField(locator, Merge(options));
        }

        public FieldAutocomplete FindFieldAutocomplete(string fieldXPath, string listXPath, string elementsXPath, Options options = null)
        {
            return session.FindFieldAutocomplete(fieldXPath, listXPath, elementsXPath, Merge(options));
        }

        public ElementScope FindSelect(string locator, Options options = null)
        {
            return session.FindSelect(locator, Merge(options));
        }

        public ElementScope FindText(string text, Options options = null)
        {
            return session.FindXPath($"//*[text()='{text}' or contains(text(),'{text}')]", Merge(options));
        }

        public ElementScope FindButton(string locator, Options options = null)
        {
            return session.FindButton(locator, Merge(options));
        }

        public ElementScope FindXPath(string locator, Options options = null)
        {
            return session.FindXPath(locator, Merge(options));
        }

        public static ElementScope Css(string locator, Options options = null)
        {
            return dummy.FindCss(locator, Merge(options));
        }

        public static ElementScope Link(string locator, Options options = null)
        {
            return dummy.FindLink(locator, Merge(options));
        }

        public static ElementScope Field(string locator, Options options = null)
        {
            return dummy.FindField(locator, Merge(options));
        }

        public static ElementScope Select(string locator, Options options = null)
        {
            return dummy.FindSelect(locator, Merge(options));
        }

        public static ElementScope Text(string text, Options options = null)
        {
            return dummy.FindXPath($"//*[text()='{text}' or contains(text(),'{text}')]", Merge(options));
        }

        public static ElementScope Button(string locator, Options options = null)
        {
            return dummy.FindButton(locator, Merge(options));
        }

        public static ElementScope XPath(string locator, Options options = null)
        {
            return dummy.FindXPath(locator, Merge(options));
        }

        public static T Container<T>(string locator, Options options = null) where T : ContainerScope, new()
        {
            if (options == null)
                options = new Options { Match = Match.First };
            var x = new T();
            x.Init(locator, dummy, options);
            return x;
        }

        public static T Container<T>(SnapshotElementScope e, Options options = null) where T : ContainerScope, new()
        {
            if (options == null)
                options = new Options { Match = Match.First };
            var x = new T();
            x.Init(e, options);
            return x;
        }

        public static T Container<T>(Options options = null) where T : ContainerScope, new()
        {
            if (options == null)
                options = new Options { Match = Match.First };
            var x = new T();
            x.Init(dummy, options);
            return x;
        }
    }
}