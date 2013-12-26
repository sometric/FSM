namespace FSM
{
	public class EntryBuilder<TState, TSignal>
		where TState: System.IComparable
		where TSignal: System.IComparable
	{
		Entry<TState, TSignal> _entry;
		Handler<TState> _handler;

		public EntryBuilder(Entry<TState, TSignal> entry)
		{
			_entry = entry;
		}

		void CheckHandler()
		{
			if (_handler == null)
				throw new System.ArgumentException("Handler is not set");
		}

		public EntryBuilder<TState, TSignal> On(TSignal signal)
		{
			_handler = _entry.CreateHandler(signal);
			return this;
		}

		public EntryBuilder<TState, TSignal> AddOnEnter(System.Action action)
		{
			_entry.OnEntered += action;
			return this;
		}
		
		public EntryBuilder<TState, TSignal> AddOnLeave(System.Action action)
		{
			_entry.OnLeft += action;
			return this;
		}

		public EntryBuilder<TState, TSignal> Advance(TState nextState)
		{
			CheckHandler();

			_handler.switcher = () => nextState;

			return this;
		}
		
		public EntryBuilder<TState, TSignal> Advance(System.Func<TState> switcher)
		{
			CheckHandler();

			_handler.switcher = switcher;

			return this;
		}
		
		public EntryBuilder<TState, TSignal> If(System.Func<bool> guard)
		{
			CheckHandler();

			_handler.guard = guard;

			return this;
		}

		public EntryBuilder<TState, TSignal> AddAction(System.Action action)
		{
			CheckHandler();

			_handler.action += action;

			return this;
		}
	}
}
