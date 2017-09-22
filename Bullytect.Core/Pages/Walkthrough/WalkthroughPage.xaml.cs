

using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Bullytect.Core.Pages.Walkthrough.Items;
using Bullytect.Core.Utils;
using Bullytect.Core.ViewModels;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Walkthrough
{
    public partial class WalkthroughPage : MvxCarouselPage<WalkthroughViewModel>
    {

		readonly Command _closeCommand;
		readonly Command _moveNextCommand;
		bool _closing;
		bool _movingNext;

        public WalkthroughPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
			_closeCommand = new Command(async () => await Close(), () => !Closing);
			_moveNextCommand = new Command(async () => await MoveNext(), () => !MovingNext);
        }

		protected async override void OnCurrentPageChanged()
		{
			base.OnCurrentPageChanged();
			var currentStep = (WalkthroughStepItemTemplate)CurrentPage;

			await currentStep.AnimateIn();
		}

		public bool Closing
		{
			get
			{
				return _closing;
			}

			set
			{
				if (_closing != value)
				{
					_closing = value;

					_closeCommand.ChangeCanExecute();
				}
			}
		}

		public bool MovingNext
		{
			get
			{
				return _movingNext;
			}

			set
			{
				if (_movingNext != value)
				{
					_movingNext = value;

					_moveNextCommand.ChangeCanExecute();
				}
			}
		}

		public ICommand CloseCommand => _closeCommand;

		public ICommand MoveNextCommand => _moveNextCommand;

		private async Task Close()
		{
            Debug.WriteLine("Close Command Called ...");
			if (!Closing)
			{
				Closing = true;

				try
				{
					await  Navigation.PopModalAsync();
				}
				finally
				{
					Closing = false;
				}
			}
		}

		private async Task MoveNext()
		{
            Debug.WriteLine("Move Next Command Called ...");
			if (!MovingNext)
			{
				MovingNext = true;

				try
				{
					await GoToStep();
				}
				finally
				{
					MovingNext = false;
				}
			}
		}

		private async Task GoToStep()
		{
            Debug.WriteLine("Go To Step ...");
			var index = Children.IndexOf(CurrentPage);
			var moveToIndex = 0;
			if (index < Children.Count - 1)
			{
				moveToIndex = index + 1;

				SelectedItem = Children[moveToIndex];
			}
			else
			{
				await Close();
			}
		}
    }
}
