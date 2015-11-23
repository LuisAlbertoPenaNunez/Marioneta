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

			//If KeyValuePair<View,Transaction> was not found

			if (transaction.Equals(default(KeyValuePair<View, Transaction>)))
				throw new Exception("MRelativeLayout.GetTransactionFor() -- Are you sure you added a view before calling this method?");

			var transactionValue = transaction.Value;

			//If the KeyValuePair<View,Transaction> was found but the transaction is null at the moment

			if (transactionValue == null)
				throw new Exception("MRelativeLayout.GetTransactionFor() -- A bug has ocurred please report the output log to developers");

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

