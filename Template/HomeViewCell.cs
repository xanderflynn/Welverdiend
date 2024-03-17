
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wel.Services;

namespace Welverdiend7.Template
{
   public class HomeViewCell : ViewCell
    {
        private ListView _Home;
        //public static readonly BindableProperty OkToShowThisProperty = BindableProperty.Create("TableName", typeof(string), typeof(Label), true);
        //string check = string.Empty;
        //public bool OkToShowThis
        //{
        //    get { return (bool)GetValue(OkToShowThisProperty); }
        //}
        public HomeViewCell()
        {

            //if (OkToShowThisProperty.ToString() != check)
            //{
            StackLayout stck = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //Margin = new Thickness(20),
                // Spacing = 1
            };

            Frame stackContent = new Frame
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(20),
                HeightRequest = 80,
                //Opacity = 0.8

            };

            Grid contentgrid = new Grid
            {
                Margin = new Thickness(20),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Colors.White,
                Opacity = 0.8
            };

            ColumnDefinition c0 = new ColumnDefinition();
            c0.Width = new GridLength(1, GridUnitType.Star);

            contentgrid.ColumnDefinitions.Add(c0);




            Label lblQuestion = new Label()
            {
                TextColor = Colors.Black,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                Padding = new Thickness(0),
                FontSize = 15,
                Margin = new Thickness(10, 0, 0, 0),
            };
            contentgrid.Children.Add(lblQuestion);
            Grid.SetColumn(lblQuestion, 0);

            lblQuestion.SetBinding(Label.TextProperty, "TableName");
            lblQuestion.BindingContextChanged += async (sender, e) =>
            {
                try
                {
                    string res = Regex.Replace(lblQuestion.Text, @"([a-z])([A-Z])", "$1 $2");
                    lblQuestion.Text = res;
                }
                catch (Exception ex)
                {

                }
            };



            Image imgCheck = new Image
            {
                Source = "checklist.png",
                HorizontalOptions = LayoutOptions.End
            };
            contentgrid.Children.Add(imgCheck);
            Grid.SetColumn(imgCheck, 1);
            //stackContent.Children.Add(lblQuestion);




            stackContent.Content = contentgrid;
            stck.Children.Add(stackContent);
            View = contentgrid;


        }
    }
}
//}

