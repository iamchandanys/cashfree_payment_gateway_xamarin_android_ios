using System;
using Xamarin.Forms;

namespace WorkingWithWebview
{
    public class Cashfree : ContentPage
    {
        public Cashfree()
        {
            //Removing Navigation Bar so that we can display CF page completly.
            NavigationPage.SetHasNavigationBar(this, false);

            this.LoadCashfreePage("<AppId>", "Order12334248", "100", "test", "Chandan", "chandan.kumar@udmglobal.com", "7019089203", "<Signature>", "https://payments-test.cashfree.com/pgbillpayuiapi/legacy/order/cancel");
        }

        public void LoadCashfreePage(string appId, string orderId, string orderAmount, string orderNote, string customerName, string customerEmail, string customerPhone, string signature, string onClickBackCFUrl)
        {
            if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(orderId) || string.IsNullOrWhiteSpace(orderAmount) ||
                string.IsNullOrWhiteSpace(orderNote) || string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(customerEmail) ||
                string.IsNullOrWhiteSpace(customerPhone) || string.IsNullOrWhiteSpace(signature))
            {
                //ToDo :: Display alert message.
                throw new ApplicationException("Missing Parameter");
            }

            string actionUrl = "https://test.cashfree.com/billpay/checkout/post/submit"; //ToDo :: Need to read from Appsettings
            string returnUrl = "<API_Url>"; //ToDo :: Need to read from Appsettings
            string notifyUrl = ""; //ToDo :: Need to read from Appsettings

            string cfHtmlStr = @"<html>
                                    <body>
                                        <form id=""redirectForm"" method=""post"" action=""$actionUrl$"">
                                            <input type=""hidden"" name=""appId"" value=""$appId$"" />
                                            <input type=""hidden"" name=""orderId"" value=""$orderId$"" />
                                            <input type=""hidden"" name=""orderAmount"" value=""$orderAmount$"" />
                                            <input type=""hidden"" name=""orderCurrency"" value=""INR"" />
                                            <input type=""hidden"" name=""orderNote"" value=""$orderNote$"" />
                                            <input type=""hidden"" name=""customerName"" value=""$customerName$"" />
                                            <input type=""hidden"" name=""customerEmail"" value=""$customerEmail$"" />
                                            <input type=""hidden"" name=""customerPhone"" value=""$customerPhone$"" />
                                            <input type=""hidden"" name=""returnUrl"" value=""$returnUrl$"" />
                                            <input type=""hidden"" name=""notifyUrl"" value=""$notifyUrl$"" />
                                            <input type=""hidden"" name=""source"" value=""custom-mob-sdk"" />
                                            <input type=""hidden"" name=""signature"" value=""$signature$"" />
                                        </form>
                                        <script>document.getElementById(""redirectForm"").submit();</script>
                                    </body>
                                </html>";

            cfHtmlStr = cfHtmlStr.Replace("$actionUrl$", actionUrl);
            cfHtmlStr = cfHtmlStr.Replace("$appId$", appId);
            cfHtmlStr = cfHtmlStr.Replace("$orderId$", orderId);
            cfHtmlStr = cfHtmlStr.Replace("$orderAmount$", orderAmount);
            cfHtmlStr = cfHtmlStr.Replace("$orderNote$", orderNote);
            cfHtmlStr = cfHtmlStr.Replace("$customerName$", customerName);
            cfHtmlStr = cfHtmlStr.Replace("$customerEmail$", customerEmail);
            cfHtmlStr = cfHtmlStr.Replace("$customerPhone$", customerPhone);
            cfHtmlStr = cfHtmlStr.Replace("$returnUrl$", returnUrl);
            cfHtmlStr = cfHtmlStr.Replace("$notifyUrl$", notifyUrl);
            cfHtmlStr = cfHtmlStr.Replace("$signature$", signature);

            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = cfHtmlStr;

            var browser = new WebView();
            browser.Source = htmlSource;
            Content = browser;

            browser.Navigating += (object sender, WebNavigatingEventArgs e) =>
            {
                var url = e.Url;

                //Navigating to Payment SUCCESS or FAILURE Page.
                if (!string.IsNullOrWhiteSpace(url) && url.StartsWith("custom-mob-sdk://"))
                {
                    if (url.Contains("SUCCESS"))
                    {
                        Navigation.PushAsync(new NavigationPage(new ThankYou()));
                    }
                    else
                    {
                        //ToDo :: Redirect to Payment Failure Page.
                    }
                }

                //On click of Cashfree back button 
                if(url == onClickBackCFUrl)
                {
                    Navigation.PopAsync();
                }
            };
        }
    }
}