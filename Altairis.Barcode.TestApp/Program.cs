using System.Drawing;
using Altairis.Barcode;

CreateBarcode(new Code25IndustrialGenerator("1234567"), "barcode-25-industrial.png");
CreateBarcode(new Code25InterleavedGenerator("1234567"), "barcode-25-interleaved.png");
CreateBarcode(new Code25IataGenerator("1234567"), "barcode-25-iata.png");
CreateBarcode(new Code39StandardGenerator("ALTAIR"), "barcode-code39.png");
CreateBarcode(new Ean8Generator("1234567"), "barcode-ean-8.png");
CreateBarcode(new Ean13Generator("123456789012"), "barcode-ean-13.png");

static void CreateBarcode(BarcodeGenerator generator, string fileName) {
    Console.Write("Using {0}...", generator.GetType());

    // Set module size
    generator.ModuleSize = new Size(4, 100);

    // Create image of appropriate size
    var imageSize = generator.TotalSize;
    using var img = new Bitmap(imageSize.Width, imageSize.Height);

    // Draw barcode to image
    using var g = Graphics.FromImage(img);
    generator.DrawTo(g);

    // Save image to file
    img.Save(fileName);
    Console.WriteLine("OK");
}