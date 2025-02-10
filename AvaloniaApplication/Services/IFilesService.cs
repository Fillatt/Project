using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Threading.Tasks;

namespace AvaloniaApplication.Services;

public interface IFilesService
{
    public Task<IStorageFile?> OpenImageFileAsync();
}
