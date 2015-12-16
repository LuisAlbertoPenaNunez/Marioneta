using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Marioneta
{
	public class RelativeBuilderTransactionManager : ITransactionManager
	{
		IDictionary<View, Transaction> _transactions;

		public RelativeBuilderTransactionManager ()
		{
			_transactions = new Dictionary<View, Transaction>();
		}

		public void AddTransaction (View viewToBePlaced, Transaction transaction)
		{
			if(viewToBePlaced == null)
				throw new ArgumentNullException("viewToBePlaced");
			
			if(transaction == null)
				throw new ArgumentNullException("transaction");

			_transactions.Add(viewToBePlaced, transaction);
		}

		public Transaction GetTransactionFor (View view)
		{
			var transaction = _transactions.First(x => x.Key == view);

			var transactionValue = transaction.Value;

			return transactionValue;
		}

		public bool ContainsTransaction (View view)
		{
			if(view == null)
				throw new ArgumentNullException("view");

			return _transactions.ContainsKey(view);
		}

		public IDictionary<View, Transaction> GetAllTransactions ()
		{
			return _transactions;
		}
	}
}

