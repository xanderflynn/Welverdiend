using Acr.UserDialogs;
using CommunityToolkit.Maui.Behaviors;
using Camera.MAUI;
using SQLite;
using Wel.Utils;

namespace Welverdiend7.Pages;

public partial class Login : ContentPage
{
    SQLiteAsyncConnection Database;
    Wel.Data.userObject user = new Wel.Data.userObject();


    public Login()
    {
        InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        txtUsername.ReturnType = ReturnType.Done;

        txtPassword.ReturnType = ReturnType.Done;

        txtPassword.Completed += TxtPassword_Completed;

        SetFocusOnEntryCompletedBehavior.SetNextElement(txtUsername, txtPassword);

    }

    public void TxtPassword_Completed(object sender, EventArgs e)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        //if (Platform.CurrentActivity.CurrentFocus != null)
        //    Platform.CurrentActivity.HideKeyboard(Platform.CurrentActivity.CurrentFocus);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    public void hide()
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        //if (Platform.CurrentActivity.CurrentFocus != null)

        //    Platform.CurrentActivity.HideKeyboard(Platform.CurrentActivity.CurrentFocus);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }


    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (cameraView.Cameras.Count > 0)
        {
            cameraView.Camera = cameraView.Cameras.First();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await cameraView.StopCameraAsync();
                await cameraView.StartCameraAsync();
            });
        }
    }

    private void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            barcodeResult.Text = $"{args.Result[0].BarcodeFormat}: {args.Result[0].Text}";
        });
    }


    private void Button_Clicked2(object sender, EventArgs e)
    {
        //UserDialogs.Instance.Loading("Loading...", async () =>
        //{

        //	UserDialogs.Instance.HideLoading();
        //}, null, true, MaskType.Gradient);

        //loaderscreen.IsRunning = true;

    }



    private async void Button_Clicked(object sender, EventArgs e)
    {
        //    using (IProgressDialog progress = UserDialogs.Instance.Progress("Login In", null, null, true, MaskType.Black))
        //    {
        //        for (int i = 0; i < 100; i++)
        //        {
        //            progress.PercentComplete = i;
        //await Task.Delay(100);
        //        }
        //}
        hide();
        using (var dlg = UserDialogs.Instance.Loading("Loging in...", null, null, true, MaskType.Black))
        {

            if (!Connection.CheckInternetConnection())
            {

                await Navigation.PushModalAsync(new NewPage1(null));
            }
            else
            {


                //senduser LoginData = new senduser();
                SQLiteAsyncConnection Database;

                if (txtPassword.Text == null && txtUsername.Text == null)
                {


                    //               txtPassword.Text = "P@ssw0rd";
                    //               txtUsername.Text = "Xander";

                    //txtPassword.Text = "P@ssword";
                    //txtUsername.Text = "EFly4";

                }



                string password = txtPassword.Text;
                string username = txtUsername.Text;



                Wel.Data.userObject user = await Wel.Services.Client.Login(username, password);

                try
                {
                    if (user.statusCode == 200 && user != null)
                    {
                        await Wel.Services.SQLFunctions.Init();
                        //List<Wel.Data.DyanamicData> dyanamicDatas = new List<Wel.Data.DyanamicData>();

                        //    dyanamicDatas = await Wel.Services.Client.GetChecklists();
                        //    if (dyanamicDatas != null)
                        //    {
                        //        string i = await Wel.Services.SQLFunctions.SaveItemAsync(dyanamicDatas);

                        //    }
                        //List<DynamicDataUploadTable> existingData = await Wel.Services.SQLFunctions.GetAllQuestions();



                        await Navigation.PushModalAsync(new NewPage1(await Wel.Services.Client.GetChecklists()));

                    }
                    else
                    {
                        await DisplayAlert("ATTENTION", "Application communication error", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Communication breakdown, please try again later.", "OK");
                }
            }
        }
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {




        //  async Task<bool> Init()
        //  {
        //      if (Database is not null)
        //          return true;

        //      Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);
        //      var result = await Database.CreateTableAsync<DyanamicData>();
        //return true;
        //  }


        //	if (user.statusCode == 200)
        //	{
        //		await Wel.Services.SQLFunctions.Init();
        //                 //List<Wel.Data.DyanamicData> dyanamicDatas = new List<Wel.Data.DyanamicData>();

        //                 //    dyanamicDatas = await Wel.Services.Client.GetChecklists();
        //                 //    if (dyanamicDatas != null)
        //                 //    {
        //                 //        string i = await Wel.Services.SQLFunctions.SaveItemAsync(dyanamicDatas);

        //                 //    }
        //                     //List<DynamicDataUploadTable> existingData = await Wel.Services.SQLFunctions.GetAllQuestions();



        //                 await Navigation.PushModalAsync(new NewPage1());

        //	}
        //	else
        //	{
        //		await DisplayAlert("ATTENTION", "Application communication error", "OK");
        //	}

        //}
    }
}



//  async Task<bool> Init()
//  {
//      if (Database is not null)
//          return true;

//      Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);
//      var result = await Database.CreateTableAsync<DyanamicData>();
//return true;
//  }

