using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wel.Services;

namespace Welverdiend7.Template
{
    public class CompletedChecklistViewCell : ViewCell
    {

        public CompletedChecklistViewCell()
        {

            Frame stackContent = new Frame
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(20),
                //HeightRequest = 80,
                //Opacity = 0.8

            };

            StackLayout stck = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Colors.Transparent,
                //Margin = new Thickness(20),
                // Spacing = 1
            };

            StackLayout stck1 = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //Margin = new Thickness(20),
                // Spacing = 1
            };

            Label lblQuestion = new Label()
            {
                TextColor = Colors.Black,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = 20
            };
            stck.Children.Add(lblQuestion);


            lblQuestion.SetBinding(Label.TextProperty, "ColumnValue");

            Label lblColumnName = new Label()
            {
                TextColor = Colors.Black,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                IsVisible = false
            };
            lblColumnName.SetBinding(Label.TextProperty, "ColumnName");
            stck.Children.Add(lblColumnName);

            Label lblTablename = new Label()
            {
                TextColor = Colors.Black,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                IsVisible = false
            };
            lblTablename.SetBinding(Label.TextProperty, "TableName");
            stck.Children.Add(lblTablename);

            Switch checkSwitch = new Switch()
            {

                IsToggled = false,
                ThumbColor = Colors.Green,
            };
            stck.Children.Add(checkSwitch);

            Label checklabel = new Label()
            {
                IsVisible = false
            };
            checklabel.SetBinding(Label.TextProperty, "Value");

            checklabel.BindingContextChanged += (sender, e) =>
            {
                if (checklabel.Text == "1")
                {
                    checkSwitch.IsToggled = true;
                }
            };


            checkSwitch.Toggled += async (sender, e) =>
            {

                //Binding columnname = new Binding("ColumnValue", BindingMode.Default);



                if (checkSwitch.IsToggled)
                {
                    await SQLFunctions.updateQuestion(lblQuestion.Text, 1.ToString());
                }
                else
                {
                    string error = await Application.Current.MainPage.DisplayPromptAsync("Please State The Isuue", "OK");



                    if (error != null)
                    {

                        string action = await Application.Current.MainPage.DisplayActionSheet("Would you like to attach a image? : ", "Cancel", null, "Take Photo", "Gallery");

                        if (action == "Take Photo")
                        {
                            if (MediaPicker.Default.IsCaptureSupported)
                            {
                                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
                                string s = photo.ContentType.ToString();
                                string base64string = "";
                                if (photo != null)
                                {
                                    // save the file into local storage
                                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                                    using Stream sourceStream = await photo.OpenReadAsync();
                                    using FileStream localFileStream = File.OpenWrite(localFilePath);

                                    await sourceStream.CopyToAsync(localFileStream);
                                    sourceStream.Dispose();
                                    localFileStream.Dispose();
                                    var imageBytes = File.ReadAllBytes(localFilePath);
                                    base64string = Convert.ToBase64String(imageBytes);

                                    string suc = await SQLFunctions.updateQuestion(null, null, error, lblColumnName.Text, base64string, lblTablename.Text, s);
                                }
                            }
                        }
                        else if (action == "Gallery")
                        {
                            if (MediaPicker.Default.IsCaptureSupported)
                            {
                                FileResult photo = await MediaPicker.Default.PickPhotoAsync();
                                string s = photo.ContentType.ToString();
                                string base64string = "";
                                if (photo != null)
                                {
                                    // save the file into local storage
                                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                                    using Stream sourceStream = await photo.OpenReadAsync();
                                    using FileStream localFileStream = File.OpenWrite(localFilePath);

                                    await sourceStream.CopyToAsync(localFileStream);
                                    sourceStream.Dispose();
                                    localFileStream.Dispose();
                                    var imageBytes = File.ReadAllBytes(localFilePath);
                                    base64string = Convert.ToBase64String(imageBytes);

                                    string suc = await SQLFunctions.updateQuestion(null, null, error, lblColumnName.Text, base64string, lblTablename.Text, s);
                                }
                            }
                        }

                        //await SQLFunctions.updateQuestion(lblQuestion.Text, 0, error);
                    }
                }
            };
            stackContent.Content = stck;
            stck1.Children.Add(stackContent);
            View = stck1;


        }

        //public async Task<string> seterror()
        //{
        //    string error = await Application.Current.MainPage.displ("Please state the issue", "OK");
        //    return error;
        //}



    }
}