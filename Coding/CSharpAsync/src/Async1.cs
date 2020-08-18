using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CSharpAsync
{

public interface IBuilderStateMachine<TResult>
{
	void SetBuilderTcs(TaskCompletionSource<TResult> tsc);
}

    internal class Async1
{
	private struct CountRecursivelyAsyncd__1 : IAsyncStateMachine, IBuilderStateMachine<int>
	{
		public int state;

		public AsyncTaskMethodBuilder<int> t__builder;

		public int count;

		private int s__1;

		private YieldAwaitable.YieldAwaiter u__1;

		private TaskAwaiter<int> u__2;



        private void MoveNextSimple()
		{
			int num = state;
			int result;
			try
			{
				TaskAwaiter<int> awaiter;
				if (num == 0)
				{
					awaiter = u__2;
					u__2 = default(TaskAwaiter<int>);
					num = (state = -1);
					goto IL_0088;
				}
				if (count > 0)
				{
					awaiter = CountRecursivelyAsync(count - 1).GetAwaiter();
					if (!awaiter.IsCompleted)
					{
						num = (state = 0);
						u__2 = awaiter;
						CountRecursivelyAsyncd__1 stateMachine = this;
						t__builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
						return;
					}
					goto IL_0088;
				}
				result = count;
				goto end_IL_0007;
				IL_0088:
				s__1 = awaiter.GetResult();
				result = 1 + s__1;
				end_IL_0007:;
			}
			catch (Exception exception)
			{
				state = -2;
				t__builder.SetException(exception);
				return;
			}
			state = -2;
			t__builder.SetResult(result);
		}


		private void MoveNext()
		{
			//MoveNextSimple();
			MoveNextSingleYield();
		}

		private void MoveNextSingleYield()
		{
			int num = state;
			int result;
			try
			{
				TaskAwaiter<int> awaiter;
				YieldAwaitable.YieldAwaiter awaiter2;
				if (num != 0)
				{
					if (num == 1)
					{
						awaiter = u__2;
						u__2 = default(TaskAwaiter<int>);
						num = (state = -1);
						goto IL_00fb;
					}
					awaiter2 = Task.Yield().GetAwaiter();
					if (!awaiter2.IsCompleted)
					{
						num = (state = 0);
						u__1 = awaiter2;
						//CountRecursivelyAsyncd__1 stateMachine = this;
						t__builder.AwaitUnsafeOnCompleted(ref awaiter2, ref this);
						return;
					}
				}
				else
				{
					awaiter2 = u__1;
					u__1 = default(YieldAwaitable.YieldAwaiter);
					num = (state = -1);
				}
				awaiter2.GetResult();
				if (count > 0)
				{
					awaiter = CountRecursivelyAsync(count - 1).GetAwaiter();
					if (!awaiter.IsCompleted)
					{
						num = (state = 1);
						u__2 = awaiter;
						//CountRecursivelyAsyncd__1 stateMachine = this;
						t__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
						return;
					}
					goto IL_00fb;
				}
				result = count;
				goto end_IL_0007;
				IL_00fb:
				s__1 = awaiter.GetResult();
				result = 1 + s__1;
				end_IL_0007:;
			}
			catch (Exception exception)
			{
				state = -2;
				t__builder.SetException(exception);
				return;
			}
			state = -2;
			t__builder.SetResult(result);
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			t__builder.SetStateMachine(stateMachine);
		}

		void IBuilderStateMachine<int>.SetBuilderTcs(TaskCompletionSource<int> tsc)
		{
			t__builder.SetTcs(tsc);
		}
		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	public static void Run(int countTo)
	{
		//Console.WriteLine(Process.GetCurrentProcess().MaxWorkingSet);
		Console.WriteLine("Counted to {0}.", CountRecursivelyAsync(countTo).Result);
	}

	private static Task<int> CountRecursivelyAsync(int count)
	{
		CountRecursivelyAsyncd__1 stateMachine = new CountRecursivelyAsyncd__1();
		stateMachine.count = count;
		stateMachine.t__builder = CSharpAsync.AsyncTaskMethodBuilder<int>.Create();
		stateMachine.state = -1;
		//stateMachine.t__builder.Start(ref stateMachine);
		//return stateMachine.t__builder.Task;Í


		AsyncTaskMethodBuilder<int> t__builder = stateMachine.t__builder;
		t__builder.Start(ref stateMachine);
		return stateMachine.t__builder.Task;
	}
}
}
