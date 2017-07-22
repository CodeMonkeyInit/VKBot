using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VkBot.UI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private const string RedirectUri = "https://oauth.vk.com/blank.html";

        private const string AccessTokenParameter = "access_token=";

        public bool DeleteCookies { get; set; }

        private string AutherizationUrl =
                $"https://oauth.vk.com/authorize?client_id=6095236&display=page&redirect_uri={RedirectUri}&scope={4096 + 65536}&response_type=token&v=5.64&state=12345"
            ;

        public Action<string> OnTokenGained;

        public LoginWindow()
        {
            InitializeComponent();

            LoginBrowser.Navigate(new Uri(AutherizationUrl));
        }
        

        private void LoginBrowserOnNavigated(object sender, NavigationEventArgs e)
        {
            if (DeleteCookies)
            {
                DeleteCookies = false;
                LoginBrowser.Navigate(new Uri(RedirectUri));

                ClearCookies();

                LoginBrowser.Navigate(new Uri(AutherizationUrl));

                return;
            }

            if (!string.IsNullOrWhiteSpace(e.Uri.ToString()))
            {
                var urlAbsolutePath = e.Uri.ToString();
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

        private void ClearCookies()
        {
            dynamic document = LoginBrowser.Document;

            document.ExecCommand("ClearAuthenticationCache", false, null);
        }
    }
}
