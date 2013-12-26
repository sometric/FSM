using System.Collections.Generic;

namespace FSM
{
	public class Entry<TState, TSignal>
		where TState: System.IComparable
		where TSignal: System.IComparable
	{
		Dictionary<TSignal, List<Handler<TState>>> _handlers = new Dictionary<TSignal, List<Handler<TState>>>();

		public System.Action OnEntered;
		public System.Action OnLeft;

		public Handler<TState> CreateHandler(TSignal signal)
		{
			List<Handler<TState>> handlerList;
	
			if (!_handlers.TryGetValue(signal, out handlerList))
			{
				handlerList = new List<Handler<TState>>();
				_handlers.Add(signal, handlerList);
			}

			var handler = new Handler<TState>();
			handlerList.Add(handler);

			return handler;
		}

		public List<Handler<TState>> GetHandlers(TSignal signal)
		{
			List<Handler<TState>> handlerList;
			
			_handlers.TryGetValue(signal, out handlerList);

			return handlerList;
		}

		public bool IsTerminal()
		{
			if (_handlers.Count == 0)
				return true;

			foreach (var handlerList in _handlers.Values)
			{
				foreach (var handler in handlerList)
				{
					if (handler.switcher != null)
						return false;
				}
			}

			return true;
		}
	}
}
