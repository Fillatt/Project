using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Serilog;
using System.Threading.Tasks;

namespace AvaloniaApplication.Services;

public class FilesService : IFilesService
{
    #region Private Fields
    private readonly Window _target;
    #endregion

    #region Constructors
    public FilesService(Window target)
    {
        _target = target;
    }
    #endregion

    public async Task<IStorageFile?> OpenImageFileAsync()
    {
        Log.Debug("FilesService.OpenImageFileAsync: Start");
        var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Open Image File",
            FileTypeFilter = [FilePickerFileTypes.ImageAll],
            AllowMultiple = false
        });

        Log.Debug("FilesService.OpenImageFileAsync: Done; Files: {Files}", files);
        return files.Count >= 1 ? files[0] : null;
    }
}
