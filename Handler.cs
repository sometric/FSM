namespace FSM
{
	public class Handler<TState>
		where TState: System.IComparable
	{
		public System.Func<TState> switcher;
		public System.Func<bool> guard;
		public System.Action action;
	}
}

