using SimpLN.Data.Entities;
using SimpLN.Models.Config;
using SimpLN.Models.TransactionHistory;
using SimpLN.Repositories;
using System.Security.Claims;

namespace SimpLN.Services.TranscationHistoryServices;

public class OutgoingPaymentsService
{
	private readonly OutgoingPaymentsRepository _repository;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public OutgoingPaymentsService(OutgoingPaymentsRepository repository, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_httpContextAccessor = httpContextAccessor;
	}



	public async Task SaveMessage(PaymentToDatabaseModel model)
	{
		var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

		var entity = new PaymentEntity
		{
			Message = model.Message,
			PaymentHash = model.PaymentHash,
			UserId = userId,
			InvoiceString = model.InvoiceString
		};

		await _repository.SaveTransactionMessage(entity);
	}


	public async Task<PaymentToDatabaseModel?> GetEntityForPaymentHash(string paymentHash)
	{
		var entity = await _repository.GetOutgoingTransactionByPaymentHash(paymentHash);
		var model = new PaymentToDatabaseModel
		{
			Id = entity.Id,
			PaymentHash = entity.PaymentHash,
			Message = entity.Message,
			InvoiceString = entity.InvoiceString,
			UserId = entity.UserId,
			ApplicationUser = entity.ApplicationUser
		};

		return model;
	}

	public async Task<List<PaymentEntity>> GetAll()
	{
		return await _repository.GetAll();
	}

	public async Task SaveUserNote(string paymentHash, string userNote)
	{
		var userId = _httpContextAccessor.HttpContext?.User?
			.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    
		if (string.IsNullOrEmpty(userId))
		{
			throw new InvalidOperationException("User not authenticated");
		}
    
		await _repository.SaveUserNote(paymentHash, userNote, userId);
	}

	public async Task<string?> GetUserNoteForTransaction(string paymentHash)
	{
		return await _repository.GetUserNoteForTransaction(paymentHash);
	}
}
