using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bullytect.Core.Rest.Models.Response;
using Bullytect.Core.Rest.Models.Exceptions;
using Bullytect.Core.Rest.Utils;

namespace Bullytect.Core.Utils
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


        static NoAlertsFoundException createNoAlertsFoundException(ApiException ex) {
			return new NoAlertsFoundException()
			{
				Response = ex.GetContentAs<APIResponse<string>>()
			};
        }

        static NoChildrenFoundException createNoChildrenFoundException(ApiException ex) {
            return new NoChildrenFoundException()
            {
                Response = ex.GetContentAs<APIResponse<string>>()
            };
        }

        static LoadProfileFailedException createLoadProfileFailedException(ApiException ex)
        {
            return new LoadProfileFailedException()
            {
                Response = ex.GetContentAs<APIResponse<string>>()
            };
        }

        static GenericErrorException createGenericErrorException(ApiException ex)
        {
            return new GenericErrorException()
            {
                Response = ex.GetContentAs<APIResponse<string>>()
            };
        }

        static UploadImageFailException createUploadImageFailException(ApiException ex) 
        {
            return new UploadImageFailException()
            {
                Response = ex.GetContentAs<APIResponse<string>>()
            };
        }

		static NoSchoolFoundException createNotSchoolFoundException(ApiException ex)
		{
			return new NoSchoolFoundException()
			{
				Response = ex.GetContentAs<APIResponse<string>>()
			};
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
                            exResponse = createNoAlertsFoundException(ex);
                            break;
                            // Parse No Children Found
                        case ResponseNames.NO_CHILDREN_FOUND_FOR_SELF_PARENT_RESPONSE:
                            exResponse = createNoChildrenFoundException(ex);
                            break;
                            // Parse Load Self Profile Error
                        case ResponseNames.PARENT_NOT_FOUND_RESPONSE:
                            exResponse = createLoadProfileFailedException(ex);
                            break;
                            // Parse Upload File Exception
                        case ResponseNames.FAILED_TO_UPLOAD_IMAGE_RESPONSE:
                            exResponse = createUploadImageFailException(ex);
                            break;
                            // Parse No Schools not found
                        case ResponseNames.NO_SCHOOLS_FOUND_RESPONSE:
                            exResponse = createNotSchoolFoundException(ex);
                            break;
							// Parse Generic Error
                        case ResponseNames.GENERIC_ERROR_RESPONSE:
                            exResponse = createGenericErrorException(ex);
                            break;

					}
				}
			}
            return exResponse ?? ex;

		}

    }
}
