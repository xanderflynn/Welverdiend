using Acr.UserDialogs;
using Wel.Data;
using Wel.Services;

namespace Welverdiend7.Pages;

public partial class SelectedItem : ContentPage
{
    //List<Wel.Data.DynamicDataUploadTable> DynamicData = new List<DynamicDataUploadTable>();
    Wel.Data.DynamicDataUploadTable item = new Wel.Data.DynamicDataUploadTable();
    Microsoft.Maui.Controls.ListView lt = new Microsoft.Maui.Controls.ListView();
    public SelectedItem(/*List<Wel.Data.DynamicDataUploadTable> DynamicData,*/ Wel.Data.DynamicDataUploadTable item)
    {
        InitializeComponent();

        //this.DynamicData = DynamicData;
        this.item = item;

        lblHeading.Text = "";
        lblHeading.Text = item.TableName;


    }

    protected async override void OnAppearing()
    {
        using (var dlg = UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
        {
            SetUpPage();


        }
    }

    public async void SetUpPage()
    {
        // bool internet = Wel.Utils.Connection.CheckInternetConnection();

        //List<Wel.Data.DyanamicData> dyanamicDatas = new List<Wel.Data.DyanamicData>();
        //List<DynamicDataUploadTable> existingData = await Wel.Services.SQLFunctions.GetAllQuestions();






        var dynam = new DataTemplate(typeof(Template.ChecklistViewCell));
        lt = new Microsoft.Maui.Controls.ListView();
        lt.HasUnevenRows = true;
        lt.SeparatorVisibility = SeparatorVisibility.None;
        //lt.AutoFitMode = AutoFitMode.DynamicHeight;


        //List<Wel.Data.DynamicDataUploadTable> filteredList = new List<DynamicDataUploadTable>();
        List<DynamicDataUploadTable> filteredList = await Wel.Services.SQLFunctions.GetQuery("Select *  from TblIntermediate where TableName = '" + item.TableName.ToString() + "' and length(ColumnValue) > 5");
        string check = "";
        //foreach (var d in DynamicData)
        //{

        //  if(d.TableName == "SupervisorsOperationsAudit")
        //{

        //}

        //    if (d.TableName == item.TableName && !string.IsNullOrEmpty(d.ColumnValue) && d.ColumnValue.Count() > 5)
        //    {
        //        filteredList.Add(d);
        //        //check = d.TableName;
        //    }
        //}

        lt.ItemsSource = filteredList;
        //}
        lt.ItemTemplate = dynam;

        stckHome.Children.Add(lt);

        //lt.ItemSelected += async (sender, e) =>
        //{
        //    var item = e.SelectedItem as Wel.Data.DyanamicData;
        //    await Navigation.PushModalAsync(new SelectedItem(dyanamicDatas, item));
        //};
        lt.ItemTapped += (sender, args) =>
        {
            ((Microsoft.Maui.Controls.ListView)sender).SelectedItem = null;
        };
    }




    protected override bool OnBackButtonPressed()
    {
        Dispatcher.Dispatch(async () =>
        {

            await SQLFunctions.DeletefromIntermediateList();
            await Navigation.PopModalAsync();
            //base.OnBackButtonPressed();

        });

        return true;
        //base.OnBackButtonPressed();
        //SQLFunctions.DeletefromIntermediateList();


        //return base.OnBackButtonPressed();
    }

    //private async Task SubmitList_Clicked(object sender, EventArgs e)
    //{
    //    List<DynamicDataUploadTable> CheckfilteredList = await Wel.Services.SQLFunctions.GetQuery("Select *  from TblIntermediate where TableName = '" + item.TableName.ToString() + "' and length(ColumnValue) > 5");

    //    foreach(DynamicDataUploadTable item in CheckfilteredList)
    //    {
    //        if(item.Completed == 0)
    //        {

    //        }
    //    }


    //}

    public async void SubmitList_Clicked(object sender, EventArgs e)
    {


        await Navigation.PushModalAsync(new SubmitPage(item));
        //List<DynamicDataUploadTable> CheckfilteredList = await Wel.Services.SQLFunctions.GetQuery("Select *  from TblIntermediate where TableName = '" + item.TableName.ToString() + "' and length(ColumnValue) > 5");

        //foreach (DynamicDataUploadTable item in CheckfilteredList)
        //{
        //    if (item.Completed == 0)
        //    {

        //    }
        //}
    }
}