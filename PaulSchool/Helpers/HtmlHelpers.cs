using System.Text;
using System.Web.Mvc;

namespace PaulSchool.Helpers
{
    public static class HtmlHelpers
    {
        public enum EButtonType
        {
            BuyNow = 1,
            PayNow = 2
        }

        public enum ECurrencyCode
        {
            /// <summary>
            /// Currency is U.S. Dollar.
            /// </summary>
            USD = 1,
            /// <summary>
            /// Currency is Euro.
            /// </summary>
            EUR = 2
        }

        /// <summary>
        /// Renders a 'Buy Now' or 'Pay Now' PayPal button.
        /// </summary>
        /// <param name="helper">Renders HTML controls in a view.</param>
        /// <param name="useSandbox">True to use the PayPal test environment as a form action, otherwise false.</param>
        /// <param name="buttonType">The type of button to render.</param>
        /// <returns>The HTML that renders a PayPal button.</returns>
        public static MvcHtmlString PayPalButton(this HtmlHelper helper,
            bool useSandbox, EButtonType buttonType, string email,
            string itemName, string itemNumber, float amount, ECurrencyCode currency,
            string completeUrl, string cancelUrl, string ipnUrl)
        {
            string action = useSandbox ?
                "https://www.sandbox.paypal.com/cgi-bin/webscr" :
                "https://www.paypal.com/cgi-bin/webscr";
            StringBuilder html = new StringBuilder("\r\n<form action=\"").Append(action).Append("\">");
            string cmd;
            string buttonImageUrl;
            string pixelImageUrl = useSandbox ? "https://www.sandbox.paypal.com/en_US/i/scr/pixel.gif" :
                "https://www.paypal.com/en_US/i/scr/pixel.gif";

            switch (buttonType)
            {
                case EButtonType.BuyNow:
                    cmd = "_xclick";
                    buttonImageUrl = useSandbox ? "https://www.sandbox.paypal.com/en_US/i/btn/btn_buynowCC_LG.gif" :
                        "https://www.paypal.com/en_US/i/btn/btn_buynowCC_LG.gif";
                    break;
                case EButtonType.PayNow:
                    cmd = "_xclick";
                    buttonImageUrl = useSandbox ? "https://www.sandbox.paypal.com/en_US/i/btn/btn_paynowCC_LG.gif" :
                      "https://www.paypal.com/en_US/i/btn/btn_paynowCC_LG.gif";
                    break;
                default:
                    cmd = "_xclick";
                    buttonImageUrl = useSandbox ? "https://www.sandbox.paypal.com/en_US/i/btn/btn_buynowCC_LG.gif" :
                      "https://www.paypal.com/en_US/i/btn/btn_buynowCC_LG.gif";
                    break;
            }

            html.Append("\r\n<input type=\"hidden\" name=\"cmd\" value=\"").Append(cmd).Append("\" />");
            html.Append("\r\n<input type=\"hidden\" name=\"business\" value=\"").Append(email).Append("\" />");
            html.Append("\r\n<input type=\"hidden\" name=\"item_name\" value=\"").Append(itemName).Append("\" />");
            html.Append("\r\n<input type=\"hidden\" name=\"item_number\" value=\"").Append(itemNumber).Append("\" />");
            html.Append("\r\n<input type=\"hidden\" name=\"amount\" value=\"").Append(amount.ToString(System.Globalization.CultureInfo.InvariantCulture)).Append("\" />");
            html.Append("\r\n<input type=\"hidden\" name=\"currency_code\" value=\"").Append(currency).Append("\" />");
            html.Append("\r\n<input type=\"hidden\" name=\"return\" value=\"").Append(completeUrl).Append("\" />");
            html.Append("\r\n<input type=\"hidden\" name=\"cancel_return\" value=\"").Append(cancelUrl).Append("\" />");
            html.Append("\r\n<input type=\"hidden\" name=\"notify_url\" value=\"").Append(ipnUrl).Append("\" />");

            // By default do not prompt customers to include a note with their payment.
            // Remove this line or set the value to 0 to enable notes.
            html.Append("\r\n<input type=\"hidden\" name=\"no_note\" value=\"1\" />");

            // Render the payment button.
            html.Append("\r\n<input type=\"image\" src=\"").Append(buttonImageUrl).Append("\" name=\"submit\" alt=\"PayPal — The safer, easier way to pay online.\" />");

            // Renders a one-pixel image that should probably be there for tracking purposes.
            html.Append("\r\n<input type=\"image\" src=\"").Append(pixelImageUrl).Append("\" width=\"1\" height=\"1\" alt=\"\" />");

            html.Append("\r\n</form>");

            return new MvcHtmlString(html.ToString());
        }
    }
}
