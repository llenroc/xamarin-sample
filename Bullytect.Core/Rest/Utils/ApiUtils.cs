using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bullytect.Core.Rest.Models.Response;
using Bullytect.Core.Rest.Models.Exceptions;
using Bullytect.Core.Rest.Utils;

namespace Bullytect.Core.Rest.Utils
{
    public static class ApiUtils
    {

        static DataInvalidException createDataInvalidException(ApiException ex) {

            DataInvalidException exResponse = null;
			var response = ex.GetContentAs<APIResponse<ValidationErrorDTO>>();
			if (response != null)
			{
				Dictionary<string, string> fieldErrors = new Dictionary<string, string>();
				List<FieldErrorDTO> responseFieldErrors = response.Data.FieldErrors;
				for (var i = 0; i < responseFieldErrors.Count(); i++)
				{
					FieldErrorDTO fieldError = responseFieldErrors.ElementAt(i);
					if (!fieldErrors.ContainsKey(fieldError.Field))
					{
						fieldErrors.Add(fieldError.Field, fieldError.Message);
					}
				}

				exResponse = new DataInvalidException()
				{
					FieldErrors = fieldErrors
				};
			}

            return exResponse;
        }

        public static Exception parseApiException(ApiException ex) {

            Exception exResponse = null;
			if (ex.Headers.Contains(ResponseNames.RESPONSE_HEADER_NAME))
			{
				IEnumerable<string> values = ex.Headers.GetValues(ResponseNames.RESPONSE_HEADER_NAME);
				string responseName = values.First();
				if (!String.IsNullOrEmpty(responseName))
				{
					Debug.WriteLine(String.Format("Response Format: {0}", responseName));
                    switch(responseName) {

                        // Parse Validation Error
                        case ResponseNames.VALIDATION_ERROR_RESPONSE:
                            exResponse = createDataInvalidException(ex);
                            break;
                            // Parse Alerts Not Found
                        case ResponseNames.NO_ALERTS_FOUND_RESPONSE:
							exResponse = new NoAlertsFoundException()
							{
								Response = ex.GetContentAs<APIResponse<string>>()
							};
                            break;
                            // Parse No Children Found
                        case ResponseNames.NO_CHILDREN_FOUND_FOR_SELF_PARENT_RESPONSE:
							exResponse = new NoChildrenFoundException(){
                                Response = ex.GetContentAs<APIResponse<string>>()
                			};
                            break;
                            // Parse Load Self Profile Error
                        case ResponseNames.PARENT_NOT_FOUND_RESPONSE:
							exResponse = new LoadProfileFailedException()
							{
								Response = ex.GetContentAs<APIResponse<string>>()
							};
                            break;
                            // Parse Upload File Exception
                        case ResponseNames.FAILED_TO_UPLOAD_IMAGE_RESPONSE:
							exResponse = new UploadImageFailException()
							{
								Response = ex.GetContentAs<APIResponse<string>>()
							};
                            break;
                            // Parse No Schools not found
                        case ResponseNames.NO_SCHOOLS_FOUND_RESPONSE:
							exResponse = new NoSchoolFoundException()
							{
								Response = ex.GetContentAs<APIResponse<string>>()
							};
                            break;
                            // Parse Bad Credentials Response
                        case ResponseNames.BAD_CREDENTIALS_RESPONSE:
							exResponse = new AuthenticationFailedException()
							{
								Response = ex.GetContentAs<APIResponse<string>>()
							};
                            break;
                        case ResponseNames.ACCOUNT_DISABLED_RESPONSE:
							exResponse = new AccountDisabledException()
							{
								Response = ex.GetContentAs<APIResponse<string>>()
							};
                            break;
						case ResponseNames.NO_NEW_ALERTS_FOUND_RESPONSE:
							exResponse = new NoNewAlertsFoundException()
							{
								Response = ex.GetContentAs<APIResponse<string>>()
							};
							break;
                        case ResponseNames.NO_ALERTS_BY_SON_FOUNDED_RESPONSE:
							exResponse = new NoAlertsFoundException()
							{
								Response = ex.GetContentAs<APIResponse<string>>()
							};
                            break;
							// Parse Generic Error
                        case ResponseNames.GENERIC_ERROR_RESPONSE:
							exResponse = new GenericErrorException()
							{
								Response = ex.GetContentAs<APIResponse<string>>()
							};
                            break;

					}
				}
			}
            return exResponse ?? ex;

		}

    }
}
