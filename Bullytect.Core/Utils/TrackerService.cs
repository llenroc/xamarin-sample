using System;
using System.Diagnostics;
using Bullytect.Core.Config;
using PCLStorage;

namespace Bullytect.Core.Services.Impl
{
    public class TrackerService
    {

        static TrackerService INSTANCE = null;

        IFile trackerFile = null;

        private TrackerService()
		{
			initFolder();
		}

        async void initFolder(){

            try{
                IFolder rootFolder = SpecialFolder.Current.Documents;
    			IFolder folder = await rootFolder.CreateFolderAsync(SharedConfig.TRACKER_FOLDER_NAME,
    				CreationCollisionOption.OpenIfExists);
                trackerFile = await folder.CreateFileAsync(String.Format("tracker_{0}.txt", new DateTime().ToString("yyyy_MM_dd")),
                                                          CreationCollisionOption.GenerateUniqueName);
            }catch(Exception ex) {
                Debug.WriteLine("TrackerService failed with Exception " + ex.Message);
            }
        }

        public static TrackerService getInstance(){

            if (INSTANCE == null)
                INSTANCE = new TrackerService();

            return INSTANCE;

        }

        public async void Save(string Data)
        {
            try
			{
			    await trackerFile?.WriteAllTextAsync(Data);
            } catch (Exception ex) {
				Debug.WriteLine("TrackerService failed with Exception " + ex.Message);
			}
        }
    }
}
