using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace FGCMSTool;

public class CMSImage
{
    public string Name;
    public int Hash;
    public long TimeStamp;

    public int Width;
    public int Height;

    // rgba when hasAlpha is true, otherwise rgb
    public byte[] Image;
    public bool HasAlpha;

    public CMSImage(string name, int hash, long timeStamp)
    {
        Name = name;
        Hash = hash;
        TimeStamp = timeStamp;
    }

    // files.map: path lenght, path text, hash, timestamp (and that repeats)
    // hash is dlc file name
    public static List<CMSImage> ReadCMSImages(string mapPath)
    {
        List<CMSImage> images = new List<CMSImage>();
        string basePath = Path.GetDirectoryName(mapPath) ?? throw new InvalidOperationException();

        using var stream = File.Open(mapPath, FileMode.Open);
        using var reader = new BinaryReader(stream, Encoding.UTF8, false);
        while (reader.BaseStream.Position != reader.BaseStream.Length)
        {
            string name = Path.GetFileNameWithoutExtension(reader.ReadString());

            // if name ends with "_" remove it
            if (name.EndsWith('_'))
                name = name.Substring(0, name.Length - 1);

            int hash = reader.ReadInt32();
            long timeStamp = reader.ReadInt64();

            CMSImage image = new CMSImage(name, hash, timeStamp);

            string path = Path.Combine(basePath, hash + ".dlc");
            byte[] rawData = File.ReadAllBytes(path);
            image.ReadFromRawData(rawData);

            images.Add(image);
        }

        return images;
    }

    // <hash>.dlc: width, height, format (5 = RGBA, 3 = RGB), colors in format
    private void ReadFromRawData(byte[] rawData)
    {
        using var stream = new MemoryStream(rawData);
        using var reader = new BinaryReader(stream, Encoding.UTF8, false);

        Width = reader.ReadInt32();
        Height = reader.ReadInt32();
        HasAlpha = reader.ReadInt32() == 5;
        Image = stream.ToArray();

        // skip 12 for metadata + 1 because idk
        Image = Image.Skip(13).ToArray();

        FlipImageVertically(Image, Width, Height, HasAlpha ? 4 : 3);
    }

    public void WriteToPngFile(string path)
    {
        using Image<Rgba32> image = new Image<Rgba32>(Width, Height);
        int pixelIndex = 0;
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                byte r = Image[pixelIndex++];
                byte g = Image[pixelIndex++];
                byte b = Image[pixelIndex++];
                byte a = HasAlpha ? Image[pixelIndex++] : (byte)255;

                image[x, y] = new Rgba32(r, g, b, a);
            }
        }

        image.SaveAsPng(path);
    }

    private static void FlipImageVertically(byte[] data, int width, int height, int pixelSize)
    {
        int rowSize = width * pixelSize;
        byte[] tempRow = new byte[rowSize];

        for (int i = 0; i < height / 2; i++)
        {
            int topRowStart = i * rowSize;
            int bottomRowStart = (height - 1 - i) * rowSize;

            Array.Copy(data, topRowStart, tempRow, 0, rowSize);
            Array.Copy(data, bottomRowStart, data, topRowStart, rowSize);
            Array.Copy(tempRow, 0, data, bottomRowStart, rowSize);
        }
    }

    public override string ToString() =>
        $"{Name} (hash: {Hash}, time: {TimeStamp}, width: {Width}, height: {Height}, has alpha: {HasAlpha})";
}
