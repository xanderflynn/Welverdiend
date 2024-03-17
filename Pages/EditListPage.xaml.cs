using Acr.UserDialogs;
using Wel.Data;
using Wel.Services;

namespace Welverdiend7.Pages;

public partial class EditListPage : ContentPage
{

    List<TblCompleted> questions = new List<TblCompleted>();
    //SfListView QuestionsList = new SfListView();

    public EditListPage()
    {
        InitializeComponent();


    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        using (var dlg = UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
        {

            questions = await SQLFunctions.GetAllCompletedQuestions();

            SetUpPage();
        }
    }
    public async void SetUpPage()
    {


        var dynam = new DataTemplate(typeof(Template.HomeViewCell));
        //ListView lt = new ListView();
        //lt.VerticalOptions = LayoutOptions.FillAndExpand;
        //lt.AutoFitMode = AutoFitMode.DynamicHeight;


        List<Wel.Data.TblCompleted> filteredList = new List<TblCompleted>();
        string check = "";
        foreach (var d in questions)
        {

            if (d.uuid != check)
            {
                filteredList.Add(d);
                check = d.uuid;
            }
        }
        lt.HasUnevenRows = true;
        lt.SeparatorVisibility = SeparatorVisibility.None;
        lt.ItemsSource = filteredList;

        lt.ItemTemplate = dynam;

        // stckHome.Children.Add(lt);

        //lt.ItemSelected += async (sender, e) =>
        //{

        //};
    }

    private async void lt_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var item = e.SelectedItem as Wel.Data.TblCompleted;
        await Navigation.PushModalAsync(new CompletedItemsEdit(questions, item));
    }

}