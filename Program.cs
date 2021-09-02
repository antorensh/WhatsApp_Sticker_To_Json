using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;


namespace WhatsApp_Sticker_To_Json
{
    class Program
    {
        static void Main(string[] args)
        {
            //main program
            Console.WriteLine("Whatsap Sticker Json auto-indexing\n");
            Console.WriteLine($"The root dir of this program is {Directory.GetCurrentDirectory().ToString()}");
            Console.WriteLine("Enter asset path");

            string assetPath = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(assetPath))
                assetPath = Directory.GetCurrentDirectory();

            string[] assetDirectory = Directory.GetDirectories(assetPath);


            //main iteration to fill the sticker container
            StickerContainer stickerContainer = new StickerContainer();

            foreach (string dir in assetDirectory)
            {
                StickerPack newStickerPack = new StickerPack();
                string[] sourceFiles = Directory.GetFiles(dir);
                string dirName = Path.GetFileName(dir);
                newStickerPack.identifier = dirName;

                foreach (string name in sourceFiles)
                {
                    if (!name.Contains("tray") && name.Length > 0)
                    {
                        Sticker newSticker = new Sticker(Path.GetFileName(name));
                        newStickerPack.AddSticker(newSticker);
                    }
                    else if (name.Contains("tray"))
                    {
                        newStickerPack.tray_image_file = Path.GetFileName(name);
                    }
                }

                stickerContainer.AddStickerPack(newStickerPack);
                Console.WriteLine($"Folder: {dirName} added {newStickerPack.stickers.Count} Stickers");
            }

            //File output
            string targetFileName = "contents.json";
            string targetPath = Path.Combine(assetPath, targetFileName);

            string jsonOutString = JsonSerializer.Serialize(stickerContainer);
            File.WriteAllText(targetPath, jsonOutString);

            Console.WriteLine("EOP - Press Enter Key to Exit");
            Console.ReadLine();
        }
    }

    public class StickerContainer
    {

        public StickerContainer(string android_play_store_link = "", string ios_app_store_link = "")
        {
            this.android_play_store_link = android_play_store_link;
            this.ios_app_store_link = ios_app_store_link;

            sticker_packs = new List<StickerPack>();
        }

        public void AddStickerPack(StickerPack newStickerPack)
        {
            sticker_packs.Add(newStickerPack);
        }

        public string android_play_store_link { get; set; }
        public string ios_app_store_link { get; set; }
        public List<StickerPack> sticker_packs { get; set; }
    }

    public class StickerPack
    {
        public StickerPack(string identifier = "", string name = "", string publisher = "",
           string tray_image_file = "", string image_data_version = "", bool avoid_cache = false, string publisher_email = "",
            string publisher_website = "", string privacy_policy_website = "", string license_agreement_website = "")
        {

            this.identifier = identifier;
            this.name = name;
            this.publisher = publisher;
            this.tray_image_file = tray_image_file;
            this.image_data_version = image_data_version;
            this.avoid_cache = avoid_cache;
            this.publisher_email = publisher_email;
            this.publisher_website = publisher_website;
            this.privacy_policy_website = privacy_policy_website;
            this.license_agreement_website = license_agreement_website;

            stickers = new List<Sticker>();

        }

        public void AddSticker(Sticker toAddSticker)
        {
            stickers.Add(toAddSticker);
        }

        public string identifier { get; set; }
        public string name { get; set; }
        public string publisher { get; set; }
        public string tray_image_file { get; set; }
        public string image_data_version { get; set; }
        public bool avoid_cache { get; set; }
        public string publisher_email { get; set; }
        public string publisher_website { get; set; }
        public string privacy_policy_website { get; set; }
        public string license_agreement_website { get; set; }
        public List<Sticker> stickers { get; set; }
    }

    public class Sticker
    {
        public Sticker(string image_file = "")
        {
            this.image_file = image_file;
            emojis = new string[3] { "", "", "" };
        }
        public string image_file { get; set; }
        public string[] emojis { get; set; }
    }
}
