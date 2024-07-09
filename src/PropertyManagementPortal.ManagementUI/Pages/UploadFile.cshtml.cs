using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PropertyManagementPortal.ManagementUI.Pages;

public class UploadFileModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public UploadFileModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
}
