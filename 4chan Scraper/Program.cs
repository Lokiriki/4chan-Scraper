using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace _4chan_Scraper
{
    class Program
    {
        static string path;

        static void Main(string[] args)
        {
            var program = new Program();

            Console.WriteLine("Please enter url to be scraped");
            string html = @Console.ReadLine();

            Console.WriteLine("Please enter folder name");
            string folderName = Console.ReadLine();

            Console.WriteLine("Images? y/n");
            string imageAnswer = Console.ReadLine();

            if (imageAnswer == "y")
            {
                List<string> imageLinks = ScrapeImages(html);
                path = @"F:\Pictures\Backgrounds\Scraped\4chan\" + folderName;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                DownloadImages(imageLinks, folderName);
            }

        }

        private static void DownloadImages(List<string> imageLinks, string folderName)
        {
            for (int i = 0; i < imageLinks.Count; i++)
            {
                DownloadImage(imageLinks, i);
            }
            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        private static void DownloadImage(List<string> imageLinks, int i)
        {
            using (WebClient client = new WebClient())
            {
                Console.WriteLine(imageLinks[i]);
                Uri address = new Uri("https:" + imageLinks[i]);
                if ((imageLinks[i].Contains(".png")) || (imageLinks[i].Contains(".jpg")))
                {
                    try
                    {
                        client.DownloadFileAsync(address, fileName: path + @"\image_" + (i + 1) + ".png");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                else if ((imageLinks[i].Contains(".webm")))
                {
                    try
                    {
                        client.DownloadFileAsync(address, fileName: path + @"\image_" + (i + 1) + ".webm");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        private static List<string> ScrapeImages(string p_html)
        {
            var m_html = p_html;
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(m_html);

            List<string> imageLinks = new List<string>();

            foreach (HtmlNode n in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))
            {
                HtmlAttribute attrib = n.Attributes["href"];
                if ((attrib.Value.Contains(".png")) || (attrib.Value.Contains(".jpg")) || (attrib.Value.Contains(".webm")))
                {
                    bool alreadyExist = imageLinks.Contains(attrib.Value);
                    if (!alreadyExist)
                    {
                        imageLinks.Add(attrib.Value);
                    }

                }

            }
            return imageLinks;
        }
    }
}
