﻿using MauiPrismDialogErrorDemoApp.Services;
using MauiPrismDialogErrorDemoApp.Views;

namespace MauiPrismDialogErrorDemoApp.ViewModel
{
    public partial class MainPageViewModel : BindableBase
    {
        public DelegateCommand DisplayPopupWithServiceCommand { get; }
        public DelegateCommand DisplayPopupLocalCommand { get; }
        public DelegateCommand AbsoluteNavigationCommand { get; }
        public DelegateCommand RelativeNavigationCommand { get; }

        private INavigationService _navigationService;
        private IDialogService _dialogService;
        private ICustomPopupService _customPopupService;

        public MainPageViewModel(INavigationService navigationService, ICustomPopupService popupService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _customPopupService = popupService;

            DisplayPopupWithServiceCommand = new DelegateCommand(DisplayPopupWithService).ObservesProperty(() => IsBusy);
            DisplayPopupLocalCommand = new DelegateCommand(DisplayPopupLocal).ObservesProperty(() => IsBusy);
            AbsoluteNavigationCommand = new DelegateCommand(PerformAbsoluteNavigation).ObservesProperty(() => IsBusy);
            RelativeNavigationCommand = new DelegateCommand(PerformRelativeNavigation).ObservesProperty(() => IsBusy);
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public async void DisplayPopupWithService()
        {
            // Trigger popupService
            var popupResult = await _customPopupService.PopupTest();
            if (popupResult == ButtonResult.None)
            {
                if (System.Diagnostics.Debugger.IsAttached)
                    System.Diagnostics.Debugger.Break();
            }
        }

        public async void DisplayPopupLocal()
        {
            // Trigger locally
            var popupResult = await _dialogService.ShowDialogAsync(nameof(PopupNotificationDialog));

            if (popupResult.Result == ButtonResult.None)
            {
                if (System.Diagnostics.Debugger.IsAttached)
                    System.Diagnostics.Debugger.Break();
            }
        }

        public async void PerformAbsoluteNavigation()
        {
            var navResult = await _navigationService.CreateBuilder()
                                                    .UseAbsoluteNavigation()
                                                    .AddNavigationPage()
                                                    .AddSegment<SecondPageViewModel>()
                                                    .NavigateAsync();
            if(!navResult.Success)
            {
                if (System.Diagnostics.Debugger.IsAttached)
                    System.Diagnostics.Debugger.Break();
            }
        }

        public async void PerformRelativeNavigation()
        {
            var navResult = await _navigationService.CreateBuilder()
                                                    .AddSegment<SecondPageViewModel>()
                                                    .NavigateAsync();
            if (!navResult.Success)
            {
                if (System.Diagnostics.Debugger.IsAttached)
                    System.Diagnostics.Debugger.Break();
            }
        }

    }
}
