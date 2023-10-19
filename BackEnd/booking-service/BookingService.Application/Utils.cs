
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Drawing.Imaging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BookingService.Service
{
    public class Utils
    {
        private readonly static string BASE_DOMAIN = "https://localhost:7278";
        private static IConfiguration? _config = null;
        public static IHostEnvironment WebEnv()
        {
            var _accessor = new HttpContextAccessor();
            return _accessor.HttpContext.RequestServices.GetRequiredService<IHostEnvironment>();
        }

        public static void Config(IConfiguration config)
        {
            _config = config;
        }
        public static string EncryptPassword(string value)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(value));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        public static string GetStaus(int status)
        {
            switch (status)
            {
                case 0: return "Inactive";
                case 1: return "Active";
                case 2: return "Delete";
                default: return "";
            }
        }

        public static string GetStausDelivery(int status)
        {
            switch (status)
            {
                case (int)Domain.Enum.DeliveryStatus.FullDelivered: return "lbl_delivered_full";
                case (int)Domain.Enum.DeliveryStatus.PartiallyDelivered: return "lbl_delivered_partialy";
                case (int)Domain.Enum.DeliveryStatus.NotDelivery: return "lbl_delivered_not";
                case (int)Domain.Enum.DeliveryStatus.ReturnDelivery: return "lbl_delivered_return";
                default: return "";
            }
        }

        public static string GetStausReceipt(int status)
        {
            switch (status)
            {
                case (int)Domain.Enum.ReceiptStatus.FullReceipt: return "lbl_receipt_full";
                case (int)Domain.Enum.ReceiptStatus.MissedReceipt: return "lbl_receipt_missing";
                case (int)Domain.Enum.ReceiptStatus.ReturedReceipt: return "lbl_receipt_return";
                default: return "";
            }
        }

        public static string GetStausDeliveryName(int status, string lang)
        {
            switch (status)
            {
                case (int)Domain.Enum.DeliveryStatus.FullDelivered: return lang == "vi" ? "Đã giao hàng thành công" : "Full Delivered";
                case (int)Domain.Enum.DeliveryStatus.PartiallyDelivered: return lang == "vi" ? "Đơn hàng không đủ" : "Partially Delivered";
                case (int)Domain.Enum.DeliveryStatus.NotDelivery: return lang == "vi" ? "Chưa giao hàng" : "Not Delivery";
                case (int)Domain.Enum.DeliveryStatus.ReturnDelivery: return lang == "vi" ? "Trả lại hàng" : "Return Delivery";
                default: return "";
            }
        }

        public static string GetStausReceiptName(int status, string lang)
        {
            switch (status)
            {
                case (int)Domain.Enum.ReceiptStatus.FullReceipt: return lang == "vi" ? "Đơn hàng đầy dủ" : "Full Receipt";
                case (int)Domain.Enum.ReceiptStatus.MissedReceipt: return lang == "vi" ? "Đơn hàng thiếu hóa đơn" : "Missed Receipt";
                case (int)Domain.Enum.ReceiptStatus.ReturedReceipt: return lang == "vi" ? "Đơn hàng bị trả lại" : "Return Receipt";
                default: return "";
            }
        }

        public static string CreateImageURL(List<IFormFile> imageFile, string folder = "", string base_domain = "", int sizeWith = 1024, int sizeHeght = 667)
        {
            var imgURL = "";
            string contentRootPath = Utils.WebEnv().ContentRootPath;
            if (imageFile != null)
            {
                if (string.IsNullOrEmpty(folder)) folder = "Public";
                if (string.IsNullOrEmpty(base_domain)) base_domain = BASE_DOMAIN;
                string path = Path.Combine(contentRootPath, "Images" + "\\" + folder + "\\");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string imagename = imageFile[0].FileName;


                if (Path.GetExtension(imagename).ToLower() == ".gif")
                {
                    string img_name_gif = Path.GetFileName(DateTime.Now.ToString("yyMMddhhmmss") + "_" + imageFile[0]);
                    // imageFile[0].(path + img_name_gif);
                    imgURL = base_domain + "/Images/" + folder + "/" + img_name_gif;
                    return imgURL;
                }

                System.Drawing.Image image = System.Drawing.Image.FromStream(imageFile[0].OpenReadStream());
                float aspectRatio = (float)image.Size.Width / (float)image.Size.Height;


                int newWidth = image.Size.Width;
                int newHeight = image.Size.Height;

                if (image.Size.Width > image.Size.Height)
                {
                    if (image.Size.Width > 600)
                    {
                        newWidth = 600;
                        newHeight = Convert.ToInt32(newWidth / aspectRatio);
                    }
                }
                else
                {
                    if (image.Size.Height > 600)
                    {
                        newHeight = 600;
                        newWidth = Convert.ToInt32(aspectRatio * newHeight);
                    }
                }

                System.Drawing.Bitmap thumbBitmap = new System.Drawing.Bitmap(newWidth, newHeight);
                System.Drawing.Graphics thumbGraph = System.Drawing.Graphics.FromImage(thumbBitmap);
                thumbGraph.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);


                string img_name = Path.GetFileName(DateTime.Now.ToString("yyMMddhhmmss") + "_" + imageFile[0].FileName);

                thumbBitmap.Save(path + img_name);
                //UploadAzureStorage(thumbBitmap, folder, img_name);
                thumbGraph.Dispose();
                thumbBitmap.Dispose();
                image.Dispose();

                imgURL = "Images" + "/" + folder + "/" + img_name;
                imgURL = path + img_name;
            }
            return imgURL;

        }

        public static string RandomString(int characterLength, int numberLength)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "123456789";
            return new string(Enumerable.Repeat(chars, characterLength)
              .Select(s => s[random.Next(s.Length)]).ToArray()) + new string(Enumerable.Repeat(numbers, numberLength)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //public static string CreateQrCode(string code)
        //{
        //    string contentRootPath = Utils.WebEnv().ContentRootPath;
        //    string path = Path.Combine(contentRootPath, "Images" + "\\QrCode\\");
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    QRCodeGenerator qrGenerator = new QRCodeGenerator();
        //    QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
        //    QRCode qrCode = new QRCode(qrCodeData);
        //    Bitmap qrCodeImage = qrCode.GetGraphic(20);
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        //        return String.Format("data:image/png;base64,{0}", Convert.ToBase64String(stream.ToArray()));
        //    }
        //}

        public static string CommandButton(List<string> command, object value, int status)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<div style=\"min-width:70px;\">");
            //Detail
            if (command.Contains("Detail"))
            {
                sb.Append($"<button title=\"Detail\" type=\"button\" class=\"btn btn-action btn-success waves-effect\" onclick='ShowDetail(\"{value}\")'><i class='material-icons'>view_list</i></button>");
            }
            else if (command.Contains("NoDetail"))
            {
                sb.Append($"<button title=\"Detail\" type=\"button\" class=\"btn btn-action btn-info waves-effect disabled\" ><i class='material-icons'>view_list</i></button>");
            }
            //Delete
            if (command.Contains("Delete"))
            {
                sb.Append($"<button title=\"Delete\" type=\"button\" class=\"btn btn-action btn-danger waves-effect\" onclick='CallDelete(\"{value}\")'><i class=\"material-icons\">delete</i></button>");
            }
            else if (command.Contains("NoDelete"))
            {
                sb.Append($"<button title=\"Delete\" type=\"button\" class=\"btn btn-action btn-danger waves-effect disabled\" ><i class=\"material-icons\">delete</i></button>");
            }
            //Edit
            if (command.Contains("Edit"))
            {
                sb.Append($"<button title=\"Edit\" type=\"button\" class=\"btn btn-action btn-warning waves-effect\" onclick='Edit(\"{value}\")'><i class=\"material-icons\">edit</i></button>");
            }
            else if (command.Contains("NoEdit"))
            {
                sb.Append($"<button title=\"Edit\" type=\"button\" class=\"btn btn-action btn-warning waves-effect disabled\" ><i class=\"material-icons\">edit</i></button>");
            }
            //ChangeActive
            if (command.Contains("ChangeActive"))
            {
                sb.Append($"<button type=\"button\" title=\"Change Active\" class=\"btn btn-action btn-primary waves-effect\" onclick='CallChange(\"{value}\")'><i class=\"material-icons\">{(status == (int)Domain.Enum.Status.Active ? "visibility" : "visibility_off")}</i></button>");
            }
            else if (command.Contains("NoChangeActive"))
            {
                sb.Append($"<button type=\"button\" title=\"Change Active\" class=\"btn btn-action btn-primary waves-effect disabled\" ><i class=\"material-icons\">{(status == (int)Domain.Enum.Status.Active ? "visibility" : "visibility_off")}</i></button>");
            }
            //Edit
            if (command.Contains("Print"))
            {
                sb.Append($"<button title=\"Print\" type=\"button\" class=\"btn btn-action bg-cyan waves-effect\" onclick='Print(\"{value}\")'><i class=\"material-icons\">print</i></button>");
            }
            else if (command.Contains("NoPrint"))
            {
                sb.Append($"<button title=\"Print\" type=\"button\" class=\"btn btn-action bg-cyan waves-effect disabled\" ><i class=\"material-icons\">print</i></button>");
            }
            //Approve
            if (command.Contains("Approve"))
            {
                sb.Append($"<button title=\"Approve\" type=\"button\" class=\"btn btn-action btn-info waves-effect\" onclick='Approve(\"{value}\")'><i class='material-icons'>done</i></button>");
            }
            else if (command.Contains("NoApprove"))
            {
                sb.Append($"<button title=\"Approve\" type=\"button\" class=\"btn btn-action btn-info waves-effect disabled\"><i class='material-icons'>done</i></button>");
            }
            //Return
            if (command.Contains("Return"))
            {
                sb.Append($"<button title=\"Return\" type=\"button\" class=\"btn btn-action btn-warning waves-effect\" onclick='Return(\"{value}\")'><i class=\"material-icons\">priority_high</i></button>");
            }
            else if (command.Contains("NoReturn"))
            {
                sb.Append($"<button title=\"Return\" type=\"button\" class=\"btn btn-action btn-warning waves-effect disabled\" ><i class=\"material-icons\">priority_high</i></button>");
            }
            //Refuse
            if (command.Contains("Refuse"))
            {
                sb.Append($"<button title=\"Refuse\" type=\"button\" class=\"btn btn-action btn-danger waves-effect\" onclick='Refuse(\"{value}\")'><i class=\"material-icons\">close</i></button>");
            }
            else if (command.Contains("NoRefuse"))
            {
                sb.Append($"<button title=\"Refuse\" type=\"button\" class=\"btn btn-action btn-danger waves-effect disabled\" ><i class=\"material-icons\">close</i></button>");
            }
            return sb.ToString();
        }

        public static async Task<string> UploadAzureStorage(List<IFormFile> imageFile, string folder)
        {
            try
            {
                var image = new List<string>();
                CloudStorageAccount storageacc = CloudStorageAccount.Parse(_config?["AzureStorage"] ?? "");

                CloudBlobClient blobClient = storageacc.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(_config?["AzureContainer"] ?? "");
                await container.CreateIfNotExistsAsync();
                container.GetDirectoryReference("");
                foreach (var img in imageFile)
                {
                    var name = folder + "/" + DateTime.Now.ToString("yyMMddhhmmss") + "_" + img.FileName;
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);
                    System.IO.Stream inputStream = img.OpenReadStream();
                    await blockBlob.UploadFromStreamAsync(inputStream);
                    image.Add(name);
                }
                return string.Join(";", image);
            }
            catch
            {
                return "";
            }

        }
    }
}
