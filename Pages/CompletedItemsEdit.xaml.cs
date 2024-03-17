using Acr.UserDialogs;
using Wel.Data;

namespace Welverdiend7.Pages;

public partial class CompletedItemsEdit : ContentPage
{
    List<Wel.Data.TblCompleted> DynamicData = new List<TblCompleted>();
    Wel.Data.TblCompleted item = new Wel.Data.TblCompleted();
    public CompletedItemsEdit(List<Wel.Data.TblCompleted> DynamicData, Wel.Data.TblCompleted item)
    {
        InitializeComponent();
        this.DynamicData = DynamicData;
        this.item = item;
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
        ListView lt = new ListView();
        lt.HasUnevenRows = true;
        lt.SeparatorVisibility = SeparatorVisibility.None; ;



        List<Wel.Data.TblCompleted> filteredList = new List<TblCompleted>();
        string check = "";
        foreach (var d in DynamicData)
        {

            if (d.TableName == item.TableName && !string.IsNullOrEmpty(d.ColumnValue) && d.ColumnValue.Count() > 10)
            {
                filteredList.Add(d);
                //check = d.TableName;
            }
        }

        lt.ItemsSource = filteredList;
        //}
        lt.ItemTemplate = dynam;

        stckHome.Children.Add(lt);


    }
}