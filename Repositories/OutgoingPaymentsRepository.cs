using Microsoft.EntityFrameworkCore;
using SimpLN.Data;
using SimpLN.Data.Entities;

namespace SimpLN.Repositories;

public class OutgoingPaymentsRepository
{
	private readonly AppDbContext _context;

	public OutgoingPaymentsRepository(AppDbContext context)
	{
		_context = context;
	}

	public async Task SaveTransactionMessage(PaymentEntity transaction)
	{
		if (transaction.Id == 0)
		{
			_context.Payment.Add(transaction);
		}
		else
		{
			_context.Payment.Update(transaction);
		}
		await _context.SaveChangesAsync();
	}

	public async Task<PaymentEntity?> GetOutgoingTransactionByPaymentHash(string paymentHash)
	{
		var paymentEntity = await _context.Set<PaymentEntity>()
			.FirstOrDefaultAsync(p => p.PaymentHash == paymentHash);

		return paymentEntity;
	}

	public async Task<List<PaymentEntity>> GetAll()
	{
		return await _context.Set<PaymentEntity>().ToListAsync();
	}

	public async Task SaveUserNote(string paymentHash, string userNote, string userId)
	{
		var payment = await GetOutgoingTransactionByPaymentHash(paymentHash);
    
		if (payment == null)
		{
			payment = new PaymentEntity
			{
				PaymentHash = paymentHash,
				UserNote = userNote,
				UserId = userId,
				Message = "",
				InvoiceString = "" 
			};
			_context.Payment.Add(payment);
		}
		else
		{
			payment.UserNote = userNote;
		}
    
		await _context.SaveChangesAsync();
	}

	public async Task<string?> GetUserNoteForTransaction(string paymentHash)
	{
		var transaction = await GetOutgoingTransactionByPaymentHash(paymentHash);
		return transaction?.UserNote ?? string.Empty;
	}
}
