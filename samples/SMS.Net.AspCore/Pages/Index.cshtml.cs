using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SMS.Net.AspCore.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ISmsService _smsService;

        public IndexModel(ILogger<IndexModel> logger, ISmsService smsService)
        {
            _logger = logger;
            this._smsService = smsService;
        }

        public void OnGet()
        {
            var message = SmsMessage.Compose()
                .To("+212606358902")
                .WithContent("this a message")
                .Build();

            _smsService.Send(message);
        }
    }
}