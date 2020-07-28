using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CSharpAsync
{
    internal class Async1
{
	private sealed class CountRecursivelyAsyncd__1 : IAsyncStateMachine
	{
		public int state;

		public AsyncTaskMethodBuilder<int> t__builder;

		public int count;

		private int s__1;

		private YieldAwaitable.YieldAwaiter u__1;

		private TaskAwaiter<int> u__2;



        private void MoveNextOld()
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
						CountRecursivelyAsyncd__1 stateMachine = this;
						t__builder.AwaitUnsafeOnCompleted(ref awaiter2, ref stateMachine);
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
						CountRecursivelyAsyncd__1 stateMachine = this;
						t__builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
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
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	private static void Run()
	{
		//Console.WriteLine(Process.GetCurrentProcess().MaxWorkingSet);
		Console.WriteLine("Counted to {0}.", CountRecursivelyAsync(100000).Result);
	}

	private static Task<int> CountRecursivelyAsync(int count)
	{
		CountRecursivelyAsyncd__1 stateMachine = new CountRecursivelyAsyncd__1();
		stateMachine.count = count;
		stateMachine.t__builder = AsyncTaskMethodBuilder<int>.Create();
		stateMachine.state = -1;
		AsyncTaskMethodBuilder<int> t__builder = stateMachine.t__builder;
		t__builder.Start(ref stateMachine);
		return stateMachine.t__builder.Task;
	}
}
}
