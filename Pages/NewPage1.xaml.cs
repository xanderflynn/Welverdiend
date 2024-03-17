using Acr.UserDialogs;
using Wel.Data;
using Wel.Services;

namespace Welverdiend7.Pages;

public partial class NewPage1 : ContentPage
{
    List<Wel.Data.DyanamicData> dyanamicDataCheck = null;
    bool InAppNavcheck = false;
    public NewPage1(List<Wel.Data.DyanamicData> dyanamicData = null, bool InAppNav = false)
    {
        InitializeComponent();

        dyanamicDataCheck = dyanamicData;
        InAppNavcheck = InAppNav;
    }




    protected async override void OnAppearing()
    {

        List<DynamicDataUploadTable> RecordNumber = await SQLFunctions.GetAllQuestions();
        if (dyanamicDataCheck != null && InAppNavcheck == false)
        {
            if (dyanamicDataCheck.Count < RecordNumber.Count || RecordNumber.Count == 0)
            {
                using (var progress = UserDialogs.Instance.Loading("Loading..."))
                {
                    await GetData();

                }
                bool internets = Wel.Utils.Connection.CheckInternetConnection();
                List<Wel.Data.TblCompleted> questions = await SQLFunctions.GetAllCompletedQuestions();
                if (internets && questions.Count != 0)
                {
                    using (var progress = UserDialogs.Instance.Loading("Sync..."))
                    {
                        await Client.SetChecklist();
                    }
                }
            }
            
            
            else
            {




                //using (var progress = UserDialogs.Instance.Loading("Loading..."))
                //{
                //    await GetData();
                //}



                bool internet = Wel.Utils.Connection.CheckInternetConnection();
                if (internet)
                {
                    using (var progress = UserDialogs.Instance.Loading("Sync..."))
                    {
                        await Client.SetChecklist();
                    }
                }

                List<DynamicDataUploadTable> filter = await Wel.Services.SQLFunctions.GetQuery("Select distinct TableName from DynamicData");








                var dynam = new DataTemplate(typeof(Template.HomeViewCell));
                ListView lt = new ListView();
                lt.HasUnevenRows = true;
                lt.SeparatorVisibility = SeparatorVisibility.None;
                lt.BackgroundColor = Colors.Transparent;
                lt.Background = Colors.Transparent;


                //lt.RefreshControlColor = Colors.Transparent;
                //lt.AutoFitMode = AutoFitMode.Height;
                //lt.AutoFitMode = AutoFitMode.DynamicHeight;
                lt.ItemTemplate = dynam;

                lt.ItemsSource = null;

                lt.ItemsSource = filter;

                //lt.Margin = new Thickness(0, 0, 0, 50);
                //}


                //lt.BackgroundColor = Colors.Black;



                if (stckHome.Children.Count > 0)
                {
                    stckHome.Children.Clear();
                }

                stckHome.Children.Add(lt);

                //lt.ItemTapped += Lt_ItemTappedAsync;

                //lt.SelectedItem += Lt_SelectionChanged;

                lt.ItemSelected += Lt_ItemSelectedNew;




                lt.ItemTapped += Lt_ItemTappedNew;

                //}






            }
        }
        else if (dyanamicDataCheck == null && InAppNavcheck == true)
        {
            bool internet = Wel.Utils.Connection.CheckInternetConnection();
            if (internet)
            {
                using (var progress = UserDialogs.Instance.Loading("Sync..."))
                {
                    await Client.SetChecklist();
                }
            }

            List<DynamicDataUploadTable> filter = await Wel.Services.SQLFunctions.GetQuery("Select distinct TableName from DynamicData");








            var dynam = new DataTemplate(typeof(Template.HomeViewCell));
            ListView lt = new ListView();
            lt.HasUnevenRows = true;
            lt.SeparatorVisibility = SeparatorVisibility.None;
            lt.BackgroundColor = Colors.Transparent;
            lt.Background = Colors.Transparent;


            //lt.RefreshControlColor = Colors.Transparent;
            //lt.AutoFitMode = AutoFitMode.Height;
            //lt.AutoFitMode = AutoFitMode.DynamicHeight;
            lt.ItemTemplate = dynam;

            lt.ItemsSource = null;

            lt.ItemsSource = filter;

            //lt.Margin = new Thickness(0, 0, 0, 50);
            //}


            //lt.BackgroundColor = Colors.Black;



            if (stckHome.Children.Count > 0)
            {
                stckHome.Children.Clear();
            }

            stckHome.Children.Add(lt);

            //lt.ItemTapped += Lt_ItemTappedAsync;

            //lt.SelectedItem += Lt_SelectionChanged;

            lt.ItemSelected += Lt_ItemSelectedNew;




            lt.ItemTapped += Lt_ItemTappedNew;

            //}
        }
        else
        {
            using (var progress = UserDialogs.Instance.Loading("Loading..."))
            {
                await GetData();

            }
        }
    }


    protected override bool OnBackButtonPressed()
    {
        base.OnBackButtonPressed();
        //Android.OS.Process.KillProcess(Android.OS.Process.MyPid());


        return base.OnBackButtonPressed();
    }



    public async void TakePhoto()
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
            string s = photo.ContentType.ToString();
            string base64string;
            string localFilePath = "";
            if (photo != null)
            {


                localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);
                await sourceStream.CopyToAsync(localFileStream);

