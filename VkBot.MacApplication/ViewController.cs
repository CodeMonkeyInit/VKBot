﻿﻿using System;
using System.Diagnostics;
 using System.IO;
 using AppKit;
//using Castle.Windsor;
using Foundation;
using VkBot.Bot;
  using VkBot.Functions;
  using VkBot.SettingsManager;

namespace VkBot.MacApp
{
    public partial class ViewController : NSViewController
    {
        private const int StartButtonTag = 1;
        private const int StopButtonTag = 2;
        private const int IntervalsTextFieldTag = 4;

        private readonly IVkSettingsManager _settingsManager;
        private IVkBot _bot;

        private NSButton _stopButton;
        private NSButton _startButton;
        private NSTextField _intervalsTextField;

        private string AccessToken => _settingsManager.Settings.Token;

        public ViewController(IntPtr handle) : base(handle)
        {
            _settingsManager = new VkSettingsManager();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitializeControlls();

            if (string.IsNullOrEmpty(AccessToken))
            {
                OpenLoginWindow();
            }
            else
            {
                _bot = new Bot.VkBot(AccessToken);
                _bot.Install(new BotFunctionsInstaller());
            }

        }

        partial void OnStartButtonClicked(NSObject sender)
        {
            var intervalInSeconds = _intervalsTextField.DoubleValue;

            _bot.Start(TimeSpan.FromSeconds(intervalInSeconds));

            _startButton.Enabled = false;

            _stopButton.Enabled = true;
        }

        partial void OnStopButtonClick(NSObject sender)
        {
            _bot.Stop();

            _startButton.Enabled = true;

            _stopButton.Enabled = false;
        }

        public override NSObject RepresentedObject
        {
            get { return base.RepresentedObject; }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

        private void OpenLoginWindow()
        {
            var browserViewController = new BrowserViewController();
        }

        private void InitializeControlls()
        {
            _stopButton = View.ViewWithTag(StopButtonTag) as NSButton;
            _startButton = View.ViewWithTag(StartButtonTag) as NSButton;
            _intervalsTextField = View.ViewWithTag(IntervalsTextFieldTag) as NSTextField;

            _stopButton.Enabled = false;
        }
    }
}