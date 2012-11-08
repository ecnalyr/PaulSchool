using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaulSchool
{
    public class CustomViewEngine : RazorViewEngine
    {
        private static readonly string[] NewPartialViewFormats =
            new[]
            {
                "~/Views/Shared/EditorTemplates/{0}.cshtml"
            };

        public CustomViewEngine()
        {
            base.PartialViewLocationFormats = base.PartialViewLocationFormats.Union(NewPartialViewFormats).ToArray();
        }
    }
}