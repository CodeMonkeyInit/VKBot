using System;
using System.Windows.Forms;
using VkBot.BotApi;
using VkBot.BotApi.Messages;
using VkBot.SettingsManager;
using VkNet.Model.RequestParams;

namespace VKWordCounter.UI
{
    public partial class MainForm : Form
    {
        private readonly IVkApi _vkApi;
        private readonly IVkSettingsManager _settingsManager;

        public MainForm()
        {
            InitializeComponent();

            _vkApi = new VkApi();
            _settingsManager = new VkSettingsManager();

            string accessToken = _settingsManager.Settings.Token;

            Authorize(accessToken);

            CheckIfAuthorized();

            SetDialogsList();
        }

        private async void SetDialogsList()
        {
            dialogsList.Enabled = false;

            VkMessages dialogs = await _vkApi.GetDialogsListAsync();

            Invoke((MethodInvoker) (() =>
            {
                foreach (VkMessageItem dialog in dialogs.Items)
                {
                    dialogsList.Items.Add(dialog);
                }

                dialogsList.Enabled = true;
            }));
        }

        private void CheckIfAuthorized()
        {
            if (!_vkApi.IsAuthorized)
            {
                OpenLoginForm();
            }
        }

        private void Authorize(string accessToken)
        {
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                _vkApi.Login(accessToken);
            }
        }

        private void SuccessfullyLoggedIn(string token)
        {
            Authorize(token);

            Settings settings = _settingsManager.Settings;

            settings.Token = token;

            _settingsManager.SaveSettings(settings);

            MessageBox.Show("Вы успешно вошли");
        }

        private void OpenLoginForm()
        {
            LoginForm loginForm = new LoginForm();

            loginForm.OnTokenGained += SuccessfullyLoggedIn;

            loginForm.ShowDialog();

            loginForm.Dispose();
        }

        private async void CountButtonClick(object sender, EventArgs e)
        {
            CheckIfAuthorized();

            string wordToCount = wordToCountTextBox.Text;

            long? messageDialogId = (dialogsList.SelectedItem as VkMessageItem)?.Message.DialogId;

            long wordCount = await _vkApi.GetWordCountAsync(wordToCount, messageDialogId);

            string wordRepeatedString = $"Слово \"{wordToCount}\" повторялось {wordCount} раз(а).\n";

            Invoke((MethodInvoker) (() => { ResultsRichTextBox.Text += wordRepeatedString; }));

            if (SendToDialogCheckBox.Checked && messageDialogId != null)
            {
                const string botString = "P.S From bot without love";

                string message = wordRepeatedString + botString;

                _vkApi.PostMessage(new MessagesSendParams
                {
                    Message = message,
                    PeerId = messageDialogId
                });
            }
        }
    }
}