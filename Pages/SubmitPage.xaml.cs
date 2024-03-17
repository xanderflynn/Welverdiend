using Acr.UserDialogs;
using System.Text;
using Wel.Data;
using Wel.Services;

namespace Welverdiend7.Pages;

public partial class SubmitPage : ContentPage
{
    string table = "";
    public SubmitPage(DynamicDataUploadTable item)
    {
        InitializeComponent();

        table = item.TableName;
    }

    private async void btnadd_Clicked(object sender, EventArgs e)
    {
        try
        {
            using (var dlg = UserDialogs.Instance.Loading("Loading...", null, null, true, Acr.UserDialogs.MaskType.Black))
            {

                //bool save = await Wel.Services.SQLFunctions.SetCompletedChecklist(table);
                string base64string = "";

                if (true)
                {
                    var stream = await SignatureView.GetImageStream(512, 512);
                    Image img = new Image();

                    img.Source = ImageSource.FromStream(() => stream);




                    //Image photo = new Image();

                    //Stream test = ConvertToBase64(stream);
                    try

                    {
                        var mStream = (MemoryStream)stream;
                        byte[] data = mStream.ToArray();
                        base64string = Convert.ToBase64String(data);


                    }
                    catch (Exception ex)
                    {

                    }



                    string imagetype = img.Source.GetType().ToString();

                    //photo.Source = ImageSource.FromStream(() => stream);


                    //if (stream != null)
                    //{
                    //    try
                    //    {
                    //        var mStream = (MemoryStream)stream;
                    //        byte[] data = mStream.ToArray();
                    //        base64string = Convert.ToBase64String(data);


                    //    }
                    //    catch (Exception ex)
                    //    {

                    //    }



                    //    string imagetype = img.Source.GetType().ToString();

                    //photo.Source = ImageSource.FromStream(() => stream);


                    if (stream != null)
                    {

                        var context = Platform.CurrentActivity;

                        string contenttype = "image/png";

                        if (OperatingSystem.IsAndroidVersionAtLeast(29))
                        {

                            Guid uuid = new Guid();
                            try
                            {
                                Android.Content.ContentResolver resolver = context.ContentResolver;
                                Android.Content.ContentValues contentValues = new();
                                contentValues.Put(Android.Provider.MediaStore.IMediaColumns.DisplayName, uuid + ".png");
                                contentValues.Put(Android.Provider.MediaStore.IMediaColumns.MimeType, "image/jpeg");
                                contentValues.Put(Android.Provider.MediaStore.IMediaColumns.RelativePath, "DCIM/" + "test");
                                Android.Net.Uri imageUri = resolver.Insert(Android.Provider.MediaStore.Images.Media.ExternalContentUri, contentValues);
                                var os = resolver.OpenOutputStream(imageUri);
                                Android.Graphics.BitmapFactory.Options options = new();
                                options.InJustDecodeBounds = true;
                                var bitmap = Android.Graphics.BitmapFactory.DecodeStream(stream);
                                bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 100, os);



                                os.Flush();
                                os.Close();
                            }
                            catch (Exception ex)
                            {

                            }


                            //Android.Content.ContentResolver resolver = context.ContentResolver;
                            //Android.Content.ContentValues contentValues = new();
                            //contentValues.Put(Android.Provider.MediaStore.IMediaColumns.DisplayName, "test.png");
                            //contentValues.Put(Android.Provider.MediaStore.IMediaColumns.MimeType, "image/jpeg");
                            //contentValues.Put(Android.Provider.MediaStore.IMediaColumns.RelativePath, "DCIM/" + "test");
                            //Android.Net.Uri imageUri = resolver.Insert(Android.Provider.MediaStore.Images.Media.ExternalContentUri, contentValues);
                            //var os = resolver.OpenOutputStream(imageUri);
                            //Android.Graphics.BitmapFactory.Options options = new();
                            //options.InJustDecodeBounds = true;
                            //var bitmap = Android.Graphics.BitmapFactory.DecodeStream(stream);
                            //bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 100, os);



                            //os.Flush();
                            //os.Close();

                        }
                        else
                        {
                            //Java.IO.File storagePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
                            //string path = System.IO.Path.Combine(storagePath.ToString(), "test.png");
                            ////System.IO.File.WriteAllBytes(path, memoryStream.ToArray());
                            //var mediaScanIntent = new Android.Content.Intent(Android.Content.Intent.ActionMediaScannerScanFile);
                            //mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(path)));
                            //context.SendBroadcast(mediaScanIntent);
                        }


                        //string suc = await SQLFunctions.updateQuestion(null, null, null, null, null, table, contenttype,base64string);



                        //                   int save = await SQLFunctions.AddToCompletedList();

                        //                   await SecureStorage.Default.SetAsync("Pk", save.ToString());

                        //                   string suc = await SQLFunctions.updateSignatures(save);
                        //               }

                        //bool del = await SQLFunctions.DeletefromIntermediateList();
                        //await Shell.Current.GoToAsync("//NewPage1");

                        string suc = await SQLFunctions.addSignatures(txtEmployeenumber.Text, base64string, contenttype);

                        SignatureView.Clear();
                        //await Navigation.PushModalAsync(new NewPage1());

                    }
                    else
                    {
                        //await Navigation.PopAsync();
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Please ensure that a employee number is added", "OK");
                }
            }
        }




        catch (Exception ex)
        {
            await Navigation.PopAsync();
        }
    }



    private async void btnSubmit_Clicked(object sender, EventArgs e)
    {
        int save = await SQLFunctions.AddToCompletedList();

        //await SecureStorage.Default.SetAsync("Pk", save.ToString());

        //string suc = await SQLFunctions.updateSignatures(save);


        bool del = await SQLFunctions.DeletefromIntermediateList();

        await Navigation.PushModalAsync(new NewPage1(null,true));

    }



    //public static Stream ConvertToBase64(Stream stream)
    //{
    //    try
    //    {
    //        string suc = await SQLFunctions.updateQuestion(null, null, null, null, null, table, contenttype, base64string);

    //        bool save = await SQLFunctions.AddToCompletedList();


    //        bool del = await SQLFunctions.DeletefromIntermediateList();
    //        //await Shell.Current.GoToAsync("//NewPage1");


    //        if (del && save)
    //        {
    //            SignatureView.Clear();
    //            await Navigation.PushModalAsync(new NewPage1());

    //        }
    //        else
    //        {
    //            //await Navigation.PopAsync();
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        await Navigation.PopAsync();
    //    }
    //}



    public static Stream ConvertToBase64(Stream stream)

    {
        byte[] bytes;
        using (var memoryStream = new MemoryStream())
        {
            stream.CopyTo(memoryStream);
            bytes = memoryStream.ToArray();
        }

        string base64 = Convert.ToBase64String(bytes);
        return new MemoryStream(Encoding.UTF8.GetBytes(base64));
    }


    public static string MimeType(byte[] type)
    {
        string mime = "";


        if (type == null)
        {
            using (var sr = new FileStream("file", FileMode.Open))
            {
                var data = type;
                int numRead = sr.Read(data, 8, data.Length);
                // numRead gives you the number of bytes read


            }
        }


        return mime;
    }
}