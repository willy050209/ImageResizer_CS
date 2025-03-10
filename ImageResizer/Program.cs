﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

public class ImageResizer
{
    public static int Main(string[] args)
    {
        if (args.Length < 3)
        {
            return 1;
        }
        Console.WriteLine("來源資料夾路徑:{0}", args[0]);
        Console.WriteLine("輸出資料夾路徑:{0}", args[1]);
        Console.WriteLine("放大倍率:{0}", args[2]);
        string folderPath = args[0]; 
        string outputFolder = args[1]; 
        double scaleFactor = Convert.ToDouble(args[2]);

        try
        {
            // 確保輸出資料夾存在
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // 取得所有圖片檔案
            string[] imageFiles = Directory.GetFiles(folderPath, "*.jpg"); // 可以根據需要更改副檔名
            string[] pngFiles = Directory.GetFiles(folderPath, "*.png");
            string[] bmpFiles = Directory.GetFiles(folderPath, "*.bmp");
            string[] gifFiles = Directory.GetFiles(folderPath, "*.gif");
            string[] tiffFiles = Directory.GetFiles(folderPath, "*.tiff");
            string[] jpegFiles = Directory.GetFiles(folderPath, "*.jpeg");

            string[] allFiles = new string[imageFiles.Length + pngFiles.Length + bmpFiles.Length + gifFiles.Length + tiffFiles.Length + jpegFiles.Length];

            imageFiles.CopyTo(allFiles, 0);
            pngFiles.CopyTo(allFiles, imageFiles.Length);
            bmpFiles.CopyTo(allFiles, imageFiles.Length + pngFiles.Length);
            gifFiles.CopyTo(allFiles, imageFiles.Length + pngFiles.Length + bmpFiles.Length);
            tiffFiles.CopyTo(allFiles, imageFiles.Length + pngFiles.Length + bmpFiles.Length + gifFiles.Length);
            jpegFiles.CopyTo(allFiles, imageFiles.Length + pngFiles.Length + bmpFiles.Length + gifFiles.Length + tiffFiles.Length);
            
            foreach (string imageFile in allFiles)
            {
                // 讀取圖片
                using (System.Drawing.Image originalImage = System.Drawing.Image.FromFile(imageFile))
                {
                    // 計算新的尺寸
                    int newWidth = (int)(originalImage.Width * scaleFactor);
                    int newHeight = (int)(originalImage.Height * scaleFactor);

                    // 建立新的 Bitmap
                    using (Bitmap resizedImage = new Bitmap(newWidth, newHeight))
                    {
                        // 建立 Graphics 物件
                        using (Graphics graphics = Graphics.FromImage(resizedImage))
                        {
                            // 設定插補模式以獲得更好的品質
                            //graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                            // 繪製放大的圖片
                            graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                        }

                        // 儲存放大的圖片
                        string outputFilePath = Path.Combine(outputFolder, Path.GetFileName(imageFile));
                        resizedImage.Save(outputFilePath);
                        Console.WriteLine($"已放大並儲存：{outputFilePath}");
                    }
                }
            }

            Console.WriteLine("所有圖片已放大並儲存！");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"發生錯誤：{ex.Message}");
        }
        return 0;
    }
}