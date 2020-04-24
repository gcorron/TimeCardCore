using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;

public static class ZipResult
{

    public static void CreateZip(string fileName, IEnumerable<string> files)
    {
        using (ZipFile zf = new ZipFile(fileName))
        {
            foreach (var file in files)
            {
                zf.Add(file);
            }
        }
    }

}
