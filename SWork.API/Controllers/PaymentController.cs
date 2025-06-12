using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using SWork.Data.DTO.Wallet.TransactionDTO;
using SWork.ServiceContract.Interfaces;

namespace SWork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly IPayOSService _payOsService;
        private readonly ITransactionService _transactionService;

        public PaymentController(ITransactionService transactionService, IPayOSService payOsService)
        {
            _transactionService = transactionService;
            _payOsService = payOsService;
        }

        [HttpPost("payos/link-payment")]
        public async Task<IActionResult> CreatePaymentPayOSLink(WalletTransactionCreateDTO model)
        {
            try
            {
                var url = await _payOsService.CreatePaymentLink(model);
                return Ok(url);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("payos/confirm-webhook")]
        public async Task<IActionResult> ConfirmWebhook(string url)
        {
            try
            {
                var confirm = await _payOsService.ConfirmWebhook(url);
                return Ok(confirm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("payos/handle-webhook")]
        public async Task<IActionResult> HandleWebhook(WebhookType webhookData)
        {
            try
            {
                await _payOsService.HandlePaymentWebhook(webhookData);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
