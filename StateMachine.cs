using System.Collections.Generic;

namespace FSM {

	public class StateMachine<TState, TSignal>
		where TState: System.IComparable
		where TSignal: System.IComparable
	{
		Entry<TState, TSignal> _entry;
		Dictionary<TState, Entry<TState, TSignal>> _entries = new Dictionary<TState, Entry<TState, TSignal>>();
		LinkedList<TSignal> _signalsQueue = new LinkedList<TSignal>();
		bool _isBusy = false;

		public void Start()
		{
			Start(default(TState));
		}

		public void Start(TState initialState)
		{
			if (!IsStopped())
				throw new System.ArgumentException("StateMachine already started");

			SwitchState(initialState);
		}

		public bool IsStopped()
		{
			return (_entry == null) || _entry.IsTerminal();
		}

		public void Handle(TSignal signal)
		{
			if (_isBusy)
			{
				_signalsQueue.AddLast(signal);
				return;
			}

			_isBusy = true;

			try {

				DoHandle(signal);

				while (_signalsQueue.Count != 0)
				{
					DoHandle(_signalsQueue.First.Value);
					_signalsQueue.RemoveFirst();
				}
			
			} finally {

				_signalsQueue.Clear();
				_isBusy = false;

			}
		}

		void DoHandle(TSignal signal)
		{
			if (_entry == null)
				return;

			var handlerList = _entry.GetHandlers(signal);

			if (handlerList == null || handlerList.Count == 0)
				return;

			foreach (var handler in handlerList)
			{
				if (handler.guard != null && !handler.guard())
					continue;
				
				if (handler.action != null)
					handler.action();
				
				if (handler.switcher != null)
					SwitchState(handler.switcher());

				break;
			}
		}

		void SwitchState(TState state)
		{
			if (_entry != null && _entry.OnLeft != null)
				_entry.OnLeft();

			_entries.TryGetValue(state, out _entry);

			if (_entry != null && _entry.OnEntered != null)
				_entry.OnEntered();
		}

		public EntryBuilder<TState, TSignal> In(TState state)
		{
			Entry<TState, TSignal> entry;

			if (!_entries.TryGetValue(state, out entry))
			{
				entry = new Entry<TState, TSignal>();
				_entries.Add(state, entry);
			}

			return new EntryBuilder<TState, TSignal>(entry);
		}
	}

}
