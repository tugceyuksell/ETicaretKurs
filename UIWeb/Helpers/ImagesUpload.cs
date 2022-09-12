namespace UIWeb.Helpers
{
    public static class ImagesUpload
    {
        public static string Upload(IFormFile images)
        {
            string[] uzantilar = { ".jpg", ".jpeg" };
            string Uzanti = "";
            string NewName = "";
            string KayitAdresi = "";
            if (images != null)
            {
                Uzanti = Path.GetExtension(images.FileName);
                if (uzantilar.Contains(Uzanti))
                {
                    NewName = Guid.NewGuid() + Uzanti;
                    KayitAdresi = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/images/" + NewName);
                    using (FileStream stream = new FileStream(KayitAdresi, FileMode.Create))
                    {
                        images.CopyTo(stream);
                    }
                    return NewName;
                }
                else
                {
                    return "0";
                }
            }
            else
            {
                return "1";
            }
        }

        public static void ImagesDelete(string ImagesDelete)
        {
            System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/images/" + ImagesDelete));
        }
    }
}
