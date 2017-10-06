
namespace Bullytect.Core.Services.Impl
{

	using System;
	using System.Reactive.Linq;
    using Bullytect.Core.Config;
    using Bullytect.Core.Exceptions;
    using Bullytect.Core.Rest.Models.Exceptions;
    using Bullytect.Core.Rest.Utils;

    public abstract class BaseService
    {

        protected IObservable<T> operationDecorator<T>(IObservable<T> operation){

            return operation.Timeout(TimeSpan.FromSeconds(SharedConfig.TIMEOUT_OPERATIONS_SERVICES_SECOND))
                .Catch<T, ApiException>(ex => Observable.Throw<T>(ApiUtils.parseApiException(ex)))
                .Catch<T, TimeoutException>((ex) => Observable.Throw<T>(new TimeoutOperationException()));
                
        }


    }
}
