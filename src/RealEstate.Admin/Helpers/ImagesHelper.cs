namespace RealEstate.Admin.Helpers;

public static class ImagesHelper
{
    private static readonly List<string> _imageExtensions = new List<string> { ".jpg", ".jpeg", ".jpe", ".bmp", ".gif", ".png" };

    public static bool IsImage(string fileName)
    {
        return _imageExtensions.Contains(Path.GetExtension(fileName).ToLowerInvariant());
    }
}