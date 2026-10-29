using AgentDesk.Models;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AgentDesk.ViewModels;

public partial class ClientDocumentItem : ObservableObject
{
    public string FileName { get; init; } = string.Empty;
    public string FilePath { get; init; } = string.Empty;
    public int? ServerId { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ProgressText))]
    private double progress;                       // 0..1

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsUploading))]
    private bool isUploaded;

    public bool IsUploading => !IsUploaded;
    public string ProgressText => $"{(int)(Progress * 100)}%";
}

public partial class DocumentsTabViewModel : BaseViewModel
{
    private const long MaxBytes = 10 * 1024 * 1024;
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };

    private int _clientId;
    private bool _isLoaded;

    public ObservableCollection<ClientDocumentItem> Documents { get; } = new();

    public async Task LoadAsync(int clientId)
    {
        //if (_isLoaded && _clientId == clientId)
        //    return;

        //_clientId = clientId;

        //var response = await _clientService.GetDocumentsAsync(clientId);

        //Documents.Clear();

        //if (response?.IsSuccess == true && response.Value is List<ClientDocument> list)
        //{
        //    foreach (var d in list)
        //    {
        //        Documents.Add(new ClientDocumentItem
        //        {
        //            FileName = d.FileName,
        //            FilePath = d.Url,
        //            ServerId = d.Id,
        //            IsUploaded = true,
        //            Progress = 1
        //        });
        //    }
        //}

        //_isLoaded = true;
    }

    public void Reset()
    {
        _isLoaded = false;
        Documents.Clear();
    }

    [RelayCommand]
    private async Task PickFilesAsync()
    {
        var results = await FilePicker.Default.PickMultipleAsync(new PickOptions
        {
            PickerTitle = "Select documents",
            FileTypes = FilePickerFileType.Images
        });

        if (results == null)
            return;

        foreach (var file in results)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(ext))
            {
                await _dialogueService.ShowToastAsync(
                    "Only JPEG and PNG files are supported.", ToastDuration.Long);
                continue;
            }

            long length;
            await using (var stream = await file.OpenReadAsync())
                length = stream.Length;

            if (length > MaxBytes)
            {
                await _dialogueService.ShowToastAsync(
                    $"{file.FileName} is larger than 10 MB.", ToastDuration.Long);
                continue;
            }

            var item = new ClientDocumentItem
            {
                FileName = file.FileName,
                FilePath = file.FullPath
            };

            Documents.Add(item);
            _ = UploadAsync(item, file);   // starts uploading immediately, shows progress bar
        }
    }

    private async Task UploadAsync(ClientDocumentItem item, FileResult file)
    {
        try
        {
            var progress = new Progress<double>(p => item.Progress = p);

            //var response = await _clientService.UploadDocumentAsync(_clientId, file, progress);

            //if (response?.IsSuccess == true)
            //{
            //    item.Progress = 1;
            //    item.IsUploaded = true;
            //}
            //else
            //{
            //    Documents.Remove(item);
            //    await _dialogueService.ShowToastAsync(
            //        response?.ErrorMessage ?? "Upload failed.", ToastDuration.Long);
            //}
        }
        catch (Exception ex)
        {
            Documents.Remove(item);
            await _dialogueService.ShowToastAsync(ex.Message, ToastDuration.Long);
        }
    }

    [RelayCommand]
    private async Task DeleteAsync(ClientDocumentItem? item)
    {
        if (item == null)
            return;

        if (item.ServerId != null)
        {
            //var response = await _clientService.DeleteDocumentAsync(item.ServerId.Value);

            //if (response?.IsSuccess != true)
            //{
            //    await _dialogueService.ShowToastAsync(
            //        response?.ErrorMessage ?? "Unable to delete document.", ToastDuration.Long);
            //    return;
            //}
        }

        Documents.Remove(item);
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Documents.Any(d => d.IsUploading))
        {
            await _dialogueService.ShowToastAsync(
                "Please wait for uploads to finish.", ToastDuration.Short);
            return;
        }

        await _dialogueService.ShowToastAsync("Documents saved successfully.", ToastDuration.Short);
    }
}
