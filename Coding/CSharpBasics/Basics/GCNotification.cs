using System;
using System.Threading;

namespace Basics
{
    public static class GCNotification
    {
        private static Action<int> _gcDone;
        public static event Action<int> GCDone
        {
            add
            {
                if (_gcDone == null){
                    new GenObject(0); new GenObject(2);
                }
                _gcDone += value;
            }
            remove
            {
                _gcDone -= value;
            }
        }

        private sealed class GenObject
        {
            private readonly int generation;

            public GenObject(int generation)
            {
                this.generation = generation;
            }

            ~GenObject()
            {
                var currentPlace = GC.GetGeneration(this);
                if (currentPlace >= generation)
                {
                    var temp = Volatile.Read(ref _gcDone);
                    if (temp != null) temp(currentPlace);
                }

                if (_gcDone != null &&
                    !AppDomain.CurrentDomain.IsFinalizingForUnload() &&
                    !Environment.HasShutdownStarted)
                {
                    if (generation == 0)
                    {
                        new GenObject(0);
                    }
                    else
                    {
                        GC.ReRegisterForFinalize(this);
                    }
                }
                else
                {
                    ;//let it go away!
                }
            }
        }
    }
}