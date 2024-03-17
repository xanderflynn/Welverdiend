
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wel.Services;

namespace Welverdiend7.Template
{
    public class ChecklistViewCell : ViewCell
    {

        public ChecklistViewCell()
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
                FontSize = 15
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
                ThumbColor = Colors.Green,
                IsToggled = true
            };
            stck.Children.Add(checkSwitch);
            checkSwitch.Toggled += async (sender, e) =>
            {
                //Binding columnname = new Binding("ColumnValue", BindingMode.Default);



                if (checkSwitch.IsToggled)
                {
                    await SQLFunctions.updateQuestion(lblQuestion.Text, 1.ToString());
                }
                else
                {
                    //await SQLFunctions.updateQuestion(lblQuestion.Text, 0.ToString());
                    string error = await Application.Current.MainPage.DisplayPromptAsync("Please State The Issue", "OK");



                    if (error != null)
                    {

                        string action = await Application.Current.MainPage.DisplayActionSheet("Would you like to attach a image? : ", "Cancel", null, "Take Photo", "Gallery");

                        if (action == "Take Photo")
                        {
                            if (MediaPicker.Default.IsCaptureSupported)
                            {


                                FileResult photo = await MediaPicker.Default.CapturePhotoAsync(new MediaPickerOptions
                                {

                                });

                                string s = photo.ContentType.ToString();
                                string base64string = "";
                                if (photo != null)
                                {
                                    // save the file into local storage
                                    string localFilePath = System.IO.Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                                    using Stream sourceStream = await photo.OpenReadAsync();
                                    using FileStream localFileStream = File.OpenWrite(localFilePath);

                                    await sourceStream.CopyToAsync(localFileStream);
                                    sourceStream.Dispose();
                                    localFileStream.Dispose();
                                    var imageBytes = File.ReadAllBytes(localFilePath);
                                    base64string = Convert.ToBase64String(imageBytes);

                                    //var newFile = System.IO.Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                                    //using (var stream = await photo.OpenReadAsync())
                                    //using (var newStream = File.OpenWrite(newFile))
                                    //    await stream.CopyToAsync(newStream);

                                    //var mStream = (MemoryStream)sourceStream;

                                    //byte[] data = mStream.ToArray();

                                    byte[] resizedImage = await ImageResizer.ResizeImage(imageBytes, 512, 512);
                                    //base64string = Convert.ToBase64String(resizedImage);
                                    base64string = Convert.ToBase64String(imageBytes);
                                    string suc = await SQLFunctions.updateQuestion(lblQuestion.Text, 0.ToString(), error, lblColumnName.Text, base64string, lblTablename.Text, s);
                                
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
                                    string localFilePath = System.IO.Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                                    using Stream sourceStream = await photo.OpenReadAsync();
                                    using FileStream localFileStream = File.OpenWrite(localFilePath);

                                    await sourceStream.CopyToAsync(localFileStream);
                                    sourceStream.Dispose();
                                    localFileStream.Dispose();
                                    var imageBytes = File.ReadAllBytes(localFilePath);
                                    //base64string = Convert.ToBase64String(imageBytes);

                                    byte[] resizedImage = await ImageResizer.ResizeImage(imageBytes, 512, 512);
                                    base64string = Convert.ToBase64String(resizedImage);

                                    string suc = await SQLFunctions.updateQuestion(lblQuestion.Text, 0.ToString(), error, lblColumnName.Text, base64string, lblTablename.Text, s);
                                }
                            }
                        }

                        //await SQLFunctions.updateQuestion(lblQuestion.Text, "0", error);
                    }

                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("NOTICE", "Please note that if you do not specify a description that the check will be marked as correct", "OK");
                        await SQLFunctions.updateQuestion(lblQuestion.Text, 1.ToString());
                        checkSwitch.IsToggled = true;
                    }
                }
            };
            stackContent.Content = stck;
            stck1.Children.Add(stackContent);
            View = stck1;


        }

        public static class ImageResizer
        {
            static ImageResizer()
            {
            }
            public static async Task<byte[]> ResizeImage(byte[] imageData, float width, float height)
            {
                return ResizeImageAndroid(imageData, width, height);
            }
            public static byte[] ResizeImageAndroid(byte[] imageData, float width, float height)
            {
                // Load the bitmap  
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                Android.Graphics.Bitmap originalImage = Android.Graphics.BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                Android.Graphics.Bitmap resizedImage = Android.Graphics.Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);

                using (MemoryStream ms = new MemoryStream())
                {
                    resizedImage.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 100, ms);
                    return ms.ToArray();
                }
            }

        }


    }


}

