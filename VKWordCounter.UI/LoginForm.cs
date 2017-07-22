using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VKWordCounter.UI
{
    public partial class LoginForm : Form
    {
        private const string RedirectUri = "https://oauth.vk.com/blank.html";

        private const string AccessTokenParameter = "access_token=";

        private string AutherizationUrl =
                $"https://oauth.vk.com/authorize?client_id=6095236&display=page&redirect_uri={RedirectUri}&scope={4096 + 65536}&response_type=token&v=5.64&state=12345"
            ;

        public Action<string> OnTokenGained;

        public LoginForm()
        {
            InitializeComponent();

            browserWindow.Url = new Uri(AutherizationUrl);
        }

        private void OnBrowserWindowDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Url.ToString()))
            {
                var urlAbsolutePath = e.Url.ToString();
                var indexOfHash = urlAbsolutePath.IndexOf('#');

                string urlSubstring =
                    urlAbsolutePath.Substring(0, indexOfHash != -1 ? indexOfHash : urlAbsolutePath.Length);

                if (urlSubstring == RedirectUri)
                {
                    var startIndex = urlAbsolutePath.LastIndexOf(AccessTokenParameter) + AccessTokenParameter.Length;

                    var indexOfNextParameter = urlAbsolutePath.IndexOf('&', startIndex);
                    string accessToken =
                        urlAbsolutePath.Substring(startIndex, indexOfNextParameter - startIndex);

                    Hide();

                    OnTokenGained(accessToken);
                }
            }
        }
    }
}