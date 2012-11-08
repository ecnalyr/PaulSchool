using System.Web;
using System.Web.Mvc;
using System.IO;

namespace PaulSchool.Controllers
{
    /// <summary>
    /// An ActionResult used to send binary data to the browser.
    /// </summary>
    public class BinaryContentResult : ActionResult
    {
        private string ContentType;
        private byte[] ContentBytes;

        public BinaryContentResult(byte[] contentBytes, string contentType)
        {
            this.ContentBytes = contentBytes;
            this.ContentType = contentType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Clear();
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.ContentType = this.ContentType;

            var stream = new MemoryStream(this.ContentBytes);
            stream.WriteTo(response.OutputStream);
            stream.Dispose();
        }
    }
}