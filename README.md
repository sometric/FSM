FSM
===

Primitive finite-state machine implementation

Example
-------

![alt text](http://upload.wikimedia.org/wikipedia/commons/thumb/9/9e/Turnstile_state_machine_colored.svg/500px-Turnstile_state_machine_colored.svg.png "Simple FSM from wikipedia")

```csharp

enum State
{
	Locked,
	Unlocked
}

enum Signal
{
	Coin,
	Push
}


var turnstile = new FSM.StateMachine<State, Signal>();

turnstile.In(State.Locked)
	.AddOnEnter(() => Say("I'm locked now"))
	.On(Signal.Push).AddAction(() => Say("Closed"))
	.On(Signal.Coin).Advance(State.Unlocked);

turnstile.In(State.Unlocked)
	.AddOnEnter(() => Say("I'm unlocked now"))
	.On(Signal.Coin).AddAction(() => Say("Thanks for the tip!"))
	.On(Signal.Push).Advance(State.Locked);

turnstile.Start(State.Locked);


```
