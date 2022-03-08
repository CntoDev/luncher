using CNTO.Launcher.Application;
using CNTO.Launcher.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CNTO.Launcher.Web.Pages;

[Authorize]
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
        Dlcs = new DlcCheckboxes();
    }

    [BindProperty]
    public DlcCheckboxes Dlcs { get; set; }

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
        _logger.LogInformation("Starting main...");
        List<RepositoryId> repositoryIds = new();
        repositoryIds.Add(new("Main"));
        repositoryIds.Add(new("Server Only"));
        List<Dlc> startingDlcs = BuildDlcList();
        await _launcherService.StartServerAsync(repositoryIds, startingDlcs, 2);
    }

    private List<Dlc> BuildDlcList()
    {
        List<Dlc> startingDlcs = new();
        if (Dlcs.Vn) startingDlcs.Add(new("vn"));
        if (Dlcs.Gm) startingDlcs.Add(new("gm"));
        if (Dlcs.Csla) startingDlcs.Add(new("csla"));
        if (Dlcs.Ws) startingDlcs.Add(new("ws"));
        return startingDlcs;
    }

    public async Task OnPostStartCampaignAsync()
    {
        _logger.LogInformation("Starting campaign...");
        List<RepositoryId> repositoryIds = new();
        repositoryIds.Add(new("Main"));
        repositoryIds.Add(new("Campaign"));
        repositoryIds.Add(new("Server Only"));
        List<Dlc> startingDlcs = BuildDlcList();
        await _launcherService.StartServerAsync(repositoryIds, startingDlcs, 2);
    }

    public class DlcCheckboxes
    {
        public bool Vn { get; set; }
        public bool Gm { get; set; }
        public bool Csla { get; set; }
        public bool Ws { get; set; }
    }
}
