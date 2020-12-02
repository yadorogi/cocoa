﻿using System;
using System.Diagnostics;
using System.Net.Http;
using Acr.UserDialogs;
using Covid19Radar.Common;
using Covid19Radar.Model;
using Covid19Radar.Resources;
using Covid19Radar.Services;
using Covid19Radar.Services.Logs;
using Covid19Radar.Views;
using Newtonsoft.Json.Linq;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Covid19Radar.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private readonly ILoggerService loggerService;
        private readonly UserDataService userDataService;
        private readonly ExposureNotificationService exposureNotificationService;
        private UserDataModel userData;
        private string _startDate;
        private string _pastDate;

        public string StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }
        public string PastDate
        {
            get { return _pastDate; }
            set { SetProperty(ref _pastDate, value); }
        }

        public HomePageViewModel(INavigationService navigationService, ILoggerService loggerService, UserDataService userDataService, ExposureNotificationService exposureNotificationService) : base(navigationService, userDataService, exposureNotificationService)
        {
            Title = AppResources.HomePageTitle;
            this.loggerService = loggerService;
            this.userDataService = userDataService;
            this.exposureNotificationService = exposureNotificationService;

            userData = this.userDataService.Get();
            StartDate = userData.GetLocalDateString();

            TimeSpan timeSpan = DateTime.UtcNow - userData.StartDateTime;
            PastDate = timeSpan.Days.ToString();
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            loggerService.StartMethod();

            // Check Version
            AppUtils.CheckVersion(loggerService);
            try
            {
                await exposureNotificationService.StartExposureNotification();
                await exposureNotificationService.FetchExposureKeyAsync();

                var statusMessage = await exposureNotificationService.UpdateStatusMessageAsync();
                loggerService.Info($"Exposure notification status: {statusMessage}");

                base.Initialize(parameters);

                loggerService.EndMethod();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                loggerService.Exception("Failed to exposure notification status.", ex);
                loggerService.EndMethod();
            }
        }

        public Command OnClickExposures => new Command(async () =>
        {
            loggerService.StartMethod();

            var count = exposureNotificationService.GetExposureCount();
            loggerService.Info($"Exposure count: {count}");
            if (count > 0)
            {
                await NavigationService.NavigateAsync(nameof(ContactedNotifyPage));
                loggerService.EndMethod();
                return;
            }
            else
            {
                await NavigationService.NavigateAsync(nameof(NotContactPage));
                loggerService.EndMethod();
                return;
            }
        });

        public Command OnClickShareApp => new Command(() =>
       {
           loggerService.StartMethod();

           AppUtils.PopUpShare();

           loggerService.EndMethod();
       });
    }
}
