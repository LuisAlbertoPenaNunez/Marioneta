using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Marioneta
{
	public interface ITransactionManager
	{
		void AddTransaction(View viewToBePlaced, Transaction transaction);

		Transaction GetTransactionFor(View view);

		bool ContainsTransaction(View view);

		IDictionary<View, Transaction> GetAllTransactions ();
	}
}