                //additional

                sourceStream.Dispose();
                localFileStream.Dispose();
                var imageBytes = File.ReadAllBytes(localFilePath);
                base64string = Convert.ToBase64String(imageBytes);
                //PhotoPath = string.Format("data:image/png;base64,{0}", PhotoPath);


                // save the file into local storage
                //string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                //using Stream sourceStream = await photo.OpenReadAsync();
                //using FileStream localFileStream = File.OpenWrite(localFilePath);


                //byte[] imageArray = System.IO.File.ReadAllBytes(localFilePath);
                //string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                //await sourceStream.CopyToAsync(localFileStream);
            }
        }
    }



    public async Task GetData()
    {
        List<DynamicDataUploadTable> RecordNumber = await SQLFunctions.GetAllQuestions();
       // using (var progress = UserDialogs.Instance.Loading("Loading..."))
        //{
            bool internet = Wel.Utils.Connection.CheckInternetConnection();
            List<Wel.Data.DyanamicData> dyanamicDatas = new List<Wel.Data.DyanamicData>();
            if (internet)
            {
                dyanamicDatas = await Wel.Services.Client.GetChecklists();
                if (dyanamicDatas != null)
                {
                    if (dyanamicDatas.Count < RecordNumber.Count || RecordNumber.Count == 0)
                    {


                        bool delete = await SQLFunctions.Deletequery("Delete from DynamicData");
                        string i = await Wel.Services.SQLFunctions.SaveItemAsync(dyanamicDatas);
                        if (i != "")
                        {
                            //await DisplayAlert("records updated", "Latest information updated", "OK");
                        }
                    }
                }
                //List<DynamicDataUploadTable> existingData = await Wel.Services.SQLFunctions.GetAllQuestions();


            }

            List<DynamicDataUploadTable> filter = await Wel.Services.SQLFunctions.GetQuery("Select distinct TableName from DynamicData");








            var dynam = new DataTemplate(typeof(Template.HomeViewCell));
            ListView lt = new ListView();
            lt.HasUnevenRows = true;
            lt.SeparatorVisibility = SeparatorVisibility.None;
            lt.BackgroundColor = Colors.Transparent;
            lt.Background = Colors.Transparent;


            //lt.RefreshControlColor = Colors.Transparent;
            //lt.AutoFitMode = AutoFitMode.Height;
            //lt.AutoFitMode = AutoFitMode.DynamicHeight;
            lt.ItemTemplate = dynam;

            lt.ItemsSource = null;

            lt.ItemsSource = filter;

            //lt.Margin = new Thickness(0, 0, 0, 50);
            //}


            //lt.BackgroundColor = Colors.Black;



            if (stckHome.Children.Count > 0)
            {
                stckHome.Children.Clear();
            }

            stckHome.Children.Add(lt);

            //lt.ItemTapped += Lt_ItemTappedAsync;

            //lt.SelectedItem += Lt_SelectionChanged;

            lt.ItemSelected += Lt_ItemSelectedNew;




            lt.ItemTapped += Lt_ItemTappedNew;




        }
  //  }

    private void Lt_ItemTappedNew(object sender, Microsoft.Maui.Controls.ItemTappedEventArgs e)
    {
        ((ListView)sender).SelectedItem = null;
    }

    //public void testcontrols()
    //{
    //    foreach (var item in myList.Items)//loops through each item in current page
    //    {
    //        UserControl control = (UserControl)item.FindControl("myItem");// accessing UserControl

    //    }
    //}

    private async void Lt_ItemSelectedNew(object sender, SelectedItemChangedEventArgs e)
    {

        using (var dlg = UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
        {
            var items = ((ListView)sender).SelectedItem as Wel.Data.DynamicDataUploadTable;
            if (items == null) return;
            try
            {
                await SQLFunctions.AddToIntermediateList(items);
            }
            catch (Exception ex)
            {

            }
            await Navigation.PushModalAsync(new SelectedItem(items));
        }


        //var item = ((ListView)sender).SelectedItem as Wel.Data.DynamicDataUploadTable;
        //if (item == null) return;
        //try
        //{
        //    await SQLFunctions.AddToIntermediateList(item);
        //}
        //catch (Exception ex)
        //{

        //}
        //await Navigation.PushModalAsync(new SelectedItem(item));

    }

    ViewCell lastCell;

    //private void ViewCell_Tapped(object sender, System.EventArgs e)
    //{
    //    if (lastCell != null)
    //        lastCell.View.BackgroundColor = Colors.Transparent;
    //    var viewCell = (ViewCell)sender;
    //    if (viewCell.View != null)
    //    {
    //        viewCell.View.BackgroundColor = Colors.Red;
    //        lastCell = viewCell;
    //    }
    //}




    public async Task<string> seterror()
    {
        string error = await DisplayPromptAsync("Please state the issue", "OK");
        return error;
    }

    private async void completedLists_Clicked(object sender, EventArgs e)
    {

        List<Wel.Data.TblCompleted> questions = await SQLFunctions.GetAllCompletedQuestions();

        //List<Wel.Data.TblCompleted> questions = await SQLFunctions.GetAllCompletedQuestions();


        if (questions != null && questions.Count > 0)
        {
            await Navigation.PushModalAsync(new EditListPage());
        }
        else
        {
            await DisplayAlert("Error", "There is currently no completed lists", "OK");
        }

    }
}

