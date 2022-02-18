using CNTO.Launcher.Application;
using CNTO.Launcher.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CNTO.Launcher.Web.Pages;

public class IndexModel : PageModel
{
    readonly AutoRestartService _autoRestartService;
    readonly LauncherService _launcherService;
    readonly IRepositoryCollection _repositoryCollection;
    readonly ILogger<IndexModel> _logger;

    public IndexModel(AutoRestartService autoRestartService,
                      LauncherService launcherService,
                      IRepositoryCollection repositoryCollection,
                      ILogger<IndexModel> logger)
    {
        _autoRestartService = autoRestartService;
        _launcherService = launcherService;
        _repositoryCollection = repositoryCollection;
        _logger = logger;
    }

    public void OnGet()
    {

    }

    public async Task OnPostRestartAsync()
    {
        _logger.LogInformation("Restarting...");
        await _autoRestartService.RestartAsync();
    }

    public async Task OnPostStartAsync()
    {
        _logger.LogInformation("Starting...");
        List<RepositoryId> repositoryIds = new();
        repositoryIds.Add(new("Main"));
        repositoryIds.Add(new("ServerOnly"));
        await _launcherService.StartServerAsync(repositoryIds, new List<Dlc>(), 1);
    }
}
