﻿using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Castle.Windsor;
using VkBot.Bot;
using VkBot.BotApi;
using VkBot.SettingsManager;

namespace VkBot.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IWindsorContainer _container;
        private IVkBot _bot;
        private readonly IVkSettingsManager _settingsManager;

        private string AccessToken => _settingsManager.Settings.Token;

        public MainWindow()
        {
            InitializeComponent();

            _container = new WindsorContainer().Install(new UIWindsorInstaller());
            _settingsManager = _container.Resolve<IVkSettingsManager>();

            if (AccessToken == null)
            {
                OpenLoginForm();
            }
            else
            {
                StartButton.IsEnabled = true;
                InitializeBot(AccessToken);
            }
        }

        private void InitializeBot(string accessToken)
        {
            _bot = _container.Resolve<IVkBot>(new {accessToken});

            _bot.Install(_container.Resolve<IBotFunctionsInstaller>());
        }

        private void SuccessfullyLoggedIn(string accessToken)
        {
            InitializeBot(accessToken);

            Settings settings = _settingsManager.Settings;

            settings.Token = accessToken;

            _settingsManager.SaveSettings(settings);

            StartButton.IsEnabled = true;

            MessageBox.Show("Вы успешно вошли");
        }

        private void OpenLoginForm(bool logout = false)
        {
            LoginWindow loginWindow = new LoginWindow();

            if (logout)
            {
                loginWindow.DeleteCookies = true;
            }

            loginWindow.OnTokenGained += SuccessfullyLoggedIn;

            loginWindow.ShowDialog();

            loginWindow.Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");

            e.Handled = regex.IsMatch(e.Text);
        }

        private void StopButtonClicked(object sender, RoutedEventArgs e)
        {
            _bot?.Stop();

            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
        }

        private void StartButtonClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            double intervalInSeconds;

            try
            {
                intervalInSeconds = double.Parse(CheckIntervalTextbox.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Введите корретное значение промежутка между проверками");
                return;
            }

            _bot.Start(TimeSpan.FromSeconds(intervalInSeconds));

            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
        }
        
        private void ExitButtonOnClick(object sender, RoutedEventArgs e)
        {
            OpenLoginForm(true);
        }
    }
}