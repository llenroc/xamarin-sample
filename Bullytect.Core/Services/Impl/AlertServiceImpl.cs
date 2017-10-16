using System;
using Bullytect.Core.Rest.Services;
using System.Reactive.Linq;
using Bullytect.Core.Rest.Models.Response;
using AutoMapper;
using System.Diagnostics;
using Bullytect.Core.Models.Domain;
using System.Collections.Generic;
using System.Collections;
using Bullytect.Core.Config;
using Bullytect.Core.I18N;
using Bullytect.Core.ViewModels.Core.Models;

namespace Bullytect.Core.Services.Impl
{
    public class AlertServiceImpl: BaseService, IAlertService
    {

        readonly IAlertRestService _alertRestService;

        public AlertServiceImpl(IAlertRestService alertRestService)
        {
            _alertRestService = alertRestService;
        }

        public IObservable<string> ClearAlertsOfSon(string SonId)
        {

            Debug.WriteLine("Clear Alerts Of Son with id : " + SonId);

			var observable = _alertRestService
                .ClearAlertsOfSon(SonId)
                .Select((response) => response.Data)
				.Finally(() => {
					Debug.WriteLine("Clear Alerts Of Son finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<string> ClearSelfAlerts()
        {
            Debug.WriteLine("Clear Self Alerts");

			var observable = _alertRestService
                .ClearSelfAlerts()
                .Select((response) => response.Data)
				.Finally(() => {
					Debug.WriteLine("Clear Self Alerts Finished ");
				});

			return operationDecorator(observable);
        }

        public IObservable<string> DeleteAlertOfSon(string SonId, string AlertId)
        {
			Debug.WriteLine("Delete Alerts Of Son : " + SonId);

			var observable = _alertRestService
				.DeleteAlertOfSon(SonId, AlertId)
                .Select((response) => response.Data)
				.Finally(() => {
					Debug.WriteLine("Delete Alerts Of Son finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<IList<AlertEntity>> GetAlertsBySon(string SonId)
        {
			Debug.WriteLine("Get Alerts By Son : " + SonId);

			var observable = _alertRestService
				.GetAlertsBySon(SonId)
                .Select((response) => response.Data)
				.Select((Alerts) => Mapper.Map<IList<AlertDTO>, IList<AlertEntity>>(Alerts))
				.Finally(() => {
					Debug.WriteLine("Get Alerts By Son Finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<IList<AlertCategoryModel>> GetAllAlertsCategories()
        {
            Debug.WriteLine("Get All Alerts Categories");

			var list = new List<AlertCategoryModel>() {
                new AlertCategoryModel() {
                    Name = AppResources.Settings_Alerts_Categories_Success_Alerts_Name,
                    Description = AppResources.Settings_Alerts_Categories_Success_Alerts_Description,
					Level = AlertLevelEnum.SUCCESS
                },
                new AlertCategoryModel() {
					Name = AppResources.Settings_Alerts_Categories_Info_Alerts_Name,
                    Description = AppResources.Settings_Alerts_Categories_Info_Alerts_Description,
					Level = AlertLevelEnum.INFO
                },
                new AlertCategoryModel() {
                    Name = AppResources.Settings_Alerts_Categories_Warning_Alerts_Name,
                    Description = AppResources.Settings_Alerts_Categories_Warning_Alerts_Description,
					Level = AlertLevelEnum.WARNING
                },
				new AlertCategoryModel() {
                    Name = AppResources.Settings_Alerts_Categories_Danger_Alerts_Name,
                    Description = AppResources.Settings_Alerts_Categories_Danger_Alerts_Description,
					Level = AlertLevelEnum.DANGER
				}
			};

            return Observable.Return(list);
        
        }

        public IObservable<AlertsPageEntity> GetLastAlertsForSelfParent()
        {
			Debug.WriteLine("Get Last Alerts For Self Parent");

			var observable = _alertRestService
                .GetLastSelfAlerts(
                    Count: Settings.Current.LastAlertsCount,
                    LastMinutes: Settings.Current.AntiquityOfAlerts, 
                    Levels: String.IsNullOrEmpty(Settings.Current.FilteredAlertCategories) ? 
                        new string[] {} : Settings.Current.FilteredAlertCategories.Split(','))
				.Select((APIResponse<AlertsPageDTO> response) => response.Data)
				.Select((AlertsPage) => Mapper.Map<AlertsPageDTO, AlertsPageEntity>(AlertsPage))
				.Finally(() => {
					Debug.WriteLine("Get Last Alerts For Self Parent finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<IList<AlertEntity>> GetSelfAlerts()
        {
			Debug.WriteLine("Get Self Alerts");

			var observable = _alertRestService
                .GetSelfAlerts()
                .Select((response) => response.Data)
                .Select((Alerts) => Mapper.Map<IList<AlertDTO>, IList<AlertEntity>>(Alerts))
				.Finally(() => {
					Debug.WriteLine("Get Self Alerts ...");
				});

			return operationDecorator(observable);
        }
    }
}
